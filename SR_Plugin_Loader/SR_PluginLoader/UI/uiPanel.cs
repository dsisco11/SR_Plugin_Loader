using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    public class uiPanel : uiControl
    {
        protected List<uiControl> children = new List<uiControl>();
        protected Dictionary<string, uiControl> child_map = new Dictionary<string, uiControl>();

        public bool CanScroll = false;
        protected Vector2 scroll_pos = Vector2.zero;
        private Rect _content_area = new Rect();
        /// <summary>
        /// The area that the panels content currently takes up.
        /// </summary>
        public virtual Rect content_area { get { return _content_area; } }
        protected override bool hasScrollbar { get { if (!this.CanScroll) { return false; } return (content_area.height > inner_area.height); } }
        public uiControl this[string key] { get { uiControl c; if (this.child_map.TryGetValue(key, out c)) { return c; } return null; } }

        #region EVENTS
        public event controlEventDelegate<uiPanel> onLayout;
        #endregion

        #region Constructors
        public uiPanel() : base(uiControlType.Panel)
        {
        }

        protected uiPanel(uiControlType ty) : base(ty)
        {
        }
        #endregion

        #region Generic child-control management
        public virtual uiControl Add(uiControl c)
        {
            this.Add_Control(c);
            return c;
        }
        
        public virtual uiControl Add(string name, uiControl c)
        {
            this.Add(c);
            uiControl tmp;
            if(child_map.TryGetValue(name, out tmp))
            {
                if(tmp != c) DebugHud.Log("[Plugin UI] Warning: assigning new control to a name that is already taken: {0}", name);
            }

            child_map[name] = c;
            return c;
        }

        public virtual void Remove(uiControl c)
        {
            this.Remove_Control(c);
        }

        public virtual bool Remove(string name)
        {
            uiControl c;
            if(child_map.TryGetValue(name, out c))
            {
                child_map.Remove(name);
                this.Remove(c);
                return true;
            }

            return false;
        }

        public virtual void Clear_Children()
        {
            this.children.Clear();
            this.child_map.Clear();
            update_area();
        }

        public virtual List<uiControl> Get_Children()
        {
            return this.children;
        }

        public virtual bool withinChild(Vector2 p)
        {
            foreach (uiControl child in this.children)
            {
                if (child.area.Contains(p)) return true;
            }

            return false;
        }

        #endregion

        #region Base child-control management (cannot be overriden)
        protected void Add_Control(uiControl c)
        {
            if (c == null) return;
            if (this.children.Contains(c)) return;

            needs_layout = true;
            this.children.Add(c);

            c.Set_Parent(this);
            if (c.gameObject != null && this.gameObject != null)
            {
                c.gameObject.transform.SetParent(base.gameObject.transform);
                //c.rect.SetParent(base.gameObject.transform, false);
            }

            update_area();
        }

        protected void Remove_Control(uiControl c)
        {
            needs_layout = true;
            this.children.Remove(c);
        }
        #endregion

        /// <summary>
        /// WAIT!
        /// Be sure to use <see cref="final_area_from_inner()"/> when calculating the size.
        /// </summary>
        /// <returns></returns>
        protected override Vector2 Get_Autosize()
        {
            // OK so since autosizing panels are making themselves expand to a certain size based on their child controls bounds
            // we need to logically consider the area the child controls occupy to be the panels "inner_area"
            // and restack all padding/margin/etc offsets ontop of it to get the size we need to return!
            return final_area_from_inner(content_area).size;
        }

        public Vector2 Get_ScrollPos()
        {
            return this.scroll_pos;
        }

        protected void propagateEventToChildren()
        {
            Event e = Event.current;
            // Decided we need to pass all events to the childrens

            //EventType ty = Event.current.GetTypeForControl(this.id);
            //if (ty == EventType.Layout || e.isMouse)
            //{
                for (int i = 0; i < children.Count; i++)
                {
                    if (children[i] == null) continue;
                    children[i].TryHandleEvent();
                }
            //}
        }

        public override void handleEvent()
        {
            base.handleEvent();
        }

        public override void handleEvent_Base()
        {
            // Since this control has children that *MUST* get control of any & all event's it receives before it does we need to propagate each event to all of the children FIRST!
            this.propagateEventToChildren();
            base.handleEvent_Base();
        }

        public override void update_area()
        {
            base.update_area();
            foreach(var child in this.children)
            {
                child.parent_area_updated();
            }
        }

        protected override void doLayout_Post()
        {
            float xMax = 0f, yMax = 0f;            
            foreach(uiControl child in this.Get_Children())
            {
                xMax = Math.Max(xMax, child.area.xMax);
                yMax = Math.Max(yMax, child.area.yMax);
            }

            _content_area = new Rect(0f, 0f, xMax, yMax);
            if (this.onLayout != null) this.onLayout(this);
        }

        protected override void Display()
        {
            base.Display();// Draw BG
            // Draw children within scrollable area
            Vector2 pre_scroll = this.scroll_pos;//track what the scroll pos was before this frame
            if (this.CanScroll) scroll_pos = GUI.BeginScrollView(_inner_area, scroll_pos, _content_area);
            else GUI.BeginClip(_inner_area);

            for (int i=0; i<children.Count; i++)
            {
                this.children[i].TryDisplay();
            }

            if (this.CanScroll)
            {
                GUI.EndScrollView(true);
                if( !Util.floatEq(scroll_pos.x, pre_scroll.x) || !Util.floatEq(scroll_pos.y, pre_scroll.y) )
                {
                    foreach (var child in this.children)
                    {
                        child.parent_scroll_updated();
                    }
                }
            }
            else GUI.EndClip();
        }
    }
}
