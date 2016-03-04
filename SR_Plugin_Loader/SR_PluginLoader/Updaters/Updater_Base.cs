using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR_PluginLoader
{
    public enum FILE_UPDATE_STATUS
    {
        /// <summary>
        /// This file is perfectly up to date
        /// </summary>
        UP_TO_DATE=0,
        /// <summary>
        /// The file has an updated version available.
        /// </summary>
        OUT_OF_DATE,
        /// <summary>
        /// This file seems to be a developer compiled one, there is no history record for it. (mostly returned from github updaters)
        /// </summary>
        DEV_FILE
    }

    public abstract class Updater_Base
    {
        protected static readonly string USER_AGENT = "SR_Plugin_Loader on GitHub";

        public virtual FILE_UPDATE_STATUS Get_Update_Status(string local_file)
        {
            return FILE_UPDATE_STATUS.DEV_FILE;
        }
    }
}
