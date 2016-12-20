using System;
using Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Runtime.Serialization.Formatters.Binary;
using SR_PluginLoader;
using Mono.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SR_PluginLoader_Installer
{
    public static class CIL_Util
    {
        // Convert an object to a byte array
        private static byte[] ObjectToByteArray(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new System.IO.MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        private static string BytesToStr(byte[] bytes)
        {
            var sb = new StringBuilder("[ ");
            foreach (var b in bytes)
            {
                sb.Append(b + ", ");
            }
            sb.Append("]");

            return sb.ToString();
        }

        public static void AddEmptyConstructor(TypeDefinition type, MethodReference baseEmptyConstructor)
        {
            /*
            var methodAttributes = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
            var method = new MethodDefinition(".ctor", methodAttributes, TypeSystem.Void);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Call, baseEmptyConstructor));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            type.Methods.Add(method);
            */
        }

        public static void Override_Property(ref FieldDefinition original_var, ref TypeDefinition class_obj, byte[] initial_value, bool static_return_value = false)
        {
            string propertyName = original_var.Name;
            TypeReference propertyType = original_var.FieldType;
            //Import the void type 
            //TypeReference voidRef = dll.Import(typeof(void).GetMethod( setter_name ));
            TypeReference voidRef = class_obj.Module.TypeSystem.Void;

            //define the field we store the value in 
            FieldDefinition field = new FieldDefinition(ConvertToFieldName(propertyName), Mono.Cecil.FieldAttributes.Private, propertyType);
            field.InitialValue = initial_value;
            class_obj.Fields.Add(field);

            MethodDefinition getter = new MethodDefinition("get_" + propertyName, Mono.Cecil.MethodAttributes.Public | Mono.Cecil.MethodAttributes.SpecialName | Mono.Cecil.MethodAttributes.HideBySig, propertyType);
            getter.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            getter.Body.Instructions.Add(Instruction.Create(OpCodes.Ldfld, field));
            getter.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc_0));
            Instruction inst = Instruction.Create(OpCodes.Ldloc_0);
            getter.Body.Instructions.Add(Instruction.Create(OpCodes.Br_S, inst));
            getter.Body.Instructions.Add(inst);
            getter.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            getter.Body.Variables.Add(new VariableDefinition("V_0", propertyType));
            getter.Body.InitLocals = true;
            getter.SemanticsAttributes = MethodSemanticsAttributes.Getter;

            class_obj.Methods.Add(getter);


            //Create the set method 
            MethodDefinition set = new MethodDefinition("set_" + propertyName, Mono.Cecil.MethodAttributes.Public | Mono.Cecil.MethodAttributes.SpecialName | Mono.Cecil.MethodAttributes.HideBySig, voidRef);

            set.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            set.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_1));
            set.Body.Instructions.Add(Instruction.Create(OpCodes.Stfld, field));
            set.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            set.Parameters.Add(new ParameterDefinition("value", Mono.Cecil.ParameterAttributes.None, propertyType));
            set.SemanticsAttributes = MethodSemanticsAttributes.Setter;
            class_obj.Methods.Add(set);


            //create the property 
            PropertyDefinition propertyDefinition = new PropertyDefinition(propertyName, 0, propertyType) { GetMethod = getter, SetMethod = set };

            //add the property to the type. 
            class_obj.Properties.Add(propertyDefinition);

        }

        public static void Lock_Value(ref FieldDefinition original_var, ref TypeDefinition class_obj, object value, bool static_return_value = false)
        {
            string propertyName = original_var.Name;
            TypeReference propertyType = original_var.FieldType;
            //Import the void type 
            //TypeReference voidRef = dll.Import(typeof(void).GetMethod( setter_name ));
            TypeReference voidRef = class_obj.Module.TypeSystem.Void;

            byte[] initial_value = ObjectToByteArray(value);
            //define the field we store the value in 
            FieldDefinition field = new FieldDefinition(ConvertToFieldName(propertyName), Mono.Cecil.FieldAttributes.Private, propertyType);
            field.Constant = value;
            field.InitialValue = initial_value;
            class_obj.Fields.Add(field);

            MethodDefinition getter = new MethodDefinition("get_" + propertyName, Mono.Cecil.MethodAttributes.Public | Mono.Cecil.MethodAttributes.SpecialName | Mono.Cecil.MethodAttributes.HideBySig, propertyType);
            getter.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            getter.Body.Instructions.Add(Instruction.Create(OpCodes.Ldfld, field));
            getter.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc_0));
            Instruction inst = Instruction.Create(OpCodes.Ldloc_0);
            getter.Body.Instructions.Add(Instruction.Create(OpCodes.Br_S, inst));
            getter.Body.Instructions.Add(inst);
            getter.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            getter.Body.Variables.Add(new VariableDefinition("V_0", propertyType));
            getter.Body.InitLocals = true;
            getter.SemanticsAttributes = MethodSemanticsAttributes.Getter;

            class_obj.Methods.Add(getter);


            //Create the set method 
            MethodDefinition set = new MethodDefinition("set_" + propertyName, Mono.Cecil.MethodAttributes.Public | Mono.Cecil.MethodAttributes.SpecialName | Mono.Cecil.MethodAttributes.HideBySig, voidRef);

            /*
            set.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            set.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_1));
            set.Body.Instructions.Add(Instruction.Create(OpCodes.Stfld, field));
            */
            set.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            set.Parameters.Add(new ParameterDefinition("value", Mono.Cecil.ParameterAttributes.None, propertyType));
            set.SemanticsAttributes = MethodSemanticsAttributes.Setter;
            class_obj.Methods.Add(set);


            //create the property 
            PropertyDefinition propertyDefinition = new PropertyDefinition(propertyName, 0, propertyType) { GetMethod = getter, SetMethod = set };

            //add the property to the type. 
            class_obj.Properties.Add(propertyDefinition);

        }

        public static void Add_Getter_Setter(ref TypeDefinition class_obj, ref FieldDefinition original_var, string new_name)
        {
            Add_Getter(ref class_obj, ref original_var, new_name);
            Add_Setter(ref class_obj, ref original_var, new_name);
            /*
            string propertyName = new_name;
            TypeReference propertyType = original_var.FieldType;
            FieldDefinition storage = original_var;

            //define the field we store the value in 
            if (String.Compare(original_var.Name, new_name) == 0)
            {
                storage = new FieldDefinition(String.Format("_{0}", new_name), Mono.Cecil.FieldAttributes.Private, propertyType);
                class_obj.Fields.Add(storage);
            }

            MethodDefinition getter = null;
            getter = new MethodDefinition("get_" + new_name, Mono.Cecil.MethodAttributes.Public | Mono.Cecil.MethodAttributes.SpecialName | Mono.Cecil.MethodAttributes.HideBySig, propertyType);
            getter.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            getter.Body.Instructions.Add(Instruction.Create(OpCodes.Ldfld, storage));
            getter.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc_0));
            Instruction inst = Instruction.Create(OpCodes.Ldloc_0);
            getter.Body.Instructions.Add(Instruction.Create(OpCodes.Br_S, inst));
            getter.Body.Instructions.Add(inst);
            getter.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            getter.Body.Variables.Add(new VariableDefinition("V_0", propertyType));
            getter.Body.InitLocals = true;
            getter.SemanticsAttributes = MethodSemanticsAttributes.Getter;
            class_obj.Methods.Add(getter);


            //Create the set method 
            MethodDefinition setter = new MethodDefinition("set_" + new_name, Mono.Cecil.MethodAttributes.Public | Mono.Cecil.MethodAttributes.SpecialName | Mono.Cecil.MethodAttributes.HideBySig, class_obj.Module.TypeSystem.Void);

            setter.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            setter.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_1));
            setter.Body.Instructions.Add(Instruction.Create(OpCodes.Stfld, storage));
            setter.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            
            setter.Parameters.Add(new ParameterDefinition("value", Mono.Cecil.ParameterAttributes.None, propertyType));
            setter.SemanticsAttributes = MethodSemanticsAttributes.Setter;
            class_obj.Methods.Add(setter);

            //create the property 
            PropertyDefinition propertyDefinition = new PropertyDefinition(propertyName, 0, propertyType) { GetMethod = getter, SetMethod = setter };

            //add the property to the type. 
            class_obj.Properties.Add(propertyDefinition);
            */

        }

        public static void Add_Getter(ref TypeDefinition class_obj, ref FieldDefinition original_var, string new_name)
        {
            string propertyName = new_name;
            TypeReference propertyType = original_var.FieldType;
            FieldDefinition storage = original_var;

            //define the field we store the value in 
            if (String.Compare(original_var.Name, new_name) == 0)
            {
                string vName = String.Format("_{0}", new_name);
                if (class_obj.Fields.FirstOrDefault(o => o.Name == vName) == null)
                {
                    storage = new FieldDefinition(vName, Mono.Cecil.FieldAttributes.Private, propertyType);
                    class_obj.Fields.Add(storage);
                }
            }

            MethodDefinition getter = null;
            getter = new MethodDefinition("get_" + new_name, Mono.Cecil.MethodAttributes.Public | Mono.Cecil.MethodAttributes.SpecialName | Mono.Cecil.MethodAttributes.HideBySig, propertyType);
            getter.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            getter.Body.Instructions.Add(Instruction.Create(OpCodes.Ldfld, storage));
            getter.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc_0));
            Instruction inst = Instruction.Create(OpCodes.Ldloc_0);
            getter.Body.Instructions.Add(Instruction.Create(OpCodes.Br_S, inst));
            getter.Body.Instructions.Add(inst);
            getter.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            getter.Body.Variables.Add(new VariableDefinition("V_0", propertyType));
            getter.Body.InitLocals = true;
            getter.SemanticsAttributes = MethodSemanticsAttributes.Getter;
            class_obj.Methods.Add(getter);


            //create or modify the property 
            PropertyDefinition propertyDefinition = class_obj.Properties.FirstOrDefault(o => o.Name == propertyName);
            if (propertyDefinition == null)
            {
                propertyDefinition = new PropertyDefinition(propertyName, 0, propertyType) { GetMethod = getter };
                //add the property to the type. 
                class_obj.Properties.Add(propertyDefinition);
            }
            else propertyDefinition.GetMethod = getter;
        }
        
        public static void Add_Setter(ref TypeDefinition class_obj, ref FieldDefinition original_var, string new_name)
        {
            string propertyName = new_name;
            TypeReference propertyType = original_var.FieldType;
            FieldDefinition storage = original_var;

            //define the field we store the value in 
            if (String.Compare(original_var.Name, new_name) == 0)
            {
                string vName = String.Format("_{0}", new_name);
                if (class_obj.Fields.FirstOrDefault(o => o.Name == vName) == null)
                {
                    storage = new FieldDefinition(vName, Mono.Cecil.FieldAttributes.Private, propertyType);
                    class_obj.Fields.Add(storage);
                }
            }

            //Create the set method 
            MethodDefinition setter = new MethodDefinition("set_" + new_name, Mono.Cecil.MethodAttributes.Public | Mono.Cecil.MethodAttributes.SpecialName | Mono.Cecil.MethodAttributes.HideBySig, class_obj.Module.TypeSystem.Void);

            setter.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            setter.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_1));
            setter.Body.Instructions.Add(Instruction.Create(OpCodes.Stfld, storage));
            setter.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));

            setter.Parameters.Add(new ParameterDefinition("value", Mono.Cecil.ParameterAttributes.None, propertyType));
            setter.SemanticsAttributes = MethodSemanticsAttributes.Setter;
            class_obj.Methods.Add(setter);

            //create or modify the property 
            PropertyDefinition propertyDefinition = class_obj.Properties.FirstOrDefault(o => o.Name == propertyName);
            if (propertyDefinition == null)
            {
                propertyDefinition = new PropertyDefinition(propertyName, 0, propertyType) { SetMethod = setter };
                //add the property to the type. 
                class_obj.Properties.Add(propertyDefinition);
            }
            else propertyDefinition.SetMethod = setter;
        }
        
        public static void Add_Get_Accessor(ref TypeDefinition class_obj, ref FieldDefinition original_var, string funcName)
        {
            TypeReference propertyType = original_var.FieldType;
            FieldDefinition storage = original_var;

            MethodDefinition method = null;
            method = new MethodDefinition(funcName, Mono.Cecil.MethodAttributes.Public, propertyType);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldfld, storage));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc_0));
            Instruction inst = Instruction.Create(OpCodes.Ldloc_0);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Br_S, inst));
            method.Body.Instructions.Add(inst);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            method.Body.Variables.Add(new VariableDefinition("V_0", propertyType));
            method.Body.InitLocals = true;
            class_obj.Methods.Add(method);
        }

        public static void Add_Set_Accessor(ref TypeDefinition class_obj, ref FieldDefinition original_var, string funcName)
        {
            TypeReference propertyType = original_var.FieldType;
            FieldDefinition storage = original_var;

            MethodDefinition method = null;
            method = new MethodDefinition(funcName, Mono.Cecil.MethodAttributes.Public, propertyType);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_1));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Stfld, storage));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            class_obj.Methods.Add(method);
        }

        public static void ChangeDefaultValue_InClass(TypeDefinition class_obj, string fieldName, object newValue)
        {

            foreach(var field in class_obj.Fields)
            {
                if( field.Name == fieldName )
                {
                    if (field.Constant != null)//its a constant! we can just change the value HERE!
                    {
                        field.Constant = newValue;
                        return;
                    }
                    MethodDefinition method = class_obj.Methods.Single(o => o.Name == ".ctor");
                    OpCode prevOpCode = Mono.Cecil.Cil.OpCodes.Nop;

                    switch (field.FieldType.MetadataType)
                    {
                        case MetadataType.Boolean:
                            ChangeDefaultBooleanValue_InMethod(method, fieldName, (bool)newValue);
                            return;
                        case MetadataType.String:
                            for (int i = 0; i < method.Body.Instructions.Count; i++)
                            {
                                var inst = method.Body.Instructions[i];
                                if (inst.Operand != null)
                                {
                                    Type ty = inst.Operand.GetType();
                                    if (ty.FullName == typeof(FieldDefinition).FullName)
                                    {
                                        if (inst.OpCode == Mono.Cecil.Cil.OpCodes.Stfld)
                                        {
                                            var f = (FieldDefinition)inst.Operand;
                                            if (f.FullName == field.FullName)
                                            {
                                                String str = new String(((string)newValue).ToArray());
                                                method.Body.Instructions[i].Previous.Operand = (string)newValue;
                                            }
                                        }
                                    }
                                }
                            }
                            return;
                        case MetadataType.Int32:
                            prevOpCode = Mono.Cecil.Cil.OpCodes.Ldc_I4;
                            break;
                        case MetadataType.Int64:
                            prevOpCode = Mono.Cecil.Cil.OpCodes.Ldc_I8;
                            break;
                        case MetadataType.Single:
                            prevOpCode = Mono.Cecil.Cil.OpCodes.Ldc_R4;
                            break;
                        case MetadataType.Double:
                            prevOpCode = Mono.Cecil.Cil.OpCodes.Ldc_R8;
                            break;
                    }

                    var il = method.Body.GetILProcessor();
                    foreach (var instruction in il.Body.Instructions)
                    {
                        if (instruction.OpCode == Mono.Cecil.Cil.OpCodes.Stfld)
                        {
                            var inst = (FieldDefinition)instruction.Operand;
                            if (inst.FullName == field.FullName)
                            {
                                var previnst = instruction.Previous;
                                if (previnst.OpCode == prevOpCode)
                                {
                                    switch (field.FieldType.MetadataType)
                                    {
                                        case MetadataType.Int32:
                                            previnst.Operand = Convert.ToInt32( newValue );
                                            break;
                                        case MetadataType.Int64:
                                            previnst.Operand = Convert.ToInt64(newValue);
                                            break;
                                        case MetadataType.Single:
                                            previnst.Operand = Convert.ToSingle(newValue);
                                            break;
                                        case MetadataType.Double:
                                            previnst.Operand = Convert.ToDouble(newValue);
                                            break;
                                    }
                                    return;
                                }
                            }
                        }
                    }//end code replacement

                }
            }

            throw new KeyNotFoundException(String.Format("Default value not found for '{0}'.", fieldName));
        }

        private static string ConvertToFieldName(string propertyName)
        {
            var fieldName = new System.Text.StringBuilder();
            fieldName.Append("_");
            fieldName.Append(propertyName[0].ToString().ToLower());
            if (propertyName.Length > 1)
                fieldName.Append(propertyName.Substring(1));

            return fieldName.ToString();
        }
        
        public static void ChangeDefaultValue_InMethod(MethodDefinition method, string fieldName, object newValue)
        {
            switch (Type.GetTypeCode(newValue.GetType()))
            {
                case TypeCode.Boolean:
                    ChangeDefaultBooleanValue_InMethod(method, fieldName, (bool)newValue);
                    break;
                case TypeCode.String:
                    ChangeDefaultStringValue_InMethod(method, fieldName, (string)newValue);
                    break;
                case TypeCode.Int32:
                    ChangeDefaultInt32Value_InMethod(method, fieldName, (Int32)newValue);
                    break;
                case TypeCode.Int64:
                    ChangeDefaultInt64Value_InMethod(method, fieldName, (Int64)newValue);
                    break;
                case TypeCode.Decimal://float32
                    ChangeDefaultFloat32Value_InMethod(method, fieldName, (Decimal)newValue);
                    break;
                case TypeCode.Double://float64
                    ChangeDefaultFloat64Value_InMethod(method, fieldName, (Double)newValue);
                    break;
            }

            throw new Exception(String.Format("Unhandled value type given for '{0}'", fieldName));
        }

        public static void ChangeDefaultStringValue_InMethod(MethodDefinition method, string oldString, string newString)
        {
            for (int i = 0; i < method.Body.Instructions.Count; i++)
            {
                var inst = method.Body.Instructions[i];
                if (inst.OpCode == Mono.Cecil.Cil.OpCodes.Ldstr && inst.Operand.ToString() == oldString)
                {
                    method.Body.Instructions[i].Operand = newString;
                }
            }
        }

        public static void ChangeDefaultBooleanValue_InMethod(MethodDefinition method, string fieldName, bool newValue)
        {
            var il = method.Body.GetILProcessor();
            foreach (var instruction in il.Body.Instructions)
            {
                if (instruction.OpCode == Mono.Cecil.Cil.OpCodes.Stsfld)
                {
                    var field = (FieldDefinition)instruction.Operand;
                    if (field.FullName == fieldName)
                    {
                        var previnst = instruction.Previous;
                        if (previnst.OpCode == Mono.Cecil.Cil.OpCodes.Ldc_I4_1 ||
                            previnst.OpCode == Mono.Cecil.Cil.OpCodes.Ldc_I4_0)
                        {
                            previnst.OpCode = newValue ? Mono.Cecil.Cil.OpCodes.Ldc_I4_1 : Mono.Cecil.Cil.OpCodes.Ldc_I4_0;
                            return;
                        }
                    }
                }
            }
            throw new KeyNotFoundException(String.Format("Default value not found for '{0}'.", newValue));
        }

        public static void ChangeDefaultInt32Value_InMethod(MethodDefinition method, string fieldName, int newValue)
        {
            var il = method.Body.GetILProcessor();
            foreach (var instruction in il.Body.Instructions)
            {
                if (instruction.OpCode == Mono.Cecil.Cil.OpCodes.Stsfld)
                {
                    var field = (FieldDefinition)instruction.Operand;
                    if (field.FullName == fieldName)
                    {
                        var previnst = instruction.Previous;
                        if (previnst.OpCode == Mono.Cecil.Cil.OpCodes.Ldc_I4)
                        {
                            previnst.Operand = newValue;
                            return;
                        }
                    }
                }
            }
            throw new KeyNotFoundException(String.Format("Default value not found for '{0}'.", newValue));
        }

        public static void ChangeDefaultInt64Value_InMethod(MethodDefinition method, string fieldName, Int64 newValue)
        {
            var il = method.Body.GetILProcessor();
            foreach (var instruction in il.Body.Instructions)
            {
                if (instruction.OpCode == Mono.Cecil.Cil.OpCodes.Stsfld)
                {
                    var field = (FieldDefinition)instruction.Operand;
                    if (field.FullName == fieldName)
                    {
                        var previnst = instruction.Previous;
                        if (previnst.OpCode == Mono.Cecil.Cil.OpCodes.Ldc_I8)
                        {
                            previnst.Operand = newValue;
                            return;
                        }
                    }
                }
            }
            throw new KeyNotFoundException(String.Format("Default value not found for '{0}'.", newValue));
        }

        public static void ChangeDefaultFloat32Value_InMethod(MethodDefinition method, string fieldName, Decimal newValue)
        {
            var il = method.Body.GetILProcessor();
            foreach (var instruction in il.Body.Instructions)
            {
                if (instruction.OpCode == Mono.Cecil.Cil.OpCodes.Stsfld)
                {
                    var field = (FieldDefinition)instruction.Operand;
                    if (field.FullName == fieldName)
                    {
                        var previnst = instruction.Previous;
                        if (previnst.OpCode == Mono.Cecil.Cil.OpCodes.Ldc_R4)
                        {
                            previnst.Operand = newValue;
                            return;
                        }
                    }
                }
            }
            throw new KeyNotFoundException(String.Format("Default value not found for '{0}'.", newValue));
        }

        public static void ChangeDefaultFloat64Value_InMethod(MethodDefinition method, string fieldName, Double newValue)
        {
            var il = method.Body.GetILProcessor();
            foreach (var instruction in il.Body.Instructions)
            {
                if (instruction.OpCode == Mono.Cecil.Cil.OpCodes.Stsfld)
                {
                    var field = (FieldDefinition)instruction.Operand;
                    if (field.FullName == fieldName)
                    {
                        var previnst = instruction.Previous;
                        if (previnst.OpCode == Mono.Cecil.Cil.OpCodes.Ldc_R8)
                        {
                            previnst.Operand = newValue;
                            return;
                        }
                    }
                }
            }
            throw new KeyNotFoundException(String.Format("Default value not found for '{0}'.", newValue));
        }

        public static TypeReference MakeGenericType(this TypeReference self, params TypeReference[] arguments)
        {
            if (self.GenericParameters.Count != arguments.Length)
                throw new ArgumentException();

            var instance = new GenericInstanceType(self);
            foreach (var argument in arguments)
                instance.GenericArguments.Add(argument);

            return instance;
        }

        public static MethodReference MakeGeneric(this MethodReference self, params TypeReference[] arguments)
        {
            var reference = new MethodReference(self.Name, self.ReturnType)
            {
                DeclaringType = self.DeclaringType.MakeGenericType(arguments),
                HasThis = self.HasThis,
                ExplicitThis = self.ExplicitThis,
                CallingConvention = self.CallingConvention,
            };

            foreach (var parameter in self.Parameters)
                reference.Parameters.Add(new ParameterDefinition(parameter.ParameterType));

            foreach (var generic_parameter in self.GenericParameters)
                reference.GenericParameters.Add(new GenericParameter(generic_parameter.Name, reference));

            return reference;
        }


        public static void Block_Function(MethodDefinition funct)
        {
            //funct.Body.Instructions.Clear();
            /*
            for(int i=0; i<funct.Body.Instructions.Count; i++)
            {
                funct.Body.Instructions[i] = Instruction.Create(OpCodes.Nop);
            }
            */
            funct.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Ret));
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
            if(Aty == typeof(Mono.Cecil.TypeReference))
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

        private static bool DuplicateChecker(MethodDefinition funct, List<Instruction> instBuf, int pos, bool reverse_search, Instruction callHook)
        {
            if(reverse_search == true)
            {
                int di = (funct.Body.Instructions.Count - instBuf.Count);
                //pos += di;
                pos -= instBuf.Count;
                for (int i = instBuf.Count-1; i>=0 ; i--)
                {
                    int idx = (i + pos);
                    if (idx >= funct.Body.Instructions.Count) return false;
                    Instruction curr = funct.Body.Instructions[idx];
                    Instruction inst = instBuf[i];
                    //if (!Compare_Instructions(curr, inst)) return false;
                    if (Compare_Instructions(curr, callHook)) return true;
                }
            }
            else
            {
                for(int i=0; i<instBuf.Count; i++)
                {
                    int idx = (i + pos);
                    if (idx >= funct.Body.Instructions.Count) return false;
                    Instruction curr = funct.Body.Instructions[idx];
                    Instruction inst = instBuf[i];
                    //if (!Compare_Instructions(curr, inst)) return false;
                    if (Compare_Instructions(curr, callHook)) return true;
                }
            }

            return false;
        }

        public static bool Funct_Returns_Value(MethodDefinition funct)
        {
            var voidTypeReference = funct.Module.GetTypeReferences().Single(t => t.FullName == "System.Void");
            bool has_type = (String.Compare(funct.ReturnType.FullName, voidTypeReference.FullName) != 0);
            return has_type;
        }

        public static int Find_Return_Point(MethodDefinition funct)
        {
            int idx = (funct.Body.Instructions.Count - 1);
            if (Funct_Returns_Value(funct))
            {
                //ok, any method that returns a VALUE will load that value to the stack and then immediately call the 'ret' opcode. so we just skip 2 instructions from the end instead of 1.
                idx -= 1;
            }

            return idx;
        }

        public static bool Should_Nuke_Ret(MethodDefinition funct)
        {
            if ( Funct_Returns_Value(funct) )
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Intelligently clips out the final return logic for a function so it can be replicated later after custom code is injected in it's place in a way that prevents interference from branch conditions which might nullify the injected code.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static List<Instruction> Replace_Return_Statement(MethodDefinition func, VariableDefinition resultVar, ref int POS)
        {
            // Alter the return instruction to store the stack value in a variable we can save for later and then return.
            if (null != func.Body.Instructions.Last().Operand)
            {
                Program.PrintWarning("When replacing function return the instruction operand was not NULL!\n\t{0}", func.Body.Instructions.Last().Operand);
            }

            var il = func.Body.GetILProcessor();
            List<Instruction> buf = new List<Instruction>();

            if (Funct_Returns_Value(func))
            {
                func.Body.Instructions.Last().OpCode = OpCodes.Stloc;
                func.Body.Instructions.Last().Operand = resultVar;
                // if the function wasnt already casting this value to a type then we should do it to avoid confusion in the compiler.
                if(func.Body.Instructions.Last().Previous.OpCode != OpCodes.Box)
                {
                    il.InsertBefore(func.Body.Instructions.Last(), il.Create(OpCodes.Box, func.ReturnType));
                    POS += 1;// Move the injection position down 1 so we don't screw it all up...
                }
                // Now we make a list of instructions to load this variable up later and return it's value.
                buf.Add(il.Create(OpCodes.Ldloc, resultVar));
                buf.Add(il.Create(OpCodes.Unbox_Any, func.ReturnType));
                buf.Add(il.Create(OpCodes.Ret));
                return buf;
            }
            else // Change the return instruction to a NOP
            {
                func.Body.Instructions.Last().OpCode = OpCodes.Nop;
                func.Body.Instructions.Last().Operand = null;
            }


            buf.Add(il.Create(OpCodes.Ret));
            return buf;
        }


        public static void Make_Branches_Unique(MethodDefinition func, IList<Instruction> branches)
        {
            // Make sure we make each branch unique in REVERSE order or else all of the branches will be made to exit in a cascading manner and it will produce some REALLY screwed up logic.
            foreach(Instruction br in branches.OrderBy(i => i.Offset).Reverse()) { Make_Branch_Unique(func, br); }
        }

        /// <summary>
        /// Rewrites a branch statement so it exits at a new instruction right below the one the previously exited at.
        /// This is a technique used to circumvent cases where multiple UNRELATED branching statements exit at the same location and prevent injected code from locating itself outside those unrelated conditionals.
        /// The branches that should be rewritten will be all of the ones below the specified branch which currently lead to the same exit point. (single-line conditionals with multiple logical checks)
        /// </summary>
        /// <returns></returns>
        public static void Make_Branch_Unique(MethodDefinition func, Instruction branch)
        {
            var il = func.Body.GetILProcessor();
            Instruction myExit = ((Instruction)branch.Operand);
            Instruction nExit = il.Create(OpCodes.Nop);//the new exit location

            // We want to insert this new exit pos BEFORE the old one because the old one occurs AFTER the branches conditional logic, so that point needs to remain outside that logic.
            il.InsertBefore(myExit, nExit);
            branch.Operand = nExit;

            /*
            // Okay now we want to insert a copy of our exit point instruction RIGHT before it.
            il.InsertBefore(myExit, nExit);

            int brIdx = func.Body.Instructions.IndexOf(branch);
            int exIdx = func.Body.Instructions.IndexOf(myExit);
            // Remember to not bother looking PAST the exit point, we don't care about branches that jump BACK
            for (int i=brIdx; i<=exIdx; i++)
            {
                Instruction inst = func.Body.Instructions[i];
                if(inst.OpCode.FlowControl == FlowControl.Branch || inst.OpCode.FlowControl == FlowControl.Cond_Branch)
                {// We found another branch instruction, does it exit at the same spot we do?
                    Instruction ex = ((Instruction)inst.Operand);
                    if (ex.Offset == myExit.Offset) inst.Operand = nExit;
                }
            }
            */
        }

        public static IList<Instruction> Get_Conditional_Branches_Exiting_At(MethodDefinition func, Instruction exitLoc)
        {
            List<Instruction> Branches = new List<Instruction>();
            for (int i = 0; i<func.Body.Instructions.Count; i++)
            {
                Instruction inst = func.Body.Instructions[i];
                if (inst.OpCode.FlowControl == FlowControl.Cond_Branch)
                {// We found another branch instruction, does it exit at the same spot we do?
                    Instruction ex = ((Instruction)inst.Operand);
                    if (ex.Offset == exitLoc.Offset) Branches.Add(inst);
                }
            }

            return Branches;
        }

        public static int Get_Number_Of_Branches_Exiting_At(MethodDefinition func, Instruction exitLoc)
        {
            int Total = 0;
            for (int i = 0; i <= func.Body.Instructions.Count; i++)
            {
                Instruction inst = func.Body.Instructions[i];
                if (inst.OpCode.FlowControl == FlowControl.Branch || inst.OpCode.FlowControl == FlowControl.Cond_Branch)
                {// We found another branch instruction, does it exit at the same spot we do?
                    Instruction ex = ((Instruction)inst.Operand);
                    if (ex.Offset == exitLoc.Offset) Total++;
                }
            }

            return Total;
        }

        public static int Get_Number_Of_Conditional_Branches_Exiting_At(MethodDefinition func, Instruction exitLoc)
        {
            int Total = 0;
            for (int i = 0; i<func.Body.Instructions.Count; i++)
            {
                Instruction inst = func.Body.Instructions[i];
                if (inst.OpCode.FlowControl == FlowControl.Cond_Branch)
                {// We found another branch instruction, does it exit at the same spot we do?
                    Instruction ex = ((Instruction)inst.Operand);
                    if (ex.Offset == exitLoc.Offset) Total++;
                }
            }

            return Total;
        }

        /// <summary>
        /// Encloses an entire function in a try catch block
        /// </summary>
        /// <param name="funct"></param>
        /// <param name="hook_class"></param>
        public static void Protect_Function(MethodDefinition funct, TypeDefinition hook_class)
        {
            var dbgRef = hook_class.Module.GetType("SR_PluginLoader.DebugHud");
            var dbg = funct.Module.ImportReference(dbgRef).Resolve();

            var logFunc = dbg.Methods.FirstOrDefault(o => o.FullName == "System.Void SR_PluginLoader.DebugHud::Log(System.Exception)");
            var logRef = funct.Module.ImportReference(logFunc);

            var il = funct.Body.GetILProcessor();
            var retPoint = il.Create(OpCodes.Ret);
            var leav_1 = il.Create(OpCodes.Leave, retPoint);
            var logCall = il.Create(OpCodes.Call, logRef);
            var leav_2 = il.Create(OpCodes.Leave, retPoint);

            funct.Body.Instructions.Add(leav_1);
            funct.Body.Instructions.Add(logCall);
            funct.Body.Instructions.Add(leav_2);

            var voidTypeReference = funct.Module.GetTypeReferences().Single(t => t.FullName == "System.Void");
            if (funct.ReturnType.FullName != voidTypeReference.FullName)
            {
                funct.Body.Instructions.Add(il.Create(OpCodes.Ldnull));// push 0 or null to the stack for the return value...
            }
            funct.Body.Instructions.Add(retPoint);
        }
        
        public static bool Intersects_With_Return(MethodDefinition funct, int POS)
        {
            if (POS >= (funct.Body.Instructions.Count - 1)) return true;

            return false;
        }

        public static OpCode Get_Assignment_OpCode_For_Reference_Param(ParameterDefinition param)
        {
            switch(param.ParameterType.GetElementType().Name.ToLower())
            {
                case "int":
                    return OpCodes.Stind_I;
                case "int8":
                    return OpCodes.Stind_I1;
                case "int16":
                    return OpCodes.Stind_I2;
                case "int32":
                    return OpCodes.Stind_I4;
                case "int64":
                    return OpCodes.Stind_I8;
                case "float32":
                    return OpCodes.Stind_R4;
                case "float64":
                    return OpCodes.Stind_R8;
                default:
                    return OpCodes.Stind_Ref;
            }

            throw new Exception("Cannot dereference unhandled parameter type!");
        }

        public static OpCode Get_Dereference_OpCode_For_Param(ParameterDefinition param)
        {
            switch (param.ParameterType.GetElementType().Name.ToLower())
            {
                case "int":
                    return OpCodes.Ldind_I;
                case "int8":
                    return OpCodes.Ldind_I1;
                case "int16":
                    return OpCodes.Ldind_I2;
                case "int32":
                    return OpCodes.Ldind_I4;
                case "int64":
                    return OpCodes.Ldind_I8;
                case "float32":
                    return OpCodes.Ldind_R4;
                case "float64":
                    return OpCodes.Ldind_R8;
                case "uint8":
                    return OpCodes.Ldind_U1;
                case "uint16":
                    return OpCodes.Ldind_U2;
                case "uint32":
                    return OpCodes.Ldind_U4;
                default:
                    return OpCodes.Ldind_Ref;
            }

            throw new Exception("Cannot dereference unhandled parameter type!");
        }

        /// <summary>
        /// Caches the original unaltered code for each method we alter so that subsequent alterations, such as for functions with MULTIPLE hooks, the injection position can still be found correctly without mismatching to OUR previously injected code.
        /// </summary>
        static Dictionary<string, Mono.Collections.Generic.Collection<Instruction>> CODE_CACHE = new Dictionary<string, Mono.Collections.Generic.Collection<Instruction>>();
        
        static int[] Get_Push_Pop_Weight(Instruction inst)
        {
            int Push = 0, Pop = 0;

            if (inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt || inst.OpCode == OpCodes.Calli)
            {
                if (inst.Operand != null)
                {
                    MethodReference meth = (MethodReference)inst.Operand;
                    if (meth.ReturnType != meth.Module.TypeSystem.Void) { Push++; }

                    if (meth.Parameters.Count > 0)
                        Pop += meth.Parameters.Count;
                }
            }
            if (inst.OpCode == OpCodes.Callvirt)// Callvirt checks the 'this' value to see if it's null, so it pops it off the stack...
                Pop++;

            switch (inst.OpCode.StackBehaviourPop)
            {
                case StackBehaviour.Pop0:
                    break;
                case StackBehaviour.Pop1:
                case StackBehaviour.Popi:
                case StackBehaviour.Popref:
                    Pop++;
                    break;
                case StackBehaviour.Varpop:
                    break;//already handled
                case StackBehaviour.Pop1_pop1:
                case StackBehaviour.Popi_pop1:
                case StackBehaviour.Popi_popi:
                case StackBehaviour.Popi_popi8:
                case StackBehaviour.Popi_popr4:
                case StackBehaviour.Popi_popr8:
                case StackBehaviour.Popref_pop1:
                case StackBehaviour.Popref_popi:
                    Pop++;
                    Pop++;
                    break;
                //case StackBehaviour.Popref_popi_pop1:
                case StackBehaviour.Popref_popi_popi:
                case StackBehaviour.Popref_popi_popi8:
                case StackBehaviour.Popref_popi_popr4:
                case StackBehaviour.Popref_popi_popr8:
                case StackBehaviour.Popref_popi_popref:
                    Pop++;
                    Pop++;
                    Pop++;
                    break;
            }

            switch (inst.OpCode.StackBehaviourPush)
            {
                case StackBehaviour.Push0:
                    break;
                case StackBehaviour.Push1:
                case StackBehaviour.Pushi:
                case StackBehaviour.Pushi8:
                case StackBehaviour.Pushr4:
                case StackBehaviour.Pushr8:
                case StackBehaviour.Pushref:
                    Push++;
                    break;
                case StackBehaviour.Varpush:
                    break;//already handled
                case StackBehaviour.Push1_push1:
                    Push++;
                    Push++;
                    break;
            }

            return new int[2]{ Pop, Push };
        }

        static bool Failsafe_Check_Void_Function_Stack_Balance(MethodDefinition funct)
        {
            int Pop = 0;
            int Push = 0;


            foreach (Instruction inst in funct.Body.Instructions)
            {
                int[] w = Get_Push_Pop_Weight(inst);
                Pop += w[0];
                Push += w[1];

                if (Push == Pop) Push = Pop = 0;
                else
                {
                    if(Pop > Push)
                    {
                        Pop -= Push;
                        Push = 0;
                    }
                    else if(Push > Pop)
                    {
                        Push -= Pop;
                        Pop = 0;
                    }
                }

                if (inst.OpCode == OpCodes.Ret)
                {
                    if (Push != Pop)
                    {
                        Log.Warn("Void function stack unbalanced, return function will be invalid!");
                        int d = (Push - Pop);
                        Log.Debug("Diff({0})  |  Pushes({1})  |  Pops({2})", d, Push, Pop);
                        Instruction i = inst;
                        do
                        {
                            if (i.OpCode.StackBehaviourPush != StackBehaviour.Push0)
                            {
                                if (i.OpCode.StackBehaviourPush != StackBehaviour.Push1_push1) d -= 1;
                                else d -= 2;
                            }

                            i = i.Previous;
                        } while (d > 0);

                        List<Instruction> list = new List<Instruction>();
                        do
                        {
                            list.Add(i);
                            i = i.Next;
                        } while (i != inst);

                        list.Add(i);//add the return opcode too

                        string ilcode = CIL_OpCodes_To_String(list);
                        Log.Debug("==== IL CODE ====\n{0}", ilcode);
                        //throw new Exception("Void function stack balance unbalanced, return function will be invalid!");
                    }
                    else
                        continue;
                }

            }

            return (Push == Pop);
        }

        public static string CIL_OpCodes_To_String(List<Instruction> code)
        {
            StringBuilder sb = new StringBuilder();
            int indent = 0;
            Stack<Instruction> istack = new Stack<Instruction>();

            foreach(Instruction inst in code)
            {
                sb.AppendLine(inst.ToString());
                /*
                if (istack.Count > 0 && inst == istack.Last())
                {
                    istack.Pop();
                    indent--;
                }

                string space = "".PadLeft(indent, ' ');
                int[] weight = Get_Push_Pop_Weight(inst);
                string sum = (-weight[0] + weight[1]).ToString("+#;-#;0");
                sb.AppendLine(String.Format("{0} {1}", sum, space+inst.ToString()));

                if (inst.OpCode.FlowControl == FlowControl.Cond_Branch || inst.OpCode.FlowControl == FlowControl.Branch)
                {
                    indent++;
                    istack.Push(inst);
                }
                */
            }
            return sb.ToString();
        }

        public static string Get_Method_IL_String(MethodDefinition funct)
        {
            /*
            StringBuilder sb = new StringBuilder();
            // write all the opcode bytes for the function into this memory stream
            foreach (Instruction inst in funct.Body.Instructions)
            {
                sb.AppendLine(inst.ToString());
            }
            return sb.ToString();
            */
            return CIL_OpCodes_To_String(funct.Body.Instructions.ToList());
        }

        internal static Type Get_Operand_Type_For_OpCode(OpCode op)
        {
            switch(op.Code)
            {
                case Code.Ldc_I4:
                case Code.Ldc_I4_0:
                case Code.Ldc_I4_1:
                case Code.Ldc_I4_2:
                case Code.Ldc_I4_3:
                case Code.Ldc_I4_4:
                case Code.Ldc_I4_5:
                case Code.Ldc_I4_6:
                case Code.Ldc_I4_7:
                case Code.Ldc_I4_8:
                case Code.Ldc_I4_M1:
                case Code.Ldc_I4_S:
                    return typeof(Int32);
            }

            return null;
        }

        public static void Serialize_Instruction(Instruction inst, BinaryWriter bw)
        {
            bw.Write((short)inst.OpCode.Value);

            object operand = inst.Operand;
            if (operand == null) return;

            Type ty = operand.GetType();
            if (ty.Equals(typeof(string))) { bw.Write((string)operand);}
            else if (ty.Equals(typeof(Instruction))) { bw.Write((operand as Instruction).Offset); }
            else if (ty.Equals(typeof(Instruction[])))
            {
                Instruction[] labels = (Instruction[])operand;
                foreach (var i in labels) { bw.Write(i.Offset); }
            }
            else if (ty.Equals(typeof(MethodReference))) { bw.Write((operand as MethodReference).FullName); }
            else if (ty.Equals(typeof(MethodDefinition))) { bw.Write((operand as MethodDefinition).FullName); }

            else if (ty.Equals(typeof(FieldReference))) { bw.Write((operand as FieldReference).FullName); }
            else if (ty.Equals(typeof(FieldDefinition))) { bw.Write((operand as FieldDefinition).FullName); }

            else if (ty.Equals(typeof(VariableReference))) { bw.Write((operand as VariableReference).Index); }
            else if (ty.Equals(typeof(VariableDefinition))) { bw.Write((operand as VariableDefinition).Index); }

            else if (ty.Equals(typeof(TypeReference))) { bw.Write((operand as TypeReference).FullName); }
            else if (ty.Equals(typeof(TypeDefinition))) { bw.Write((operand as TypeDefinition).FullName); }

            else if (ty.Equals(typeof(ParameterReference))) { bw.Write((operand as ParameterReference).Index); }
            else if (ty.Equals(typeof(ParameterDefinition))) { bw.Write((operand as ParameterDefinition).Index); }

            else if (ty.Equals(typeof(GenericInstanceMethod))) { bw.Write((operand as GenericInstanceMethod).FullName); }
            else if (ty.Equals(typeof(GenericInstanceType))) { bw.Write((operand as GenericInstanceType).FullName); }

            else if (ty.Equals(typeof(byte))) { bw.Write((byte)operand); }
            else if (ty.Equals(typeof(sbyte))) { bw.Write((sbyte)operand); }
            else if (ty.Equals(typeof(ushort))) { bw.Write((ushort)operand); }
            else if (ty.Equals(typeof(short))) { bw.Write((short)operand); }
            else if (ty.Equals(typeof(uint))) { bw.Write((uint)operand); }
            else if (ty.Equals(typeof(int))) { bw.Write((int)operand); }
            else if (ty.Equals(typeof(ulong))) { bw.Write((ulong)operand); }
            else if (ty.Equals(typeof(long))) { bw.Write((long)operand); }
            else if (ty.Equals(typeof(float))) { bw.Write((float)operand); }
            else if (ty.Equals(typeof(double))) { bw.Write((double)operand); }
            else { throw new NotImplementedException(String.Format("Unhandled OpCode operand type({0}) OpCode: {1}", ty.ToString(), inst.OpCode)); }

        }

        public static string Calc_Function_Hash(MethodDefinition funct)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            // write all the opcode bytes for the function into this memory stream
            foreach (Instruction inst in funct.Body.Instructions) { Serialize_Instruction(inst, bw); }
            return Util.Git_Blob_Sha1_Hash(ms.GetBuffer());
        }


        public static bool Hook_Function(ModuleDefinition dll, MethodDefinition funct, TypeDefinition hook_class, MethodDefinition hook, FieldDefinition eventDef, string Event_Name, Hook_Dbg_Data info)
        {
#region VARIABLE SETUP
            var il = funct.Body.GetILProcessor();
        // LET'S GET SOME TYPE REFERENCES OUT OF THE WAY
            var voidTypeReference = funct.Module.GetTypeReferences().Single(t => t.FullName == "System.Void");
            var objTypeReference = funct.Module.GetTypeReferences().Single(t => t.FullName == "System.Object");
            var int32TypeReference = funct.Module.GetTypeReferences().Single(t => t.FullName == "System.Int32");
            //get a reference to the _hook_result class
            var hrDef = dll.GetType("SR_PluginLoader._hook_result");
            var hrRef = funct.Module.ImportReference(hrDef);
            var hrCancelRef = funct.Module.ImportReference(hrDef.Fields.Single(o => o.Name == "abort"));
            var hrArgsRef = funct.Module.ImportReference(hrDef.Fields.Single(o => o.Name == "args"));
            var argVarTypeReference = hrArgsRef.FieldType;

            // SETUP SOME BASIC VARS FOR LOGIC
            bool IS_POST_HOOK = info.is_post;

        // MAKE SOME BASIC VARS TO HOLD OUR CODE AND THE POSITION IT'S INJECTED
            List<Instruction> instBuf = new List<Instruction>();
            #endregion

            // We HAVE to rewrite ALL branching instructions such that they lead to a NOP call in to avoid stack imbalances when a branching call leads to an ld* type opcode 
            // which would make our injected hook get placed after said ld* code and we would have an extra value on the stack... not good
            while (true)
            {
                bool done = true;
                foreach (Instruction inst in funct.Body.Instructions)
                {
                    if (inst.OpCode == OpCodes.Switch) continue;
                    //if (inst.OpCode.FlowControl == FlowControl.Branch || inst.OpCode.FlowControl == FlowControl.Cond_Branch)
                    if (inst.OpCode.FlowControl == FlowControl.Cond_Branch)
                    {
                        var sub = (Instruction)inst.Operand;
                        if (sub.OpCode == OpCodes.Nop) continue;
                        if (sub.OpCode.StackBehaviourPush == StackBehaviour.Push0) continue;
                        //insert a NOP code before this instruction and make the branching call go there instead...
                        Instruction nop = il.Create(OpCodes.Nop);
                        il.InsertBefore(sub, nop);
                        inst.Operand = nop;

                        done = false;
                        break;
                    }
                }

                if(done) break;
            }

#region FINDING INJECTION POSITION
            Collection<Instruction> SEARCH_CODE = null;
            if (!CODE_CACHE.TryGetValue(funct.FullName, out SEARCH_CODE)) CODE_CACHE[funct.FullName] = new Collection<Instruction>(funct.Body.Instructions);
            CODE_CACHE.TryGetValue(funct.FullName, out SEARCH_CODE);

            int POS = 0;
            int SEARCH_DIR = Math.Max(-1, Math.Min(1, info.pos));
            int SEARCH_N = Math.Abs(info.pos);
            Instruction TARGET_INST = null;
            Instruction TARGET_BRANCH = null;
            //IEnumerable<Instruction> SEARCH_INSTRUCTIONS = funct.Body.Instructions.AsEnumerable();
            //if (SEARCH_DIR < 0) SEARCH_INSTRUCTIONS = funct.Body.Instructions.Reverse();
            IEnumerable<Instruction> SEARCH_INSTRUCTIONS = SEARCH_CODE.AsEnumerable<Instruction>();
            if (SEARCH_DIR < 0) SEARCH_INSTRUCTIONS = SEARCH_CODE.Reverse();
            #region locating instructions logic
            switch (info.method)
            {
                case debug_positioning.Instruction:// Find the n-th instruction, our pos is right before it.
                    POS -= 1;// this is to offset the final pos we arrive at and counteract the +1 automatically applied after we find the target function
                    foreach (Instruction inst in SEARCH_INSTRUCTIONS)
                    {
                        // We do it this way rather then just doing "simple" math to keep it VERY VERY simple and avoid unforseen errors.
                        SEARCH_N--;
                        if (SEARCH_N > 0) continue;
                        TARGET_INST = inst;
                        break;
                    }
                    break;
                case debug_positioning.Branch_Start:// Find the n-th occurance of a branch-type instruction, our pos is right after that code.
                    foreach (Instruction inst in SEARCH_INSTRUCTIONS)
                    {
                        if (inst.OpCode.FlowControl == FlowControl.Branch)
                        {
                            SEARCH_N--;
                            if (SEARCH_N > 0) continue;
                            TARGET_BRANCH = inst;
                            TARGET_INST = inst;
                            break;
                        }
                    }
                    break;
                case debug_positioning.Branch_Exit:// Find the n-th occurance of a branch-type instruction, our pos is right after the exit point of that code.
                    foreach (Instruction inst in SEARCH_INSTRUCTIONS)
                    {
                        if (inst.OpCode.FlowControl == FlowControl.Branch)
                        {
                            SEARCH_N--;
                            if (SEARCH_N > 0) continue;
                            TARGET_BRANCH = inst;
                            TARGET_INST = ((Instruction)inst.Operand);
                            break;
                        }
                    }
                    break;
                case debug_positioning.Cond_Branch_Start:// Find the n-th occurance of a conditional branch-type instruction, our pos is right after that code.
                    foreach (Instruction inst in SEARCH_INSTRUCTIONS)
                    {
                        if (inst.OpCode.FlowControl == FlowControl.Cond_Branch)
                        {
                            SEARCH_N--;
                            if (SEARCH_N > 0) continue;
                            TARGET_BRANCH = inst;
                            TARGET_INST = inst;
                            break;
                        }
                    }
                    break;
                case debug_positioning.Cond_Branch_Exit:// Find the n-th occurance of a conditional branch-type instruction, our pos is right after the exit point of that code.
                    foreach (Instruction inst in SEARCH_INSTRUCTIONS)
                    {
                        if (inst.OpCode.FlowControl == FlowControl.Cond_Branch)
                        {
                            SEARCH_N--;
                            if (SEARCH_N > 0) continue;
                            TARGET_BRANCH = inst;
                            TARGET_INST = ((Instruction)inst.Operand);
                            break;
                        }
                    }
                    break;
                case debug_positioning.Field_Ref:// Find the n-th reference to a specified field where field-fullName is given by 'arg'
                    foreach (Instruction inst in SEARCH_INSTRUCTIONS)
                    {
                        if (String.Compare(((FieldReference)inst.Operand).FullName, info.arg) == 0)
                        {
                            SEARCH_N--;
                            if (SEARCH_N > 0) continue;
                            TARGET_INST = inst;
                            break;
                        }
                    }
                    break;
                case debug_positioning.Method_Ref:// Find the n-th reference to a specified method where method-fullName is given by 'arg'
                    foreach (Instruction inst in SEARCH_INSTRUCTIONS)
                    {
                        if (String.Compare(((MethodReference)inst.Operand).FullName, info.arg) == 0)
                        {
                            SEARCH_N--;
                            if (SEARCH_N > 0) continue;
                            TARGET_INST = inst;
                            break;
                        }
                    }
                    break;
                case debug_positioning.OpCode:// Find the n-th reference to a specified opcode where opcode-name is given by 'arg'
                    foreach (Instruction inst in SEARCH_INSTRUCTIONS)
                    {
                        if (String.Compare(inst.OpCode.Name, info.arg) == 0)
                        {
                            SEARCH_N--;
                            if (SEARCH_N > 0) continue;
                            TARGET_INST = inst;
                            break;
                        }
                    }
                    break;
            }
            #endregion

            POS += (1 + funct.Body.Instructions.IndexOf(TARGET_INST));
            if(TARGET_BRANCH != null)
            {

                if(info.method == debug_positioning.Cond_Branch_Exit)
                {
                    // Are we trying to position ourself relative to the bottom of this branch?
                    if(info.relative < 0)
                    {// Yep we sure are!
                        // Check if the branch we are trying to be at the bottom of has an 'else' type branch right before the instruction it exits too.
                        // If it does then we need to offset ourself one more instruction still
                        Instruction exitInst = TARGET_BRANCH.Operand as Instruction;
                        if (exitInst!=null)
                        {
                            Instruction prev = exitInst.Previous;
                            if(prev.OpCode.Code == Code.Br || prev.OpCode.Code == Code.Br_S)
                            {
                                POS -= 1;
                            }
                        }
                    }

                }

                // If multiple conditional branches exit at the spot we want to position ourself at then we need to make sure they dont do that...
                /*
                if (Get_Number_Of_Conditional_Branches_Exiting_At(funct, TARGET_INST) > 0)
                {
                    IList<Instruction> branches = Get_Conditional_Branches_Exiting_At(funct, TARGET_INST);
                    Make_Branches_Unique(funct, branches);
                    // if these two are not equal then that means the injection pos is relative to a branch exit!
                    // and since we are just rerouting the original branches exit instruction to one below it we will want to inject our stuff below the original exit instruction.
                    if (TARGET_BRANCH != TARGET_INST) POS += 1;
                }
                */
            }

    // Ensure we were able to calculate the POS correctly.
            if (TARGET_INST == null)
            {
                SLog.Error("Unable to calculate inject position for hook<{0}>, skipping.", info.hook);
                return false;
            }

            // Further alter the injection location via the sub_pos information.
            if (info.method != debug_positioning.OpCode)
            {
                switch(info.relative_method)
                {
                    case debug_positioning.Instruction:
                        POS += info.relative;
                        break;
                }
            }

            // Make sure POS didn't go out of bounds
            int clamped = Math.Min(funct.Body.Instructions.Count, Math.Max(0, POS));
            if (POS != clamped)
            {
                Log.Warn("  {0}: Injection pos OOB, clamping | Original({1}) | Clamped({2})", Event_Name, POS, clamped);
                POS = clamped;
            }

#endregion

#region SETUP THE RETURN VALUE OVERRIDE BUFFER
            List<Instruction> resultInstBuf = null;
            // Because any function i nthe game can have both a pre & post hook, and those hooks each are injected seperately we need to first CHECK if the function currently has the return value var setup.
            VariableDefinition returnValueArg = funct.Body.Variables.FirstOrDefault(o => o.Name == "returnValueArg");
            if (returnValueArg == null)
            {
                // Our return value var *MUST* always be an object type so we can always pass it by ref to the hook call and handle it everywhere without cast exceptions that might screw the game up.
                returnValueArg = new VariableDefinition("returnValueArg", funct.Module.TypeSystem.Object);
                resultInstBuf = new List<Instruction>();
                funct.Body.Variables.Add(returnValueArg);

                //now setup this returnvalue with a default value
                switch (funct.ReturnType.Name)
                {
                    case "Int32":
                    case "Boolean":
                        resultInstBuf.Add(il.Create(OpCodes.Ldc_I4_0));
                        resultInstBuf.Add(il.Create(OpCodes.Box, funct.ReturnType));
                        break;
                    default:
                        //XXX: Something to consider, not all functions ingame will be happy getting a NULL value back. It might even be the cause for some current issues
                        resultInstBuf.Add(il.Create(OpCodes.Ldnull));
                        break;
                }
                resultInstBuf.Add(il.Create(OpCodes.Stloc, returnValueArg));
                resultInstBuf.Add(il.Create(OpCodes.Nop));
            }
            List<Instruction> returnBuf = null;
            if (Intersects_With_Return(funct, POS))
            {
                IS_POST_HOOK = true;//This can pretty much ONLY be a post hook...
                if(info.method != debug_positioning.Branch_Exit && info.method != debug_positioning.Cond_Branch_Exit)
                    POS += 1;// Add 1 to the position so now the hook call will actually be placed after where the return code is currently, the return code will be replaced with an opcode that stores whatever value it WAS going to return.
                returnBuf = Replace_Return_Statement(funct, returnValueArg, ref POS);
            }
#endregion

        //SETUP SOME BASIC VARS
            bool ALLOW_ARG_OVERWRITES = !IS_POST_HOOK;// if it's at the end of the function, it wouldnt do anything.
            bool HAS_RETURN_TYPE = !(String.Compare(funct.ReturnType.FullName, voidTypeReference.FullName) == 0);
            bool PARSE_RESULT_ARGS = (ALLOW_ARG_OVERWRITES && funct.Parameters.Count > 0);


            // HERE'S WHERE WE START CREATING CODE

#region CONVERTING ByRef PARAMETERS
            Dictionary<int, VariableDefinition> refVars = new Dictionary<int, VariableDefinition>();
            for (int i = 0; i < funct.Parameters.Count; i++)
            {
                var param = funct.Parameters[i];
                if (!param.ParameterType.IsByReference) continue;

                TypeReference ty = param.ParameterType;
                if(ty.IsByReference) ty = ((ByReferenceType)ty).ElementType;//turn the reference type into a non-reference type
                //VariableDefinition vr = new VariableDefinition(String.Format("pita_{0}", i), ty);
                VariableDefinition vr = new VariableDefinition(String.Format("pita_{0}", i), funct.Module.TypeSystem.Object);
                funct.Body.Variables.Add(vr);
                refVars[i] = vr;

                instBuf.Add(il.Create(OpCodes.Ldarg, param));// push arg#idx onto stack
                instBuf.Add(il.Create(Get_Dereference_OpCode_For_Param(param)));// load object reference onto stack
                //instBuf.Add(il.Create(OpCodes.Box, funct.Module.TypeSystem.Object));// cast the value
                instBuf.Add(il.Create(OpCodes.Box, ty));// cast the value
                instBuf.Add(il.Create(OpCodes.Stloc, vr));// 
            }
#endregion

#region EVENT FIRING  
            // LOAD THE EVENT'S HOOK_ID
            // Unfortunately we cant actually get away with just passing the hook id number to the event call function, we HAVE to pass the HOOK_ID instance.
            // It's ok though, it has minimal performance impact.
            var eventRef = funct.Module.ImportReference(eventDef);
            instBuf.Add(il.Create(OpCodes.Ldsfld, eventRef));//set event to pass to function.

        // PASS 'This'
            if (funct.IsStatic) instBuf.Add(il.Create(OpCodes.Ldnull));// craft a new null (object)
            else instBuf.Add(il.Create(OpCodes.Ldarg, (int)0));// pass 'this' reference
        // PASS 'ref returnValue'
            //we ALWAYS have a return value var defined
            instBuf.Add(il.Create(OpCodes.Ldloca, returnValueArg));

        // PASS 'new object[]{ ... }' OR 'null'
            if(funct.Parameters.Count > 0)
            {
                instBuf.Add(il.Create(OpCodes.Ldc_I4, funct.Parameters.Count));// 
                instBuf.Add(il.Create(OpCodes.Newarr, objTypeReference));// 
                //instBuf.Add(il.Create(OpCodes.Stloc_0));// save the array so we can keep using it below...

                for (int i = 0; i < funct.Parameters.Count; i++)
                {
                    var param = funct.Parameters[i];
                    instBuf.Add(il.Create(OpCodes.Dup));// dupe our array back onto the stack
                    //instBuf.Add(il.Create(OpCodes.Ldloc_0));// push our array to the stack
                    instBuf.Add(il.Create(OpCodes.Ldc_I4, i));// push the index of this array item onto the stack

                    TypeReference ty = param.ParameterType;
                    if (ty.IsByReference) ty = ((ByReferenceType)ty).ElementType;//turn the reference type into a non-reference type

                    if (param.ParameterType.IsByReference)
                    {
                        instBuf.Add(il.Create(OpCodes.Ldloc, refVars[i]));// 
                    }
                    else
                    {
                        instBuf.Add(il.Create(OpCodes.Ldarg, param));// push arg#idx onto stack
                        instBuf.Add(il.Create(OpCodes.Box, ty));// cast the value
                    }

                    instBuf.Add(il.Create(OpCodes.Stelem_Ref));// store the object reference in the array
                }
                // The array still MUST be on the stack so it gets passed to the hook.
            }
            else instBuf.Add(il.Create(OpCodes.Ldnull));
            
    // CALL THE HOOK FUNCTION
            instBuf.Add(il.Create(OpCodes.Call, funct.Module.ImportReference(hook)));//go ahead and load the event number we will fire, then load all of our args(if any) into an object[] array
#endregion

#region EVENT RESULT HANDLING

            VariableDefinition hookRes = new VariableDefinition("hookResult", hrRef);
            funct.Body.Variables.Add(hookRes);
            // HANDLE _hook_result
            if (HAS_RETURN_TYPE || PARSE_RESULT_ARGS || !IS_POST_HOOK)
            {
                instBuf.Add(il.Create(OpCodes.Stloc, hookRes));// store the function result at var slot 0
            }
            else//if we arent going to do anything with the hook_results then pop it off of the stack so the function doesnt try to fucking return it...
            {
                instBuf.Add(il.Create(OpCodes.Pop));// pop the hook-results off the stack
            }


    // if(hook_result.abort != false)
            if (!IS_POST_HOOK)// WE CANNOT CANCEL AN EVENT THAT IS INSERTED AT THE END OF A METHOD!
            {
                // CHECK IF THE USER WANTS TO CANCEL.
                var continueLoc = il.Create(OpCodes.Nop);//this is the instruction that our 'cancel' value check will jump to if it passes
                instBuf.Add(il.Create(OpCodes.Ldloc, hookRes));// load hook function result var
                instBuf.Add(il.Create(OpCodes.Ldfld, hrCancelRef));// push specified field value from returned object onto stack
                instBuf.Add(il.Create(OpCodes.Brfalse, continueLoc));// if the event returned false then jump to the referenced instruction
                //if (HAS_RETURN_TYPE) instBuf.Add(il.Create(OpCodes.Ldnull));// push 0 or null to the stack for the return value...
                if (HAS_RETURN_TYPE)
                {
                    instBuf.Add(il.Create(OpCodes.Ldloc, returnValueArg));
                    instBuf.Add(il.Create(OpCodes.Unbox_Any, funct.ReturnType));
                }
                instBuf.Add(il.Create(OpCodes.Ret));// abort this function
                instBuf.Add(continueLoc);// end if
            }


    // if(hook_result.args != null)
            if (PARSE_RESULT_ARGS)
            {
                var endif = il.Create(OpCodes.Nop);
                //we need a few bool vars next
                instBuf.Add(il.Create(OpCodes.Ldloc, hookRes));// load hook function result var
                instBuf.Add(il.Create(OpCodes.Ldfld, hrArgsRef));// push the args value to the stack
                instBuf.Add(il.Create(OpCodes.Brfalse, endif));//make sure hr_args != null
                //ok so now we know the args value is not null and its also in var slot 1 !
                
                for (int i = 0; i < funct.Parameters.Count; i++)
                {
                    int idx = (i + (funct.IsStatic ? 0 : 1));
                    var param = funct.Parameters[i];

                    if (param.ParameterType.IsByReference)
                    {
                        //if(i > 1) 
                        //continue;
                        instBuf.Add(il.Create(OpCodes.Ldarg_S, (byte)idx));// go ahead and load our ref argument's memory address
                    }
                    instBuf.Add(il.Create(OpCodes.Ldloc, hookRes));// load hook function result var
                    instBuf.Add(il.Create(OpCodes.Ldfld, hrArgsRef));// push the args value to the stack
                    instBuf.Add(il.Create(OpCodes.Ldc_I4, i));//push our array index
                    instBuf.Add(il.Create(OpCodes.Ldelem_Ref));// load element at array idx onto stack

                    TypeReference ty = param.ParameterType;
                    if (ty.IsByReference) ty = ((ByReferenceType)ty).ElementType;//turn the reference type into a non-reference type

                    if (!ty.IsPrimitive && ty.IsByReference) instBuf.Add(il.Create(OpCodes.Castclass, ty));//cast the value
                    else instBuf.Add(il.Create(OpCodes.Unbox_Any, ty));// cast the value 

                    //instBuf.Add(il.Create(OpCodes.Unbox_Any, ty));

                    if (param.ParameterType.IsByReference)
                    {
                        var rop = Get_Assignment_OpCode_For_Reference_Param(param);
                        instBuf.Add(il.Create(rop));// replace argument#idx with the value from the array
                    }
                    else instBuf.Add(il.Create(OpCodes.Starg_S, (byte)idx));// replace argument#idx with the value from the array
                }

                instBuf.Add(endif);
            }
#endregion

#region DUPLICATE PREVENTION
            // make sure we arent re-adding the hook, so that multiple runs of the program dont make functions call the same hook like 5 times.
            var callFunc = il.Create(OpCodes.Call, funct.Module.ImportReference(hook));
            if (DuplicateChecker(funct, instBuf, POS, false, callFunc))
            {
                Log.Info("[{0}] {1}", XTERM.redBright("--"), Event_Name);
                //Program.PrintError(String.Format("Function already hooked: {0}", funct.FullName));
                return false;
            }
            #endregion

            #region INJECTION PHASE
            try
            {
                // Append our new code to a temporary opcode list first
                Collection<Instruction> INST = new Collection<Instruction>(funct.Body.Instructions);
                // Append our main chunk of logic to the code
                for (int i = (instBuf.Count - 1); i >= 0; i--)
                {
                    Instruction inst = instBuf[i];
                    INST.Insert(POS, inst);
                }
                // Okay now that we know all of the code CAN be added without error we can really inject it!
                funct.Body.Instructions.Clear();
                for(int i=0; i<INST.Count; i++)
                {
                    funct.Body.Instructions.Add(INST[i]);
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                return false;
            }


            // Append any method return value initialization logic
            if (resultInstBuf != null)// insert the result var default value decleration
            {
                //int idx = POS;
                int idx = 0;
                if (IS_POST_HOOK) idx = 0;

                for (int i = (resultInstBuf.Count - 1); i >= 0; i--)
                {
                    Instruction inst = resultInstBuf[i];
                    funct.Body.Instructions.Insert(idx, inst);
                }
            }

            // Append any method return overriding logic
            if (returnBuf!=null)
            {
                for (int i = 0; i < returnBuf.Count; i++)
                //for (int i = (returnBuf.Count-1); i>=0; i--)
                {
                    Instruction inst = returnBuf[i];
                    funct.Body.Instructions.Add(inst);
                    //funct.Body.Instructions.Insert(POS, inst);
                }
            }
#endregion
            //Protect_Function(funct, hook_class);

            if(funct.ReturnType == funct.Module.TypeSystem.Void)// Do a failsafe check to make sure that no return calls will try and return a value...
            {
                //Failsafe_Check_Void_Function_Stack_Balance(funct);
            }
            return true;
        }
    }
}
