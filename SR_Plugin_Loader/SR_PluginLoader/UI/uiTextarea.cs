using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace SR_PluginLoader
{
    /// <summary>
    /// Multiline version of the uiText component.
    /// </summary>
    public class uiTextArea : uiText
    {

        public uiTextArea()
        {
            this.style.wordWrap = true;
        }

        protected override Vector2 Get_Autosize()
        {
            if (content == null || content.text == null || content.text.Length <= 0) return new Vector2(0, 0);
            //make sure text will not run outside of it's parent controls bounds.
            Vector2 sz = base.Get_Autosize();
            if (parent != null)
            {
                float pw = (parent.Get_Inner_Area().width - area.x);
                if (pw > 0f)
                {
                    sz.x = Mathf.Min(sz.x, pw);
                    sz.y = style.CalcHeight(content, sz.x);
                }
            }

            return sz;
        }
    }
}
