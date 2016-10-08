using Logging;
using System;
using System.Diagnostics;
using System.IO;

namespace SR_PluginLoader_Uninstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.showModuleNames = false;
            Logger.showTimestamps = false;
            Logger.Begin("uninstaller.log");

            string file = Path.GetFullPath("./SlimeRancher_Data/Managed/Assembly-CSharp.dll");
            Log.Info("Removing: {0}", file);
            if (!File.Exists(file))
            {
                Log.Info("  File not found!");
            }
            else
            {
                SlimeRancher.SR.Reinstall_SR_Assembly(file);
            }

            Log.Info();
            Log.Success("Plugin Loader uninstalled.");
            Console.WriteLine("Press ANY key to exit.");
            Console.ReadKey();
        }
        
    }
}
