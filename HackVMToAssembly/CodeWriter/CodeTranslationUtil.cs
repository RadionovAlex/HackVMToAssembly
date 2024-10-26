namespace HackVMToAssembly.CodeWriter
{
    public static class CodeTranslationUtil
    {
        public static Dictionary<string, string> FunctionAndVmImplementations = new Dictionary<string, string>()
        {
            {"add", VmToAssemblyStandardFunctions.AddDefinition},
            {"sub", VmToAssemblyStandardFunctions.SubDefinition},
            {"neg", VmToAssemblyStandardFunctions.NegDefinition},
            {"eq", VmToAssemblyStandardFunctions.EqualDefiniton},
            {"gt", VmToAssemblyStandardFunctions.GreatThanDefinition},
            {"lt", VmToAssemblyStandardFunctions.LessThanDefinition},
            {"and", VmToAssemblyStandardFunctions.AndDefinition},
            {"or", VmToAssemblyStandardFunctions.OrDefinition},
            {"not", VmToAssemblyStandardFunctions.NotDefinition},
            {"pop", VmToAssemblyStandardFunctions.PopDefinition}
            /*{"push", VmToAssemblyStandardFunctions.PushDefinition},
            {"pop", VmToAssemblyStandardFunctions.PopDefinition},*/
        };

        public static Dictionary<string, string> SegmentsVMToAssemblyMap = new Dictionary<string, string>()
        {
            {"local", "LCL" },
            {"argument", "ARG" },
            {"this", "THIS" },
            {"that", "THAT" },
        };

        public static Dictionary<string, int> PointersValues = new Dictionary<string, int>()
        {
            {"pointer", 3},
            {"temp", 5},
            {"static", 16},
        };
    }
}
