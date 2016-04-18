using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace SR_PluginLoader
{
    /// <summary>
    /// This component just allows us to be more specific about the default behaviour of the control.
    /// even though any instance of a uiPanel may enable scrolling.
    /// </summary>
    public class uiScrollPanel : uiPanel
    {
        public uiScrollPanel()
        {
            this.CanScroll = true;
        }
    }
}
