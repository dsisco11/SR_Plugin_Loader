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
        private bool maintains_aspect = true;
        public bool MaintainAspectRatio { get { return maintains_aspect; }  set { maintains_aspect = value;  update_area(); } }

        public Texture2D image { get { return (Texture2D)content.image; } set { content.image = value; update_size(); } }
        public override bool isDisplayed { get { if (image == null) { return false; } return base.isDisplayed; } set { base.isDisplayed = value; } } 
        protected override Vector2 size { get { if (image == null) { return new Vector2(0f, 0f); } return base.size; } }
        protected override Vector2 Get_Autosize(Vector2? starting_size = null)
        {
            if (image != null)
            {
                Vector2 csz = content_size_to_inner(new Vector2(image.width, image.height));
                // This control's only INTENDED content is an image, which should maintain it's aspect ratio, unless told not to.
                if (maintains_aspect)
                {
                    if (Explicit_H ^ Explicit_W)
                    {
                        if (Explicit_W) csz.y = (_size.x * (image.height / image.width));
                        else csz.x = (_size.y * (image.width / image.height));
                    }
                    else// If we don't have an explicit Width/Height given to us, then we find our smallest dimension and size according to it because the smaller one is the one being constrained by some factor.
                    {
                        if (csz.x < csz.y) csz.y = (csz.x * (image.height / image.width));
                        else csz.x = (csz.y * (image.width / image.height));
                    }
                }
                return base.Get_Autosize(csz);
            }
            return base.Get_Autosize(starting_size);
        }

        public uiIcon() : base(uiControlType.Icon)
        {
            Autosize = true;//auto size by default until the user gives us an explicit size
            Set_Background(Color.clear);
        }

        protected override void Display()
        {
            Display_BG();// Draw Background
            // Previously the DrawTexture call used 'draw_area' for the area, idk why, short sightedness probably.
            if(image != null) GUI.DrawTexture(inner_area, image, ScaleMode.ScaleToFit);
        }
    }
}
