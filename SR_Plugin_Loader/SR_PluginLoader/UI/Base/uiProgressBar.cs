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

        /// <summary>
        /// The current progress to be displayed stored as a floating point value from 0.0 to 1.0
        /// </summary>
        public float progress { get { return _progress; } set { _progress = value; text.Text = String.Format("{0:0p}", value); update_progress_area(); onProgress?.Invoke(this, _progress, text.Text); } }
        protected float _progress = 0f;

        private uiText text;
        public GUIStyle text_style { get { return text.local_style; } set { text.local_style = value; } }

        public ProgressBar_Element bar;
        public GUIStyle bar_style { get { return bar.local_style; } set { bar.local_style = value; } }

        public bool show_progress_text { get { return _show_progress_text; } set { _show_progress_text = value; text.isVisible = value; } }
        private bool _show_progress_text = true;


        public uiProgressBar() : base(uiControlType.Progress)
        {
            text = Create<uiText>(this);
            text.Autosize_Method = AutosizeMethod.FILL;
            text.TextAlign = TextAnchor.MiddleCenter;
            Util.Set_BG_Color(local_style.normal, new Color(0f, 0f, 0f, 0.5f));

            bar = Create<ProgressBar_Element>(this);
            bar.FloodY();

            progress = 0f;
            update_progress_area();
        }

        protected void update_progress_area() { bar.Set_Width(Area.width * _progress); }

        public override void doLayout() { update_progress_area(); }
    }
}
