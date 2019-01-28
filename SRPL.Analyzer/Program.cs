using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace SRPL.Analyzer
{
    class Program
    {
        private static IAssemblyResolver asmResolver;

        static void Main(string[] args)
        {
            asmResolver = new DefaultAssemblyResolver();

            List<ModuleType> moduleTypes = new List<ModuleType>();

            string filePath = @"D:\SteamLibrary\steamapps\common\Slime Rancher\SlimeRancher_Data\Managed\Assembly-Csharp.dll";
            ModuleDefinition gameAssembly = ModuleDefinition.ReadModule(filePath, new ReaderParameters { AssemblyResolver = asmResolver, ReadingMode = ReadingMode.Immediate });
            foreach (TypeDefinition type in gameAssembly.Types)
            {
                List<string> extends = new List<string>();
                if (type.BaseType != null) extends.Add(type.BaseType.FullName);
                if (type.HasInterfaces)
                {
                    foreach (TypeReference inter in type.Interfaces)
                    {
                        extends.Add(inter.FullName);
                    }
                }
                List<string> fields = new List<string>();
                foreach (FieldDefinition field in type.Fields)
                {
                    string access = "";
                    if (field.IsPrivate) access = "private ";
                    else if (field.IsPublic) access = "public ";
                    if (field.IsStatic) access += "static ";
                    string fieldType = field.FieldType.FullName;
                    fields.Add(access + fieldType + " " + field.FullName.Split(':')[2]);
                }
                List<string> funcs = new List<string>();
                foreach (MethodDefinition func in type.Methods)
                {
                    string access = "";
                    if (func.IsPrivate) access = "private ";
                    else if (func.IsPublic) access = "public ";
                    if (func.IsStatic) access += "static ";
                    string funcType = func.ReturnType.FullName;
                    if (funcType.StartsWith("System.") && funcType.Split('.').Length < 3) funcType = funcType.Split('.')[1].ToLower();
                    string funcName = func.FullName.Split(':')[2];
                    string funcDec = funcType + " " + funcName;
                    if (funcName.StartsWith(".ctor"))
                    {
                        funcDec = type.FullName + funcName.Substring(5);
                    }
                    funcs.Add(access + funcDec);
                }
                moduleTypes.Add(new ModuleType(type.FullName, extends.ToArray(), fields.ToArray(), funcs.ToArray()));
            }

            Console.WriteLine("Completed index");
            while (true)
            {
                Console.WriteLine("Enter Query:");
                string input = Console.ReadLine();
                string output = "";
                if (input == "exit") break;

                if (!input.StartsWith("$"))
                {
                    if (input.Contains("."))
                    {
                        string type = input.Split('.')[0];
                        string func = input.Split('.')[1];
                        ModuleType foundModule = moduleTypes.FirstOrDefault(x => x.Name.ToLower() == type.ToLower());
                        if (foundModule.Name != null)
                        {
                            TypeDefinition typeDef = gameAssembly.Types.FirstOrDefault(x => x.FullName == foundModule.Name);
                            MethodDefinition methDef = typeDef.Methods.FirstOrDefault(x => x.Name.ToLower() == func.ToLower());
                            if (methDef != null)
                            {
                                string ops = "";
                                if (methDef.HasBody)
                                {
                                    foreach (Instruction op in methDef.Body.Instructions)
                                    {
                                        ops += "    " + op.ToString().Substring(9) + Environment.NewLine;
                                    }
                                }
                                output = foundModule.Funcs.First(x => x.ToLower().Contains(func.ToLower())) + " {" + Environment.NewLine;
                                output += ops;
                                output += "}" + Environment.NewLine;
                            }
                            else output = "Method not found";
                        }
                        else output = "Module not found";
                    }
                    else
                    {
                        ModuleType foundModule = moduleTypes.FirstOrDefault(x => x.Name.ToLower() == input.ToLower());
                        if (foundModule.Name == null) output = "Module not found";
                        else
                        {
                            output = foundModule.ToString() + Environment.NewLine;
                        }
                    }
                }
                else
                {
                    string command = input.Split(':')[0].Substring(1);
                    string query = input.Split(':')[1];

                    //input = command + "(\"" + query + "\")";
                    IEnumerable<ModuleType> foundModules;

                    switch (command)
                    {
                        case "StartsWith":
                            foundModules = moduleTypes.Where(x => x.Name.ToLower().StartsWith(query.ToLower()));
                            if (foundModules.Count() == 0) output = "No matches found";
                            else output = string.Join(Environment.NewLine, foundModules.Select(x => x.Name)) + Environment.NewLine;
                            break;
                        case "Extends":
                        case "Implements":
                            foundModules = moduleTypes.Where(x => x.Extensions.Select(e => e.ToLower()).Contains(query.ToLower()));
                            if (foundModules.Count() == 0) output = "No matches found";
                            else output = string.Join(Environment.NewLine, foundModules.Select(x => x.Name)) + Environment.NewLine;
                            break;
                    }
                }

                Console.Clear();
                Console.WriteLine("Query: " + input + Environment.NewLine);
                Console.WriteLine(output);
            }
        }
    }
}
