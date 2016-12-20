using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR_PluginLoader_Installer
{
    public static class Prompts
    {
        public static bool Prompt_Yes_or_No()
        {
            string yn_msg = "Please type 'Yes' or 'No' then press ENTER: ";
            Log.Info(yn_msg);
            string input = Console.ReadLine();

            while (input.TrimEnd(new char[] { '\n', '\r' }).ToLower().IndexOfAny(new char[] { 'y', 'n' }) < 0)
            {
                Log.Info("");
                Log.Info("Sorry, you entered an invalid response.");
                Log.Info(yn_msg);
                input = Console.ReadLine();
            }

            input = input.TrimEnd(new char[] { '\n', '\r' }).ToLower();
            return input.StartsWith("y");
        }

        public static void Prompt_Exit(string msg = null)
        {
            if (msg != null && msg.Length > 0) SR_PluginLoader.SLog.Error(msg);

            Log.Info("Press any key to exit.");
            Console.ReadKey();
            throw new Exception("The process exited prematurely.");
        }

    }
}
