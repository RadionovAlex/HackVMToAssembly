namespace HackVMToAssembly.FunctionsTable
{
    public class FunctionEntrance
    {
        public FunctionEntrance(string functionName)
        {
            FunctionName = functionName;
            FunctionLabel = functionName;
            if (FunctionName != "Sys.init")
                FunctionLabel = FunctionLabel + "LBL";
        }

        public string FunctionName { get; }

        public string FunctionLabel { get; }
    }
}
