using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    public class Plugin_StoreItem : uiPanel
    {
        public static float DEFAULT_WIDTH = 180f;
        public static float DEFAULT_HEIGHT = 42f;
        private string _plugin_hash = null;
        public string plugin_hash { get { return _plugin_hash; } }
        public uiProgressBar progress_bar = null;


        public Plugin_StoreItem() : base(uiControlType.Panel)
        {
            _typename = "Plugin_ListItem";
            padding = new RectOffset(4, 2, 2, 2);
            margin = new RectOffset(2, 2, 2, 0);
            Set_Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
            
            Util.Set_BG_Color(local_style.normal, new Color32(32, 32, 32, 200));
            Util.Set_BG_Color(local_style.hover, new Color32(64, 64, 64, 255));
            //Utility.Set_BG_Color(local_style.hover, new Color32(32, 40, 60, 255));


            uiText name = uiControl.Create<uiText>();
            GUIStyle nm_style = new GUIStyle();
            nm_style.fontStyle = FontStyle.Bold;
            nm_style.fontSize = 14;
            nm_style.normal.textColor = Color.white;

            name.Set_Style(nm_style);
            this.Add("name", name);
            
            uiText auth = uiControl.Create<uiText>();
            GUIStyle au_style = new GUIStyle();
            au_style.fontStyle = FontStyle.Normal;
            au_style.fontSize = 12;
            au_style.normal.textColor = Color.white;

            auth.Set_Style(au_style);
            this.Add("author", auth);

            uiProgressBar prog = Create<uiProgressBar>();
            Util.Set_BG_Color(prog.bar_style.normal, new Color(0.1f, 0.5f, 1.0f));
            prog.show_progress_text = false;
            this.Add("progress", prog);
            this.progress_bar = prog;
        }

        public void Set_Plugin_Data(Plugin_Data data)
        {
            _plugin_hash = data.Hash;
            ((uiText)this["name"]).text = data.NAME;
            ((uiText)this["author"]).text = data.AUTHOR;
            this.progress_bar.progress = 0f;
        }

        public override void doLayout()
        {
            uiControl name = this["name"];
            uiControl auth = this["author"];
            uiControl prog = this["progress"];

            name.area = new Rect(0f, 0f, area.width, name.style.lineHeight);
            auth.area = new Rect(10f, name.area.yMax, area.width, name.style.lineHeight);

            prog.moveBelow(auth, 0f);
            prog.Set_Size(area.width, 4f);
        }
    }
}
