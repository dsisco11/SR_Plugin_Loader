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
        public Plugin_Version VERSION = null;
        public List<Plugin_Dependency> DEPENDENCIES = new List<Plugin_Dependency>();
        public Update_Method UPDATE_METHOD = null;

        public Plugin_Data()
        {
        }

        public string Hash { get { return Util.SHA(String.Format("{0}.{1}", this.AUTHOR, this.NAME)); } }
        public static Plugin_Data FromJSON(SimpleJSON.JSONNode data)
        {
            return new Plugin_Data()
            {
                NAME = data["name"],
                AUTHOR = data["author"]
            };
        }
    }
}
