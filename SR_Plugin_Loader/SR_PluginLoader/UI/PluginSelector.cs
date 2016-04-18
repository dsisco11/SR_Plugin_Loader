using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace SR_PluginLoader
{
    public class PluginSelector : uiPanel
    {
        public string Hash { get { if (this.plugin == null) { return null; } return this.plugin.Hash; } }
        private Plugin plugin = null;
        private uiText pl_title = null, pl_version = null, pl_status = null;
        private uiIcon pl_icon = null;

        private bool plugin_state_init = false;
        private bool last_plugin_en_state = false;
        

        public PluginSelector()
        {
            this.Set_Size(200f, 50f);
            this.margin = new RectOffset(5, 5, 2, 0);
            this.padding = new RectOffset(2, 2, 2, 2);
            this.border.normal = new uiBorderStyleState() { color = new Color32(40, 40, 40, 255), size = new RectOffset(1, 1, 1, 1) };
            this.border.hover = new uiBorderStyleState() { color = new Color32(64, 64, 64, 255) };

            float shade = 0.2f;
            //set normal styles bg color
            //local_style.normal.background = new Texture2D(1, 1);
            //local_style.normal.background.SetPixel(0, 0, new Color(shade, shade, shade, 1f));
            //local_style.normal.background.Apply();
            
            Util.Set_BG_Color(this.local_style.normal, shade, shade, shade, 0.9f);
            local_style.active.background = Util.Get_Gradient_Texture(200, GRADIENT_DIR.TOP_BOTTOM, new Color(shade, shade, shade), new Color32(25, 99, 141, 255));

            shade += 0.3f;
            Util.Set_BG_Color(local_style.hover, shade, shade, shade, 0.5f);


            pl_icon = Create<uiIcon>();
            this.Add("icon", pl_icon);

            pl_title = Create<uiText>();
            GUIStyle tSty = new GUIStyle();
            tSty.normal.textColor = new Color(1f, 1f, 1f);
            tSty.fontStyle = FontStyle.Bold;
            tSty.fontSize = 16;
            pl_title.Set_Style(tSty);
            this.Add("title", pl_title);

            pl_version = Create<uiText>();
            GUIStyle vSty = new GUIStyle();
            vSty.normal.textColor = new Color(0.7f, 0.7f, 0.7f, 0.9f);
            vSty.fontStyle = FontStyle.Italic;
            vSty.fontSize = 12;
            pl_version.Set_Style(vSty);
            this.Add("version", pl_version);

            pl_status = Create<uiText>();
            Util.Set_BG_Color(pl_status.local_style.normal, 0f, 0f, 0f, 0.3f);
            pl_status.selfPadding = new RectOffset(2, 2, 2, 2);
            pl_status.local_style.fontStyle = FontStyle.Bold;
            pl_status.local_style.fontSize = 12;
            pl_status.local_style.alignment = TextAnchor.MiddleCenter;
            //pl_status.local_style.normal.textColor = Color.red;// plugin is disabled
            //pl_status.local_style.active.textColor = Color.green;// plugin is enabled
            this.Add("status", pl_status);
        }

        private void Update()
        {
            if (plugin == null) return;
            if (plugin.enabled != last_plugin_en_state || !plugin_state_init)
            {
                plugin_state_init = true;
                last_plugin_en_state = plugin.enabled;
                Plugin_State_Changed();
            }
        }

        private void Plugin_State_Changed()
        {
            this.pl_status.active = plugin.enabled;
            this.pl_status.text = (plugin.enabled ? "Enabled" : "Disabled");
            this.pl_status.local_style.normal.textColor = (plugin.enabled ? Color.green : Color.red);
        }

        public override void doLayout()
        {
            float icon_sz = this.inner_area.height;
            pl_icon.Set_Pos(0, 0);
            pl_icon.Set_Size(icon_sz, icon_sz);

            const float xPad = 4f;
            pl_title.moveRightOf(pl_icon, xPad);

            pl_version.moveRightOf(pl_icon, xPad);
            pl_version.moveBelow(pl_title);

            pl_status.alignBottom();
            pl_status.alignRightSide();
        }

        public Plugin Get_Plugin()
        {
            return this.plugin;
        }

        public void Set_Plugin(Plugin p)
        {
            if (p == null)
            {
                DebugHud.Log(new Exception("Plugin is null!"));
                return;
            }

            try
            {
                this.plugin = p;
                if (plugin.data == null)
                {
                    DebugHud.Log(new Exception("Plugin data is NULL!"));
                    return;
                }

                this.pl_title.text = plugin.data.NAME;
                this.pl_version.text = plugin.data.VERSION.ToString();

                if (plugin.icon != null) this.pl_icon.image = plugin.icon;
                else if (Loader.tex_unknown != null) this.pl_icon.image = Loader.tex_unknown;
                else this.pl_icon.image = null;

                this.plugin_state_init = false;
            }
            catch (Exception ex)
            {
                DebugHud.Log(ex);
            }
        }

    }
}
