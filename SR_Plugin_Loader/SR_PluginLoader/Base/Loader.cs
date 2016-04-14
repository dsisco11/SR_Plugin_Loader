using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
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
        public static bool atMainMenu = true;
        public static string TITLE { get { return String.Format("Sisco++'s Plugin Loader {0}", Loader.VERSION); } }
        public static string DOWNLOADTITLE { get { return String.Format("Download Plugins"); } }
        public static string NAME { get { return String.Format("[Plugin Loader] {0} by Sisco++", Loader.VERSION); } }
        public static Plugin_Version VERSION = new Plugin_Version(0, 4);// even though really this isnt a plugin, I guess if we ever do major changes a plugin could specify the loader as a requirement and set a specific version.

        private static GameObject root = null;
        public static Dictionary<string, Plugin> plugins = new Dictionary<string, Plugin>();
        private static string pluginDir = null;
        public static int _plugin_id = 0;
        
        public static Texture2D tex_unknown = new Texture2D(1, 1);
        public static Texture2D tex_alert = new Texture2D(1, 1);
        public static Texture2D tex_close = new Texture2D(1, 1);
        public static Texture2D tex_close_dark = new Texture2D(1, 1);
        public static Texture2D tex_logo = new Texture2D(1, 1);
        public static Texture2D tex_checkbox = new Texture2D(1, 1);
        public static Texture2D tex_checkmark = new Texture2D(1, 1);

        public static bool has_updates = false;

        public static string[] INCLUDE_DIRS = new string[] {  };
        public static FileStream config_stream = null;
        private static bool IN_LOADING_PHASE = false;
        private static WebClient web = new WebClient();
        private static string update_helper_file = null;
        private static List<string> available_updates= new List<string>();// This isn't for plugin updates (yet)
        
        private static MainMenu menu = null;
        private static Plugin_Update_Viewer plugin_updater = null;


        public static void init(string hash)
        {
            if (Loader.config_stream != null) return;
            if (!Loader.Load_Config_Stream()) return;

            try
            {
                Loader.root = new GameObject();
                UnityEngine.Object.DontDestroyOnLoad(Loader.root);

                DebugHud.Init();
                Loader.menu = Loader.root.AddComponent<MainMenu>();

                SiscosHooks.init();
                
                Setup_Update_Helper();
                Load_Assets();
                bool ok = Verify_PluginLoader_Hash(hash);
                if (!ok) return;

                IN_LOADING_PHASE = true;
                Setup_Plugin_Dir();
                Check_For_Updates();

                Setup_Assembly_Resolver();
                Assemble_Plugin_List(); 
                Load_Config();
                IN_LOADING_PHASE = false;

                plugin_updater = uiControl.Create<Plugin_Update_Viewer>();// This control manages itself and is only able to become visible under certain conditions which it will control. Therefore it needs no var to track it.
                plugin_updater.Show();
            }
            catch(Exception ex)
            {
                DebugHud.Log("Exception during PluginLoader initialization!");
                DebugHud.Log(ex);
            }
        }
        
        /// <summary>
        /// Checks the given hash against the current dll files hash to make sure the proper version is installed
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        private static bool Verify_PluginLoader_Hash(string hash)
        {
            string dll_hash = Utility.Git_File_Sha1_Hash(Assembly.GetExecutingAssembly().Location);
            bool ok = (String.Compare(dll_hash, hash) == 0);

            if(!ok)
            {
                new UI_Notification()
                {
                    msg = "The current loader's hash does not match the hash of the one that was installed, click here to install this version.",
                    title = "Version Mismatch",
                    icon = Loader.tex_alert,
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

        public static void Load_Assets()
        {
            Loader.TryLoadAsset(ref Loader.tex_unknown, "unknown.png");
            Loader.TryLoadAsset(ref Loader.tex_alert, "alert.png");
            Loader.TryLoadAsset(ref Loader.tex_close, "close.png");
            Loader.TryLoadAsset(ref Loader.tex_close_dark, "close.png");
            Loader.TryLoadAsset(ref Loader.tex_logo, "logo.png");
            Loader.TryLoadAsset(ref Loader.tex_checkbox, "checkbox.png");
            Loader.TryLoadAsset(ref Loader.tex_checkmark, "checkmark.png");

            Utility.Tint_Texture(Loader.tex_close_dark, new Color(1f, 1f, 1f, 0.5f));
        }

        public static void TryLoadAsset(ref Texture2D tex, string asset)
        {
            byte[] buf = Utility.Load_Resource(asset);
            if (buf == null) return;
            tex.LoadImage(buf);
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

            if(Loader.menu != null)
            {
                if(MainMenu.plugin_manager != null)
                {
                    MainMenu.plugin_manager.Update_Plugins_List();
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
            try
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
                string[] en = str.Split('\n');
                
                Load_Enabled_Plugins(en);
            }
            catch (Exception ex)
            {
                DebugHud.Log(ex);
            }
        }

        /// <summary>
        /// This function serves two purposes.
        /// 1) It loads our config data, which plugins th euser has enabled and whatnot.
        /// 2) It aquires a file lock on the config file, so that even though the loaders entry site seems to be run simultaneously on multiple threads on start. Only one instance can end up getting the lock and all others will exit!
        /// </summary>
        public static bool Load_Config_Stream()
        {
            try
            {
                FileStream stream = new FileStream(Get_CFG_File(), FileMode.OpenOrCreate, FileAccess.ReadWrite);
                if (stream == null) return false;

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
                if (config_stream == null)
                {
                    DebugHud.Log("CRITICAL ERROR: Config stream not loaded, loading now!");
                    Load_Config_Stream();
                }
                if(plugins == null)
                {
                    DebugHud.Log("CRITICAL ERROR: Active plugins list is null!");
                    return;
                }

                List<string> arr = new List<string>();
                foreach (KeyValuePair<string, Plugin> kv in Loader.plugins)
                {
                    if (kv.Key == null) continue;
                    if(kv.Value == null)
                    {
                        DebugHud.Log("NULL PLUGIN: {0}", kv.Key);
                        continue;
                    }

                    if (kv.Value.enabled == true)
                    {
                        DebugHud.Log("Enabled plugin: {0}", kv.Key);
                        arr.Add(kv.Key);
                    }
                }

                byte[] buf = Encoding.ASCII.GetBytes(String.Join("\n", arr.ToArray()));

                Loader.config_stream.SetLength(0);// erase all config file's contents
                Loader.config_stream.Seek(0, SeekOrigin.Begin);// go back to the config file's beginning
                Loader.config_stream.Write(buf, 0, (int)buf.Length);
                Loader.config_stream.Flush();
                
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
        
        public static bool Is_Plugin_Installed(Plugin_Data data)
        {
            return Is_Plugin_Installed(data.Hash);
        }

        public static bool Is_Plugin_Installed(string hash)
        {
            foreach (KeyValuePair<string, Plugin> kv in Loader.plugins)
            {
                if (String.Compare(kv.Value.Hash, hash) == 0) return true;
            }
            return false;
        }

        public static Plugin Get_Plugin(string name_hash)
        {
            foreach (KeyValuePair<string, Plugin> kv in Loader.plugins)
            {
                if (String.Compare(kv.Value.Hash, name_hash) == 0) return kv.Value;
            }

            return null;
        }
        
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

        public static void Restart_App()
        {
            try
            {
                byte[] buf = Utility.Load_Resource("Restart_Helper.exe");
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
            catch(Exception ex)
            {
                DebugHud.Log(ex);
            }
        }
    }
}
