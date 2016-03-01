using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace SR_PluginLoader
{
    public class UI_Notification
    {
        public Texture2D icon = null;
        public string msg = null;
        public string title=null;
        public Action onClick = null;

        private Rect area = new Rect(0f, 0f, notification_width, notification_height);
        public float height { get { return this.area.height; } }
        public float width { get { return this.area.width; } }

        /// <summary>
        /// Set to true to kill on the next update
        /// </summary>
        public bool should_die = false;
        /// <summary>
        /// Number of seconds this notice will live for.
        /// </summary>
        public float lifetime = -1f;
        private bool has_lifetime = false;

        private static Rect title_pos, msg_pos, icon_pos;
        private static GUIStyle style = null;
        private static GUIStyle title_style = null;
        private static GUIStyle msg_style = null;
        private bool isHovering = false, isDepressed = false;
        private bool needs_layout = true;

        private const float ICON_SIZE = 24f;
        public const float notification_width = 250f;
        public const float notification_height = 30f;


        public void Set_Width(float w)
        {
            area.size = new Vector2(w, area.size.y);
        }

        public void Set_Height(float h)
        {
            area.size = new Vector2(area.size.x, h);
        }

        public void Set_Size(Vector2 sz)
        {
            area.size = sz;
        }

        public void Set_Size(float w, float h)
        {
            area.size = new Vector2(w, h);
        }

        public void Set_Pos(Vector2 p)
        {
            area.position = p;
        }

        public void Set_Pos(float x, float y)
        {
            area.position = new Vector2(x, y);
        }

        private void init_styles()
        {
            style = new GUIStyle();
            msg_style = new GUIStyle();
            title_style = new GUIStyle();

            Utility.Set_BG_Color(style.normal, new Color(0.15f, 0.15f, 0.15f, 0.65f));

            msg_style.wordWrap = true;
            msg_style.normal.textColor = new Color(0.8f, 0.8f, 0.8f);
            msg_style.richText = true;
            msg_style.fontSize = 14;

            title_style.wordWrap = true;
            title_style.normal.textColor = Color.white;
            title_style.fontStyle = FontStyle.Bold;
            title_style.richText = true;
            title_style.fontSize = 16;
        }

        public void Update()
        {
            if (this.lifetime > 0f) this.has_lifetime = true;

            if(this.has_lifetime)
            {
                this.lifetime -= Time.deltaTime;
                if (this.lifetime <= 0f)
                {
                    this.should_die = true;
                    this.has_lifetime = false;//prevent further life depletion
                }
            }
        }

        public bool Display()
        {
            if (needs_layout) this.doLayout();

            int id = GUIUtility.GetControlID(0, FocusType.Passive, area);
            var evt = Event.current.GetTypeForControl(id);
            this.isHovering = this.area.Contains(Event.current.mousePosition);

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
                        if (this.isHovering)
                        {
                            try
                            {
                                if (this.onClick != null) this.onClick.Invoke();
                            }
                            catch(Exception ex)
                            {
                                DebugHud.Log(ex);
                            }
                            return true;
                        }
                    }
                    return false;
                case EventType.Repaint:
                    bool focus = false;
                    
                    GUI.Box(area, GUIContent.none, style);

                    GUI.BeginGroup(this.area);
                    if(this.title != null) title_style.Draw(title_pos, this.title, this.isHovering, this.isDepressed, true, focus);
                    if (this.msg != null) msg_style.Draw(msg_pos, this.msg, this.isHovering, this.isDepressed, true, focus);
                    if (this.icon != null) GUI.DrawTexture(icon_pos, this.icon);
                    else GUI.DrawTexture(icon_pos, Loader.tex_alert);
                    GUI.EndGroup();
                    
                    break;
            }
            return false;
        }


        public void doLayout()
        {
            if (style == null || msg_style == null || title_style == null) this.init_styles();
            this.needs_layout = false;
            
            icon_pos = new Rect(3f, (notification_height * 0.5f) - (ICON_SIZE * 0.5f), ICON_SIZE, ICON_SIZE);
            var title_sz = title_style.CalcSize(new GUIContent(this.title));
            title_pos = new Rect(new Vector2(icon_pos.xMax+6f, 2f), title_sz);

            float max_msg_width = (notification_width - (title_pos.x+2f));
            float msg_height = msg_style.CalcHeight(new GUIContent(this.msg), max_msg_width);
            DebugHud.Log("Notice MSG height: {0}", msg_height);

            msg_pos = new Rect(new Vector2(title_pos.xMin, title_pos.yMax+2f), new Vector2(max_msg_width, msg_height));
            /*
            icon_pos = new Rect(3f, 3f, ICON_SIZE, ICON_SIZE);
            float max_msg_width = (area.width - (icon_pos.xMax + 3f));
            float msg_height = msg_style.CalcHeight(new GUIContent(this.msg), max_msg_width)+3f;

            msg_pos = new Rect(new Vector2(icon_pos.xMax + 3f, 3f), new Vector2(max_msg_width, msg_height));
            */
            area.size = new Vector2(notification_width, Math.Max(msg_pos.yMax, notification_height));
        }
    }
}
