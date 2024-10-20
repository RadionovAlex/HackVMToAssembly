using HackVMToAssembly.FunctionsTable;

namespace HackVMToAssembly.CodeWriter
{
    public class CodeWriter : ICodeWriter
    {
        private Dictionary<string, string> _functionAndVmImplementations = new Dictionary<string, string>()
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
            {"push", VmToAssemblyStandardFunctions.PushDefinition},
        };

        private int _currentCodeRaw;

        private Dictionary<string, FunctionEntrance> _functionsEntrances = new();

        private Dictionary<string, int> _functionCallCounts = new();

        private StreamWriter _writer;

        public void SetCurrentCodeRaw(int raw) =>
            _currentCodeRaw = raw;

        public void Close()
        {
            _writer?.Flush();
            _writer?.Close();
        }

        public void SetFileName(string fileName)
        {
            _writer = new StreamWriter(fileName, append: false);

            WriteStandardFunctions();
        }

        public void WriteProgramStartLabel() => 
            _writer.Write(VmToAssemblyStandardFunctions.ProgramStartDefinition);

        public void WriteProgramVariablesInitialization() => 
            _writer.Write(VmToAssemblyStandardFunctions.ProgramVariablesInitialization);

        public void WriteArithmetic(string command)
        {
            WriteGoToCommand(command);
        }

        public void WritePushPop(CommandType command, string segment, int index)
        {
            if(command == CommandType.C_Push)
            {
                switch (segment)
                {
                    case "constant":
                        {
                            _writer.Write(VmToAssemblyStandardFunctions.PutConstantIntoD(index));
                            WriteGoToCommand("push");
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private void WriteStandardFunctions()
        {
            _writer.Write(VmToAssemblyStandardFunctions.GoToProgramStartDefinition);
            foreach(var kvp in _functionAndVmImplementations)
            {
                var functionEntrance = new FunctionEntrance(kvp.Key);
                _functionsEntrances.Add(kvp.Key, functionEntrance);
                var funcCode = AssemblyFunctionDefinitionCreator.PackAssemblyCode(functionEntrance, kvp.Value);
                _writer.Write(funcCode);
            }   
        }

        private void WriteGoToCommand(string command)
        {
            if(_functionCallCounts.TryGetValue(command, out var count))
                _functionCallCounts[command] = count + 1;
            else
                _functionCallCounts[command] = 0;

            var callsCount = _functionCallCounts[command];
            var callLabel = @$"{command}Call_{callsCount}";

            var functionEntrance = _functionsEntrances[command];
            var code = @$"
@{callLabel}
D=A
@R13
M=D
@{functionEntrance.FunctionLabel}
0;JMP
({callLabel})
";

            _writer.Write(code);
        }
    }
}
