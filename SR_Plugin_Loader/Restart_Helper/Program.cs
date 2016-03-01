using System;
using System.Diagnostics;

namespace Restart_Helper
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("NO ARGUMENTS GIVEN");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Starting.");

            int target_id = Convert.ToInt32(args[0]);
            var proc = Process.GetProcessById(target_id);

            string target_exe = proc.MainModule.FileName;
            Console.WriteLine("Found process, terminating.");
            proc.Kill();
            Console.WriteLine("Waiting for exit.");
            proc.WaitForExit();
            Console.WriteLine("Restarting");

            proc.StartInfo.FileName = target_exe;
            proc.Start();
            
            Console.WriteLine("Done.");
        }
    }
}
