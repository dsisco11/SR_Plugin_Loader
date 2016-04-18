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
        public Texture2D image { get { return (Texture2D)this.content.image; } set { this.content.image = value; this.update_area(); } }
        public override Vector2 size { get { if (image == null) { return new Vector2(0f, 0f); } return base.size; } }
        protected override Vector2 Get_Autosize()
        {
            if (image != null) return new Vector2(image.width, image.height);
            return base.Get_Autosize();
        }

        public uiIcon() : base(uiControlType.Icon)
        {
        }

        protected override void Display()
        {
            base.Display();
            if(this.image != null) GUI.DrawTexture(draw_area, this.image);
        }
        
    }
}
