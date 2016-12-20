
using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
#if LINUX
#else
using Microsoft.Win32;
#endif

namespace SlimeRancher
{
    public static class Steam_Utility
    {        
        public struct GCV_WINDOW
        {
            public IntPtr Handle;
            public string Title;
            public bool IsDone;

            public GCV_WINDOW(string title, IntPtr handle)
            {
                Title = title;
                Handle = handle;

                string TARGET_WIN = ("Validating Steam files - 100% complete").Replace(" ", "").ToLower();
                IsDone = (String.Compare(TARGET_WIN, title.ToLower().Replace(" ", "")) == 0);
            }

            public void Close()
            {
                if(Util.IsLinux)
                {
                }
                else if(Util.IsWindows)
                {
                    if (Handle != IntPtr.Zero) { SendMessage(Handle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero); }
                }
            }
        }

        public enum GCV_STATE { NONE=0, VALIDATING, DONE }

        private static string grabSteamRegistry()
        {
            string keypath = @"Software\Valve\Steam";
            RegistryKey key = Registry.CurrentUser.OpenSubKey(keypath);
            string registeredFilePath = key.GetValue("SteamPath").ToString();
            return registeredFilePath;
        }

        public static string Get_Install_Dir(bool force_manual = false)
        {
            string srDir = null;
            if (Util.IsLinux)
            {
                string steamDir = "~/.local/share/Steam";
                srDir = String.Format("{0}/SteamApps/common/Slime Rancher/", steamDir);
            }
            else if (Util.IsWindows)
            {
                string steamDir = grabSteamRegistry();
                //string cfgFile = String.Format("{0}/../config/config.vdf", steamDir);
                srDir = String.Format("{0}/SteamApps/common/Slime Rancher/", steamDir);
            }

            if (srDir == null || force_manual)
            {
                Log.Warn("Sorry, we're unable to find your slimerancher install directory.");
                Log.Warn("Please enter the path to your SlimeRancher's '.exe' file, or drag it's folder to this window and then hit <enter>.");
                srDir = Console.ReadLine();
            }
            else
            {
                //transform the path from relative to absolute.
                srDir = System.IO.Path.GetFullPath(srDir);

                if (!System.IO.File.Exists(String.Format("{0}\\SlimeRancher.exe", System.IO.Path.GetDirectoryName(srDir))))
                {
                    Log.Warn("Sorry, we're unable to find your slimerancher install directory.");
                    Log.Warn("Please enter the path to your SlimeRancher's '.exe' file, or drag it's folder to this window and then hit <enter>.");
                    srDir = Console.ReadLine();
                }
            }

            srDir = srDir.Trim(new char[] { '\"' });
            string dir = System.IO.Path.GetDirectoryName(srDir);
            if (System.IO.Directory.Exists(srDir)) dir = srDir;

            string srFile = String.Format("{0}\\SlimeRancher.exe", dir);

            while (srDir==null || srDir.Length<=0 || !System.IO.File.Exists(srFile))
            {
                if (srDir != null && srDir.Length > 0)
                {
                    Log.Warn(String.Format("Cannot find file: \"{0}\"", srFile));
                    Log.Warn("Please enter the path to your SlimeRancher's '.exe' file, or drag it's folder to this window and then hit <enter>.");
                }
                srDir = Console.ReadLine();
            }
            
            srDir = String.Format("{0}/SlimeRancher_Data/Managed/", dir);
            srDir = System.IO.Path.GetFullPath(srDir);


            // 
            return srDir.TrimEnd(new char[] { '\\', '/' });
        }


        public static List<KeyValuePair<IntPtr, string>> Get_Steam_Windows()
        {
            List<KeyValuePair<IntPtr, string>> swin = new List<KeyValuePair<IntPtr, string>>();
            if (Util.IsWindows)
            {
                int steamID = Process.GetProcessesByName("steam").First().Id;
                foreach (IntPtr handle in EnumerateProcessWindowHandles(steamID))
                {// Find the title for each of these windows belonging to steam
                    StringBuilder message = new StringBuilder(1000);
                    SendMessage(handle, WM_GETTEXT, message.Capacity, message);
                    swin.Add(new KeyValuePair<IntPtr, string>(handle, message.ToString()));
                }
            }
            else if(Util.IsLinux)
            {
            }

            return swin;
        }

        public static GCV_WINDOW Get_GCV_Window()
        {
            var swin = Steam_Utility.Get_Steam_Windows();
            string TARGET_WIN = ("Validating Steam files-").Replace(" ", "").ToLower();
            // Find any previously existing game cache validation windows, our validation command will NOT trigger if there is already one open!
            foreach (var kvp in swin)
            {
                string wtitle = kvp.Value.ToLower().Replace(" ", "");
                if (wtitle.StartsWith(TARGET_WIN)) { return new GCV_WINDOW(kvp.Value, kvp.Key); }
            }

            return new GCV_WINDOW("", IntPtr.Zero);
        }

        /// <summary>
        /// Returns the Steam's current GameCache Validation state
        /// </summary>
        public static GCV_STATE Get_GCV_State()
        {
            GCV_WINDOW win = Get_GCV_Window();
            if (win.Handle == IntPtr.Zero) return GCV_STATE.NONE;
            if (win.IsDone) return GCV_STATE.DONE;

            return GCV_STATE.VALIDATING;
        }

        public static void Validate_Game_Cache(int AppID)
        {
            if (Util.IsWindows)
            {
                GCV_WINDOW win = Get_GCV_Window();
                win.Close();
            }
            else if(Util.IsLinux)
            {
            }

            System.Threading.Thread.Sleep(300);

            Process proc = new Process();
            proc.StartInfo.FileName = String.Format("steam://validate/{0}", AppID);
            proc.Start();
        }

        #region LOW LEVEL CALLS

        private const uint WM_GETTEXT = 0x000D;
        private const uint WM_CLOSE = 0x0010;
        delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);


        private static IEnumerable<IntPtr> EnumerateProcessWindowHandles(int processId)
        {
            var handles = new List<IntPtr>();
            foreach (ProcessThread thread in Process.GetProcessById(processId).Threads)
                EnumThreadWindows(thread.Id, (hWnd, lParam) => {
                    handles.Add(hWnd); return true;
                }, IntPtr.Zero);
            return handles;
        }
        #endregion
    }
}
