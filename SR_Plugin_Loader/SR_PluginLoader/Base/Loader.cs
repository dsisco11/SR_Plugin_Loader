using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System.Net;
using SimpleJSON;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Diagnostics;

namespace SR_PluginLoader
{
    public static class Loader
    {
        /// <summary>
        /// The version for the loader itself
        /// </summary>
        public static Plugin_Version VERSION = new Plugin_Version(0, 6, 2);
        
        public static string TITLE { get { return String.Format("Sisco++'s Plugin Loader {0}", Loader.VERSION); } }
        public static string NAME { get { return String.Format("[Plugin Loader] {0} by Sisco++", Loader.VERSION); } }

        private static GameObject root = null;
        public static Dictionary<string, Plugin> plugins = new Dictionary<string, Plugin>();
        private static string pluginDir = null;

        public static bool has_updates = false;

        public static string[] INCLUDE_DIRS = new string[] {  };
        public static FileStream config_stream = null;
        private static bool IN_LOADING_PHASE = false;
        private static WebClient web = new WebClient();
        private static string update_helper_file = null;
        private static List<string> available_updates= new List<string>();// This isn't for plugin updates (yet)
        public static SettingsFile Config = null;
        
        private static Plugin_Update_Viewer plugin_updater = null;
        private static DevMenu dev_tools = null;


        public static void init(string hash)
        {
            if (Loader.Config != null) return;
            Stopwatch timer = new Stopwatch();
            timer.Start();

            if (!Loader.Load_Config_Stream()) return;

            try
            {
                Loader.root = new GameObject();
                UnityEngine.Object.DontDestroyOnLoad(Loader.root);

                DebugHud.Init();
                TextureHelper.Load_Common();
                SiscosHooks.Setup();
                PluginLoader_Watermark.Setup();
                MainMenu.Setup();
                DebugUI.Setup();

                Setup_Update_Helper();
                bool ok = Verify_PluginLoader_Hash(hash);
                if (!ok) return;

                IN_LOADING_PHASE = true;
                Setup_Plugin_Dir();
                Check_For_Updates();

                Setup_Assembly_Resolver();
                Upgrades.Setup();
                Assemble_Plugin_List(); 
                Load_Config();
                IN_LOADING_PHASE = false;
                ResourceExt.map_SR_Icons();

                plugin_updater = uiControl.Create<Plugin_Update_Viewer>();// This control manages itself and is only able to become visible under certain conditions which it will control. Therefore it needs no var to track it.
                plugin_updater.Show();

                dev_tools = uiControl.Create<DevMenu>();
                //dev_tools.Show();
                //dev_tools.onShown += (uiWindow w) => { GameTime.Pause(); };
                //dev_tools.onHidden += (uiWindow w) => { GameTime.Unpause(); };

                //Misc_Experiments.Find_Common_Classes_For_Idents(new HashSet<Identifiable.Id> { Identifiable.Id.PINK_RAD_LARGO });
            }
            catch(Exception ex)
            {
                DebugHud.Log("Exception during PluginLoader initialization!");
                DebugHud.Log(ex);
            }
            finally
            {
                timer.Stop();
                DebugHud.LogSilent("Plugin Loader initialized! Took: {0}ms", timer.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// Checks the given hash against the current dll files hash to make sure the proper version is installed
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        private static bool Verify_PluginLoader_Hash(string hash)
        {
            string dll_hash = Util.Git_File_Sha1_Hash(Assembly.GetExecutingAssembly().Location);
            bool ok = (String.Compare(dll_hash, hash) == 0);

            if(!ok)
            {
                new UI_Notification()
                {
                    msg = "The current loader's hash does not match the hash of the one that was installed, click here to install this version.",
                    title = "Version Mismatch",
                    icon = TextureHelper.icon_alert,
                    onClick = () =>
                    {
                        Restart_App();
                    }
                };
            }

            return ok;
        }

        private static void Setup_Update_Helper()
        {
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            update_helper_file = String.Format("{0}/Auto_Update_Helper.exe", dir);

            //Let's keep the users folder as uncluttered as possible eh?
            if (File.Exists(update_helper_file)) File.Delete(update_helper_file);
        }
        
        /// <summary>
        /// Reads the plugins directory and makes a record of all the plugins which exist.
        /// </summary>
        public static void Assemble_Plugin_List()
        {
            plugins.Clear();
            string[] files = Directory.GetFiles(pluginDir);
            foreach(string file in files)
            {
                Add_Plugin_To_List(file);
            }
        }

        public static bool Add_Plugin_To_List(string file)
        {
            string ext = Path.GetExtension(file);
            if (ext != ".dll") return false;

            Plugin plug = new Plugin(file);

            string name = Path.GetFileNameWithoutExtension(file);
            plugins[name] = plug;

            try
            {
                plug.load();
                foreach(KeyValuePair<string, Plugin> kv in plugins)
                {
                    if (String.Compare(name, kv.Key) == 0) continue;
                    if(String.Compare(plug.Hash, kv.Value.Hash)==0)// These plugins are different files but are the same plugin according to their hash (not data hash, id hash so they might be differing versions of the same plugin).
                    {//we need to unload, maybe delete one of them. figure out which one is the latest
                        Plugin trash = null;

                        if (plug.data.VERSION > kv.Value.data.VERSION) trash = kv.Value;
                        else if (plug.data.VERSION < kv.Value.data.VERSION) trash = plug;
                        else
                        {
                            if (trash == null)
                            {
                                if (plug.file_time > kv.Value.file_time) trash = plug;
                                else trash = kv.Value;
                            }
                        }

                        if (trash == null) DebugHud.Log("Found multiple instances of the same plugin and cannot determine which plugin file to ignore! Plugin: ", plug);
                        else
                        {
                            DebugHud.Log("Multiple instances of the same plugin have been found, uninstalling file: {0}", trash.file);
                            trash.Uninstall();
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DebugHud.Log(ex);
                return false;
            }

            if(MainMenu.isReady)
            {
                if(PluginManager.Instance != null)
                {
                    PluginManager.Instance.Update_Plugins_List();
                }
            }
            return true;
        }

        public static void Setup_Plugin_Dir()
        {
            string dataDir = Path.GetDirectoryName(UnityEngine.Application.dataPath);
            pluginDir = Path.GetFullPath(String.Format("{0}/plugins/", dataDir));
            //DebugHud.Log("Setup_Plugin_Dir: {0}", pluginDir);

            if (!Directory.Exists(pluginDir))
            {
                Directory.CreateDirectory(pluginDir);
                FileStream strm = new FileStream(Get_CFG_File(), FileMode.CreateNew);
                strm.Close();
            }
        }
        


        private static string Get_CFG_File()
        {
            return String.Format("{0}/plugins.cfg", UnityEngine.Application.dataPath);
        }
        
        public static void Load_Enabled_Plugins(string[] list)
        {
            foreach (var name in list)
            {
                Plugin p = null;
                if (plugins.TryGetValue(name, out p))
                {
                    try
                    {
                        p.Enable();
                    }
                    catch(Exception ex)
                    {
                        DebugHud.Log(ex);
                    }
                }
            }
        }

        public static void Load_Config()
        {
            Config = new SettingsFile("plugins.json");
            if (Config == null) throw new Exception("CONFIG is not ready!");

            try
            {
                if (Config["ENABLED_PLUGINS"] != null)
                {
                    List<string> list = Config.Get_Array<string>("ENABLED_PLUGINS");
                    Load_Enabled_Plugins(list.ToArray());
                }
                else// Load from the old save format!
                {
                    string[] list = new string[] { };

                    if (Loader.config_stream != null)
                    {
                        byte[] buf = new byte[Loader.config_stream.Length];
                        int read = Loader.config_stream.Read(buf, 0, (int)Loader.config_stream.Length);
                        if (read < (int)Loader.config_stream.Length)
                        {
                            int remain = ((int)Loader.config_stream.Length - read);
                            int r = 0;
                            while (r < remain && remain > 0)
                            {
                                r = Loader.config_stream.Read(buf, read, remain);
                                read += r;
                                remain -= r;
                            }
                        }

                        string str = Encoding.ASCII.GetString(buf);
                        list = str.Split('\n');
                        Load_Enabled_Plugins(list);

                    }

                    Config.Set_Array<string>("ENABLED_PLUGINS", list);
                    Config.Save();
                }

                if (Loader.config_stream != null)
                {
                    Loader.config_stream.Close();
                    File.Delete(Get_CFG_File());
                }
            }
            catch(Exception ex)
            {
                DebugHud.Log(ex);
            }
        }
        
        [Obsolete("Remove around v0.6.5")]
        public static bool Load_Config_Stream()
        {
            try
            {
                if (!File.Exists(Get_CFG_File())) return true;

                FileStream stream = new FileStream(Get_CFG_File(), FileMode.Open, FileAccess.ReadWrite);
                if (stream == null) return true;

                Loader.config_stream = stream;
                return true;
            }
            catch(Exception ex)
            {
                DebugHud.Log(ex);
                return false;
            }
        }
        
        public static void Save_Config()
        {
            if (IN_LOADING_PHASE==true) return;

            try
            {
                if(plugins == null)
                {
                    DebugHud.Log("CRITICAL ERROR: Active plugins list is null!");
                    return;
                }
                
                // Select the keynames of all non-null, enabled plugins from our dictionary.
                List<string> list = Loader.plugins.Where(kv => (kv.Value != null && kv.Value.enabled)).Select(kv => kv.Key).ToList();

                Config.Set_Array<string>("ENABLED_PLUGINS", list);
                Config.Save();
            }
            catch (Exception ex)
            {
                DebugHud.Log(ex);
            }
        }

        public static void Plugin_Status_Change(Plugin p, bool enabled)
        {
            if (IN_LOADING_PHASE) return;
            DebugHud.LogSilent("Plugin_Status_Change: Saving config...");
            Loader.Save_Config();
        }


        public static void Setup_Assembly_Resolver()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }
        
        public static string Find_Assembly_In_Include_Dirs(string name)
        {
            //try and see if the file is in the target DLL's dir first...
            string folderPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string assemblyPath = Path.Combine(folderPath, new AssemblyName(name).Name + ".dll");
            if (File.Exists(assemblyPath) == true) return assemblyPath;

            foreach (string dir in Loader.INCLUDE_DIRS)
            {
                folderPath = Path.GetDirectoryName((string)dir);
                assemblyPath = Path.Combine(folderPath, new AssemblyName(name).Name + ".dll");
                if (File.Exists(assemblyPath) == true) return assemblyPath;
            }

            return null;
        }
        
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string assemblyPath = Find_Assembly_In_Include_Dirs(args.Name);

            if (File.Exists(assemblyPath) == false) return null;

            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }

        #region AUTO-UPDATER

        private static void Check_For_Updates()
        {
            has_updates = Do_Update_Check();
            if (has_updates == true)
            {
                new UI_Notification()
                {
                    msg = "A new version of the plugin loader is available\nClick this box to update!",
                    title = "Update Available",
                    onClick = delegate () { Loader.Auto_Update(); }
                };
            }
        }

        public static void Auto_Update()
        {
            foreach(var url in available_updates)
            {
                DebugHud.Log("Updating plugin loader...");
                byte[] buf = web.DownloadData(url);

                string file = String.Concat(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Path.GetFileName(url));
                string new_file = String.Format("{0}.tmp", file);
                string old_file = String.Format("{0}.old", file);

                File.WriteAllBytes(new_file, buf);
                if (File.Exists(old_file)) File.Delete(old_file);
                File.Replace(new_file, file, old_file);
            }
            // We have to restart the game for this to take effect.
            Restart_App();
        }

        /// <summary>
        /// Checks GitHub to see if the currently installed version of the plugin loader is the most up to date.
        /// </summary>
        private static bool Do_Update_Check()
        {
            try
            {
                string assembly_url = "https://raw.github.com/dsisco11/SR_Plugin_Loader/master/Installer/SR_PluginLoader.dll";
                var assembly_status = Git_Updater.instance.Get_Update_Status(assembly_url, Assembly.GetExecutingAssembly().Location);
                if (assembly_status == FILE_UPDATE_STATUS.OUT_OF_DATE)
                {
                    available_updates.Add(assembly_url);
                    //the assembly is out of date!
                    DebugHud.Log("[AutoUpdate] The plugin loader is out of date!");
                }

                string installer_path = String.Format("{0}\\SR_PluginLoader_Installer.exe", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                string installer_url = "https://raw.github.com/dsisco11/SR_Plugin_Loader/master/Installer/SR_PluginLoader_Installer.exe";
                var installer_status = Git_Updater.instance.Get_Update_Status(installer_url, installer_path);
                if (installer_status == FILE_UPDATE_STATUS.OUT_OF_DATE)
                {
                    available_updates.Add(installer_url);
                    //the installer is out of date!
                    DebugHud.Log("[AutoUpdate] The installer is out of date!");
                }

                return (available_updates.Count > 0);
            }
            catch(Exception ex)
            {
                DebugHud.Log(ex);
            }

            return false;
        }

        public static void Restart_App()
        {
            try
            {
                byte[] buf = Util.Load_Resource("Restart_Helper.exe");
                if (buf != null && buf.Length > 0)
                {
                    File.WriteAllBytes(update_helper_file, buf);
                    string args = String.Format("{0}", Process.GetCurrentProcess().Id);
                    Process.Start(update_helper_file, args);
                }
                else
                {
                    DebugHud.Log("Failed to unpack the auto update helper!");
                }
            }
            catch (Exception ex)
            {
                DebugHud.Log(ex);
            }
        }
        #endregion
                
        private static bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            //Return true if the server certificate is ok
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            bool acceptCertificate = true;

            //The server did not present a certificate
            if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNotAvailable) == SslPolicyErrors.RemoteCertificateNotAvailable)
            {
                acceptCertificate = false;
            }
            else
            {
                //The certificate does not match the server name
                if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNameMismatch) == SslPolicyErrors.RemoteCertificateNameMismatch)
                {
                    acceptCertificate = false;
                }

                //There is some other problem with the certificate
                if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) == SslPolicyErrors.RemoteCertificateChainErrors)
                {
                    foreach (X509ChainStatus item in chain.ChainStatus)
                    {
                        if (item.Status != X509ChainStatusFlags.RevocationStatusUnknown && item.Status != X509ChainStatusFlags.OfflineRevocation)
                            break;

                        if (item.Status != X509ChainStatusFlags.NoError)
                        {
                            acceptCertificate = false;
                        }
                    }
                }
            }

            //If Validation failed
            if (acceptCertificate == false)
            {
                acceptCertificate = true;
            }

            return acceptCertificate;
        }
    }
}
