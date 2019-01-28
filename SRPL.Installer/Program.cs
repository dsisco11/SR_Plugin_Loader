using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

using SRPL.Logging;

namespace SRPL.Installer
{
    class Program
    {
        private const string ASSEMBLY_ENTRY_POINT_TYPE = "SECTR_AudioSystem";
        private const string ASSEMBLY_ENTRY_POINT_METHOD = "OnEnable";

        private const string LOADER_ENTRY_POINT_TYPE = "SRPL.Loader";
        private const string LOADER_ENTRY_POINT_METHOD = "Init";

        private static IAssemblyResolver asmResolver;

        private static void Main(string[] args)
        {
            Logger.Begin("installer.log");

            asmResolver = new DefaultAssemblyResolver();

            // TODO: Clean up installer logic
            // TODO: Add error logging
            string assemblyFilePath = getAssemblyFilePath();
            if (!canOpenFile(assemblyFilePath)) error("Could not open file " + assemblyFilePath);
            string loaderFilePath = getLoaderFilePath();
            if (!canOpenFile(loaderFilePath)) error("Could not open file " + loaderFilePath);

            // TODO: Backup Assembly DLL
            // Copy Loader Assembly to game directory
            string gamePath = getGameDirectory();
            File.Copy(loaderFilePath, gamePath + "\\SRPL.dll");
            loaderFilePath = gamePath + "\\SRPL.dll";
            if (!canOpenFile(loaderFilePath)) error("Could not open file " + loaderFilePath);

            // Load both modules
            FileStream assemblyFileStream = File.Open(assemblyFilePath, FileMode.Open, FileAccess.ReadWrite);
            ModuleDefinition assemblyModule = ModuleDefinition.ReadModule(assemblyFileStream, new ReaderParameters { AssemblyResolver = asmResolver, ReadingMode = ReadingMode.Immediate });
            FileStream loaderFileStream = File.Open(loaderFilePath, FileMode.Open, FileAccess.ReadWrite);
            ModuleDefinition loaderModule = ModuleDefinition.ReadModule(loaderFileStream, new ReaderParameters { AssemblyResolver = asmResolver, ReadingMode = ReadingMode.Immediate });

            // Find loader entry point type
            TypeDefinition loaderEntryPointType = loaderModule.GetType(LOADER_ENTRY_POINT_TYPE);
            if (loaderEntryPointType == null) error("Could not find entry point type in loader: " + LOADER_ENTRY_POINT_TYPE);
            // Find loader entry point method
            MethodReference loaderEntryPointMethod = loaderEntryPointType.Methods.FirstOrDefault(x => x.Name == LOADER_ENTRY_POINT_METHOD);
            if (loaderEntryPointMethod == null) error("Could not find entry point method in loader: " + LOADER_ENTRY_POINT_TYPE + "." + LOADER_ENTRY_POINT_METHOD);

            // Find assembly entry point type
            TypeDefinition assemblyEntryPointType = assemblyModule.GetType(ASSEMBLY_ENTRY_POINT_TYPE);
            if (assemblyEntryPointType == null) error("Could not find entry point type in game: " + ASSEMBLY_ENTRY_POINT_TYPE);
            // Find assembly entry point method
            MethodDefinition assemblyEntryPointMethod = assemblyEntryPointType.Methods.FirstOrDefault(x => x.Name == ASSEMBLY_ENTRY_POINT_METHOD);
            if (assemblyEntryPointMethod == null || !assemblyEntryPointMethod.HasBody) error("Could not find entry point method in game: " + ASSEMBLY_ENTRY_POINT_TYPE + "." + ASSEMBLY_ENTRY_POINT_METHOD);

            ILProcessor methodILProcessor = assemblyEntryPointMethod.Body.GetILProcessor();
            Instruction entryPoint = methodILProcessor.Create(OpCodes.Call, loaderEntryPointMethod);

            // Check if the entry point has already been injected
            if (findMatchingInstruction(assemblyEntryPointMethod.Body.Instructions, entryPoint) != -1) return;

            // Find the static field named "system"
            FieldDefinition sndSystem = assemblyEntryPointType.Fields.Single(o => o.Name == "system");
            // Create a new instruction that sets the value of the "system" static field
            // We'll be searching for this instruction in the method, rather than injecting it
            Instruction entryPointIndicator = methodILProcessor.Create(OpCodes.Stsfld, sndSystem);

            // Find the index of the instruction in the method
            int entryPointIndex = findMatchingInstruction(assemblyEntryPointMethod.Body.Instructions, entryPointIndicator);
            if (entryPointIndex < 0) return;

            assemblyEntryPointMethod.Body.Instructions.Insert(entryPointIndex + 1, entryPoint);
            // We should insert instructions to load any arguments we need here

            Logger.Info("Installer", "Installation complete");
            Console.ReadLine();
        }

        /// <summary>
        /// Returns the index of an instruction within a collection of instructions.
        /// Returns -1 if the collection does not contain the instruction
        /// </summary>
        /// <param name="instructions">The collection to search through</param>
        /// <param name="search">The instruction to search for</param>
        /// <returns></returns>
        private static int findMatchingInstruction(Mono.Collections.Generic.Collection<Instruction> instructions, Instruction search)
        {
            for (int i = 0; i < instructions.Count; i++)
            {
                Instruction o = instructions[i];
                if (compareInstructions(o, search))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Compares 2 instructions by value
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        private static bool compareInstructions(Instruction A, Instruction B)
        {
            if (A == null ^ B == null) return false;
            if (A == B) return true;
            if (!compareOpCodes(A.OpCode, B.OpCode)) return false;
            if (!compareOperands(A.Operand, B.Operand)) return false;

            return true;
        }

        private static bool compareOpCodes(OpCode A, OpCode B)
        {
            if (A == null ^ B == null) return false;
            if (A.Code != B.Code) return false;

            return true;
        }

        private static bool compareOperands(object A, object B)
        {
            if (A == null ^ B == null) return false;
            if (A == B) return true;
            Type Aty = A.GetType();
            Type Bty = B.GetType();
            if (string.Compare(Aty.FullName, Bty.FullName) != 0) return false;
            if (Aty == typeof(TypeReference))
            {
                var a = (TypeReference)A;
                var b = (TypeReference)B;
                if (string.Compare(a.FullName, b.FullName) != 0) return false;
            }
            else if (Aty == typeof(FieldReference))
            {
                var a = (FieldReference)A;
                var b = (FieldReference)B;
                if (string.Compare(a.FullName, b.FullName) != 0) return false;
            }
            else if (Aty == typeof(MethodReference))
            {
                var a = (MethodReference)A;
                var b = (MethodReference)B;
                if (string.Compare(a.FullName, b.FullName) != 0) return false;
            }
            else if (Aty == typeof(VariableDefinition))
            {
                var a = (VariableDefinition)A;
                var b = (VariableDefinition)B;
                if (a.VariableType.FullName == b.VariableType.FullName) return true;
                if (a.Name.Length > 0 && b.Name.Length > 0 && string.Compare(a.Name, b.Name) == 0) return true;

                return false;
            }
            else if (Aty == typeof(Instruction))
            {
                var a = (Instruction)A;
                var b = (Instruction)B;
                if (!compareOpCodes(a.OpCode, b.OpCode)) return false;
                if (!compareOperands(a.Operand, b.Operand)) return false;

                return true;
            }
            else
            {
                if (!A.Equals(B)) return false;
            }

            return true;
        }

        /// <summary>
        /// Fetches the filepath of the Slime Rancher assembly, or prompts the user to provide it
        /// </summary>
        /// <returns>Absolute path of the Slime Rancher assembly</returns>
        private static string getAssemblyFilePath()
        {
            // TODO: Add Autodetection logic
            return getGameDirectory() + @"\SlimeRancher_Data\Managed\Assembly-Csharp.dll";
        }

        /// <summary>
        /// Fetches the root directory of the game files
        /// </summary>
        /// <returns>Absolute install path of Slime Rancher</returns>
        private static string getGameDirectory()
        {
            // TODO: Add Autodetection logic
            return @"D:\SteamLibrary\steamapps\common\Slime Rancher";
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
            catch (Exception)
            {
                // TODO: Log exception
                return false;
            }
        }

        private static void error(string message)
        {
            error(new Exception(message));
        }
        private static void error(Exception ex)
        {
            Logger.Error("Installer", ex);
        }
    }
}
