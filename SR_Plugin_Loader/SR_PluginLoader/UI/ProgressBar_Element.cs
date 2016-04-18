using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    public class ProgressBar_Element : uiControl
    {

        public ProgressBar_Element()
        {
            GUIStyle sty = new GUIStyle();
            sty.normal.background = Util.Get_Gradient_Texture(64, GRADIENT_DIR.TOP_BOTTOM, new Color(0.1f, 0.5f, 1.0f), new Color(1f, 1f, 1f, 1f), true, 0.3f);
            sty.normal.textColor = Color.white;

            this.Set_Style(sty);
        }
    }
}
