using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngineInternal;
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using SR_PluginLoader;
using SR_PluginLoader.SiscosHooks;

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
            NAME = "ExtraControls",
            DESCRIPTION = @"
- Holding the ALT key will make the VacPak's suction ignore slimes.
- Changes plort collectors so when using the vac-pak on them they will immediately suck out any loose plorts within their corral.
            ",
            VERSION = new Plugin_Version(0, 1)
        };
        private static GameObject root;

        public static void Load()
        {
            SR_Plugin.root = new GameObject("ExtraControlsMod");
            SR_Plugin.root.AddComponent<ExtraControls>();

            UnityEngine.Object.DontDestroyOnLoad(SR_Plugin.root);
            SR_Plugin.root.GetComponent<ExtraControls>().init();
        }

        public static void Unload()
        {
            UnityEngine.Object.Destroy(SR_Plugin.root);
        }

    }
}
