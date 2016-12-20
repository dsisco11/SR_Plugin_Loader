using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logging;
using System.Reflection;
using SR_PluginLoader;
using SlimeRancher;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Injected_Hook_Decompiler
{
    class Program
    {
        public static string install_path = null;
        public static string assembly_file = "Assembly-CSharp.dll";
        public static string assembly_dll_file = null;

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomain_AssemblyResolve;

            Logger.Show_Log_Levels = false;
            Logger.showModuleNames = false;
            Logger.showTimestamps = false;
            Logger.Begin("hooks_decompiled.cs");

            //Log.Debug("// Args: {0}", args.Length);
            //for(int i=0; i<args.Length; i++) { string arg = args[i]; Log.Debug("// {0}", arg); }

            install_path = Steam_Utility.Get_Install_Dir();
            assembly_dll_file = Path.Combine(install_path, assembly_file);

            // Parse any cmdline args we were given
            foreach(string arg in args)
            {
                if (arg.StartsWith("-dll "))
                {
                    assembly_dll_file = arg.Split(new char[] { ' ' }, 2)[1];
                }
            }

            Log.Info("// Targeted Assembly DLL: {0}", assembly_dll_file);
            Log.Info("\n");

            Run_Program();
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("UnityEngine, ")) return Assembly.LoadFrom(Path.Combine(install_path, "UnityEngine.dll"));
            if (args.Name.StartsWith("Assembly-CSharp, ")) return Assembly.LoadFrom(Path.Combine(install_path, "Assembly-CSharp.dll"));

            return null;
        }



        private static void Run_Program()
        {
            DefaultAssemblyResolver resolver = new DefaultAssemblyResolver();
            resolver.AddSearchDirectory(install_path);

            ReaderParameters module_params = new ReaderParameters() { AssemblyResolver = resolver };
            ModuleDefinition dll = ModuleDefinition.ReadModule(assembly_dll_file, module_params);
            ICSharpCode.Decompiler.DecompilerContext Decompiler = new ICSharpCode.Decompiler.DecompilerContext(dll);
                        
            ICSharpCode.ILSpy.DecompilationOptions Options = new ICSharpCode.ILSpy.DecompilationOptions();
            ICSharpCode.ILSpy.CSharpLanguage Lang = new ICSharpCode.ILSpy.CSharpLanguage();

            Dictionary<string, string> CODE = new Dictionary<string, string>();

            foreach (Hook_Dbg_Data hook in SR_PluginLoader.HOOKS.HooksList)
            {
                if (CODE.ContainsKey(hook.name)) continue;

                ICSharpCode.Decompiler.PlainTextOutput TextOutput = new ICSharpCode.Decompiler.PlainTextOutput();
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

                Lang.DecompileMethod(funct, TextOutput, Options);
                CODE.Add(hook.name, String.Format("/*\n{0}\n*/\n\n{1}", sb.ToString(), TextOutput.ToString()));
            }


            foreach(KeyValuePair<string, string> kvp in CODE.OrderBy(o => o.Key))
            {
                Log.Info(kvp.Value);
            }
        }
    }
}
