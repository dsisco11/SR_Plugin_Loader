using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SR_PluginLoader;

namespace ExtraControls
{
    public static class SR_Plugin
    {
        /*
        THIS EXAMPLE REQUIRES THAT THE USER HAVE 'SiscosHooks' INSTALLED IN ORDER TO RUN!
        */
        public static Plugin_Data PLUGIN_INFO = new Plugin_Data()
        {
			AUTHOR = "Sisco++",
            NAME = "Extra Controls",
            DESCRIPTION = @"
- Changes plort collectors so when using the vac-pak on them they will immediately suck out any loose plorts within their corral.
            ",
            VERSION = new Plugin_Version(0, 1)
        };

        public static void Load(GameObject gmObj)
        {
            gmObj.AddComponent<ExtraControls>();
        }

        public static void Unload(GameObject gmObj)
        {
        }

    }
}
