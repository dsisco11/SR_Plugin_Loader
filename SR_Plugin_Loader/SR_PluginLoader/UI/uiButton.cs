using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    public class uiButton : uiControl
    {
        public string text { get { return this.content.text; } set { this.content.text = value; this.update_area(); } }


        public uiButton() : base(uiControlType.Button)
        {
            _typename = "uiButton";
            selfPadding = new RectOffset(4, 4, 2, 2);
            
            border.normal.color = new Color(1f, 1f, 1f, 0.1f);
            border.hover.color = new Color(1f, 1f, 1f, 0.3f);
            border.normal.stipple_size = 2;
        }

        protected override void Display()
        {
            base.Display();// Draw BG
            styleNoBG.Draw(inner_area, content, id);
        }
    }
}
