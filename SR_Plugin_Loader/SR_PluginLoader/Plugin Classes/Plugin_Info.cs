using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR_PluginLoader
{
    // DEVELOPERS NOTE:
    // Be very weary of altering the structure of this class as *any* changes will break backwards compatability with all plugins
    // until their creators get around to recompiling them
    // *IF* changes must be made to the base plugin data class it might be more prudent to Create a new class which inherits from it.
    public class Plugin_Data
    {
        public string AUTHOR = null;
        public string NAME = null;
        public string DESCRIPTION = null;
        public Plugin_Version VERSION = null;
        public List<Plugin_Dependency> DEPENDENCIES = new List<Plugin_Dependency>();
        public string UPDATES_URL = null;
        public Update_Method UPDATE_METHOD = null;

        public Plugin_Data()
        {
        }
    }
}
