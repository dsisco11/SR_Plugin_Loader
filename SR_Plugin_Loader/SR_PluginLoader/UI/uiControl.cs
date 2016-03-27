using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    public enum uiBorderType { NONE = 0, LINE, GLOW, LINE_GLOW }
    public enum cPosDir { NONE = 0, ABOVE, BELOW, LEFT, RIGHT, TOP_OF, BOTTOM_OF, LEFT_SIDE_OF, RIGHT_SIDE_OF, CENTER_X, CENTER_Y }
    public enum uiControlType
    {
        Generic=0,
        Panel,
        Window,
        Button,
        Label,
        Textbox,
        Textfield,
        Checkbox,
        Progress,
        Icon
    }

    public delegate void onClickDelegate<T>(T c) where T : uiControl;
    public delegate void onChangeDelegate<T>(T c) where T : uiControl;
    public delegate void controlEventDelegate<T>(T c) where T : uiControl;


    public class uiBorderStyleState
    {
        public int? stipple_size = null;// The size of each stipple segment
        public int? stipple_gap = null;// Size of the space between each stippled segment (defaults to 1 if null)
        public RectOffset size = null;
        private Color? _color = null;
        public Texture2D texture = null;
        //public Color color { get { return (this.texture == null ? Color.clear : this.texture.GetPixel(0, 0)); } set { _color = value; if (this.texture == null) { this.texture = new Texture2D(1, 1); } texture.SetPixel(0, 0, value); texture.Apply(); } }
        public Color? color { get { return _color; } set { _color = value; texture = null; if (size == null) { size = new RectOffset(1, 1, 1, 1); } } }

        public uiBorderStyleState()
        {
        }

        public uiBorderStyleState(uiBorderStyleState st)
        {
            this.size = st.size;
            this.color = st.color;
            this.stipple_size = st.stipple_size;
            this.stipple_gap = st.stipple_gap;
        }


        public void take(uiBorderStyleState st)
        {
            if(st.size != null) size = st.size;
            if(st.color.HasValue) color = st.color;
            if(st.texture != null) texture = st.texture;
            if (st.stipple_size.HasValue) stipple_size = st.stipple_size;
            if (st.stipple_gap.HasValue) stipple_gap = st.stipple_gap;
        }

        public void reset()
        {
            size = new RectOffset();
            texture = null;
            color = null;
        }

        public void prepare_texture(Vector2 sz)
        {
            if ((int)sz.x <= 0 || (int)sz.y <= 0) return;
            if (!color.HasValue) return;
            texture = new Texture2D((int)sz.x, (int)sz.y);
            //DebugHud.Log("Preparing texture ({0}, {1}), {2}", texture.width, texture.height, color);

            int stpl_sz = 0;
            if (stipple_size.HasValue) stpl_sz = stipple_size.Value;
            int stpl_gap = 1;
            if (stipple_gap.HasValue) stpl_gap = stipple_gap.Value;

            int stpl_sum = (stpl_sz + stpl_gap);

            for (int x = 0; x < texture.width; x++)
            {
                bool mask_x = (x % stpl_sum < stpl_sz || stpl_sz==0);// stippling mask for X value
                bool xleft = false;
                bool xright = false;
                if (size.left > 0 && x >= 0 && x < size.left) xleft = true;
                if(size.right > 0 && x >= (texture.width - size.right) && x < texture.width) xright = true;

                for (int y = 0; y < texture.height; y++)
                {
                    bool mask_y = (y % stpl_sum < stpl_sz || stpl_sz == 0);// stippling mask for Y value
                    bool ytop = false;
                    bool ybottom = false;
                    if (size.top > 0 && y >= 0 && y < size.top) ytop = true;
                    if (size.bottom > 0 && y >= (texture.height - size.bottom) && y < texture.height) ybottom = true;

                    bool withinBounds = ((xleft || xright || ytop || ybottom) && ((xleft ^ xright) && mask_y) || ((ytop ^ ybottom) && mask_x));//does the current pixel fall on the border line?

                    texture.SetPixel(x, y, (withinBounds ? color.Value : Color.clear));
                }
            }

            texture.Apply();
        }

        public override bool Equals(object obj)
        {
            uiBorderStyleState o = (uiBorderStyleState)obj;
            bool sizeEq = (((o.size == null) == (this.size == null)) && Utility.floatEq(o.size.left, size.left) && Utility.floatEq(o.size.right, size.right) && Utility.floatEq(o.size.top, size.top) && Utility.floatEq(o.size.bottom, size.bottom));
            bool colorEq = (o.color.HasValue == this.color.HasValue);
            //if both colors have a value then we want to be more specific with our equality check.
            if (colorEq && this.color.HasValue) colorEq = (Utility.floatEq(o.color.Value.r, this.color.Value.r) && Utility.floatEq(o.color.Value.b, this.color.Value.b) && Utility.floatEq(o.color.Value.g, this.color.Value.g) && Utility.floatEq(o.color.Value.a, this.color.Value.a));

            return (sizeEq && colorEq);
        }
    }

    public class uiBorderStyle
    {
        public uiBorderType type = uiBorderType.LINE;
        /// <summary>
        /// Style to use by default
        /// </summary>
        public uiBorderStyleState normal = new uiBorderStyleState();
        /// <summary>
        /// Style to use when the mouse is overtop the control
        /// </summary>
        public uiBorderStyleState hover = new uiBorderStyleState();
        /// <summary>
        /// Style to use when the control is in an 'on' or 'activated' state
        /// </summary>
        public uiBorderStyleState active = new uiBorderStyleState();
        /// <summary>
        /// Style to use when the control has input focus.
        /// </summary>
        public uiBorderStyleState focused = new uiBorderStyleState();

        /// <summary>
        /// Used to store the final values as the seperate fields of each stylestate all cascade downwards and override each others values for each applicable state to the control to reach the final values that should be used.
        /// </summary>
        public uiBorderStyleState _cached = new uiBorderStyleState();
    }

    public class ControlPositioner
    {
        private cPosDir dir;
        private float offset = 0f;
        private uiControl target;
        public ControlPositioner(uiControl c, float offset, cPosDir dir)
        {
            this.target = c;
            this.offset = offset;
            this.dir = dir;
        }

        public void Apply(uiControl control)
        {
            switch(dir)
            {
                case cPosDir.ABOVE:
                    control.moveAbove(target, offset);
                    //target.Reposition_horizontal();
                    break;
                case cPosDir.BELOW:
                    control.moveBelow(target, offset);
                    //target.Reposition_horizontal();
                    break;
                case cPosDir.LEFT:
                    control.moveLeftOf(target, offset);
                    //target.Reposition_vertical();
                    break;
                case cPosDir.RIGHT:
                    control.moveRightOf(target, offset);
                    //target.Reposition_vertical();
                    break;
                case cPosDir.TOP_OF:
                    control.alignTop(offset);
                    break;
                case cPosDir.BOTTOM_OF:
                    control.alignBottom(offset);
                    break;
                case cPosDir.LEFT_SIDE_OF:
                    control.alignLeftSide(offset);
                    break;
                case cPosDir.RIGHT_SIDE_OF:
                    control.alignRightSide(offset);
                    break;
                case cPosDir.CENTER_X:
                    control.CenterX();
                    break;
                case cPosDir.CENTER_Y:
                    control.CenterY();
                    break;
            }
        }

        public bool Equals(uiControl targ, float off, cPosDir d)
        {
            return (this.target == targ && Utility.floatEq(this.offset, off) && this.dir == d);
        }
    }


    public abstract class uiControl : MonoBehaviour
    {
        #region VARIABLES
        protected int cached_id = 0;
        protected string _typename = null;
        public string typename { get { if (_typename == null) { return this.GetType().Name; } return _typename; } }
        public int id { get { if (cached_id <= 0) { this.cached_id = GUIUtility.GetControlID(this.focus_type); } return cached_id; } }

        /// <summary>
        /// Is this control in an activated / selected state?
        /// </summary>
        private bool _active = false;
        /// <summary>
        /// Can this control be interacted with?
        /// </summary>
        private bool _enabled = false;

        /// <summary>
        /// Can this control be interacted with?
        /// </summary>
        public new bool enabled { get { return _enabled; } set { _enabled = value; update_area(); } }

        /// <summary>
        /// Is this control in an activated / selected state?
        /// </summary>
        public bool active { get { return _active; } set { _active = value; update_area(); } }

        /// <summary>
        /// Should this control be rendered?
        /// </summary>
        public bool visible { get { return this.gameObject.activeSelf; } set { this.gameObject.SetActive(value); } }

        protected FocusType focus_type = FocusType.Passive;// Almost all controls will be using this FocusType, the only different ones will be ones that need keyboard input.
        protected static readonly GUISkin skin = null;
        protected GUIStyle _local_style = null;
        protected GUIStyle _style_no_bg = null;
        private bool uses_parent_text_style { get { return (type == uiControlType.Label || type == uiControlType.Textbox); } }
        /// <summary>
        /// Readonly reference to the GUIStyle this particular control instance should be using to draw itself.
        /// </summary>
        public GUIStyle style { get { return (this._local_style != null ? local_style : get_skin_style_for_type(this.type)); } }
        public GUIStyle local_style { get { if (this._local_style == null) { _local_style = new GUIStyle(get_skin_style_for_type(this.type)); update_area(); } return _local_style; } set { _local_style = value; _style_no_bg = null; } }
        protected GUIStyle styleNoBG { get { if (_style_no_bg == null) { styleNoBG = null; } return _style_no_bg; }
            set
            {
                _style_no_bg = new GUIStyle(style);
                _style_no_bg.normal.background = null;
                _style_no_bg.active.background = null;
                _style_no_bg.hover.background = null;
                _style_no_bg.focused.background = null;
            }
        }
        public uiBorderStyle border = new uiBorderStyle();
        /// <summary>
        /// Returns the border style that should currently be used.
        /// Used by the internal drawing functions.
        /// </summary>
        private uiBorderStyleState borderStyle
        {
            get
            {
                border._cached.reset();
                if (border.type != uiBorderType.NONE)
                {
                    border._cached.take(this.border.normal);
                    if (this.isMouseOver) border._cached.take(this.border.hover);
                    if (this.isActive) border._cached.take(this.border.active);
                    if (this.isFocused) border._cached.take(this.border.focused);
                }
                return this.border._cached;
            }
        }
        private uiBorderStyleState cached_borderStyle = null;


        #region State flags
        protected bool isMouseDown = false;
        protected bool isMouseOver = false;

        protected bool isFocused { get { return (GUIUtility.hotControl == this.id || GUIUtility.keyboardControl == this.id); } }// Focused means the control is receiving user input
        protected virtual bool isActive { get { return (this.isMouseDown || this._active); } }// the meaning of 'Active' differs with each control type but means something similar to being toggled 'on' or being the active selection in a list.
        protected virtual bool hasScrollbar { get { return false; } }
        #endregion

        protected bool _autosize = false;//this is the value which is explicitly set by the user, if the control should defy it's default nature and always stick to autosizing then the user indirectly set's this to TRUE.
        protected bool should_autosize { get { return (type == uiControlType.Button || type == uiControlType.Label || type == uiControlType.Textbox); } }// Determines the default autosize nature of a control (wether it autosizes by default)
        private bool set_explicit_size = false;// Tracks whether the control has had an explicit size set with Set_Size() 
        private bool set_explicit_W = false;// Tracks whether the control has had an explicit width set
        private bool set_explicit_H = false;// Tracks whether the control has had an explicit height set
        public bool autosize { get { return (_autosize || (should_autosize && !(set_explicit_W || set_explicit_H))); } set { _autosize = value; update_area(); } }// If this is true then the control will autosize itself, each control type can have custom autosizing logic, the user can use this field to force any control to attempt autosizing.

        protected bool isClickable { get { return (type == uiControlType.Button || type == uiControlType.Checkbox || type == uiControlType.Textbox || type == uiControlType.Textfield || type == uiControlType.Panel || type == uiControlType.Window); } }

        protected bool can_have_positioners { get { return !(type == uiControlType.Window); } }
        #region EVENTS
        public event controlEventDelegate<uiControl> onClicked;
        public event controlEventDelegate<uiControl> onAreaUpdated;
        public event controlEventDelegate<uiControl> onParentAreaUpdated;
        #endregion


        protected bool needs_layout = true;
        protected GUIContent _content = new GUIContent();
        protected virtual GUIContent content { get { return _content; } set { _content = value; } }
        public readonly uiControlType type;

        private RectTransform rect = null;// Only needed so the event system raycaster can properly find our controls at all locations they might be onscreen.

        protected uiPanel parent = null;
        /// <summary>
        /// This is the field that stores the position this control was SET to occupy and the size that was SET explicitly for it
        /// If no size was explicitly set then it's size values will always be -1
        /// </summary>
        protected Rect _area = new Rect(0, 0, -1, -1);
        private Rect? cached_area = null;
        
        protected Rect _inner_area = new Rect();
        /// <summary>
        /// The area this control will draw it's contents within
        /// this field is overrideable so that controls derived from the uiWindow class can position their components using the inner_area reference without said reference also encompasing the uiWindow's titlebar which screws up positioning...
        /// </summary>
        protected virtual Rect inner_area { get { return _inner_area; } }

        /// <summary>
        /// The area this control will draw its background within
        /// </summary>
        protected Rect draw_area = new Rect();//{ get { if (borderStyle == null || borderStyle.size == null) { return border_area; } return borderStyle.size.Remove(border_area); } }

        /// <summary>
        /// The area this control will draw its border within
        /// </summary>
        protected Rect border_area = new Rect();

        /// <summary>
        /// This control's position within whatever other control may contain it.
        /// This is the area a control is intended to occupy.
        /// When drawing the control use "draw_area" as the intended area is altered by padding and margin values.
        /// </summary>
        public Rect area { get { if (cached_area.HasValue) { return cached_area.Value; } return _area; } set { _area = value; this.update_area(); } }

        /// <summary>
        /// The absolute position where this control will render it's content, including child controls.
        /// </summary>
        public Vector2 innerPos { get { return (this.inner_area.position); } }

        /// <summary>
        /// The absolute position where this control will appear on screen.
        /// </summary>
        public Vector2 absPos { get { return absArea.position; } }

        /// <summary>
        /// The absolute area this control occupies on screen.
        /// </summary>
        public Rect absArea { get { if (!_absArea.HasValue) { _absArea = new Rect(_area.position + parentPosInner - parentScroll, area.size); } return _absArea.Value; } set { _absArea = null; } }
        private Rect? _absArea = null;

        protected Vector2 parentPos { get { return !parent ? Vector2.zero : parent.absPos; } }
        protected Vector2 parentPosInner { get { if (cached_parentPosInner.HasValue) { return cached_parentPosInner.Value; } cached_parentPosInner = (!parent ? Vector2.zero : (parent.absPos + new Vector2(parent.padding.left + parent.margin.left + borderStyle.size.left, parent.padding.top + parent.margin.top + borderStyle.size.top))); return cached_parentPosInner.Value; } }
        private Vector2? cached_parentPosInner = null;// By caching the calculated parent inner pos we can save some processing power especially with controls that are deeply nested.

        private Vector2? cached_parentScroll = null;
        protected Vector2 parentScroll { get { if (cached_parentScroll.HasValue) { return cached_parentScroll.Value; } cached_parentScroll = (!parent ? Vector2.zero : parent.Get_ScrollPos()); return cached_parentScroll.Value; } }

        /// <summary>
        /// The (relative) position of this control.
        /// </summary>
        public virtual Vector2 pos { get { return this.area.position; } }
        /// <summary>
        /// The position of this control as set by the user, or the auto positioning system.
        /// </summary>
        public virtual Vector2 _pos { get { return this._area.position; } }
        /// <summary>
        /// The final size of this control.
        /// </summary>
        public virtual Vector2 size { get { if (autosize) { return auto_size; } if (cached_area.HasValue) { return cached_area.Value.size; } return this._area.size; } }

        public Vector2 auto_size { get { var sz = this.Get_Autosize(); if (set_explicit_W) { sz.x = area.width; } if (set_explicit_H) { sz.y = area.height; } return sz; } }
        /// <summary>
        /// The base size of this control without padding, border, or margins accounted for.
        /// </summary>
        public Vector2 _size { get { return this._area.size; } set { this._area.size = value; update_area(); } }

        public Vector2 size_min = new Vector2(-1, -1), size_max = new Vector2(-1, -1);

        protected RectOffset _margin = new RectOffset(0, 0, 0, 0);
        public RectOffset margin { get { return _margin; } set { _margin = value; update_area(); } }

        protected RectOffset _padding = new RectOffset(0, 0, 0, 0);
        public RectOffset padding { get { return _padding; } set { _padding = value; update_area(); } }

        protected RectOffset _selfPadding = new RectOffset(0, 0, 0, 0);// Padding that is applied only to autosized controls.
        public RectOffset selfPadding { get { return _selfPadding; } set { _selfPadding = value; update_area(); } }

        private ControlPositioner vertical_positioner = null;
        private ControlPositioner horizontal_positioner = null;

        public bool isChild { get { return (this.parent != null); } }
        #endregion

        public override string ToString()
        {
            return String.Format("id.{0} {1}", this.id, this.typename);
        }

        #region Constructors
        static uiControl()
        {
            skin = ScriptableObject.CreateInstance<GUISkin>();

            Utility.Set_BG_Color(skin.box.normal, new Color(0f, 0f, 0f, 0.2f));
            Utility.Set_BG_Color(skin.window.normal, new Color32(50, 50, 50, 255));

            byte g = 20;
            Utility.Set_BG_Color(skin.textArea.normal, new Color32(g, g, g, 255));
            Utility.Set_BG_Color(skin.textField.normal, new Color32(g, g, g, 255));

            //Utility.Set_BG_Color(skin.button.normal, new Color32(0, 0, 0, 0));// buttons have no background by default.
            //Utility.Set_BG_Color(skin.button.hover, new Color(1f, 1f, 1f, 0.06f));// slightly light up

            // We want a sheen texture for buttons
            //skin.button.normal.background = Utility.Create_Sheen_Texture(100, Color.white);
            //Utility.Set_BG_Color(skin.button.normal, new Color32(32, 32, 32, 180));
            Utility.Set_BG_Gradient(skin.button.normal, 100, GRADIENT_DIR.TOP_BOTTOM, new Color32(45, 45, 45, 180), new Color32(32, 32, 32, 180));
            //skin.button.hover.background = Utility.Create_Sheen_Texture(100, new Color(0.20f, 0.595f, 1.0f));


            skin.box = defaultFont(skin.box);
            skin.button = defaultFont(skin.button);
            skin.window = defaultFont(skin.window);
            skin.label = defaultFont(skin.label);
            skin.textArea = defaultFont(skin.textArea);
            skin.textField = defaultFont(skin.textField);
            skin.scrollView = defaultFont(skin.scrollView);

            skin.label.alignment = TextAnchor.MiddleLeft;
            skin.button.alignment = TextAnchor.MiddleCenter;

            skin.window.fontSize = 12;
            skin.button.fontSize = 14;

            skin.settings.cursorFlashSpeed = 0.8f;
            skin.settings.selectionColor = new Color(0.196f, 0.592f, 0.992f, 0.5f);

            int scrollbar_width = 9;

            skin.verticalScrollbar = new GUIStyle();
            skin.verticalScrollbar.fixedWidth = scrollbar_width;
            Utility.Set_BG_Color(skin.verticalScrollbar.normal, new Color32(16, 16, 16, 200));

            skin.verticalScrollbarThumb = new GUIStyle();
            skin.verticalScrollbarThumb.fixedWidth = scrollbar_width;
            Utility.Set_BG_Color(skin.verticalScrollbarThumb.normal, new Color32(80, 80, 80, 255));

        }

        public uiControl(uiControlType ty = uiControlType.Generic)
        {
            type = ty;
        }

        public static T Create<T>(uiPanel parent = null) where T : uiControl
        {
            GameObject obj = new GameObject();
            obj.SetActive(true);
            obj.layer = 5;//GUI layer
            UnityEngine.GameObject.DontDestroyOnLoad(obj);
            
            T c = obj.AddComponent<T>();
            if (parent != null) parent.Add(c);
            return c;
        }

        private static GUIStyle defaultFont(GUIStyle style)
        {
            style.clipping = TextClipping.Overflow;
            style.alignment = TextAnchor.MiddleLeft;
            style.fontSize = 14;

            style.normal.textColor = Color.white;
            style.hover.textColor = Color.white;
            style.active.textColor = Color.white;
            style.focused.textColor = Color.white;

            return style;
        }
        #endregion

        #region Accessor Functions
        public void Set_Style(GUIStyle style)
        {
            this.local_style = style;
        }

        public void Set_Pos(float x, float y)
        {
            area = new Rect(new Vector2(x, y), _area.size);
        }

        public void Set_Pos(Vector2 pos)
        {
            area = new Rect(pos, _area.size);
        }

        public void Set_Size(float w, float h)
        {
            set_explicit_W = true;
            set_explicit_H = true;
            area = new Rect(_area.position, new Vector2(w, h));
        }

        public void Set_Size(Vector2 sz)
        {
            set_explicit_W = true;
            set_explicit_H = true;
            area = new Rect(_area.position, sz);
        }

        public void Set_Width(float val)
        {
            set_explicit_W = true;
            area = new Rect(_area.position, new Vector2(val, _area.size.y));
        }

        public void Set_Height(float val)
        {
            set_explicit_H = true;
            area = new Rect(_area.position, new Vector2(_area.size.x, val));
        }

        internal void Set_Parent(uiControl parent)
        {
            this.parent = (uiPanel)parent;
            this.parent_area_updated();
        }

        protected virtual Vector2 Get_Autosize()
        {
            if (this.content == null) return new Vector2(6f, 6f);// we can't do anything else.
            return (style.CalcSize(this.content) + new Vector2(_selfPadding.horizontal, _selfPadding.vertical));
        }
        /// <summary>
        /// This is the method child controls should use to obtain the area they may occupy within their parent control.
        /// </summary>
        /// <returns></returns>
        public Rect Get_Inner_Area()
        {
            if (this.hasScrollbar)
            {
                RectOffset re = new RectOffset(0, 16, 0, 0);
                return re.Remove(this.inner_area);
            }
            return this.inner_area;
        }
        #endregion
        #region Property update handlers
        /// <summary>
        /// Call each time ANY factor that determines the controls size is changed.
        /// </summary>
        public virtual void update_area()
        {
            try
            {
                if (parent) parent.needs_layout = true;//cause the parent to update due to this control altering it's position
                needs_layout = true;
                _absArea = null;//cause our absolute area to be recalculated.
                cached_area = null;// we *WANT* to set the cached value to null here so when we call this.size we don't get the previously cached value stored there, otherwise the size would never change after we explicitly set it.

                if (autosize)
                {// If we are autosizing then go ahead and set the current autosize value.
                    /*
                    Vector2 nsz = _area.size;
                    Vector2 asz = auto_size;

                    if (!set_explicit_W) nsz.x = asz.x;
                    if (!set_explicit_H) nsz.y = asz.y;
                    _area = new Rect(_area.position, nsz);
                    */
                    _area = new Rect(_area.position, auto_size);
                }
                cached_area = new Rect(_area.position, constrain_size(size));

                /*
                // This WOULD be the correct sizing implementation for the W3C Box-Model but without a content reflow system it is pointless to use the box-model.
                var tmp_area = new Rect(_area.position, this.size);
                cached_area = _margin.Add( borderStyle.size.Add( _padding.Add( tmp_area)));
                inner_area = tmp_area;
                draw_area = _padding.Add( inner_area);
                border_area = borderStyle.size.Add( draw_area);
                */
                border_area = _margin.Remove(cached_area.Value);
                draw_area = borderStyle.size.Remove(border_area);
                //_inner_area = _padding.Remove(draw_area);
                _inner_area = final_area_to_inner(cached_area.Value);
            }
            finally
            {
                Reposition();
                if (onAreaUpdated != null) onAreaUpdated(this);
            }
        }

        /// <summary>
        /// Takes a Rect instance and makes sure its X/Y and WIDTH/HEIGHT values are whole value numbers
        /// </summary>
        private Rect lock_area_wholevalues(Rect rect)
        {
            return new Rect((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
        }

        /// <summary>
        /// Takes the final area and removes all the margin/padding/border offsets to it to return the inner area.
        /// </summary>
        protected Rect final_area_to_inner(Rect final_area)
        {
            return _margin.Remove(borderStyle.size.Remove(_padding.Remove(final_area)));
        }

        /// <summary>
        /// Takes an inner area size and adds all the margin/padding/border offsets to it and returns the final "outter" area.
        /// </summary>
        protected Rect final_area_from_inner(Rect inner_area)
        {
            return _margin.Add(borderStyle.size.Add(_padding.Add(inner_area)));
        }

        public void parent_area_updated()
        {
            cached_parentPosInner = null;
            update_area();
            if (onParentAreaUpdated != null) onParentAreaUpdated(this);
        }

        public void parent_scroll_updated()
        {
            cached_parentScroll = null;
            update_area();
        }

        /// <summary>
        /// Reapplies horizontal and vertical positioning logic for this control.
        /// </summary>
        private void Reposition()
        {
            if(!can_have_positioners)
            {
                vertical_positioner = null;
                horizontal_positioner = null;
                return;
            }

            if (vertical_positioner != null) vertical_positioner.Apply(this);
            if (horizontal_positioner != null) horizontal_positioner.Apply(this);
        }

        internal void Reposition_vertical()
        {
            if (vertical_positioner != null) vertical_positioner.Apply(this);
        }
        internal void Reposition_horizontal()
        {
            if (horizontal_positioner != null) horizontal_positioner.Apply(this);
        }
        #endregion
        #region Positioning Helpers
        private bool maybeUpdate_Pos(Vector2 new_pos)
        {
            if (!Utility.floatEq(_area.position.x, new_pos.x) || !Utility.floatEq(_area.position.y, new_pos.y))
            {
                this.area = new Rect(new_pos, this.size);
                return true;
            }

            return false;
        }

        public void moveBelow(uiControl targ, float yOff = 0f)
        {
            this.maybeUpdate_Pos(new Vector2(_area.position.x, targ.area.yMax + yOff));
            if (vertical_positioner == null || !vertical_positioner.Equals(targ, yOff, cPosDir.BELOW)) vertical_positioner = new ControlPositioner(targ, yOff, cPosDir.BELOW);
        }

        /// <summary>
        /// Positions the control so it's bottom edge is yOff above the top edge of another given control.
        /// </summary>
        public void moveAbove(uiControl targ, float yOff = 0f)
        {
            this.maybeUpdate_Pos(new Vector2(_area.position.x, targ.area.yMin - this.area.height - yOff));
            if (vertical_positioner == null || !vertical_positioner.Equals(targ, yOff, cPosDir.ABOVE)) vertical_positioner = new ControlPositioner(targ, yOff, cPosDir.ABOVE);
        }

        public void moveRightOf(uiControl targ, float xOff = 0f)
        {
            this.maybeUpdate_Pos(new Vector2(targ.area.xMax + xOff, _area.position.y));
            if (horizontal_positioner == null || !horizontal_positioner.Equals(targ, xOff, cPosDir.RIGHT)) horizontal_positioner = new ControlPositioner(targ, xOff, cPosDir.RIGHT);
        }

        /// <summary>
        /// Positions the control so it's right edge is xOff away from the left edge of another given control.
        /// </summary>
        public void moveLeftOf(uiControl targ, float xOff = 0f)
        {
            this.maybeUpdate_Pos(new Vector2(targ.area.xMin - this.area.width - xOff, _area.position.y));
            if (horizontal_positioner == null || !horizontal_positioner.Equals(targ, xOff, cPosDir.LEFT)) horizontal_positioner = new ControlPositioner(targ, xOff, cPosDir.LEFT);
        }

        public void alignTop(float yOff = 0f)
        {
            this.maybeUpdate_Pos(new Vector2(_area.position.x, yOff));
            if (vertical_positioner == null || !vertical_positioner.Equals(null, yOff, cPosDir.TOP_OF)) vertical_positioner = new ControlPositioner(null, yOff, cPosDir.TOP_OF);
        }

        public void alignBottom(float yOff = 0f)
        {
            float val = Screen.height;
            if(parent) val = ((uiPanel)parent).inner_area.height;

            this.maybeUpdate_Pos(new Vector2(_area.position.x, val - this.area.height - yOff));
            if (vertical_positioner == null || !vertical_positioner.Equals(null, yOff, cPosDir.BOTTOM_OF)) vertical_positioner = new ControlPositioner(null, yOff, cPosDir.BOTTOM_OF);
        }

        public void alignLeftSide(float xOff = 0f)
        {
            this.maybeUpdate_Pos(new Vector2(xOff, _area.position.y));
            if (horizontal_positioner == null || !horizontal_positioner.Equals(null, xOff, cPosDir.LEFT_SIDE_OF)) horizontal_positioner = new ControlPositioner(null, xOff, cPosDir.LEFT_SIDE_OF);
        }

        public void alignRightSide(float xOff = 0f)
        {
            float val = Screen.width;
            if(parent) val = ((uiPanel)parent).inner_area.width;

            this.maybeUpdate_Pos(new Vector2(val - this.area.width - xOff, _area.position.y));
            if (horizontal_positioner == null || !horizontal_positioner.Equals(null, xOff, cPosDir.RIGHT_SIDE_OF)) horizontal_positioner = new ControlPositioner(null, xOff, cPosDir.RIGHT_SIDE_OF);
        }
        
        public void CenterY(float yOff = 0f)
        {
            float val = Screen.height;
            if (parent) val = parent.Get_Inner_Area().height;
            
            this.maybeUpdate_Pos(new Vector2(_area.position.x, (val * 0.5f) - (this.area.height * 0.5f) - yOff));
            if (vertical_positioner == null || !vertical_positioner.Equals(null, 0f, cPosDir.CENTER_Y)) vertical_positioner = new ControlPositioner(null, 0f, cPosDir.CENTER_Y);
        }

        public void CenterX(float xOff = 0f)
        {
            float val = Screen.width;
            if (parent) val = parent.Get_Inner_Area().width;

            this.maybeUpdate_Pos(new Vector2((val * 0.5f) - (this.area.width * 0.5f) - xOff, _area.position.y));
            if (horizontal_positioner == null || !horizontal_positioner.Equals(null, 0f, cPosDir.CENTER_X)) horizontal_positioner = new ControlPositioner(null, 0f, cPosDir.CENTER_X);
        }
        #endregion
        #region Sizing Helpers
        private bool maybeUpdate_Size(Vector2 sz)
        {
            if (!Utility.floatEq(_area.width, sz.x) || !Utility.floatEq(_area.height, sz.y))
            {
                this.area = new Rect(_area.position, sz);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Resizes the control so it's bottom edge is yOff away from the top edge of another given control.
        /// </summary>
        public void snapBottomSideTo(uiControl targ, float yOff = 0f)
        {
            this.maybeUpdate_Size(new Vector2(this._size.x, targ.area.yMin - this.area.yMin - yOff));
        }
        
        /// <summary>
        /// Resizes the control so it's right edge is xOff away from the left edge of another given control.
        /// </summary>
        public void snapRightSideTo(uiControl targ, float xOff = 0f)
        {
            this.maybeUpdate_Size(new Vector2(targ.area.xMin - this.area.xMin - xOff, this._size.y));
        }

        public void floodY(float yOff = 0f)
        {
            if (this.parent == null)
            {
                DebugHud.Log("[{0}] Called floodY() on control without a parent!", this.typename);
                return;
            }
            
            this.maybeUpdate_Size(new Vector2(this._size.x, parent.Get_Inner_Area().height - this.area.y - yOff));
        }

        public void floodX(float xOff = 0f)
        {
            if (this.parent == null)
            {
                DebugHud.Log("[{0}] Called floodX() on control without a parent!", this.typename);
                return;
            }
            
            this.maybeUpdate_Size(new Vector2(parent.Get_Inner_Area().width - this.area.x - xOff, this._size.y));
        }
        
        public void floodXY(float xOff=0f, float yOff=0f)
        {
            this.floodX(xOff);
            this.floodY(yOff);
        }
        /// <summary>
        /// Restricts a given size to be within this controls set min/max size range if any.
        /// </summary>
        /// <param name="sz"></param>
        /// <returns></returns>
        protected Vector2 constrain_size(Vector2 sz)
        {
            float x = sz.x;
            float y = sz.y;

            if (size_min.x > 0f) x = Mathf.Max(x, size_min.x);
            if (size_min.y > 0f) y = Mathf.Max(y, size_min.y);

            if (size_max.x > 0f) x = Mathf.Min(x, size_max.x);
            if (size_max.y > 0f) y = Mathf.Min(y, size_max.y);

            sz.x = x;
            sz.y = y;

            return sz;
        }
        #endregion

        /// <summary>
        /// Takes a position which is in absolute screen coordinates and returns the position relative to this control.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        #region HELPER STUFF
        protected void check_styleNoBG()
        {
            bool dirty = false;
            if (styleNoBG.normal.textColor != style.normal.textColor) dirty = true;
            if (styleNoBG.active.textColor != style.active.textColor) dirty = true;
            if (styleNoBG.hover.textColor != style.hover.textColor) dirty = true;
            if (styleNoBG.focused.textColor != style.focused.textColor) dirty = true;

            if (dirty) styleNoBG = null;//recache
        }

        protected Vector2 absToRelativePos(Vector2 pos)
        {
            return (pos - absPos + area.position);
        }

        protected GUIStyle get_skin_style_for_type(uiControlType ty)
        {
            //if (uses_parent_text_style && parent != null) return parent.styleNoBG;

            switch (ty)
            {
                case uiControlType.Window:
                    return skin.window;
                case uiControlType.Panel:
                    return skin.box;
                case uiControlType.Button:
                    return skin.button;
                case uiControlType.Label:
                    return skin.label;
                case uiControlType.Textbox:
                    return skin.textArea;
                case uiControlType.Textfield:
                    return skin.textField;
                case uiControlType.Checkbox:
                    return skin.toggle;
                default:
                    return skin.box;
            }
        }
        
        private void clear_cached_areas()
        {
            cached_area = null;
        }
        #endregion

        #region LOGIC
        /// <summary>
        /// An empty layout function for inheriting classes to override and put their own static layout logic into without screwing up the normal layout stuff (I forget why I had to add this specifically)
        /// </summary>
        protected virtual void doLayout_Post()
        {
        }

        /// <summary>
        /// Empty overrideable base class layout function
        /// </summary>
        public virtual void doLayout()
        {
        }

        public virtual void handleEvent()
        {
        }

        protected void handleLayout()
        {
            this.doLayout();
            this.doLayout_Post();
            // we want to set this layout var to fale AFTER we do the layout logic because child controls which move during said layout functions will 
            // inevitably cause this var to set to true, which is invalid in this context because we just updated all the ontrols around them anyway
            // which is the entire point of them doing so.
            this.needs_layout = false;

            this.clear_cached_areas();
        }
        /// <summary>
        /// 'base' event handlling logic for setting state flags for the control
        /// </summary>
        public virtual void handleEvent_Base()
        {
            //if (evt == EventType.MouseDown && GUIUtility.hotControl != 0) return false;
            // XXX:  Problem, if we allow parented controls to process their own mouse events they will not take into acount their parent containers scroll offset -
            // We need to make absArea account for parent scrolling.

            Event evt = Event.current;
            EventType et = evt.GetTypeForControl(this.id);
            if (this.needs_layout || et == EventType.Layout) this.handleLayout();
            
            //if(this.typename.Length>0 && evt.isMouse && et!= EventType.Repaint && et!= EventType.Used && et!= EventType.Ignore && et!= EventType.Layout)
                //DebugHud.Log("[{0}] Event: {1}", this.typename, evt);

            this.isMouseOver = absArea.Contains(evt.mousePosition);
            bool use_event = false;
            switch (et)
            {
                case EventType.MouseDown:
                    if (isMouseOver && isClickable)
                    {
                        if (GUIUtility.hotControl == 0)
                        {
                            this.isMouseDown = true;
                            GUIUtility.hotControl = id;
                            use_event = true;
                        }
                    }
                    break;
                case EventType.MouseUp:
                    bool wasDown = this.isMouseDown;
                    this.isMouseDown = false;
                    if (isMouseOver && isClickable && wasDown && onClicked != null)
                    {
                        this.onClicked(this);
                    }

                    if (GUIUtility.hotControl == id)
                    {
                        GUIUtility.hotControl = 0;
                        use_event = true;
                    }
                    break;
                case EventType.MouseMove:// This event is never sent to any controls except unity's EditorWindow typed controls
                    break;
                case EventType.Ignore:
                case EventType.Used:
                    return;
                case EventType.Layout:
                    Event.current.Use();
                    return;
            }

            this.handleEvent();// Give this control a chance to perform it's custom logic, for things like windows, buttons, etc.
            if (use_event && Event.current.GetTypeForControl(this.id) != EventType.Used) evt.Use();// We need to check and make sure that none of the custom logic for this control used the event before we try to.
        }

        public void TryDisplay()
        {
            if (!visible) return;
            this.Display();
        }

        protected virtual void Display()
        {
            this.draw_border();
            style.Draw(draw_area, GUIContent.none, this.isMouseOver || this.isActive, this.isActive, false, this.isFocused);
        }

        protected void draw_border()
        {
            if (cached_borderStyle == null) return;
            if (cached_borderStyle.texture == null) cached_borderStyle.prepare_texture(size);
            if (cached_borderStyle.texture == null) return;

            GUI.DrawTexture(border_area, cached_borderStyle.texture);
        }

        private void OnGUI()
        {
            // Abort drawing the panel IF we are not currently visible
            if (!this.visible) return;//non-visible controls should also not get events, so don't...
            this.handleEvent_Base();

            if (cached_borderStyle == null || !cached_borderStyle.Equals(borderStyle))// If the border SIZE changed then update the controls area again
            {
                update_area();
                cached_borderStyle = new uiBorderStyleState(borderStyle);
                cached_borderStyle.prepare_texture(size);
            }

            this.check_styleNoBG();

            var et = Event.current.GetTypeForControl(this.id);
            if (et == EventType.Ignore || et == EventType.Used) return;
            if (et == EventType.Repaint && this.isChild) return;


            GUISkin prevSkin = GUI.skin;
            GUI.skin = skin;

            this.Display();

            GUI.skin = prevSkin;
        }

        #endregion
    }
}
