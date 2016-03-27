using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace SR_PluginLoader
{
    public class uiToggle : uiControl
    {
        private string[] text = new string[2] { "Enable", "Disable" };

        /// <summary>
        /// The text to display when toggled off
        /// </summary>
        public string text_off { get { return text[0]; } set { text[0] = value; this.update_area(); } }

        /// <summary>
        /// The text to display when toggled on
        /// </summary>
        public string text_on { get { return text[1]; } set { text[1] = value; this.update_area(); } }

        private bool _checked = false;
        protected bool Checked { get { return _checked; } set { _checked = value; this.update_area(); if (onChange != null) { this.onChange(this, true); } } }
        public bool isChecked { get { return _checked; } set { _checked = value; this.update_area(); if (onChange != null) { this.onChange(this, false); } } }

        protected override bool isActive { get { return (this._checked || base.isActive); } }

        public delegate void uiToggle_changed_EventDelegate<T>(T c, bool was_clicked) where T : uiControl;
        public event uiToggle_changed_EventDelegate<uiToggle> onChange;

        private Color[] text_clr = new Color[2] { new Color(0.7f, 0.7f, 0.7f), new Color32(16, 16, 16, 255) };

        public uiToggle() : base(uiControlType.Button)
        {
            _typename = "uiToggle";
            selfPadding = new RectOffset(4, 4, 2, 2);
            this.onClicked += UiToggle_onClicked;

            //set the default text styling
            this.local_style.alignment = TextAnchor.MiddleCenter;
            this.local_style.fontStyle = FontStyle.Bold;
            this.local_style.fontSize = 16;
            
            //this.local_style.hover.textColor = new Color(1f, 1f, 1f);// text color for when the BG is black and the mouse is over

            // borders
            this.border.active.color = new Color32(0, 0, 0, 255);// black
            this.border.normal.color = new Color32(250, 160, 0, 180);// orange

            // background designs
            this.local_style.normal.background = Utility.Get_Gradient_Texture(64, GRADIENT_DIR.TOP_BOTTOM, 0.3f, 0.15f);// black
            this.local_style.active.background = Utility.Get_Gradient_Texture(64, GRADIENT_DIR.TOP_BOTTOM, 1.0f, 0.6f, new Color32(250, 160, 0, 255));// orange
        }
        
        private void UiToggle_onClicked(uiControl c)
        {
            this.Checked = !this.Checked;
        }

        public override void doLayout()
        {
            this.content.text = (_checked ? this.text_on : this.text_off);// this.text[_checked? 1:0];
            this.local_style.normal.textColor = (_checked? this.text_clr[1] : this.text_clr[0]);
            base.doLayout();
        }

        protected override void Display()
        {
            base.Display();// Draw BG
            styleNoBG.Draw(draw_area, content, this.isMouseOver || this.isActive, this.isActive, false, this.isFocused);//Draw text
        }

    }
}
