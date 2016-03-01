using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SR_PluginLoader
{
    public class PluginsPanel : MonoBehaviour
    {
        public Vector2 size = new Vector2(650f, 400f);
        private Vector2 scroll = Vector2.zero;
        private GUISkin skin = null;
        private GUIStyle title_style = null, screen_darkener = null, shadow_style = null, highlight_style = null, plugin_list_style = null, scrollbar_style = null;
        private GUIStyle plugin_title_text = null, plugin_vers_text = null, plugin_desc_text = null;
        private Plugin selected = null;
        private List<PluginSelector> plugin_selectors = new List<PluginSelector>();
        private GUIContent pl_title = new GUIContent(), pl_desc = new GUIContent(), pl_vers = new GUIContent();
        private Texture pl_thumb = null;
        private Rect screen_area, area, window_area, tb_area, tb_shadow_area, close_btn_area, left_shadow_area, right_shadow_area, bottom_shadow_area;
        private Rect pl_title_area, pl_desc_area, pl_vers_area, pl_thumb_area, pl_toggle_area;
        private Vector2 selected_plugin_info_scroll = Vector2.zero, plugin_list_scroll = Vector2.zero;
        private Rect selected_plugin_info_area, selected_plugin_info_inner_area;
        private Rect plugin_list_area, plugin_list_inner_area;
        private float tb_height { get { return (title_bar_height - 4f); } }
        private ToggleSwitch pl_toggle = new ToggleSwitch();
        private bool needs_layout = true;


        private Vector2 _pos = Vector2.zero;
        private Vector2 pos { get { if(_pos == Vector2.zero) _pos = new Vector2((float)((Screen.width * 0.5f) - (this.size.x * 0.5f)), (float)((Screen.height * 0.5f) - (this.size.x * 0.3f))); return _pos; } }


        private const float scrollbar_width = 8f;
        private const float plugin_list_width = 220f;
        private const float list_pad = 5f;
        private const float list_pad2 = (list_pad*2f);
        private const float title_bar_height = 28f;
        private const float thumbnail_size = 150f;




        private void Awake()
        {
            //this.pl_toggle = base.gameObject.AddComponent<ToggleSwitch>();
            this.pl_toggle.SetText("Disable", "Enable");
        }

        private void init_skin()
        {
            //DebugHud.Log("Initializing Plugins panel");
            this.skin = ScriptableObject.CreateInstance<GUISkin>();
            this.skin.button = GUI.skin.button;
            this.skin.button.normal.textColor = Color.white;
            this.skin.button.fontStyle = FontStyle.Bold;

            skin.verticalScrollbar = new GUIStyle();
            skin.verticalScrollbar.fixedWidth = scrollbar_width;
            Utility.Set_BG_Color(skin.verticalScrollbar.normal, new Color32(16, 16, 16, 200));
            
            skin.verticalScrollbarThumb = new GUIStyle();
            skin.verticalScrollbarThumb.fixedWidth = scrollbar_width;
            Utility.Set_BG_Color(skin.verticalScrollbarThumb.normal, new Color32(80, 80, 80, 255));


            float grey = 0.1f;
            Utility.Set_BG_Color(this.skin.box.normal, grey, grey, grey, 0.8f);

            this.title_style = new GUIStyle();
            this.title_style.normal.background = Utility.Get_Gradient_Texture(32, GRADIENT_DIR.TOP_BOTTOM, 0.3f, 0.1f);
            this.title_style.normal.textColor = new Color(0.9f, 0.9f, 0.9f);
            this.title_style.alignment = TextAnchor.MiddleCenter;
            this.title_style.fontStyle = FontStyle.Bold;
            this.title_style.fontSize = 15;

            this.screen_darkener = new GUIStyle();
            Utility.Set_BG_Color(this.screen_darkener.normal, 0f, 0f, 0f, 0.7f);

            this.shadow_style = new GUIStyle();
            Utility.Set_BG_Color(this.shadow_style.normal, 0f, 0f, 0f, 0.6f);

            this.highlight_style = new GUIStyle();
            Utility.Set_BG_Color(this.highlight_style.normal, 1f, 1f, 1f, 0.2f);
            
            this.plugin_list_style = new GUIStyle();
            this.plugin_list_style.normal.background = Utility.Get_Gradient_Texture((int)PluginsPanel.plugin_list_width, GRADIENT_DIR.LEFT_RIGHT, 0.0f, 0.15f);

            //var plugin_title_text = new GUIStyle();
            plugin_title_text = new GUIStyle();
            plugin_title_text.wordWrap = true;
            plugin_title_text.fontSize = 24;
            plugin_title_text.fontStyle = FontStyle.Bold;
            plugin_title_text.normal.textColor = Color.white;


            plugin_vers_text = new GUIStyle();
            plugin_vers_text.wordWrap = true;
            plugin_vers_text.fontSize = 12;
            plugin_vers_text.fontStyle = FontStyle.Bold;
            plugin_vers_text.normal.textColor = new Color(0.7f, 0.7f, 0.7f, 0.8f);


            plugin_desc_text = new GUIStyle();
            plugin_desc_text.wordWrap = true;
            plugin_desc_text.fontSize = 14;
            plugin_desc_text.fontStyle = FontStyle.Normal;
            plugin_desc_text.normal.textColor = new Color32(200, 200, 200, 255);
            plugin_desc_text.richText = true;
            
        }

        private void close()
        {
            this.gameObject.SetActive(false);
            MainMenu.mainmenu.SetActive(true);
        }

        private void Update()
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                this.close();
            }
        }

        private bool handleEvents()
        {

            int id = GUIUtility.GetControlID(0, FocusType.Passive, area);
            var evt = Event.current.GetTypeForControl(id);

            switch(evt)
            {
                case EventType.Layout:
                    this.doLayout();
                    return false;
                case EventType.Ignore:
                case EventType.Used:
                    return false;
            }
            
            return true;
        }

        private void OnGUI()
        {
            if (this.skin == null)
            {
                this.init_skin();
                this.Update_Plugins_UI();
            }
            if (this.needs_layout) this.doLayout();
            if (!this.gameObject.activeSelf) return;
            this._pos = Vector2.zero;//recalculate every frame, idk it just makes me feel safe.

            if (!handleEvents()) return;
            

            var prev_depth = GUI.depth;
            GUI.depth = 1;
            GUISkin prev_skin = GUI.skin;
            GUI.skin = this.skin;

            //slightly darken fill the screen behind the plugin menu
            GUI.Box(this.screen_area, GUIContent.none, this.screen_darkener);

            GUI.BeginGroup(this.area);
            //draw the main window
            this.render_window();
            this.render_close_button();
            this.render_titlebar();
            GUI.EndGroup();

            plugin_list_style.Draw(plugin_list_area, GUIContent.none, false, false, false, false);
            plugin_list_scroll = GUI.BeginScrollView(plugin_list_area, plugin_list_scroll, plugin_list_inner_area, false, false);
            
            foreach (PluginSelector sel in this.plugin_selectors)
            {
                if( sel.Display() )
                {
                    this.select_plugin(sel.Get_Plugin());
                }
            }
            
            GUI.EndScrollView(true);

            this.render_selected_plugin_info();
            
            GUI.skin = prev_skin;
            GUI.depth = prev_depth;
        }

        private void render_selected_plugin_info()
        {
            if (this.selected == null || this.selected.data == null) return;

            this.selected_plugin_info_scroll = GUI.BeginScrollView(selected_plugin_info_area, this.selected_plugin_info_scroll, selected_plugin_info_inner_area, false, true);
            //DebugHud.Log("{0}", selected_plugin_info_scroll);

            plugin_title_text.Draw(this.pl_title_area, this.pl_title, false, false, false, false);
            plugin_vers_text.Draw(this.pl_vers_area, this.pl_vers, false, false, false, false);
            
            plugin_desc_text.Draw(this.pl_desc_area, this.pl_desc, false, false, false, false);
            if(this.pl_thumb != null) GUI.DrawTexture(this.pl_thumb_area, this.pl_thumb);
            

            if( this.pl_toggle.Display() )
            {
                this.selected.Toggle();
                this.pl_toggle.Toggle();
            }
            
            GUI.EndScrollView(true);
        }

        private void render_close_button()
        {
            if (GUI.Button(this.close_btn_area, "Close"))
            {
                this.close();
            }
        }

        private void render_titlebar()
        {
            //draw the title bar area
            this.title_style.Draw(tb_area, Loader.TITLE, false, false, false, false);
            //draw a small shadow beneath the title bar
            this.shadow_style.Draw(tb_shadow_area, GUIContent.none, false, false, false, false);
        }

        private void render_window()
        {
            //draw the window itself
            GUI.Box(this.window_area, GUIContent.none);

            //draw a small shadow on the left of the window
            GUI.Box(left_shadow_area, GUIContent.none, this.shadow_style);
            //draw a small shadow beneath the window
            GUI.Box(bottom_shadow_area, GUIContent.none, this.shadow_style);
            //draw a small shadow to the right of the window
            GUI.Box(right_shadow_area, GUIContent.none, this.shadow_style);
        }



        public void Update_Plugins_UI()
        {
            this.needs_layout = true;
            if (plugin_selectors.Count > 0)
            {
                foreach (PluginSelector sel in plugin_selectors)
                {
                    Destroy(sel);
                }

                plugin_selectors.Clear();
            }
            
            float dY = 3f;
            foreach (KeyValuePair<string, Plugin> kv in Loader.plugins)
            {
                try
                {
                    var sel = base.gameObject.AddComponent<PluginSelector>();
                    sel.Set_Plugin(kv.Value);

                    sel.Set_Pos(0f, dY);
                    dY += (PluginSelector.DEFAULT_HEIGHT + 2f);

                    this.plugin_selectors.Add(sel);
                }
                catch (Exception ex)
                {
                    DebugHud.Log(ex);
                }
            }

            if (Loader.plugins.Count > 0)
            {
                this.select_plugin(Loader.plugins.First().Value);
            }
        }
        
        private void doLayout()
        {
            this.needs_layout = false;

            this.screen_area = new Rect(0f, 0f, Screen.width, Screen.height);
            this.area = new Rect(pos.x-1f, pos.y-1f, this.size.x+2f, this.size.y+2f);//give our rendering area more space for borders
            this.window_area = new Rect(0f, 0f, this.size.x, this.size.y);

            this.tb_area = new Rect(0f, 0f, this.size.x, tb_height);
            this.tb_shadow_area = new Rect(tb_area.xMin, tb_area.yMax, tb_area.size.x, 1f);

            this.left_shadow_area = new Rect(window_area.x - 1f, window_area.y, 1f, window_area.size.y);
            this.right_shadow_area = new Rect(window_area.x + window_area.size.x, window_area.y, 1f, window_area.size.y);
            this.bottom_shadow_area = new Rect(window_area.x, window_area.y + window_area.size.y, window_area.size.x, 1f);

            this.plugin_list_area = new Rect(pos.x + list_pad, pos.y + title_bar_height, plugin_list_width - list_pad2, this.size.y - title_bar_height - list_pad);
            this.plugin_list_inner_area = new Rect(0f, 0f, plugin_list_area.width, this.plugin_selectors.Last().position.yMax);

            correctInnerScrollArea(ref plugin_list_area, ref plugin_list_inner_area);

            float close_w = 60f;
            float close_h = 25f;
            float close_pad = 3f;
            this.close_btn_area = new Rect((0f + this.size.x) - (close_w + 0f) - close_pad, (0f + this.size.y) - (close_h + 0f) - close_pad, close_w, close_h);


            float SBW = 0f;
            if ((PluginSelector.DEFAULT_HEIGHT * (float)Loader.plugins.Count) > plugin_list_area.height) SBW = scrollbar_width;

            foreach (var sel in this.plugin_selectors)
            {
                sel.Set_Width(plugin_list_inner_area.width - SBW);
            }

            if (this.selected != null)
            {
                this.select_plugin(this.selected);
            }
        }

        private void select_plugin(Plugin p)
        {
            //set ALL selectors to inactive first.
            foreach (var sel in this.plugin_selectors)
            {
                sel.Set_Active(false);
            }

            this.selected = p;
            if(p != null)
            {
                var sel = FindSelector(p);
                sel.Set_Active(true);
            }


            float thumb_aspect = 1f;
            float thumb_sz = 256f;

            if (p == null || p.data == null)
            {
                this.pl_title.text = "";
                this.pl_desc.text = "";
                this.pl_vers.text = "";
                this.pl_thumb = Loader.tex_unknown;
            }
            else
            {
                this.pl_title.text = p.data.NAME;
                this.pl_desc.text = p.data.DESCRIPTION;
                this.pl_vers.text = p.data.VERSION.ToString();
                this.pl_thumb = p.thumbnail;
                if (this.pl_thumb == null) thumb_sz = 0f;
                else thumb_aspect = ((float)this.pl_thumb.height / (float)this.pl_thumb.width);
            }


            float thumb_height = (thumb_sz * thumb_aspect);
            float width = (size.x - plugin_list_width - 2f);
            float content_width = (width - scrollbar_width - 5f);

            this.pl_thumb_area = new Rect(((content_width - thumb_sz) * 0.5f), 0f, thumb_sz, thumb_height);

            const float pl_toggle_width = 100f;
            this.pl_toggle_area = new Rect(((content_width - pl_toggle_width) * 0.5f), pl_thumb_area.yMax + 5f, pl_toggle_width, 20f);
            this.pl_toggle.rect = this.pl_toggle_area;
            this.pl_toggle.SetToggle(this.selected.enabled);


            this.pl_title_area = this.calcTextRect(list_pad, pl_toggle_area.yMax + 15f, this.pl_title, plugin_title_text);
            this.pl_vers_area = this.calcTextRect(pl_title_area.xMax+5f, pl_title_area.y, content_width, this.pl_vers, plugin_vers_text);
            this.pl_desc_area = this.calcTextRect(list_pad, this.pl_title_area.yMax + 10f, content_width, this.pl_desc, plugin_desc_text);


            this.selected_plugin_info_area = new Rect(area.xMin + plugin_list_width, area.yMin + title_bar_height, width, (this.close_btn_area.yMin - 3f) - (this.window_area.yMin + title_bar_height));
            this.selected_plugin_info_inner_area = new Rect(0, 0, this.selected_plugin_info_area.size.x, this.pl_desc_area.yMax);
            correctInnerScrollArea(ref selected_plugin_info_area, ref selected_plugin_info_inner_area);
        }




        private void correctInnerScrollArea(ref Rect outter, ref Rect inner)
        {
            inner.width = Math.Max(outter.width, inner.width);
            inner.height = Math.Max(outter.height, inner.height);
        }

        private bool hasScroll(Rect outter, Rect inner)
        {
            return (inner.height > outter.height);
        }
        
        private Rect calcTextRect(float x, float y, GUIContent content, GUIStyle style = null)
        {
            Vector2 sz = Vector2.zero;
            if (style != null) sz = style.CalcSize(content);

            return new Rect(x, y, sz.x, sz.y);
        }

        private Rect calcTextRect(float x, float y, float width, float height, GUIContent content, GUIStyle style = null)
        {
            if (style != null) height = style.CalcSize(content).y;

            return new Rect(x, y, width, height);
        }

        private Rect calcTextRect(float x, float y, float width, GUIContent content, GUIStyle style = null)
        {
            float height = 16f;
            //if (style != null) height = style.CalcSize(content).y;
            if (style != null) height = style.CalcHeight(content, width);

            //DebugHud.Log("calcTextRect: {0} => {1}", height, content.text);

            return new Rect(x, y, width, height);
        }

        private PluginSelector FindSelector(Plugin plugin)
        {
            foreach (var sel in this.plugin_selectors)
            {
                if (sel.Get_Plugin() == plugin)
                {
                    return sel;
                }
            }

            return null;
        }


    }
}
