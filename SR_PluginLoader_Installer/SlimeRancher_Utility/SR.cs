using System;
using Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
#if USING_CECIL
using Mono.Cecil;
#endif

namespace SlimeRancher
{
    public static class SR
    {
        public static string Get_Sha1_Filename(string file) { return String.Concat(file, ".sha1"); }
        public static string Get_Temp_Filename(string file) { return String.Concat(file, ".tmp"); }
        public static string Get_Backup_Filename(string file) { return String.Concat(file, ".bak"); }

        /// <summary>
        /// Completely removes the specified file and any of it's SHA1 hash files that might be laying around
        /// </summary>
        /// <param name="file"></param>
        public static void Purge_SR_File(string file)
        {
            Log.Debug("Purging file: {0}", file);
            string sha_file = Get_Sha1_Filename(file);
            if (File.Exists(sha_file)) File.Delete(sha_file);
            if (File.Exists(file)) File.Delete(file);
        }

        public static void Purge_SR_Sha_File(string file)
        {
            string sha_file = Get_Sha1_Filename(file);
            if (File.Exists(sha_file)) File.Delete(sha_file);
        }

        public static void Cache_File_Sha(string file)
        {
            string sha_file = Get_Sha1_Filename(file);
            File.WriteAllText(sha_file, Util.Git_File_Sha1_Hash(file));
        }

        public static void Reinstall_SR_Assembly(string file)
        {
            Log.Info("Steam will reinstall {0}", Path.GetFileName(file));
                
        // delete all file backups and hashes.
            string backup_file = Get_Backup_Filename(file);
            Purge_SR_File(file);
            Purge_SR_File(backup_file);


            Steam_Utility.GCV_STATE gcv = Steam_Utility.Get_GCV_State();
            if(gcv != Steam_Utility.GCV_STATE.NONE)
            {
                if (gcv == Steam_Utility.GCV_STATE.VALIDATING)
                {// Steam is still validating some other file, we have to wait for it
                    Log.Info("Steam is already validating other files, the installer must wait for it to finish.");
                    Console.Write("Waiting");
                    int wel = 0;
                    do
                    {
                        if (++wel > 3)
                        {
                            SR.ClearConsoleLine("Waiting");
                            wel = 0;
                        }
                        else Console.Write(".");
                        System.Threading.Thread.Sleep(300);
                        gcv = Steam_Utility.Get_GCV_State();
                    }
                    while (gcv == Steam_Utility.GCV_STATE.VALIDATING);
                    // okay so now it's done, lets make sure that last GCV window is closed before we continue
                    Steam_Utility.Get_GCV_Window().Close();
                }
            }

            int SR_AppID = 433340;
            // tell steam to redownload it now that we are watching for it.
            Steam_Utility.Validate_Game_Cache(SR_AppID);

            Log.Info("Starting GCV");
            int i = 4;
            int deltaTime = 250;
            int ellipse = 0;
            Console.CursorVisible = false;
            bool verified = false;// Have we been able to verify that the game cache validation started?

            while ( !File.Exists(file) )
            {
                int delta = (i * deltaTime);
                int sec = (delta / 1000);
                int secMod = (delta % 1000);

                if (!verified)
                {
                    Steam_Utility.GCV_STATE state = Steam_Utility.Get_GCV_State();
                    if (state == Steam_Utility.GCV_STATE.NONE)
                    {
                        Log.Debug("Retrying Steam GCV...");
                        Steam_Utility.Validate_Game_Cache(SR_AppID);
                    }
                    else
                    {
                        verified = true;
                        Log.Info("Steam Validation Started...");
                        Console.Write("Waiting");
                    }
                }
                else
                {
                    if ((i++ % 2) == 0)
                    {
                        if (++ellipse > 3)
                        {
                            SR.ClearConsoleLine("Waiting");
                            ellipse = 0;
                        }
                        else Console.Write(".");
                    }
                }

                System.Threading.Thread.Sleep(deltaTime);
            }
            Console.CursorVisible = true;
            SR.ClearConsoleLine();
            Console.WriteLine("...");//finish the line off
            Log.Info("Awaiting assembly release...");
            System.Threading.Thread.Sleep(1000);
            Log.Info("{0} HASH:  {1}", Path.GetFileName(file), Util.Git_File_Sha1_Hash(file));
            Steam_Utility.Get_GCV_Window().Close();// Be nice and close the annoying window from steam.
            Cache_File_Sha(file);
        }

        public static void ClearConsoleLine(string previous_text="")
        {
            int idx = previous_text.Length;
            if (idx < 0) idx += Console.CursorLeft;

            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(idx, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth-idx));
            Console.SetCursorPosition(idx, currentLineCursor);
        }

        /// <summary>
        /// Restores a specified file from it's backup copy
        /// </summary>
        public static void Restore_File(string file)
        {
            string backup_file = Get_Backup_Filename(file);
            File.Copy(backup_file, file, true);

            SR.Purge_SR_Sha_File(file);
        }
        
        /// <summary>
        /// Creates a temporary copy of 'file' and returns the path to it. 
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static string Create_Temp_File(string file)
        {
            string temp = Get_Temp_Filename(file);
            if (File.Exists(temp)) File.Delete(temp);
            File.Copy(file, temp, true);
            return temp;
        }
        
        /// <summary>
        /// Looks for a temporary version of 'file' and deletes it if found.
        /// </summary>
        /// <param name="file"></param>
        public static void Purge_Temp_File(string file)
        {
            string temp = Get_Temp_Filename(file);
            if (File.Exists(temp)) File.Delete(temp);
        }

        /// <summary>
        /// Creates a backup of the specified file
        /// </summary>
        public static void Backup_File(string file)
        {
            Log.Info("Creating backup: {0}", file);
            string backup_file = Get_Backup_Filename(file);
            File.Copy(file, backup_file, true);
            //File.WriteAllBytes(backup_file, File.ReadAllBytes(file));
        }

#if USING_CECIL
        public static void Ensure_Clean_Assembly_File(string dll_file, IAssemblyResolver assembly_resolver)
        {
            // First and foremost we want to check and see if the given assembly file's current hash matches what it was last time we interacted with it.
            // If it DOESN'T match that means another program changed it, likely steam, meaning the game was updated.
            // However; it could have been something other then steam so just to be sure we should reinstall the file from steam in that case.
            if (SR.File_Differs_From_Hash(dll_file))
            {
                Log.Warn("Detected external assembly file changes, a complete purge and reinstall of all required files will now take place.");
                // Okay we need to completely purge the main file and it's backups and reinstall it via steam
                SR.Purge_SR_File(dll_file);
                SR.Purge_SR_File(SR.Get_Backup_Filename(dll_file));
                SR.Reinstall_SR_Assembly(dll_file);
                // Now that we have a clean version of the assembly file, let's make a backup of it.
                SR.Backup_File(dll_file);
                SR.Cache_File_Sha(dll_file);
            }
            else
            {
                bool have_main = false, have_backup = false;
                string dll_backup_file = SR.Get_Backup_Filename(dll_file);

                if (File.Exists(dll_backup_file))
                {// Make sure the backup file is clean and doesn't reference the plugin loader already
                    have_backup = true;
                    if (Check_References_PluginLoader(dll_backup_file, assembly_resolver))
                    {
                        Log.Warn("Backup assembly contaminated.");
                        have_backup = false;// The backup assembly file is contaminated. trash it.
                        SR.Purge_SR_File(dll_backup_file);
                    }
                }
                else { SR.Purge_SR_Sha_File(dll_backup_file); }// Make sure to clean out the leftover sha file since we don't even have the old file now

                if (File.Exists(dll_file))
                {// Make sure the main assembly file is clean and doesn't reference the plugin loader yet
                    have_main = true;
                    if (Check_References_PluginLoader(dll_file, assembly_resolver))
                    {
                        Log.Warn("Main assembly contaminated.");
                        have_main = false;
                        SR.Purge_SR_File(dll_file);
                    }
                }
                else { SR.Purge_SR_Sha_File(dll_file); }// Make sure to clean out the leftover sha file since we don't even have the old file now


                // Here is where we take steps to make sure the assembly file is clean before we begin installing
                if (!have_main)
                {
                    if (have_backup)// We don't have the main assembly file BUT we do seem to have a backup of it, so let's use it!
                    {
                        SR.Restore_File(dll_file);
                    }
                    else// We have no main assembly file OR any backup for it, so get it from steam and then back it up
                    {
                        SR.Reinstall_SR_Assembly(dll_file);
                        SR.Backup_File(dll_file);
                    }
                }
                else
                {
                    if (!have_backup)// So we have a copy of the main assembly file but no backup of it, so let's make a backup...
                    {
                        //backup the original (clean) file
                        SR.Backup_File(dll_file);
                        SR.Cache_File_Sha(dll_backup_file);
                    }
                    else// The Main assembly file is clean AND we have a backup of it... we really don't need to do anything except contiue the installation
                    {
                    }
                }
            }
        }

        public static bool Check_References_PluginLoader(string dll_file, IAssemblyResolver assembly_resolver)
        {
            ModuleDefinition dll = null;
            FileStream stream = File.Open(dll_file, FileMode.Open, FileAccess.Read, FileShare.None);
            var parameters = new ReaderParameters { AssemblyResolver = assembly_resolver, ReadingMode = ReadingMode.Deferred };
            dll = ModuleDefinition.ReadModule(stream, parameters);
            stream.Close();
            //foreach (var asy in dll.AssemblyReferences) { Log.Debug("  Referenced: {0}  |  {1}", asy.FullName, asy.Name); }
            var asy = dll.AssemblyReferences.FirstOrDefault(r => String.Compare("SR_PluginLoader", r.Name) == 0);
            dll = null;
            return (asy != null);
        }
#endif


        public static bool File_Differs_From_Hash(string file)
        {
            if (!File.Exists(file))
            {
                Log.Error("File missing: {0}", file);
                return true;//yea it changed alright. it's friggin' GONE!
            }

            string sha_file = Get_Sha1_Filename(file);
            if (!File.Exists(sha_file)) return true;// if we didn't even have a sha file cached then, yes, it's different.

            string file_sha = Util.Git_File_Sha1_Hash(file);
            string file_sha_prev = File.ReadAllText(sha_file);

            bool diff = (String.Compare(file_sha, file_sha_prev) != 0);
            
            string fHash = (String.IsNullOrEmpty(file_sha) ? "N/A" : file_sha);
            string pHash = (String.IsNullOrEmpty(file_sha_prev) ? "N/A" : file_sha_prev);
            Log.Debug("Checking Hash for: {0}", file);
            Log.Debug("Current: {0}  |  Previous: {1}", fHash, pHash);

            return diff;
        }

        public static bool File_Sha_Exists(string file)
        {
            string sha_file = Get_Sha1_Filename(file);
            return File.Exists(sha_file);
        }
                
    }
}
