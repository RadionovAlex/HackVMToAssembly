using HackVMToAssembly.FunctionsTable;
using System.Text;

namespace HackVMToAssembly.CodeWriter
{
    public static class AssemblyFunctionDefinitionCreator
    {
        public static string PackAssemblyCode(FunctionEntrance entrance, string assemblyCleanImpl)
        {
            StringBuilder _sb = new StringBuilder(assemblyCleanImpl.Length * 2);
            _sb.Append("\n");
            _sb.Append($"// {entrance.FunctionName} definition \n");
            _sb.Append($"({entrance.FunctionLabel}) \n");
            _sb.Append("\n");
            _sb.Append(assemblyCleanImpl);
            _sb.Append("\n");

            return _sb.ToString();
        }        
    }
}
