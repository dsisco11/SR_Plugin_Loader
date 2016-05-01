﻿using System;
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
        private JSONNode json;
        public string Name;
        public string Author;
        public string Description;
        public string URL;
        public Texture2D Icon = Loader.tex_unknown;
        public Updater_Base Updater = null;
        public string Hash { get { return Util.SHA(String.Format("{0}.{1}", Author, Name)); } }
        public string Title { get { return String.Format("{0}.{1}", Author, Name); } }
        public string Filename { get { string name = String.Format("{0}.{1}.dll", Author, Name); foreach (char c in System.IO.Path.GetInvalidFileNameChars()) { name = name.Replace(c.ToString(), String.Empty); } return name; } }
        public bool isInstalled { get { return Loader.Is_Plugin_Installed(Hash); } }



        public Plugin_Download_Data(JSONNode info)
        {
            json = info;
            
            this.Author = TryGet("author");
            this.Name = TryGet("name");
            this.Description = TryGet("description");
            this.URL = TryGet("url");

            string method = TryGet("update_method");
            if(method != null) this.Updater = Updater_Base.Get_Instance((UPDATER_TYPE)Enum.Parse(typeof(UPDATER_TYPE), method, true));
        }

        string TryGet(string key)
        {
            if (json[key] == null)
            {
                DebugHud.Log(new ArgumentNullException(String.Format("\"{0}\" is null!", key)));
                return null;
            }

            return json[key];
        }
    }
}