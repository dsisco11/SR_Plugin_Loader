using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Security;
using System.Security.Permissions;
using SR_PluginLoader;
using Logging;
using SlimeRancher;


namespace SR_PluginLoader_Installer
{
    enum Stage
    {
        NONE=0,
        HOOKS,
        VARIABLES,
        METHODS,
    }

    class Program : MarshalByRefObject
    {
        /// <summary>
        /// A map of all the current hashes for methods we are about to inject into within the game's assembly file
        /// </summary>
        public static Dictionary<string, string> EVENT_HOOK_HASHES = new Dictionary<string, string>();
        public static Dictionary<string, string> EVENT_HOOK_IL = new Dictionary<string, string>();

        /// <summary>
        /// A map of all hook injection location hashes that are different from what we expect.
        /// </summary>
        public static Dictionary<string, string> EVENT_HOOK_HASH_MISMATCHES = new Dictionary<string, string>();
        public static Dictionary<Stage, int[]> STATUS = new Dictionary<Stage, int[]>();
        public static ModuleDefinition loader_dll;

        public static string install_path = null;
        public static string assembly_file = "Assembly-CSharp.dll";
        public static string assembly_dll_file = null;
        public static string loader_file = "SR_PluginLoader.dll";
        public static string loader_dll_file = null;

        public static FileStream dll_stream = null;
        public static Stopwatch TIMER = null;
        public static Plugin_Version VERSION = null;

        public static Type HOOK_ID = null;
        public static DefaultAssemblyResolver assembly_resolver = new DefaultAssemblyResolver();
        public static bool IS_FAST_MODE = false;
        public static bool IS_UNINSTALLING = false;
        public static bool IS_IL_DUMP = false;

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomain_AssemblyResolve;

            if (args.ToList().Contains("-fast")) IS_FAST_MODE = true;
            if (args.ToList().Contains("-uninstall")) IS_UNINSTALLING = true;
            if (args.ToList().Contains("-ildump")) IS_IL_DUMP = true;

            Logger.showModuleNames = false;
            Logger.showTimestamps = false;

            if (IS_IL_DUMP)
            {
                Logger.Show_Log_Levels = false;
                Logger.Begin("ildump.cs", LogLevel.Warn, LogLevel.Info);
            }
            else {
                LogLevel clevel = LogLevel.Info;
                LogLevel fLevel = LogLevel.Debug;
                if (IS_FAST_MODE)
                {
                    fLevel = LogLevel.Success;
                    clevel = LogLevel.Success;
                }
                Logger.Begin("installer.log", clevel, fLevel);
            }

            foreach (Stage s in Enum.GetValues(typeof(Stage))) { STATUS.Add(s, new int[2] { 0, 0 }); }

            VERSION = SR_PluginLoader.PluginLoader_Info.VERSION;
            TIMER = new Stopwatch();
            TIMER.Start();

            Setup_Install_Path();
            Run_Program();
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("UnityEngine, ")) return Assembly.LoadFrom(Path.Combine(install_path, "UnityEngine.dll"));
            if (args.Name.StartsWith("Assembly-CSharp, ")) return Assembly.LoadFrom(Path.Combine(install_path, "Assembly-CSharp.dll"));

            return null;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
#if !DEBUG
            Exception ex = (Exception)e.ExceptionObject;
            PrintError(ex);
            Console.WriteLine("Press ANY key to exit");
            Console.ReadKey();
            Environment.Exit(0);
#endif
        }

        public Program()
        {
            Run_Program();
        }
        
        public static void Run_Program()
        {
            if (IS_IL_DUMP)
            {
                var dll = Load_Assembly_DLL(false);
                Log.Info("// Beginning complete dump of all targeted IL code...");
                foreach (Hook_Dbg_Data hook in SR_PluginLoader.HOOKS.HooksList)
                {
                    if (hook.name == null) throw new ArgumentNullException("Hook name cannot be NULL!");
                    int hook_ID = (int)hook.id;
                    string hook_name = hook.hook.ToString();

                    if ((int)hook.ext != 0) hook_name = hook.ext.ToString();

                    string[] spl = hook.name.Split('.');
                    string class_name = spl[0];
                    string func_name = spl[1];

                    TypeDefinition class_obj = dll.GetType(class_name);
                    if (class_obj == null) throw new Exception(String.Format("Cannot find SlimeRancher class(\"{0}\") for hook: {1}", class_name, hook_name));

                    MethodDefinition funct = class_obj.Methods.Single(o => o.Name == func_name);
                    if (funct == null) throw new Exception(String.Format("Cannot find injection site(\"{0}\") for hook: {1}", func_name, hook_name));

                    StringBuilder sb = new StringBuilder();
                    // write all the opcode bytes for the function into this memory stream
                    foreach (Instruction inst in funct.Body.Instructions)
                    {
                        sb.AppendLine(inst.ToString());
                    }

                    Log.Info("//  {0}\n/*\n{1}\n*/", hook.name, sb.ToString());
                }
            }
            else
            {
                string LOADER_NAME = XTERM.whiteBright(String.Format("Sisco's Plugin Loader {0}", XTERM.magentaBright(VERSION)));

                Log.Info("{0}: {1}", (IS_UNINSTALLING ? "Uninstalling" : "Installing"), LOADER_NAME);
                Log.Debug("Destination: {0}", install_path);

                if (IS_UNINSTALLING)
                {
                    SR.Reinstall_SR_Assembly(assembly_dll_file);
                }
                else
                {
                    //Wait_For_File(inject_dll_file);

                    Self_Copy();
                    Uninstaller_Copy();
                    ModuleDefinition dll = Load_Assembly_DLL();
                    Modify_Assembly_DLL(dll);

                    bool ok1 = Print_Status_Report(Stage.HOOKS, "Hooks Installed", new string[] { "Ignored", "Installed" });
                    bool ok2 = Print_Status_Report(Stage.VARIABLES, "Base Code Alterations", new string[] { "Failed", "Completed" });
                    bool ok3 = Print_Status_Report(Stage.METHODS, "Base Class Alterations", new string[] { "Failed", "Completed" });
                    bool ok4 = Verify_Hook_Hashes();

                    TIMER.Stop();
                    Log.Success(("Finished in " + XTERM.white() + "{0}ms!"), TIMER.ElapsedMilliseconds);
                    bool OK = false;

                    if (ok1 && ok2 && ok3)
                    {
                        OK = true;
                        if (!ok4)
                        {
                            Log.Warn("Some event hook hashes did not match what they should have.");
                            Log.Warn("This likely means that SlimeRancher's code has changed.");
                            Log.Warn("If you continue some plugins MIGHT not work correctly.");
                            Log.Warn("Would you like to install anyway?");
                            Console.Write("<Y/N>: ");
                            string resp = null;
                            do
                            {
                                resp = Console.ReadLine().ToLower();
                            } while (resp[0] != 'y' && resp[0] != 'n');

                            if (resp[0] == 'n') OK = false;
                        }
                    }

                    if (OK)
                    {
                        // Ok we are all good and everything installed just fine, so we can safely finalize the changes we made!
                        Finalize_DLL_Mods(dll, assembly_dll_file);
                        Log.Success("Installed {0}", LOADER_NAME);
                        Log.Success("Happy Sliming!");
                    }
                    else// FAILURE CONDITION
                    {
                        Revert_DLL_Mods(dll, assembly_dll_file);
                        Log.Info(XTERM.redBright("Failed to install {0}"), LOADER_NAME);
                    }
                }

                if (!IS_FAST_MODE)
                {
                    Console.WriteLine("Press ANY key to exit.");
                    Console.ReadKey();
                }
            }
        }

        public static bool Verify_Hook_Hashes()
        {
            if(EVENT_HOOK_HASH_MISMATCHES.Count > 0)
            {
                Log.Warn("WARNING: {0} hook injection points have changed since they were last verified!", EVENT_HOOK_HASH_MISMATCHES.Count);
                Log.Warn("Please verify that the following hooks install correctly, then update their new hashes in the safe list!");
                foreach(KeyValuePair<string, string> kvp in EVENT_HOOK_HASH_MISMATCHES.OrderBy(o => o.Key))
                {
                    Log.Warn(XTERM.whiteBright("{0}")+XTERM.yellow(" Hash Mismatch!\n\tExpected: {1}\n\tCurrent: {2}"), kvp.Key, kvp.Value, EVENT_HOOK_HASHES[kvp.Key]);
                    Log.Debug("==== IL CODE ====\n{0}", EVENT_HOOK_IL[kvp.Key]);
                }
            }

            return (EVENT_HOOK_HASH_MISMATCHES.Count == 0);
        }

        public static void Setup_Install_Path()
        {
            install_path = Steam_Utility.Get_Install_Dir();
            string[] dirs = assembly_resolver.GetSearchDirectories();
            foreach (string dir in dirs)
            {
                assembly_resolver.RemoveSearchDirectory(dir);
            }
            assembly_resolver.AddSearchDirectory(install_path);

            assembly_dll_file = String.Format("{0}\\{1}", install_path, assembly_file);
            loader_dll_file = String.Format("{0}\\{1}", install_path, loader_file);

            Wait_For_File(assembly_dll_file);
        }

        public static void Wait_For_File(string file)
        {
            Log.Debug("Waiting for file: {0}", file);
            if (!File.Exists(file)) return;
            Int32 start = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            Int32 dt = 0;

            while (dt < 10)//only wait for 60 seconds
            {
                Int32 now = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                dt = (now - start);
                try
                {
                    using (FileStream fs = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        return;//we were able to open the file, we are ok now so exit
                    }
                }
                catch(Exception ex)
                {
                    Log.Error(ex);
                }

                System.Threading.Thread.Sleep(5);
            }
        }

        public static bool Compare_Paths(string pathA, string pathB)
        {
            string A = Path.GetFullPath(pathA);
            string B = Path.GetFullPath(pathB);

            return string.Equals(A, B, StringComparison.OrdinalIgnoreCase);
        }

        public static void Self_Copy()
        {
            string file = Assembly.GetExecutingAssembly().Location;
            string dest = Path.GetFullPath(String.Format("{0}\\{1}", install_path, Path.GetFileName(file)));

            if (Compare_Paths(file, dest)) return;
            Log.Debug("Copying: {0} => {1}", Path.GetFileName(file), dest);

            File.Copy(file, dest, true);
            Log.Debug("Copied {0} => {1}", Path.GetFileName(file), dest);
        }

        public static void Uninstaller_Copy()
        {
            string file = String.Concat(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "/", "SR_PluginLoader_Uninstaller.exe");
            string dest = Path.GetFullPath(String.Format("{0}\\..\\..\\{1}", install_path, Path.GetFileName(file)));

            if (Compare_Paths(file, dest)) return;
            //Print("COPY: {0}\n{1}", file, dest);
            Log.Debug("Copying uninstaller to SlimeRancher install directory...");
            if (!File.Exists(file))
            {
                Log.Warn("Unable to locate PluginLoader Uninstaller!");
                return;
            }

            File.Copy(file, dest, true);
            Log.Debug("Copied {0} => {1}", Path.GetFileName(file), dest);
        }
        
#region Printing
        
        public static void PrintWarning(string format, params object[] args)
        {
            string str = String.Format(format, args);
            Log.Warn(str);
        }
        
        
        public static void PrintError(Exception ex)
        {
            string MSG = ex.Message;
            string TRACE = ex.StackTrace;
            if (ex.InnerException != null)
            {
                MSG = String.Format("{0} {1}", MSG, ex.InnerException.Message);
                TRACE = String.Format("{0}\n\nOutter Stack Trace: {1}", ex.InnerException.StackTrace, ex.StackTrace);
            }

            Log.Error("{0}\nStack Trace: {1}", MSG, TRACE);
        }
        
        public static void Print_Tagged(string tag, ConsoleColor tag_clr, string msg = null, ConsoleColor msg_clr = ConsoleColor.Gray)
        {
            Log.Info(String.Concat("[{0}] ", XTERM.Console_Color(msg_clr, msg)), XTERM.Console_Color(tag_clr, tag));
        }
        
        public static bool Print_Status_Report(Stage stage, string title, string[] state_str)
        {
            string topline = "---------------------------------------";
            int tlen = topline.Length;
            int pad = (tlen / 2);
            if ((pad * 2) != tlen)
            {
                pad++;
                topline += "-";
            }
            title = title.PadLeft(pad).PadRight(pad);

            Log.Info(topline);
            Log.Info(title);
            Log.Info();
            Print_Tagged(STATUS[stage][1].ToString(), ConsoleColor.Green, state_str[1], ConsoleColor.White);
            Print_Tagged(STATUS[stage][0].ToString(), ConsoleColor.Red, state_str[0], ConsoleColor.White);
            Log.Info();

            return (STATUS[stage][0] == 0);
        }
#endregion

        static ModuleDefinition Load_Assembly_DLL(bool ENSURE_ASSEMBLY_CLEAN = true)
        {
            Wait_For_File(assembly_dll_file);

            if (ENSURE_ASSEMBLY_CLEAN)
            {
                Log.Info("Checking Assembly...");
                SR.Ensure_Clean_Assembly_File(assembly_dll_file, assembly_resolver);
            }
            else
            {
                Log.Warn("Not enforcing clean assembly.");
            }

            // First thing we want to do is delete any instance of a temp version of this dll
            string dll_temp_file = SR.Create_Temp_File(assembly_dll_file);

            if (assembly_dll_file == null || assembly_dll_file.Length < 1) throw new Exception(String.Format("[CONFIG] Invalid DLL file: {0}", assembly_dll_file));

            // Okay, now let's open this dll and map all of the data we can for it: function names/addresses & variable names/addresses
            ModuleDefinition dll = null;
            try
            {
                Log.Debug("Loading Assembly: {0}", dll_temp_file);
                Log.Debug("Assembly Hash: {0}", SlimeRancher.Util.Git_File_Sha1_Hash(dll_temp_file));
                dll_stream = File.Open(dll_temp_file, FileMode.Open, FileAccess.Read, FileShare.None);
                var parameters = new ReaderParameters { AssemblyResolver = assembly_resolver, ReadingMode = ReadingMode.Deferred };
                dll = ModuleDefinition.ReadModule(dll_stream, parameters);
                dll_stream.Close();
                //dll = ModuleDefinition.ReadModule(dll_file, parameters);
            }
            catch (Exception ex)
            {
                Log.Error("Error loading target assembly DLL");
                Log.Error(ex);
                throw ex;
            }

            if (ENSURE_ASSEMBLY_CLEAN && Check_References_PluginLoader(dll))
            {
                // Reinstall the assembly file you DIP
                dll = null;
                //dll_stream.Close();
                dll_stream = null;

                SR.Reinstall_SR_Assembly(assembly_dll_file);
                return Load_Assembly_DLL();
            }

            return dll;
        }

        static void Modify_Assembly_DLL(ModuleDefinition dll)
        {
            Install_Loader_DLL();
            Do_Variable_Changes(ref dll);
            Do_Type_Changes(ref dll);
            Inject_DLL(ref dll);
        }

        public static bool Check_References_PluginLoader(ModuleDefinition dll)
        {
            //foreach (var asy in dll.AssemblyReferences) { Log.Debug("  Referenced: {0}  |  {1}", asy.FullName, asy.Name); }
            var asy = dll.AssemblyReferences.FirstOrDefault(r => String.Compare("SR_PluginLoader", r.Name) == 0);
            return (asy != null);
        }

        public static void Finalize_DLL_Mods(ModuleDefinition dll, string dll_file)
        {
            Log.Info("Finalizing changes...");
            string dll_temp_file = SR.Get_Temp_Filename(dll_file);
            dll.Write(dll_temp_file);// Save our modified dll to disk
            //dll_stream.Close();//close the stream

            // Now we can replace the original dll with the altered one!
            if (File.Exists(dll_file)) File.Delete(dll_file);
            File.Move(dll_temp_file, dll_file);
            SR.Cache_File_Sha(dll_file);
        }

        public static void Revert_DLL_Mods(ModuleDefinition dll, string dll_file)
        {
            Log.Warn("Reverting changes...");
            string dll_temp_file = SR.Get_Temp_Filename(dll_file);
            if (File.Exists(dll_temp_file)) File.Delete(dll_temp_file);
            SR.Restore_File(dll_file);
            SR.Cache_File_Sha(dll_file);
        }

        public static void Install_Loader_DLL()
        {
            string dest_file = loader_dll_file;
            string dll_file = String.Format("{0}\\{1}", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), loader_file);

            try
            {
                if (Compare_Paths(dll_file, dest_file)) return;
                Log.Debug("Copying: {0}", dest_file);

                if (File.Exists(dest_file)) File.Delete(dest_file);
                System.IO.File.Copy(dll_file, dest_file);
            }
            catch (Exception ex)
            {
                SLog.Error("Error when moving PluginLoader files to target directory");
                PrintError(ex);
                throw ex;
            }

        }

        public static void Inject_DLL(ref ModuleDefinition dll)
        {
            System.IO.FileStream loader_dll_stream = null;
            //string temp_fileName = String.Format("{0}.tmp", inject_dll_file);
            //File.Copy(inject_dll_file, temp_fileName, true);// Make a copy of the ST_PluginLoader.dll file real quick so we are able to open it (because the installer itself already has it open and is using it).
            try
            {
                Log.Debug("Loader Hash: {0}", SlimeRancher.Util.Git_File_Sha1_Hash(loader_dll_file));
                Log.Debug("Loading {0}\n\t\tFrom: {1}", Path.GetFileName(loader_dll_file), loader_dll_file);
                loader_dll_stream = new System.IO.FileStream(loader_dll_file, System.IO.FileMode.Open, FileAccess.Read);
                //dll_stream = new System.IO.FileStream(temp_fileName, System.IO.FileMode.Open, FileAccess.Read);
            }
            catch (Exception ex)
            {
                SLog.Error("Error opening file stream for target DLL");
                PrintError(ex);
                throw ex;
            }

            // Okay, now let's open this dll and map all of the data we can for it: function names/addresses & variable names/addresses
            try
            {
                var parameters = new ReaderParameters { AssemblyResolver = assembly_resolver, ReadingMode = ReadingMode.Immediate };
                loader_dll = ModuleDefinition.ReadModule(loader_dll_stream, parameters);
                loader_dll_stream.Close();//make sure to close the filestream so that when we resolve the functions from the dll we don't get an error!
                //File.Delete(temp_fileName);
            }
            catch (Exception ex)
            {
                SLog.Error("Error loading target assembly DLL");
                PrintError(ex);
                throw ex;
            }

            Inject_Mod_Loader(ref dll);
            Inject_SiscosHooks(ref dll);
        }

        private static void Inject_Mod_Loader(ref ModuleDefinition dll)
        {
            const string LOADER_FULLNAME = "SR_PluginLoader.Loader";
            TypeDefinition meth = loader_dll.GetType(LOADER_FULLNAME);
            MethodReference initFunc = null;
            if (meth != null)
            {
                meth = dll.ImportReference(meth).Resolve();
                initFunc = meth.Methods.Single(o => o.Name == "init");
                if (initFunc == null)
                {
                    SLog.Error("Unable to locate loader function!");
                    SLog.Error("Installation failed!");
                    return;
                }

                initFunc = dll.ImportReference(initFunc);
            }
            else Log.Error(new ArgumentNullException("Unable to resolve "+ LOADER_FULLNAME));

            Log.Debug("Injecting: {0}", loader_dll_file);
            
            TypeDefinition sndClass = dll.GetType("SECTR_AudioSystem");
            if (sndClass == null)
            {
                SLog.Error("Unable to locate injection site!");
                SLog.Error("Installation failed!");
                return;
            }

            MethodDefinition func = sndClass.Methods.Single(o => o.Name == "OnEnable");
            var il = func.Body.GetILProcessor();


            var loaderInitFunct = il.Create(OpCodes.Call, initFunc);
            int f = Find_Matching_Instruction(func.Body.Instructions, loaderInitFunct);
            if (f > -1)
            {
                throw new Exception("PluginLoader already installed, the installer should have replaced the assembly with a clean version before this message. REPORT THIS AS A BUG!");
            }
            else
            {
                // WE WANT TO INJECT OUR PLUGIN LOADER'S INIT RIGHT AFTER THE SECTR_AUDIOSYSTEM SETS IT'S 'system' field to 'this'
                FieldDefinition sndSystem = sndClass.Fields.Single(o => o.Name == "system");
                var search = il.Create(OpCodes.Stsfld, sndSystem);

                int idx = Find_Matching_Instruction(func.Body.Instructions, search);
                if (idx > -1)
                {
                    func.Body.Instructions.Insert(idx + 1, loaderInitFunct);
                    func.Body.Instructions.Insert(idx + 1, il.Create(OpCodes.Ldstr, SlimeRancher.Util.Git_File_Sha1_Hash(Program.loader_dll_file)));
                }
                else
                {
                    SLog.Error("Unable to calculate injection offset!");
                }
            }
        }

        private static void Inject_SiscosHooks(ref ModuleDefinition dll)
        {
            Hook_Dbg_Data[] inject_list = Load_Hooks_List();

            TypeDefinition shooks_class = loader_dll.GetType("SR_PluginLoader.SiscosHooks");
            MethodDefinition shook_call = null;

            if (shooks_class != null)
            {
                shooks_class = dll.ImportReference(shooks_class).Resolve();
                if(shooks_class == null)
                {
                    Prompts.Prompt_Exit("Unable to load SiscosHooks!");
                }

                shook_call = shooks_class.Methods.Single(o => o.Name == "call");
                if (shook_call != null)
                {
                    shook_call = dll.ImportReference(shook_call).Resolve();
                }
                else
                {
                    Prompts.Prompt_Exit("Unable to reference SiscosHooks event firing method!");
                }

#if DEBUG
                //var example = shooks_class.Methods.Single(o => o.Name == "Example");
                //TypeDefinition example_class = hooking_dll.GetType("SR_PluginLoader.Example");
#endif
            }
            else
            {
                SLog.Error("Unable to locate hooking library methods!");
                Prompts.Prompt_Exit();
            }

            
            Log.Debug("==========================================");
            Log.Debug("Reflection \"SR_PluginLoader.HOOK_ID\" results:");
            FieldInfo[] rFields = typeof(SR_PluginLoader.HOOK_ID).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach(FieldInfo fi in rFields)
            {
                Log.Debug("  - {0}", fi.Name);
            }
            Log.Debug("");
            Log.Debug("==========================================");
            


            TypeDefinition hooks_enum = Program.loader_dll.GetType("SR_PluginLoader.HOOK_ID");//get the enum
            if (hooks_enum == null)
            {
                SLog.Error("Unable to locate Hooks enum!");
                Prompts.Prompt_Exit();
                return;
            }
            hooks_enum = dll.ImportReference(hooks_enum).Resolve();
            Dictionary<string, FieldDefinition> hooks_map = new Dictionary<string, FieldDefinition>();

            Log.Debug("Pre-Mapping & Resolving all HOOK_ID field values.");
            foreach (var field in hooks_enum.Fields)
            {
                if (field != null)
                {
                    if (!String.IsNullOrEmpty(field.Name))
                    {
                        if (hooks_map.ContainsKey(field.Name.ToLower())) throw new Exception("Found duplicate field name in Hooks_Enum! Field:  Name("+field.Name+") FullName: "+field.FullName);

                        Log.Debug("   Resolving: {0}", field.FullName);
                        var fld = dll.ImportReference(field).Resolve();
                        hooks_map.Add(field.Name.ToLower(), fld);                        
                    }
                }
            }

            Log.Debug("{0} HOOK_ID field values Pre-Mapped & Resolved.", hooks_map.Keys.Count);
            
            if (inject_list == null) throw new ArgumentNullException("Unable to load event hooks list!");

            Hash_All_Injected_Functions(ref dll, inject_list);


            foreach (Hook_Dbg_Data hook in inject_list)
            {
                bool res = false;
                string hook_name = null;

                try
                {
                    if (hook.name == null) throw new ArgumentNullException("Hook name cannot be NULL!");
                    int hook_ID = (int)hook.id;
                    hook_name = hook.hook.ToString();

                    if ((int)hook.ext != 0) hook_name = hook.ext.ToString();

                    string[] spl = hook.name.Split('.');
                    string class_name = spl[0];
                    string func_name = spl[1];

                    TypeDefinition class_obj = dll.GetType(class_name);
                    if (class_obj == null) throw new Exception(String.Format("Cannot find SlimeRancher class(\"{0}\") for hook: {1}", class_name, hook_name));

                    MethodDefinition funct = class_obj.Methods.Single(o => o.Name == func_name);
                    if (funct == null) throw new Exception(String.Format("Cannot find injection site(\"{0}\") for hook: {1}", func_name, hook_name));

                    //FieldDefinition evnt = hooks_enum.Fields.FirstOrDefault(field => (field!=null && !String.IsNullOrEmpty(field.Name) && String.Compare(field.Name.ToLower(), hook_name.ToLower())==0));
                    FieldDefinition evnt = null;
                    if (!hooks_map.TryGetValue(hook_name.ToLower(), out evnt))
                    {
                        throw new Exception(String.Format("Cannot find injection hook_enum field reference (\"{0}\") for hook: {1}", hook_name, hook.name));
                    }
                    //else evnt = dll.ImportReference(evnt).Resolve();// Already done this when we pre-mapped al lof the enum's values above.

                    Log.Debug("Hooking {0}", hook_name);
                    res = CIL_Util.Hook_Function(loader_dll, funct, shooks_class, shook_call, evnt, hook_name, hook);
                    if (res == true)
                    {
                        STATUS[Stage.HOOKS][1]++;
                        Print_Tagged("OK", ConsoleColor.Green, hook_name);
                    }
                    else STATUS[Stage.HOOKS][0]++;
                }
                catch(Exception ex)
                {
                    Print_Tagged("BAD", ConsoleColor.Red, hook_name);
                    Log.Error("EXCEPTION WHILE INSTALLING HOOK: {0}", hook_name);
                    throw;
                }
            }
        }
        
        /// <summary>
        /// Hashes the code for all methods we will inject hooks into and makes sure none of them have changed since we last verified that our hooks inject into the correct spots.
        /// </summary>
        private static void Hash_All_Injected_Functions(ref ModuleDefinition dll, Hook_Dbg_Data[] hook_list)
        {
            foreach (Hook_Dbg_Data hook in hook_list)
            {
                bool res = false;
                string hook_name = null;

                try
                {
                    if (hook.name == null) throw new ArgumentNullException("Hook name cannot be NULL!");
                    int hook_ID = (int)hook.id;
                    hook_name = hook.hook.ToString();

                    if ((int)hook.ext != 0) hook_name = hook.ext.ToString();

                    string[] spl = hook.name.Split('.');
                    string class_name = spl[0];
                    string func_name = spl[1];

                    TypeDefinition class_obj = dll.GetType(class_name);
                    if (class_obj == null) throw new Exception(String.Format("Cannot find SlimeRancher class(\"{0}\") for hook: {1}", class_name, hook_name));

                    MethodDefinition funct = class_obj.Methods.SingleOrDefault(o => o.Name == func_name);
                    if (funct == null) throw new Exception(String.Format("Cannot find injection site(\"{0}\") for hook: {1}", func_name, hook_name));

                    if (!EVENT_HOOK_HASHES.ContainsKey(hook.name))
                    {
                        string hash = CIL_Util.Calc_Function_Hash(funct);
                        //string hash = CIL_Util.Calc_Function_Hash(funct);
                        Log.Debug("Injection Point: {0}  |  SHA1: {1}", hook.name, hash);
                        EVENT_HOOK_HASHES.Add(hook.name, hash);
                    }

                    if(!EVENT_HOOK_IL.ContainsKey(hook.name)) EVENT_HOOK_IL.Add(hook.name, CIL_Util.Get_Method_IL_String(funct));
                    else EVENT_HOOK_IL[hook.name] = CIL_Util.Get_Method_IL_String(funct);
                }
                catch (Exception ex)
                {
                    Print_Tagged("BAD", ConsoleColor.Red, hook.name);
                    Log.Error("EXCEPTION WHILE HASHING INJECTION LOCATION: {0}", hook.name);
                    throw;
                }
            }

            StringBuilder sb = new StringBuilder();
            Log.Debug("==== Current Hook SHA1 Map ====");
            sb.AppendLine("");
            foreach (KeyValuePair<string, string> kvp in EVENT_HOOK_HASHES.OrderBy(o => o.Key))
            {
                sb.AppendLine(String.Format("\t\t\t{{ \"{0}\", \"{1}\" }},", kvp.Key, kvp.Value));
            }
            Log.Debug(sb.ToString());
            Log.Debug("===============================");
            
            // Check each of the hashes we just computed and make sure they match what we expect of them.
            foreach(KeyValuePair<string, string> kvp in EVENT_HOOK_HASHES)
            {
                if (HOOK_SHAS.SAFE_HASHES.ContainsKey(kvp.Key))
                {
                    string safe_hash = HOOK_SHAS.SAFE_HASHES[kvp.Key];
                    string curr_hash = kvp.Value;
                    if(String.Compare(curr_hash, safe_hash) != 0)
                    {
                        EVENT_HOOK_HASH_MISMATCHES[kvp.Key] = safe_hash;// Copy the hash we EXPECTED to our mismatches list.
                    }
                }
                else
                {
                    EVENT_HOOK_HASH_MISMATCHES[kvp.Key] = null;// The value we EXPECTED to see goes into the mismatches list, adding NULL tells us that the hook is new and probably ok. just needs to be added to the list of safe hashes.
                }
            }
        }

        private static FieldDefinition Find_Enum_Field(TypeDefinition ENUM, string SearchStr)
        {
            //hooks_enum.Fields.FirstOrDefault(field => (field != null && !String.IsNullOrEmpty(field.Name) && String.Compare(field.Name.ToLower(), hook_name.ToLower()) == 0));
            foreach(var field in ENUM.Fields)
            {
                if(field != null)
                {
                    if(!String.IsNullOrEmpty(field.Name))
                    {
                        if (String.Compare(field.Name.ToLower(), SearchStr.ToLower()) == 0) return field;
                    }
                }
            }

            return null;
        }

        private static void Do_Type_Changes(ref ModuleDefinition dll)
        {
            foreach (VariableChange change in Variable_Alterations.types)
            {
                string[] spl = change.name.Split('.');
                string class_name = spl[0];
                string type_name = spl[1];
                string new_name = change.new_name;
                if (new_name == null) new_name = type_name;

                TypeDefinition method = dll.GetType(class_name);
                if (method == null)
                {
                    SLog.Error("Unable to locate class for: {0}", change.name);
                    continue;
                }

                TypeDefinition targ = method.NestedTypes.FirstOrDefault(o => o.Name == type_name);
                if (targ == null)
                {
                    SLog.Error("Unable to locate field for: {0}", change.name);
                    continue;
                }

                try
                {
                    switch (change.act)
                    {
                        case VarChange.PRIVATE_TO_PUBLIC:
                            if ((targ.Attributes & Mono.Cecil.TypeAttributes.NestedPrivate) > 0)
                            {
                                targ.Attributes &= ~Mono.Cecil.TypeAttributes.NestedPrivate;
                                targ.Attributes |= Mono.Cecil.TypeAttributes.NestedPublic;
                            }
                            break;
                        default:
                            throw new Exception(String.Format("Unimplemented action for: {0}", change.name));
                    }

                    STATUS[Stage.METHODS][1]++;// success
                }
                catch (Exception ex)
                {
                    STATUS[Stage.METHODS][0]++;// failure
                    PrintError(ex);
                }
            }

        }


        private static void Do_Variable_Changes(ref ModuleDefinition dll)
        {
            foreach (VariableChange change in Variable_Alterations.vars)
            {
                string[] spl = change.name.Split('.');
                string class_name = spl[0];
                string var_name = spl[1];
                string new_name = change.new_name;
                if (new_name == null) new_name = var_name;

                TypeDefinition method = dll.GetType(class_name);
                if (method == null)
                {
                    SLog.Error("Unable to locate class for: {0}", change.name);
                    continue;
                }

                FieldDefinition field = method.Fields.FirstOrDefault(o => o.Name == var_name);
                if (field == null)
                {
                    SLog.Error("Unable to locate field for: {0}", change.name);
                    continue;
                }

                try
                {
                    switch (change.act)
                    {
                        case VarChange.PRIVATE_TO_PUBLIC:
                            if ((field.Attributes & Mono.Cecil.FieldAttributes.Private) > 0)
                            {
                                field.Attributes &= ~Mono.Cecil.FieldAttributes.Private;
                                field.Attributes |= Mono.Cecil.FieldAttributes.Public;
                            }
                            break;
                        case VarChange.ADD_GETTER_SETTER:
                            CIL_Util.Add_Getter_Setter(ref method, ref field, new_name);
                            break;
                        case VarChange.ADD_GETTER:
                            CIL_Util.Add_Getter(ref method, ref field, new_name);
                            break;
                        case VarChange.ADD_SETTER:
                            CIL_Util.Add_Setter(ref method, ref field, new_name);
                            break;
                        case VarChange.ADD_GET_ACCESSOR:
                            CIL_Util.Add_Get_Accessor(ref method, ref field, change.function_name);
                            break;
                        case VarChange.ADD_SET_ACCESSOR:
                            CIL_Util.Add_Set_Accessor(ref method, ref field, change.function_name);
                            break;
                        default:
                            throw new Exception(String.Format("Unimplemented action for: {0}", change.name));
                    }

                    STATUS[Stage.VARIABLES][1]++;// success
                }
                catch (Exception ex)
                {
                    STATUS[Stage.VARIABLES][0]++;// failure
                    PrintError(ex);
                }
            }
        }

        public static Hook_Dbg_Data[] Load_Hooks_List()
        {
            Log.Debug("Examening hooks {0}", loader_dll_file);

            /*
            var inject_dll = Assembly.LoadFrom(inject_dll_file);
            Print("Inspecting...");
            HOOK_ID = inject_dll.GetType("SR_PluginLoader.HOOK_ID");

            Hook_Dbg_Data[] ret = new Hook_Dbg_Data[] { };
            //we load it from file because we might be referencing a newer one that was output to the slime rancher game folder directly, and thus is not the current one we have loaded.
            Type inj = inject_dll.GetType("SR_PluginLoader.HOOKS");
            if (inj != null)
            {
                var field = inj.GetField("HooksList", BindingFlags.Public | BindingFlags.Static);
                object val = field.GetValue(null);
                ret = (Hook_Dbg_Data[])val;
            }
            */
            Hook_Dbg_Data[] ret = SR_PluginLoader.HOOKS.HooksList;

            Log.Info("Preparing to install {0} hooks...", ret.Length);
            return ret;
        }


        private static int Find_Matching_Instruction(Mono.Collections.Generic.Collection<Instruction> inst, Instruction search)
        {
            for(int i=0; i<inst.Count; i++)
            {
                Instruction o = inst[i];
                if(Compare_Instructions(o, search))
                {
                    return i;
                }
            }

            return -1;
        }

        public static bool Compare_OpCodes(OpCode A, OpCode B)
        {
            if (A == null ^ B == null) return false;
            if (A.Code != B.Code) return false;

            return true;
        }

        public static bool Compare_Operands(object A, object B)
        {
            if (A == null ^ B == null) return false;
            if (A == B) return true;
            Type Aty = A.GetType();
            Type Bty = B.GetType();
            if (String.Compare(Aty.FullName, Bty.FullName) != 0) return false;
            if (Aty == typeof(Mono.Cecil.TypeReference))
            {
                var a = (TypeReference)A;
                var b = (TypeReference)B;
                if (String.Compare(a.FullName, b.FullName) != 0) return false;
            }
            else if (Aty == typeof(FieldReference))
            {
                var a = (FieldReference)A;
                var b = (FieldReference)B;
                if (String.Compare(a.FullName, b.FullName) != 0) return false;
            }
            else if (Aty == typeof(MethodReference))
            {
                var a = (MethodReference)A;
                var b = (MethodReference)B;
                if (String.Compare(a.FullName, b.FullName) != 0) return false;
            }
            else if (Aty == typeof(VariableDefinition))
            {
                var a = (VariableDefinition)A;
                var b = (VariableDefinition)B;
                if (a.VariableType.FullName == b.VariableType.FullName) return true;
                if (a.Name.Length > 0 && b.Name.Length > 0 && String.Compare(a.Name, b.Name) == 0) return true;

                return false;
            }
            else if (Aty == typeof(Instruction))
            {
                var a = (Instruction)A;
                var b = (Instruction)B;
                if (!Compare_OpCodes(a.OpCode, b.OpCode)) return false;
                if (!Compare_Operands(a.Operand, b.Operand)) return false;

                return true;
            }
            else
            {
                if (!A.Equals(B)) return false;
            }

            return true;
        }

        public static bool Compare_Instructions(Instruction A, Instruction B)
        {
            if (A == null ^ B == null) return false;
            if (A == B) return true;
            if (!Compare_OpCodes(A.OpCode, B.OpCode)) return false;
            if (!Compare_Operands(A.Operand, B.Operand)) return false;

            return true;
        }

    }
}
