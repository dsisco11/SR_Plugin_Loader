using System;

namespace SRPL.Analyzer
{
    public struct ModuleType
    {
        public string Name;
        public string[] Extensions;
        public string[] Fields;
        public string[] Funcs;

        public ModuleType(string name, string[] extensions, string[] fields, string[] funcs)
        {
            Name = name;
            Extensions = extensions;
            Fields = fields;
            Funcs = funcs;
        }

        public override string ToString()
        {
            string output = "";
            output += Name;
            if (Extensions.Length > 0)
            {
                output += " : ";
                for(int i = 0; i < Extensions.Length; i++)
                {
                    output += Extensions[i];
                    if (i < Extensions.Length - 1) output += ", ";
                }
            }
            output += Environment.NewLine;
            if (Fields.Length > 0) output += " - Fields" + Environment.NewLine;
            foreach(string field in Fields)
            {
                output += "    - " + field + Environment.NewLine;
            }
            if (Funcs.Length > 0) output += " - Methods" + Environment.NewLine;
            foreach(string func in Funcs)
            {
                output += "    - " + func + Environment.NewLine;
            }

            return output;
        }
    }
}
