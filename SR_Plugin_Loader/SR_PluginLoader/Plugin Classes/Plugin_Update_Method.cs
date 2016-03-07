using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR_PluginLoader
{
    public class Update_Method
    {
        /// <summary>
        /// The url to look for update information at.
        /// </summary>
        public string URL = null;
        /// <summary>
        /// Which updater type should be used to detect updates for the plugin.
        /// See: 
        /// </summary>
        public UPDATER_TYPE METHOD = UPDATER_TYPE.NONE;


        public Update_Method(string url=null, UPDATER_TYPE method = UPDATER_TYPE.NONE)
        {
            this.URL = url;
            this.METHOD = method;
        }
    }
}
