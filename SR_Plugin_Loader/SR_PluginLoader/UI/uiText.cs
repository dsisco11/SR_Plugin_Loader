using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    public class uiText : uiControl
    {
        public string text { get { return content.text; } set { content.text = value; update_area(); } }

        public uiText() : base(uiControlType.Label)
        {
            this.selfPadding = new RectOffset(1, 1, 1, 1);
        }

        protected override Vector2 Get_Autosize()
        {
            if (content == null || content.text == null || content.text.Length <= 0) return new Vector2(0, 0);
            return base.Get_Autosize();
        }

        protected override void Display()
        {
            base.Display();// Draw BG
            styleNoBG.Draw(draw_area, content, this.isMouseOver, false, false, false);//Draw text
        }
    }
}
