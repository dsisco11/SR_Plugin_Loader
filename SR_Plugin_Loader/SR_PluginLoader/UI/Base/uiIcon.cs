using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace SR_PluginLoader
{
    public class uiIcon : uiControl
    {
        public Texture2D image { get { return (Texture2D)content.image; } set { content.image = value; update_size(); } }
        public override Vector2 size { get { if (image == null) { return new Vector2(0f, 0f); } return base.size; } }
        protected override Vector2 Get_Autosize(Vector2? starting_size = null)
        {
            if (image != null) return base.Get_Autosize(content_size_to_inner(new Vector2(image.width, image.height)));
            return base.Get_Autosize(starting_size);
        }

        public uiIcon() : base(uiControlType.Icon)
        {
            Autosize = true;//auto size by default until the user gives us an explicit size
        }

        protected override void Display()
        {
            Display_BG();// Draw Background
            // Previously the DrawTexture call used 'draw_area' for the area, idk why, short sightedness probably.
            if(image != null) GUI.DrawTexture(inner_area, image, ScaleMode.ScaleToFit);
        }
    }
}
