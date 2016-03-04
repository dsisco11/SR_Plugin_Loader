using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace SR_PluginLoader
{
    public class PluginSelector : MonoBehaviour
    {
        private static GUIStyle style = null;
        private static GUIStyle border_style = null;
        private static GUIStyle status_style = null;
        private static GUIStyle txt_style = null;
        private static GUIStyle txt_style_title = null;
        private static GUIStyle txt_style_vers = null;

        private Plugin plugin = null;
        private bool active = false;
        private bool needs_layout = true;
        public static float DEFAULT_HEIGHT = 50f;
        private Vector2 size = new Vector2(190f, DEFAULT_HEIGHT);

        private Vector2 pos = Vector2.zero;
        private Rect _position, plugin_title_pos, plugin_status_pos, plugin_version_pos;
        public Rect position { get { return this._position; } }

        private GUIContent plugin_title = new GUIContent();
        private GUIContent plugin_status = new GUIContent();
        private GUIContent plugin_version = new GUIContent();
        private GUIContent plugin_icon = new GUIContent();
        

        /// <summary>
        /// Is the mouse clicking this control?
        /// </summary>
        private bool isDepressed = false;

        /// <summary>
        /// Is the mouse hovering over this control?
        /// </summary>
        private bool isHovering = false;

        public void Set_Pos(float x, float y)
        {
            this.pos.x = x;
            this.pos.y = y;
            this.needs_layout = true;
        }

        public void Set_Width(float f)
        {
            this.size.x = f;
            this.needs_layout = true;
        }

        public void Set_Height(float f)
        {
            this.size.y = f;
            this.needs_layout = true;
        }
        
        private GUIContent CreateText(string str)
        {
            //DebugHud.Log("TryCreateContent: {0}", obj.ToString());
            var txt = new GUIContent(str);

            return txt;
        }

        public void Set_Active(bool b)
        {
            this.active = b;
        }

        public Plugin Get_Plugin()
        {
            return this.plugin;
        }

        public void Set_Plugin(Plugin p)
        {
            if(p == null)
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

                this.plugin_title.text = plugin.data.NAME;
                this.plugin_version.text = plugin.data.VERSION.ToString();

                if (plugin.icon != null) this.plugin_icon = new GUIContent(plugin.icon);
                else if (Loader.tex_unknown) this.plugin_icon = new GUIContent(Loader.tex_unknown);
                else this.plugin_icon = GUIContent.none;
            }
            catch(Exception ex)
            {
                DebugHud.Log(ex);
            }
        }
        
        private void Init_Style()
        {
            PluginSelector.style = new GUIStyle();
            int border_sz = 2;
            style.border = new RectOffset(border_sz, border_sz, border_sz, border_sz);

            PluginSelector.border_style = new GUIStyle();
            Utility.Set_BG_Color(PluginSelector.border_style.normal, new Color32(0, 0, 0, 255));
            //Utility.Set_BG_Color(PluginSelector.border_style.hover, new Color32(128, 128, 128, 255));
            //Utility.Set_BG_Color(PluginSelector.border_style.active, new Color32(200, 200, 200, 255));


            PluginSelector.status_style = new GUIStyle();
            Color bg_status_fade = new Color32(32, 32, 32, 100);
            Utility.Set_BG_Color(PluginSelector.status_style.normal, bg_status_fade);
            PluginSelector.status_style.normal.textColor = new Color32(220, 32, 32, 164);
            Utility.Set_BG_Color(PluginSelector.status_style.active, bg_status_fade);
            PluginSelector.status_style.active.textColor = new Color32(32, 170, 32, 180);
            PluginSelector.status_style.fontStyle = FontStyle.Bold;
            PluginSelector.status_style.fontSize = 12;
            PluginSelector.status_style.alignment = TextAnchor.MiddleCenter;
            
            PluginSelector.txt_style = new GUIStyle();
            PluginSelector.txt_style.normal.textColor = new Color(1f, 1f, 1f);
            PluginSelector.txt_style.fontStyle = FontStyle.Italic;

            PluginSelector.txt_style_title = new GUIStyle();
            PluginSelector.txt_style_title.normal.textColor = new Color(1f, 1f, 1f);
            PluginSelector.txt_style_title.fontStyle = FontStyle.Bold;
            PluginSelector.txt_style_title.fontSize = 16;

            PluginSelector.txt_style_vers = new GUIStyle();
            PluginSelector.txt_style_vers.normal.textColor = new Color(0.7f, 0.7f, 0.7f, 0.9f);
            PluginSelector.txt_style_vers.fontStyle = FontStyle.Italic;
            PluginSelector.txt_style_vers.fontSize = 12;


            float shade = 0.2f;
            //set normal styles bg color
            Utility.Set_BG_Color(PluginSelector.style.normal, shade, shade, shade, 0.9f);
            PluginSelector.style.active.background = Utility.Get_Gradient_Texture(200, GRADIENT_DIR.TOP_BOTTOM, new Color(shade, shade, shade), new Color32(25, 99, 141, 255));

            shade += 0.3f;
            Utility.Set_BG_Color(style.hover, shade, shade, shade, 0.5f);
        }

        private void OnGUI()
        {
        }

        public bool Display()
        {
            //if (plugin == null && this.plugin != null) plugin = this.plugin;
            if (style == null) this.Init_Style();
            if(this.needs_layout) this.doLayout();

            
            int id = GUIUtility.GetControlID(0, FocusType.Passive, _position);
            var evt = Event.current.GetTypeForControl(id);
            this.isHovering = this._position.Contains(Event.current.mousePosition);

            switch (evt)
            {
                case EventType.MouseDown:
                    if (this.isHovering)
                    {
                        GUIUtility.hotControl = id;
                        Event.current.Use();
                        this.isDepressed = true;
                    }
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == id)
                    {
                        GUIUtility.hotControl = 0;
                        Event.current.Use();
                        this.isDepressed = false;
                        if (this.isHovering) return true;
                    }
                    return false;
                case EventType.Repaint:
                    bool focus = (GUIUtility.hotControl == id);
                    bool isActive = (this.isDepressed || this.active);

                    //PluginSelector.border_style.Draw(_position, GUIContent.none, this.isHovering, false, false, false);
                    PluginSelector.style.Draw(_position, this.plugin_icon, this.isHovering || isActive, isActive, false, focus);
                    GUI.BeginGroup(this._position);
                        PluginSelector.txt_style_title.Draw(this.plugin_title_pos, this.plugin_title, this.isHovering, this.isDepressed, true, focus);
                        PluginSelector.txt_style_vers.Draw(this.plugin_version_pos, this.plugin_version, this.isHovering, this.isDepressed, true, focus);
                    GUI.EndGroup();

                    this.Draw_Plugin_Status();
                    break;
            }

            return false;
        }

        private void Draw_Plugin_Status()
        {
            const float status_inner_pad_x = 3f;
            const float status_inner_pad_y = 1f;
            const float status_outter_pad = 2f;
            bool plugin_en = this.plugin.enabled;
            var status_text = new GUIContent(plugin_en ? "Enabled" : "Disabled");
            var sz = PluginSelector.status_style.CalcSize(status_text);
            sz.x += (status_inner_pad_x*2f);
            sz.y += (status_inner_pad_y*2f);

            //plugin status is relative, like the title and version texts
            this.plugin_status_pos = new Rect(this._position.xMax - (sz.x + status_outter_pad), this._position.yMax - (sz.y + status_outter_pad), sz.x, sz.y);

            PluginSelector.status_style.Draw(this.plugin_status_pos, status_text, plugin_en, plugin_en, false, false);
        }

        private void doLayout()
        {
            this.needs_layout = false;

            float width = this.size.x;
            float height = this.size.y;

            float content_width = (width - height - 5f);
            float padding_y = 0f;
            float padding_y2 = (padding_y * 2f);

            float padding_x = 3f;
            float padding_x2 = (padding_x * 2f);

            float inner_width = (width - padding_y2);
            float inner_height = (height - padding_y2);

            float status_width = 60f;
            float status_height = 20f;
            float status_pad = 5f;

            _position = new Rect(pos.x + padding_x, pos.y + padding_y, width - padding_x2, height - padding_y2);

            plugin_title_pos = new Rect(55f, 10f, content_width, 15f);
            plugin_status_pos = new Rect(width - (status_width + status_pad), height - (status_height + status_pad), status_width, status_height);
            plugin_version_pos = new Rect(55f, 28f, content_width, 15f);
        }
    }
}
