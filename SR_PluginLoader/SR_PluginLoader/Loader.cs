using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngineInternal;
using System.Threading;
using System.Diagnostics;

namespace SR_PluginLoader
{
    public static class Loader
    {
        public static string TITLE { get { return String.Format("[Sisco++'s Plugin Loader] {0}", Loader.VERSION); } }
        public static string NAME { get { return String.Format("[Plugin Loader] {0} by Sisco++", Loader.VERSION); } }
        public static Plugin_Version VERSION = new Plugin_Version(0, 1);// even though really this isnt a plugin...

        private static GameObject root = null;
        public static Dictionary<string, Plugin> plugins = new Dictionary<string, Plugin>();
        private static string pluginDir = null;
        public static int _plugin_id = 0;
        public static Texture2D tex_unknown = new Texture2D(1, 1);
        public static Texture2D tex_alert = new Texture2D(1, 1);
        public static string[] INCLUDE_DIRS = new string[] {  };
        public static FileStream config_stream = null;
        private static bool CONFIG_LOCK = false;


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

                DebugHud.Log("Unity v{0}", Application.unityVersion);
                Setup_Plugin_Dir();
                Load_Assets();
                Setup_Assembly_Resolver();
                Assemble_Plugin_List();
                Load_Config();
                Update_Plugins_UI();
            }
            catch(Exception ex)
            {
                DebugHud.Log(ex.Message);
            }
        }

        public static void Update_Plugins_UI()
        {
            var panel = MainMenu.plugins_panel.GetComponent<PluginsPanel>();
            panel.Update_Plugins_UI();
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
            string[] dirs = Directory.GetDirectories(pluginDir);
            foreach(string dir in dirs)
            {
                string folder = dir.Remove(0, pluginDir.Length);
                string file = String.Format("{0}/{1}.dll", dir, folder);
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
            //DebugHud.Log("Assemble_Plugin_List FINISHED!");
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
    }
}
