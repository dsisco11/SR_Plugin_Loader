using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SR_PluginLoader
{
    /// <summary>
    /// Manages saving & loading a sequence of config key/value pairs to a file.
    /// </summary>
    public class SettingsFile
    {
        private string FILE = null;
        private JSONClass json = null;
        /// <summary>
        /// if TRUE then all values will be resaved to disk on the next update pass.
        /// </summary>
        private bool dirty = false;

        /// <summary>
        /// Pleas enote that with the <see cref="SimpleJSON"/> system key-value associations are actually instances of <see cref="JSONClass"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public JSONNode this[string key] { get { return json[key]; } set { json[key] = value; set_dirty(); } }



        public SettingsFile(string filename, string dir=null)
        {
            if (dir == null) dir = UnityEngine.Application.dataPath;
            FILE = Path.GetFullPath(Path.Combine(dir, filename));
            Load();
        }

        #region Saving & Loading

        /// <summary>
        /// Load the specified settings file, please note that the file is already loaded by the constructor so this function really RELOADS the file.
        /// </summary>
        public void Load()
        {
            json = new JSONClass();// Go ahead and create an empty instance just incase we can't actually load from the specified file.
            if (!File.Exists(FILE))
            {//it's ok we will create it!
                var fs = File.CreateText(FILE);
                fs.Close();
                Save();
                return;
            }

            string str = File.ReadAllText(FILE);
            if (str == null || str.Length <= 0) return;
            
            json = (JSONClass)JSON.Parse(str);
        }

        public void Save()
        {
            dirty = false;
            File.WriteAllText(FILE, json.ToString(), Encoding.UTF8);
        }

        private void set_dirty()
        {
            dirty = true;
        }
        #endregion

        #region Setters

        public void Set_Bool(string key, bool v)
        {
            JSONData dat = new JSONData(v);
            if (json[key] == null) json.Add(key, dat);
            else json[key] = dat;

            set_dirty();
        }

        public void Set_Int(string key, int v)
        {
            JSONData dat = new JSONData(v);
            if (json[key] == null) json.Add(key, dat);
            else json[key] = dat;

            set_dirty();
        }

        public void Set_Float(string key, float v)
        {
            JSONData dat = new JSONData(v);
            if (json[key] == null) json.Add(key, dat);
            else json[key] = dat;

            set_dirty();
        }

        public void Set_Double(string key, double v)
        {
            JSONData dat = new JSONData(v);
            if (json[key] == null) json.Add(key, dat);
            else json[key] = dat;

            set_dirty();
        }

        public void Set_String(string key, string v)
        {
            JSONData dat = new JSONData(v);
            if (json[key] == null) json.Add(key, dat);
            else json[key] = dat;

            set_dirty();
        }
        #endregion

        #region Getters

        public bool Get_Bool(string key) { return json[key].AsBool; }

        public int Get_Int(string key) { return json[key].AsInt; }

        public long Get_Long(string key)
        {
            long v = 0;
            if (long.TryParse(json[key].Value, out v)) return v;
            return 0;
        }

        public ulong Get_ULong(string key)
        {
            ulong v = 0;
            if (ulong.TryParse(json[key].Value, out v)) return v;
            return 0;
        }

        public float Get_Float(string key) { return json[key].AsFloat; }

        public double Get_Double(string key) { return json[key].AsDouble; }

        public string Get_String(string key) { return json[key].Value; }
        #endregion
    }
}
