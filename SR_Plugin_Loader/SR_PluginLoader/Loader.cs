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
        public static string TITLE { get { return String.Format("[Sisco++'s Plugin Loader] {0}", Loader.VERSION); } }
        public static string NAME { get { return String.Format("[Plugin Loader] {0} by Sisco++", Loader.VERSION); } }
        public static Plugin_Version VERSION = new Plugin_Version(0, 2);// even though really this isnt a plugin, I guess if we ever do major changes a plugin could specify the loader as a requirement and set a specific version.

        private static GameObject root = null;
        public static Dictionary<string, Plugin> plugins = new Dictionary<string, Plugin>();
        private static string pluginDir = null;
        public static int _plugin_id = 0;
        public static Texture2D tex_unknown = new Texture2D(1, 1);
        public static Texture2D tex_alert = new Texture2D(1, 1);
        public static bool has_updates = false;

        public static string[] INCLUDE_DIRS = new string[] {  };
        public static FileStream config_stream = null;
        private static bool CONFIG_LOCK = false;
        private static WebClient web = new WebClient();
        private static string update_helper_file = null;


        private static MainMenu menu = null;

        public static void init()
        {
            if (Loader.config_stream != null) return;
            if (!Loader.Load_Config_Stream()) return;

            try
            {
                Loader.root = new GameObject();
                UnityEngine.Object.DontDestroyOnLoad(Loader.root);

                DebugHud.Init();
                Loader.menu = Loader.root.AddComponent<MainMenu>();

                Setup_Update_Helper();
                Setup_Plugin_Dir();
                Load_Assets();
                Check_For_Updates();

                Setup_Assembly_Resolver();
                Assemble_Plugin_List();
                Load_Config();
                //Update_Plugins_UI();
            }
            catch(Exception ex)
            {
                DebugHud.Log(ex.Message);
            }
        }

        private static void Setup_Update_Helper()
        {
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            update_helper_file = String.Format("{0}/Auto_Update_Helper.exe", dir);

            //Let's keep the users folder as uncluttered as possible eh?
            if (File.Exists(update_helper_file)) File.Delete(update_helper_file);
        }

        public static void Update_Plugins_UI()
        {
            MainMenu.plugins_panel.Update_Plugins_UI();
        }

        public static void Load_Assets()
        {
            Loader.TryLoadAsset(ref Loader.tex_unknown, "unknown.png");
            Loader.TryLoadAsset(ref Loader.tex_alert, "alert.png");
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
                string ext = Path.GetExtension(file);
                if (ext != ".dll") continue;

                Plugin plug = new Plugin(file);

                string name = Path.GetFileNameWithoutExtension(file);
                plugins[name] = plug;

                try
                {
                    plug.load();
                }
                catch (Exception ex)
                {
                    DebugHud.Log(ex);
                }
            }
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
            CONFIG_LOCK = true;
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
            CONFIG_LOCK = false;
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
            if (CONFIG_LOCK) return;
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
            DebugHud.LogSilent("Plugin_Status_Change: Saving config...");
            Loader.Save_Config();
        }

        public static void Add_Notice(UI_Notification notice)
        {
            MainMenu.plugins_panel.Add_Notice(notice);
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
            if(has_updates == true)
            {
                Add_Notice(new UI_Notification()
                {
                    msg = "A new version of the plugin loader is available\nClick this box to update!",
                    title = "Update Available",
                    onClick = delegate(){ Loader.Auto_Update(); }
                });
            }
        }

        public static void Auto_Update()
        {
            DebugHud.Log("Updating plugin loader...");
            byte[] buf = web.DownloadData("https://raw.github.com/dsisco11/SR_Plugin_Loader/master/Installer/SR_PluginLoader.dll");
            string file = Assembly.GetExecutingAssembly().Location;
            string new_file = String.Format("{0}.tmp", file);
            string old_file = String.Format("{0}.old", file);
            
            File.WriteAllBytes(new_file, buf);
            if (File.Exists(old_file)) File.Delete(old_file);
            File.Replace(new_file, file, old_file);
            // We have to restart the game for this to take effect.
            Restart_App();
        }

        /// <summary>
        /// Checks GitHub to see if the currently installed version of the plugin loader is the most up to date.
        /// </summary>
        private static bool Do_Update_Check()
        {
            // Time to check with GitHub and see if there is a newer version of the plugin loader out!
            try
            {
                // Add a useragent string so GitHub doesnt return 403 and also so they can have a chat if they like.
                web.Headers.Add("user-agent", "SR_Plugin_Loader on GitHub");
                // Add a handler for SSL certs because mono doesnt have any trusted ones by default
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => { return true; });

                // Fetch repo information
                string url = "https://api.github.com/repos/dsisco11/SR_Plugin_Loader/git/trees/master?recursive=1";
                string jsonStr = web.DownloadString(url);
                if (jsonStr == null || jsonStr.Length <= 0)
                {
                    DebugHud.Log("[AutoUpdater] Got a NULL or empty response from server!");
                    return false;
                }

                // Parse the json response from GitHub
                var git = SimpleJSON.JSON.Parse(jsonStr);
                var tree = git["tree"].AsArray;

                // Find the plugin loaders DLL installation file
                foreach (JSONNode file in tree)
                {
                    if(String.Compare(file["path"], "Installer/SR_PluginLoader.dll") == 0)
                    {
                        // Compare the SHA1 hash for the dll on GitHub to the hash for the one currently installed
                        string nSHA = file["sha"];
                        string cSHA = Get_Current_Version_Sha();

                        if (String.Compare(nSHA, cSHA) != 0)
                        {
                            // ok they don't match, now let's just make double sure that it isn't because we are using an unreleased developer version
                            // First find the url for the file on GitHub
                            string tmpurl = file["url"];
                            // now we want to replace it's hash with ours and check if it exists!
                            tmpurl = tmpurl.Replace(nSHA, cSHA);
                            string jStr = "";
                            
                            // Try and download the file info for the currently installed loaders sha1 hash
                            try
                            {
                                jStr = web.DownloadString(tmpurl);
                            }
                            catch
                            {
                                // A file for this hash does not exhist on the github repo. So this must be a Dev version.
                                return false;
                            }
                            
                            return true;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                DebugHud.Log(ex);
            }
            return false;//no update
        }

        public static Plugin GetPluginByHash(string hash)
        {
            return null;
        }
        /// <summary>
        /// Gets the SHA1 hash for the currently installed version of the plugin loader so it can be compared to the one on github and updated if needed
        /// </summary>
        /// <returns></returns>
        private static string Get_Current_Version_Sha()
        {
            string file = Assembly.GetExecutingAssembly().Location;
            var buf = File.ReadAllBytes(file);
            string data = Encoding.ASCII.GetString(buf);
            data = ("blob" + data.Length + "\0" + data);

            System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create();
            byte[] hash = sha1.ComputeHash( Encoding.ASCII.GetBytes(data));

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
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

        private static void Restart_App()
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
    }
}
