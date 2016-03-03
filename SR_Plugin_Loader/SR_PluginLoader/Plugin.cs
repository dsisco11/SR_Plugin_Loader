using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SR_PluginLoader
{
    public class Plugin 
    {
        public Plugin_Data data = null;
        private int _id = 0;
        public int id { get { return this._id; } }
        public string Hash { get { return Utility.SHA(String.Format("{0}.{1}", this.data.AUTHOR, this.data.NAME)); } }
        public bool IsInstalled { get { return (Loader.GetPluginByHash(Hash) != null); } }


        public bool enabled = false;
        /// <summary>
        /// Does this plugin have dependencys that arent currently met?
        /// </summary>
        public bool has_dependency_issues = false;
        public List<Plugin_Dependency> unmet_dependencys = new List<Plugin_Dependency>();

        public string file = null;
        public string dir = null;
        private string dll_name = null;

        public List<string> errors = new List<string>();

        public Texture2D icon = null;
        public Texture2D thumbnail = null;
        
        private Assembly dll = null;
        private Type pluginClass = null;
        private MethodInfo load_funct = null;
        private MethodInfo unload_funct = null;


        public Plugin(string file, bool en = false)
        {
            this._id = Loader._plugin_id++;
            this.file = file;
            this.dir = Path.GetDirectoryName(file);
            this.dll_name = Path.GetFileName(file);

            this.enabled = en;
        }


        /// <summary>
        /// This is where we actually do all the loading, to prevent any exceptions from causing the plugin instance to not be put into our global plugins map.
        /// This way we can ensure any errors that we DO get while loading can be properly displayed at the plugins menu!
        /// </summary>
        public void load()
        {
            this.Load_DLL();
            this.Load_Assets();
            this.Load_Plugin_Info();
        }
        
        private void Load_Assets()
        {
            if (this.dll == null) return;
            string icon_file = "icon.png";
            string thumb_file = "thumb.png";

            
            if (icon_file != null)
            {
                byte[] buf = this.Load_Resource(icon_file);
                if (buf != null)
                {
                    this.icon = new Texture2D(0, 0, TextureFormat.RGBA32, true);
                    this.icon.filterMode = FilterMode.Trilinear;
                    this.icon.LoadImage(buf);
                    this.icon.Apply(true);
                }
                else
                {
                    this.icon = null;
                }
            }

            if (thumb_file != null)
            {
                byte[] buf = this.Load_Resource(thumb_file);
                if (buf != null)
                {
                    this.thumbnail = new Texture2D(0, 0, TextureFormat.RGBA32, true);
                    this.thumbnail.filterMode = FilterMode.Trilinear;
                    this.thumbnail.LoadImage(buf);
                    this.thumbnail.Apply(true);
                }
                else
                {
                    this.thumbnail = null;
                }
            }
        }

        private void Load_Plugin_Info()
        {
            try
            {
                var field = this.pluginClass.GetField("PLUGIN_INFO", BindingFlags.Public | BindingFlags.Static);
                if (field == null) return;

                this.data = (Plugin_Data)field.GetValue(null);
                this.data.DESCRIPTION = this.data.DESCRIPTION.Trim(new char[] {'\n', '\r' });
            }
            catch(Exception ex)
            {
                DebugHud.Log(ex);
            }
        }

        private void Add_Error(string format, params object[] args)
        {
            string str = DebugHud.Format_Log(format, 1, args);
            this.errors.Add(str);
            DebugHud.Log("[<b>{0}</b>] {1}", this.dll_name, str);
        }

        private void Add_Error(Exception ex)
        {
            string str = DebugHud.Format_Log(ex.Message, 1);
            this.errors.Add(str);
            DebugHud.Log("[<b>{0}</b>] {1}", this.dll_name, str);
        }

        private bool Load_DLL()
        {
            try
            {
                //DebugHud.Log("Load_Assembly()");
                this.dll = this.load_assembly(this.dll != null);
                //DebugHud.Log("Searching assembly...");

                //find the static SR_Plugin class amongst however many namespaces this library has.
                foreach (Type ty in dll.GetExportedTypes())
                {
                    if (ty.Name == "SR_Plugin")
                    {
                        if (ty.IsPublic == false) continue;
                        this.pluginClass = ty;
                        break;
                    }
                }

                if (this.pluginClass == null)
                {
                    this.Add_Error("Unable to locate a static 'SR_Plugin' class in the loaded library.");
                    return false;
                }
                //DebugHud.Log("Found plugin class.");


                this.load_funct = this.pluginClass.GetMethod("Load", BindingFlags.Static | BindingFlags.Public);
                this.unload_funct = this.pluginClass.GetMethod("Unload", BindingFlags.Static | BindingFlags.Public);

                if (this.load_funct == null) this.Add_Error("Unable to find Load function!");
                if (this.unload_funct == null) this.Add_Error("Unable to find Unload function!");



                if (this.load_funct == null || this.unload_funct == null)
                {
                    DebugHud.Log("Unable to locate load/unload functions.");
                    return false;
                }

                //DebugHud.Log("[{0}] Plugin loaded!", this.dll_name);
                return true;
            }
            catch(Exception ex)
            {
                DebugHud.Log(ex);
                return false;
            }
        }

        private Assembly load_assembly(bool reload=false)
        {
            try
            {
                if (reload == true)
                {
                    Assembly asm = Assembly.LoadFile(this.file);//  https://msdn.microsoft.com/en-us/library/b61s44e8(v=vs.110).aspx
                    return asm;
                }
                else
                {
                    //DebugHud.LogSilent("Loading: {0}", this.dll_name);
                    string pdb_file = String.Format("{0}\\{1}.pdb", this.dir, Path.GetFileNameWithoutExtension(this.file));

                    byte[] dll_buf = this.Load_Bytes(this.file);
                    byte[] pdb_buf = null;
                    if(File.Exists(pdb_file)) this.Load_Bytes(pdb_file);

                    if (dll_buf == null) return null;

                    Assembly asm;
                    if (pdb_buf != null) asm = Assembly.Load(dll_buf, pdb_buf);
                    else asm = Assembly.Load(dll_buf);

                    //DebugHud.LogSilent("LOADED: {0}", this.dll_name);
                    return asm;
                }
            }
            catch(Exception ex)
            {
                this.Add_Error(ex);
            }

            return null;
        }

        private byte[] Load_Bytes(string file)
        {
            try
            {
                using (FileStream stream = new FileStream(file, FileMode.Open))
                {
                    byte[] buf = new byte[stream.Length];
                    int read = stream.Read(buf, 0, (int)stream.Length);
                    if (read < (int)stream.Length)
                    {
                        int remain = ((int)stream.Length - read);
                        int r = 0;
                        while (r < remain && remain > 0)
                        {
                            r = stream.Read(buf, read, remain);
                            read += r;
                            remain -= r;
                        }
                    }

                    return buf;
                }
            }
            catch (Exception ex)
            {
                this.Add_Error(ex);
            }

            return null;
        }

        public void Process_Dependencys()
        {
            if (this.data.DEPENDENCYS.Count <= 0) return;
            this.unmet_dependencys.Clear();
            Dictionary<Plugin_Dependency, PLUGIN_DEP_COMPARISON_FLAG> met_depends = new Dictionary<Plugin_Dependency, PLUGIN_DEP_COMPARISON_FLAG>();
            foreach(var dep in data.DEPENDENCYS)
            {
                met_depends[dep] = PLUGIN_DEP_COMPARISON_FLAG.FALSE;
            }

            foreach(KeyValuePair<string, Plugin> kv in Loader.plugins)
            {
                if (kv.Value == this) continue;
                foreach (Plugin_Dependency dep in data.DEPENDENCYS)
                {
                    var cmp_res = dep.Compare(kv.Value);
                    if (cmp_res != PLUGIN_DEP_COMPARISON_FLAG.FALSE)
                    {
                        met_depends[dep] = cmp_res;
                        break;//go to next plugin
                    }
                }
            }

            foreach(KeyValuePair<Plugin_Dependency, PLUGIN_DEP_COMPARISON_FLAG> kv in met_depends)
            {
                if (kv.Value != PLUGIN_DEP_COMPARISON_FLAG.SAME) unmet_dependencys.Add(kv.Key);
            }
        }

        public void Enable()
        {
            if(this.has_dependency_issues == true)
            {
                this.enabled = false;
                return;
            }

            this.enabled = true;
            try
            {
                if (this.load_funct != null)
                {
                    this.load_funct.Invoke(null, null);
                    Loader.Plugin_Status_Change(this, this.enabled);
                }
            }
            catch(Exception ex)
            {
                this.Add_Error(ex);
            }
        }

        public void Disable()
        {
            this.enabled = false;
            try
            {
                if (this.unload_funct != null)
                {
                    this.unload_funct.Invoke(null, null);
                }
            }
            catch (Exception ex)
            {
                this.Add_Error(ex);
            }
        }

        public void Toggle()
        {
            if (this.enabled == true)
            {
                this.Disable();
            }
            else
            {
                this.Enable();
            }
        }

        
        public byte[] Load_Resource(string name)
        {
            try
            {
                string[] assets = this.dll.GetManifestResourceNames();
                if (assets == null || assets.Length <= 0) return null;
                
                string asset_name = assets.FirstOrDefault(r => r.EndsWith(name));
                if (asset_name == null || asset_name.Length <= 0) return null;


                using (Stream stream = this.dll.GetManifestResourceStream(asset_name))
                {
                    if (stream == null) return null;

                    byte[] buf = new byte[stream.Length];
                    int read = stream.Read(buf, 0, (int)stream.Length);
                    if (read < (int)stream.Length)
                    {
                        int remain = ((int)stream.Length - read);
                        int r = 0;
                        while (r < remain && remain > 0)
                        {
                            r = stream.Read(buf, read, remain);
                            read += r;
                            remain -= r;
                        }
                    }

                    return buf;
                }
            }
            catch (Exception ex)
            {
                DebugHud.Log(ex);
                return null;
            }
        }
    }
}
