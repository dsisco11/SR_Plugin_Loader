using UnityEngine;
using SR_PluginLoader;


namespace SlimeFilter
{
    public static class SR_Plugin
    {
        /*
        THIS EXAMPLE REQUIRES THAT THE USER HAVE 'SiscosHooks' INSTALLED IN ORDER TO RUN!
        */
        public static Plugin_Data PLUGIN_INFO = new Plugin_Data()
        {
			AUTHOR = "Sisco++",
            NAME = "SlimeFilter",
            DESCRIPTION = @"- Holding the ALT key will make the VacPak's suction ignore slimes.",
            VERSION = new Plugin_Version(1, 0)// v1.0
        };

        private static GameObject root;

        public static void Load()
        {
            SR_Plugin.root = new GameObject("SlimeFilterMod");
            SR_Plugin.root.AddComponent<SlimeFilter>();

            UnityEngine.Object.DontDestroyOnLoad(SR_Plugin.root);
        }

        public static void Unload()
        {
            UnityEngine.Object.Destroy(SR_Plugin.root);
        }

    }
}
