using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    public class uiProgressBar : uiPanel
    {
        #region EVENTS
        public delegate void onProgressEvent(uiProgressBar c, float progress, string text);
        public event onProgressEvent onProgress;
        #endregion

        protected float _progress = 0f;
        /// <summary>
        /// The current progress to be displayed stored as a floating point value from 0.0 to 1.0
        /// </summary>
        public float progress { get { return this._progress; } set { this._progress = value; this.text.text = String.Format("{0:0}%", value*100.0f); this.update_progress_area(); if (onProgress != null) { onProgress(this, this._progress, this.text.text); } } }

        private uiText text;
        public GUIStyle text_style { get { return text.local_style; } set { text.local_style = value; } }

        public ProgressBar_Element bar;
        public GUIStyle bar_style { get { return bar.local_style; } set { bar.local_style = value; } }

        private bool _show_progress_text = true;
        public bool show_progress_text { get { return _show_progress_text; } set { _show_progress_text = value; this.text.visible = value; } }


        public uiProgressBar() : base(uiControlType.Progress)
        {
            text = Create<uiText>(this);
            text.local_style.alignment = TextAnchor.MiddleCenter;
            Utility.Set_BG_Color(local_style.normal, new Color(0f, 0f, 0f, 0.5f));

            bar = Create<ProgressBar_Element>(this);

            this.progress = 0f;
        }

        public void update_progress_area()
        {
            bar.area = new Rect(area.x, area.y, (area.width * _progress), area.height);
        }

        public override void doLayout()
        {
            this.update_progress_area();
            this.text.area = new Rect(0f, 0f, area.width, area.height);
        }

        protected override void Display()
        {
            base.Display();
            bar.TryDisplay();
        }
    }
}
