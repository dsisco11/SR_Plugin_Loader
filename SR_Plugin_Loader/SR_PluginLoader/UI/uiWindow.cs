using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    public class uiWindow : uiPanel
    {
        private Rect title_area = new Rect(), title_bar_abs_area = new Rect(), title_stipple_area = new Rect(), title_stipple_coords = new Rect(), titlebar_buttons_area = new Rect();
        protected uiScrollView content_panel = null;
        private GUIStyle style_title = null, style_titlebar = null;
        private Texture2D title_bar_texture = null;
        private int title_bar_height { get { return 26; } }
        private bool dragging = false;
        private uiButton closeBtn = null;
        public event controlEventDelegate<uiWindow> onClosed;
        public event controlEventDelegate<uiWindow> onShown;
        public event controlEventDelegate<uiWindow> onHidden;
        /// <summary>
        /// Specifies weather or not this window can be moved around by the player.
        /// </summary>
        public bool draggable = true;
        public string title { get { return content.text; } set { content.text = value; needs_layout = true; } }
        public override Rect content_area { get { return content_panel.content_area; } }
        protected override Rect inner_area { get { return content_panel.Get_Inner_Area(); } }

        // Size defines for the stippled titlebar pattern
        const int stipple_pattern_w = 4;
        const int stipple_pattern_h = 5;


        public uiWindow() : base(uiControlType.Window)
        {
            autosize = false;
            this.visible = false;//hidden by default

            //selfPadding = new RectOffset(0, 0, title_bar_height, 0);
            title = "Window";
            closeBtn = Create<uiButton>();
            GUIStyle sty = new GUIStyle();
            sty.normal.background = Loader.tex_close_dark;
            sty.hover.background = Loader.tex_close;
            closeBtn.border.hover.size = new RectOffset(1, 1, 1, 1);
            closeBtn.border.hover.color = new Color(1f, 1f, 1f, 0.2f);
            closeBtn.border.type = uiBorderType.NONE;
            closeBtn.Set_Size(title_bar_height, title_bar_height);
            closeBtn.Set_Style(sty);
            closeBtn.onClicked += CloseBtn_onClicked;
            closeBtn.margin = new RectOffset(6, 6, 6, 6);

            content_panel = Create<uiScrollView>();
            content_panel.margin = new RectOffset(3,3,0,3);
            content_panel.autosize = false;


            this.Add_Control(content_panel);
            this.Add_Control(closeBtn);

            this.layout_content_area();
        }
        
        private void Awake()
        {
            style_titlebar = new GUIStyle();
            Utility.Set_BG_Color(style_titlebar.normal, new Color(1f, 1f, 1f, 0.05f));

            this.style_title = new GUIStyle();
            style_title.normal.textColor = Color.white;
            style_title.fontSize = 14;
            style_title.alignment = TextAnchor.MiddleLeft;


            var tex = new Texture2D(stipple_pattern_w, title_bar_height);
            tex.wrapMode = TextureWrapMode.Repeat;
            Color clearClr = new Color(0f, 0f, 0f, 0f);

            // Clear the textures pixels so they are all transparent
            for (int x = 0; x < tex.width; x++)
            {
                for (int y = 0; y < tex.height; y++)
                {
                    tex.SetPixel(x, y, clearClr);
                }
            }

            Color pClr = new Color(1f, 1f, 1f, 0.3f);
            // Now write the stippled pattern we want
            int cY = (int)(((float)tex.height / 2f) - ((float)stipple_pattern_h / 2f) + 1);
            tex.SetPixel(0, cY - 2, pClr);
            tex.SetPixel(2, cY, pClr);
            tex.SetPixel(0, cY + 2, pClr);

            tex.Apply();
            title_bar_texture = tex;
        }

        private void CloseBtn_onClicked(uiControl c)
        {
            this.Close();
        }

        public void Close()
        {
            if (!this.visible) return;
            this.visible = false;
            if(onClosed != null) this.onClosed(this);
        }

        public void Show()
        {
            if (this.visible) return;
            this.visible = true;
            if (onShown != null) this.onShown(this);
        }

        public void Hide()
        {
            if (!this.visible) return;
            this.visible = false;
            if (onHidden != null) this.onHidden(this);
        }

        public void Center()
        {
            float X = (Screen.width * 0.5f) - (area.width * 0.5f);
            float Y = (Screen.height * 0.5f) - (area.height * 0.5f);

            this.Set_Pos(new Vector2(X, Y));
        }

    #region Remap Add / Remove / Clear functions to our content_panel control
        public override void Add(uiControl c)
        {
            content_panel.Add(c);
        }

        public override void Add(string name, uiControl c)
        {
            content_panel.Add(name, c);
        }

        public override void Remove(uiControl c)
        {
            content_panel.Remove(c);
        }

        public override bool Remove(string name)
        {
            return content_panel.Remove(name);
        }

        public override void Clear_Children()
        {
            content_panel.Clear_Children();
        }

        public override bool withinChild(Vector2 p)
        {
            if (closeBtn.area.Contains(p)) return true;
            return content_panel.withinChild(p);
        }

        public override List<uiControl> Get_Children()
        {
            return this.content_panel.Get_Children();
        }
        #endregion

        protected override void doLayout_Post()
        {
            title_bar_abs_area = new Rect(absArea.x, absArea.y, area.width, title_bar_height);
            var titlebar_area = new Rect(area.x, area.y, area.width, title_bar_height);

            int btn_area_width = (title_bar_height + 4);
            titlebar_buttons_area = new Rect(titlebar_area.width-btn_area_width, 0, btn_area_width, titlebar_area.height);

            Vector2 tsz = style_title.CalcSize(content);
            title_area = new Rect(6f, 0f, tsz.x+8f, titlebar_area.height);

            //ensure the stipple pattern always ends on the first column of the next repetition
            int stipple_width = (int)(titlebar_buttons_area.xMin - title_area.xMax - 6f);
            if ((stipple_width % stipple_pattern_w) != 1) stipple_width = (((stipple_width / stipple_pattern_w) * stipple_pattern_w) + 1);

            title_stipple_area = new Rect(title_area.xMax, 0f, stipple_width, titlebar_area.height);
            title_stipple_coords = new Rect(0f, 0f, (title_stipple_area.width/(float)stipple_pattern_w), 1f);
            
            closeBtn.Set_Pos(titlebar_area.width - title_bar_height, 0);
            //closeBtn.area = new Rect(titlebar_area.xMax - close_btn_size, titlebar_area.yMin, close_btn_size, close_btn_size);
            this.layout_content_area();
            base.doLayout_Post();
        }

        private void layout_content_area()
        {
            content_panel.Set_Pos(0, title_bar_height);
            float content_vs_title_padding = 2f;
            content_panel.margin.top = (int)content_vs_title_padding;
            content_panel.Set_Size(_inner_area.width, (_inner_area.height - content_vs_title_padding - title_bar_height));
        }

        public override void handleEvent()
        {
            if (!visible) DebugHud.Log("[{0}] Window handling events while INVISIBLE!", this);
            Event evt = Event.current;
            switch (evt.GetTypeForControl(this.id))
            {
                case EventType.MouseDrag:
                    if(this.dragging)
                    {
                        evt.Use();
                        Vector2 dtPos = evt.delta;
                        this.Set_Pos(dtPos.x+area.x, dtPos.y+area.y);
                    }
                    break;
                case EventType.MouseDown:
                    this.dragging = false;
                    if (this.title_bar_abs_area.Contains(evt.mousePosition) && this.draggable) this.dragging = true;
                    if (absArea.Contains(evt.mousePosition))
                    {
                        GUIUtility.hotControl = id;
                        evt.Use();
                    }
                    break;
                case EventType.MouseUp:
                    this.dragging = false;
                    if (GUIUtility.hotControl == 0)
                    {
                        if (absArea.Contains(evt.mousePosition))
                            evt.Use();
                    }

                    if (GUIUtility.hotControl == id)
                    {
                        GUIUtility.hotControl = 0;
                        evt.Use();
                    }
                    break;
                default:
                    base.handleEvent();
                    break;
            }
        }

        protected override void Display()
        {
            base.Display();// Draw BG
            GUI.BeginGroup(title_bar_abs_area);
                style_title.Draw(title_area, content, false, false, false, false);// Draw the titlebar text.
                GUI.DrawTextureWithTexCoords(title_stipple_area, title_bar_texture, title_stipple_coords, true);
            GUI.EndGroup();

            //GUI.BeginClip(_inner_area);
            GUI.BeginGroup(_inner_area);

            for (int i = 0; i < children.Count; i++)
            {
                this.children[i].TryDisplay();
            }

            GUI.EndGroup();
            //GUI.EndClip();
        }

        private void Update()
        {
            if (!this.visible) return;
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                this.Close();
            }
        }
    }
}
