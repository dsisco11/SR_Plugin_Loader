using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    class Plugin_Update_Item : uiPanel
    {
        private string _plugin_hash = null;
        public string plugin_hash { get { return _plugin_hash; } }
        public uiProgressBar progress_bar = null;
        private uiCheckbox checkbox = null;
        private uiText progress_text = null;

        public bool isChecked { get { return this.checkbox.isChecked; } set { this.checkbox.isChecked = value; } }


        public Plugin_Update_Item()
        {
            this._typename = "Plugin_Update_Item";
            this.autosize = true;
            this.onClicked += Plugin_Update_Item_onClicked;

            this.border.normal.color = new Color(1f, 1f, 1f, 0.1f);
            this.border.active.color = skin.settings.selectionColor;
            this.border.normal.size = new RectOffset(0, 0, 0, 1);

            Utility.Set_BG_Color(local_style.normal, new Color32(32, 32, 32, 200));

            progress_bar = Create<uiProgressBar>(this);
            progress_bar.show_progress_text = false;
            Utility.Set_BG_Color(progress_bar.bar_style.normal, new Color(0.1f, 0.4f, 0.8f, 0.7f));
            progress_bar.onProgress += Progress_bar_onProgress;

            checkbox = Create<uiCheckbox>(this);
            checkbox.label.local_style.fontSize = 16;

            progress_text = Create<uiText>(this);
            progress_text.local_style.fontSize = 11;
            progress_text.local_style.fontStyle = FontStyle.BoldAndItalic;
            //progress_text.local_style.normal.textColor = new Color(0.3f, 0.7f, 1.0f, 0.9f);
        }

        private void Progress_bar_onProgress(uiProgressBar c, float progress, string text)
        {
            if(progress_text != null) progress_text.text = text;
        }

        private void Plugin_Update_Item_onClicked(uiControl c)
        {
            if(checkbox != null) checkbox.isChecked = !checkbox.isChecked;
        }

        protected override Vector2 Get_Autosize()
        {
            var sz = base.Get_Autosize();
            if(parent != null) sz.x = parent.Get_Inner_Area().width;
            return sz;
        }

        public void Set_Plugin_Data(Plugin_Data data)
        {
            _plugin_hash = data.Hash;
            this.progress_bar.progress = 0f;
            checkbox.text = String.Format("<b>{0}</b>  <color=#BBBBBB>By</color> <i>{1}</i>", data.NAME, data.AUTHOR);
            update_area();
        }

        public override void doLayout()
        {
            progress_bar.alignLeftSide();
            progress_bar.floodXY();

            checkbox.alignLeftSide();
            checkbox.floodXY();
            checkbox.CenterY();

            progress_text.alignRightSide(5f);
            progress_text.Set_Height(Get_Inner_Area().height);
        }
    }
}
