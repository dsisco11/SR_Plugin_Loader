using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SimpleJSON;

namespace SR_PluginLoader
{
    public class Plugin_Download_Data
    {
        public string Name;
        public string Author;
        public string Description;
        public string URL;
        public Texture2D Icon = Loader.tex_unknown;
        public Updater_Base Updater = null;
        public string Hash { get { return Utility.SHA(String.Format("{0}.{1}", Author, Name)); } }
        public string Title { get { return String.Format("{0}.{1}", Author, Name); } }
        public string Filename { get { string name = String.Format("{0}.{1}.dll", Author, Name); foreach (char c in System.IO.Path.GetInvalidFileNameChars()) { name = name.Replace(c.ToString(), String.Empty); } return name; } }
        public bool isInstalled { get { return Loader.Is_Plugin_Installed(Hash); } }



        public Plugin_Download_Data(JSONNode info)
        {
            this.Author = info["author"];
            this.Name = info["name"];
            this.Description = info["description"];
            this.URL = info["url"];
            this.Updater = Updater_Base.Get((UPDATER_TYPE)Enum.Parse(typeof(UPDATER_TYPE), info["update_method"], true));
        }
    }
}
