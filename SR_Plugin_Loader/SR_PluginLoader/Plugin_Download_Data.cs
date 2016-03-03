using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    public class Plugin_Download_Data
    {
        public string Name;
        public string Author;
        public string Description;
        public string DownloadURL;
        public Texture2D Icon = Loader.tex_unknown;
        public string Hash { get { return Utility.SHA(String.Format("{0}.{1}", Author, Name)); } }
        public bool IsInstalled { get { return (Loader.GetPluginByHash(Hash) != null); } }

        public Plugin_Download_Data(string author, string name, string description, string downloadurl)
        {
            this.Author = author;
            this.Name = name;
            this.Description = description;
            this.DownloadURL = downloadurl;
        }
    }
}
