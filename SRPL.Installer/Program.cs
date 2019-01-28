using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Mono.Cecil;

namespace SRPL.Installer
{
    class Program
    {
        private const string ENTRY_POINT_TYPE = "SECTR_AudioSystem";
        private const string ENTRY_POINT_METHOD = "OnEnable";

        private static IAssemblyResolver asmResolver;

        private static void Main(string[] args)
        {
            asmResolver = new DefaultAssemblyResolver();
            
            // TODO: Add installer logic
        }

        /// <summary>
        /// Fetches the filepath of the Slime Rancher assembly, or prompts the user to provide it
        /// </summary>
        /// <returns>Absolute path of the Slime Rancher assembly</returns>
        private static string getAssemblyFilePath()
        {
            // TODO: Add Autodetection logic
            return @"D:\SteamLibrary\steamapps\common\Slime Rancher\SlimeRancher_Data\Managed\Assembly-Csharp.dll";
        }

        /// <summary>
        /// Fetches the filepath of the DLL to be injected into the game
        /// </summary>
        /// <returns>Absolute path of the Loader assembl</returns>
        private static string getLoaderFilePath()
        {
            // TODO: Don't assume loader DLL is in same directory as installer
            return AppDomain.CurrentDomain.BaseDirectory + "\\SRPL.dll";
        }

        /// <summary>
        /// Checks for access to the given file path. Will return true if the file exists, can be opened, and has Read/Write permissions
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>Whether the application has sufficient access to the file</returns>
        private static bool canOpenFile(string filePath)
        {
            try
            {
                using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    return true;
                }
            }
            catch (IOException)
            {
                // TODO: Log exception
                return false;
            }
        }
    }
}
