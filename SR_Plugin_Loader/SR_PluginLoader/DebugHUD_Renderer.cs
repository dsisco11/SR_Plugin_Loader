using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SR_PluginLoader
{
    public class DebugHUD_Renderer : MonoBehaviour
    {
        private const float PANEL_WIDTH = 400f;
        private List<string> lines = new List<string>();
        private Dictionary<string, int> stacks = new Dictionary<string, int>();
        //private String lines_joined = "";
        private GUIContent console_lines = new GUIContent();
        private GUIContent alert_content = new GUIContent();
        private GUIContent alert_sub_content = new GUIContent();

        private Rect screen_area, console_text_area, console_inner_area, console_inner_text_area, console_scrollbar_area, fade_area;

        private const float alert_size = 32f;
        private const float alert_icon_offset = 20f;
        private Rect alertPos = new Rect(alert_icon_offset, alert_icon_offset, alert_size, alert_size);
        private Rect alert_txtPos = new Rect(0f, 0f, 0f, 0f);
        private Rect alert_sub_txtPos = new Rect(0f, 0f, 0f, 0f);
        private Vector2 alert_txtSz = new Vector2();
        private Vector2 alert_sub_txtSz = new Vector2();
        private Vector2 console_scroll = Vector2.zero;


        private static Texture2D bg_fade = null;
        private static GUIStyle blackout = new GUIStyle();
        private static GUIStyle text_style = new GUIStyle();
        private static GUIStyle subtext_style = new GUIStyle();
        private static GUIStyle console_text_style = new GUIStyle();
        

        private const float scrollbar_width = 6f;

        private static GUISkin skin = null;

        private bool open = false;
        private bool has_new = false;
        private int new_count = 0;
        private bool pressed = false, hover = false;
        private int id = 0;

        private KeyCode OPEN_KEY = KeyCode.Tab;


        public void Awake()
        {
            //this.setupEvents();
            this.Clear();
        }

        public void Clear()
        {
            this.lines.Clear();
            this.stacks.Clear();
        }

        public void Add_Line(string str)
        {
            this.has_new = true;
            this.new_count++;
            this.lines.Add(str);

            this.console_lines.text = String.Join("\n", this.lines.ToArray());
            string msg = String.Format("{0} new errors", this.new_count);
            alert_content.text = msg;
            alert_sub_content.text = String.Format("<i>Press <b>{0}</b> to open the plugins console.</i>", this.OPEN_KEY);

            alert_txtSz = DebugHUD_Renderer.text_style.CalcSize(alert_content);
            alert_txtPos = new Rect(alertPos.x + alertPos.width + 3f, alertPos.y + (alertPos.height * 0.5f) - (alert_txtSz.y * 0.5f), alert_txtSz.x, alert_txtSz.y);

            alert_sub_txtSz = DebugHUD_Renderer.subtext_style.CalcSize(alert_sub_content);
            alert_sub_txtPos = new Rect(alert_txtPos.xMin, alert_txtPos.yMax+3f, alert_sub_txtSz.x, alert_sub_txtSz.y);
        }

        public void Add_Tally(string str, int cnt=1)
        {
            int tmp = 0;
            if (!this.stacks.TryGetValue(str, out tmp)) this.stacks[str] = 0;

            this.stacks[str] += cnt;
        }

        private void OnLevelWasLoaded(int lvl)
        {
            //on second thought, it makes no sense to clear the debug log just because we loaded a new level...
            //this.Clear();
        }

        private void Init_BG_Fade()
        {
            DebugHUD_Renderer.bg_fade = Utility.Get_Gradient_Texture(400, GRADIENT_DIR.LEFT_RIGHT, new Color(0f,0f,0f,1f), new Color(0f,0f,0f,0f), true, 1.5f);
            Utility.Set_BG_Color(DebugHUD_Renderer.blackout.normal, 0.1f, 0.1f, 0.1f, 0.7f);

            DebugHUD_Renderer.text_style.normal.textColor = Color.white;
            DebugHUD_Renderer.text_style.active.textColor = Color.white;
            DebugHUD_Renderer.text_style.hover.textColor = Color.white;
            DebugHUD_Renderer.text_style.fontSize = 16;
            DebugHUD_Renderer.text_style.fontStyle = FontStyle.Bold;

            DebugHUD_Renderer.subtext_style = new GUIStyle(DebugHUD_Renderer.text_style);
            DebugHUD_Renderer.subtext_style.fontSize = 12;
            DebugHUD_Renderer.subtext_style.fontStyle = FontStyle.Normal;
            DebugHUD_Renderer.subtext_style.richText = true;

            DebugHUD_Renderer.skin = ScriptableObject.CreateInstance<GUISkin>();

            skin.verticalScrollbar = new GUIStyle();
            skin.verticalScrollbar.fixedWidth = scrollbar_width;
            Utility.Set_BG_Color(skin.verticalScrollbar.normal, new Color32(16, 16, 16, 200));

            skin.verticalScrollbarThumb = new GUIStyle();
            skin.verticalScrollbarThumb.fixedWidth = scrollbar_width;
            Utility.Set_BG_Color(skin.verticalScrollbarThumb.normal, new Color32(80, 80, 80, 255));


            DebugHUD_Renderer.console_text_style.normal.textColor = Color.white;
            DebugHUD_Renderer.console_text_style.normal.textColor = Color.white;
            DebugHUD_Renderer.console_text_style.active.textColor = Color.white;
            DebugHUD_Renderer.console_text_style.hover.textColor = Color.white;
            DebugHUD_Renderer.console_text_style.fontSize = 14;
            DebugHUD_Renderer.console_text_style.fontStyle = FontStyle.Normal;
            DebugHUD_Renderer.console_text_style.richText = true;
        }
        

        private void Update()
        {
            if(Input.GetKeyUp(KeyCode.BackQuote) || Input.GetKeyUp(KeyCode.Tab) || (Input.GetKeyUp(KeyCode.Escape) && this.open))
            {
                this.open = (!this.open);
                if (this.open)
                {
                    this.has_new = false;
                    this.new_count = 0;
                }
            }
        }

        private void setupEvents()
        {
            // Add event handling
            GameObject esObject = base.gameObject;
            EventSystem esClass = esObject.AddComponent<EventSystem>();
            esClass.sendNavigationEvents = true;
            esClass.pixelDragThreshold = 5;

            StandaloneInputModule stdInput = esObject.AddComponent<StandaloneInputModule>();
            stdInput.horizontalAxis = "Horizontal";
            stdInput.verticalAxis = "Vertical";

            //esObject.AddComponent<TouchInputModule>();
        }


        private void doLayout()
        {
            float offsetH = 50f;

            screen_area = new Rect(0f, 0f, Screen.width, Screen.height);
            fade_area = new Rect(0f, 0f, PANEL_WIDTH, Screen.height);
            console_text_area = new Rect(5f, offsetH, PANEL_WIDTH, (Screen.height - offsetH));

            float console_width = (PANEL_WIDTH + scrollbar_width);//lest I ever change the width and neglect to also change the below text height calculation's width.
            console_inner_area = new Rect(0f, 0f, console_width, console_text_style.CalcHeight(console_lines, console_width));
            console_inner_text_area = new Rect(console_inner_area.x+scrollbar_width+3f, console_inner_area.y, console_inner_area.width, console_inner_area.height);
            console_scrollbar_area = new Rect(0f, 0f, scrollbar_width, console_text_area.height);
        }
        /// <summary>
        /// Uses all of the system mouse movement, hover, and input events so we can prevent all controls under this one from getting them.
        /// Because this is a fullscreen overlay.
        /// </summary>
        private bool handleEvents()
        {
            id = GUIUtility.GetControlID(id, FocusType.Keyboard, screen_area);
            var evt = Event.current.GetTypeForControl(id);
            switch(evt)
            {
                case EventType.Layout:
                    this.doLayout();
                    return false;
                case EventType.Ignore:
                case EventType.Used:
                    return false;
                case EventType.Repaint:
                    break;
                case EventType.MouseMove:
                case EventType.MouseDown:
                case EventType.MouseUp:
                        GUIUtility.hotControl = id;
                        Event.current.Use();
                    break;
                default:
                        Event.current.Use();
                    break;
            }

            return true;
        }

        public void OnGUI()
        {
            if (bg_fade == null) this.Init_BG_Fade();
            if(Event.current.GetTypeForControl(id) == EventType.Layout)
            {
                this.doLayout();
                return;
            }

            if (!this.open)
            {
                if (this.new_count > 0)
                {
                    var clr = new Color(1f, 1f, 1f, 0.85f);
                    
                    //GUI.Label(alert_txtPos, alert_content, DebugHUD_Renderer.text_style);
                    DebugHUD_Renderer.text_style.Draw(alert_txtPos, alert_content, id);
                    DebugHUD_Renderer.subtext_style.Draw(alert_sub_txtPos, alert_sub_content, id);

                    var prevClr = GUI.color;
                    GUI.color = clr;

                    GUI.DrawTexture(alertPos, Loader.tex_alert, ScaleMode.ScaleToFit);
                    GUI.color = prevClr;
                }
            }
            else
            {
                var prev_depth = GUI.depth;
                GUI.depth = 100;
                if (!this.handleEvents()) return;

                // darken the entire screen
                DebugHUD_Renderer.blackout.Draw(screen_area, GUIContent.none, id);
                // draw a black fade
                GUI.DrawTexture(fade_area, DebugHUD_Renderer.bg_fade, ScaleMode.StretchToFill);

                var prev_skin = GUI.skin;
                GUI.skin = skin;
                // draw the debug console text
                console_scroll = GUI.BeginScrollView(console_text_area, console_scroll, console_inner_area, false, false, skin.horizontalScrollbar, skin.horizontalScrollbar);
                    console_text_style.Draw(console_inner_text_area, console_lines, id);
                    GUI.VerticalScrollbar(console_scrollbar_area, console_scroll.y, console_text_area.height, 0f, console_text_area.height, skin.verticalScrollbar);
                GUI.EndScrollView(true);

                GUI.depth = prev_depth;
                GUI.skin = prev_skin;
            }
        }
    }
}
