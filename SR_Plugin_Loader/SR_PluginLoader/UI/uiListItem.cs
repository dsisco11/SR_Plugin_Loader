using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    /// <summary>
    /// An item that goes into a uiListView.
    /// Has: an icon, title, and description
    /// </summary>
    class uiListItem : uiPanel
    {
        protected uiText title = null, description = null;
        protected uiIcon icon = null;

        public string Title { get { return this.title.text; } set { this.title.text = value; } }
        public string Description { get { return this.description.text; } set { this.description.text = value; } }
        public Texture2D Icon { get { return this.icon.image; } set { this.icon.image = value; } }

        public bool Selected { get { return active; } set { active = value; } }
        public bool Selectable { get { return _selectable; } set { _selectable = value; if (!value) { Selected = false; } } }
        protected bool _selectable = true;


        public uiListItem() : base(uiControlType.ListItem)
        {
            this.autosize = true;
            padding = new RectOffset(2, 2, 2, 2);
            margin = new RectOffset(2, 2, 2, 0);
            Util.Set_BG_Color(local_style.normal, new Color32(32, 32, 32, 200));
            Util.Set_BG_Color(local_style.hover, new Color32(42, 42, 42, 255));
            Util.Set_BG_Color(local_style.active, new Color32(55, 55, 55, 255));

            border.active.color = new Color32(255, 255, 255, 200);

            icon = uiControl.Create<uiIcon>(this);
            icon.padding = new RectOffset(2, 2, 2, 2);
            icon.autosize = true;

            title = uiControl.Create<uiText>(this);
            title.local_style.fontSize = 16;
            title.local_style.fontStyle = FontStyle.Bold;

            description = uiControl.Create<uiText>(this);
            description.margin = new RectOffset(1, 1, 1, 1);
            description.local_style.fontSize = 12;
            description.local_style.fontStyle = FontStyle.Italic;
            description.local_style.normal.textColor = new Color32(255, 255, 255, 180);


            this.onClicked += (uiControl c) => { if (Selectable) { this.Selected = true; } };
        }

        protected override Vector2 Get_Autosize()
        {
            var sz = base.Get_Autosize();
            if(parent != null) sz.x = final_area_to_inner(parent.area).width;

            return sz;
        }

        public override void doLayout()
        {
            icon.alignTop();
            icon.alignLeftSide();
            if (Icon == null) icon.Set_Size(0f, 0f);
            else
            {
                icon.floodY();
                icon.Set_Width(icon.size.y);
            }

            title.alignTop();
            title.moveRightOf(icon);
            title.floodX();

            description.alignLeftSide();
            description.moveBelow(title);
        }
    }
}
