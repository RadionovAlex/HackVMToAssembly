﻿namespace HackVMToAssembly.Parser
{
    public static class RawsUtil
    {
        public static HashSet<string> ArithmeticFunctions = new HashSet<string>()
        {
            "add",
            "sub",
            "neg",
            "eq",
            "gt",
            "lt",
            "and",
            "or",
            "not"
        };

        public static List<string> RemoveComments(List<string> lines)
        {
            List<string> cleanLines = new List<string>();

            foreach (var line in lines)
            {
                // Split the line by '//' and take the part before it (index 0)
                string cleanLine = line.Split(new[] { "//" }, StringSplitOptions.None)[0].Trim();
                cleanLines.Add(cleanLine);
            }

            return cleanLines;
        }
    }
}
