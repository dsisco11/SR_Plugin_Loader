using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace SR_PluginLoader
{
    /// <summary>
    /// A two part text component which allows us to easily display a changing variable text value right next to an unchanging label text value which describes it.
    /// </summary>
    public class uiVarText : uiPanel
    {
        private uiText _label = null, _text = null;

        public string label { get { return _label.text; } set { _label.text = String.Format("{0} ", value); update_area(); } }
        public string text { get { return _text.text; } set { _text.text = value; update_area(); } }

        public GUIStyle label_style { get { return _label.local_style; } set { _label.local_style = value; } }
        public GUIStyle text_style { get { return _text.local_style; } set { _text.local_style = value; } }


        public uiVarText() : base(uiControlType.Label)
        {
            _label = Create<uiText>(this);
            _text = Create<uiText>(this);

            _text.sitRightOf(_label);
        }
        
    }
}
