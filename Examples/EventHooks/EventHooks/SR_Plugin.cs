using SR_PluginLoader;
using UnityEngine;

namespace EventHooks
{
    public static class SR_Plugin
    {
        public static Plugin_Data PLUGIN_INFO = new Plugin_Data()
        {
            NAME = "Event Hooks Example",
            AUTHOR = "Sisco++",
            DESCRIPTION = @"
A collection of examples on how to use the EventHooks system included with the Plugin loader.
",
            VERSION = new Plugin_Version(1, 0)// v1.0
        };
        
        public static void Load(GameObject go)
        {
            // Add the actual class that handles our plugin's logic to our assigned GameObject.
            go.AddComponent<ExamplePlugin>();
        }

        public static void Unload()
        {
            // If we have any last words. Best to speak them now...
        }
    }
}
