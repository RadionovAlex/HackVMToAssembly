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

        public static string GetGoToCommand(string toGoCommand, Dictionary<string, int> funcCallsCount, Dictionary<string, FunctionEntrance> funcEntrances)
        {
            if (funcCallsCount.TryGetValue(toGoCommand, out var count))
                funcCallsCount[toGoCommand] = count + 1;
            else
                funcCallsCount[toGoCommand] = 0;

            var callsCount = funcCallsCount[toGoCommand];
            var callLabel = @$"{toGoCommand}Call_{callsCount}";

            var functionEntrance = funcEntrances[toGoCommand];
            var code = @$"
@{callLabel}
D=A
@R13
M=D
@{functionEntrance.FunctionLabel}
0;JMP
({callLabel})
";

            return code;
        }
    }
}
