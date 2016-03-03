using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR_PluginLoader
{
    public class Plugin_Data
    {
        public string AUTHOR = null;
        public string NAME = null;
        public string DESCRIPTION = null;
        public string UPDATEURL = null;
        public Plugin_Version VERSION = null;
        public List<Plugin_Dependency> DEPENDENCYS = new List<Plugin_Dependency>();

        public Plugin_Data()
        {
        }
    }
}
