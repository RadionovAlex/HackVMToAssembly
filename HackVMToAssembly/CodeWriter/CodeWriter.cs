using HackVMToAssembly.FunctionsTable;

namespace HackVMToAssembly.CodeWriter
{
    public class CodeWriter : ICodeWriter
    {
        public Dictionary<string, FunctionEntrance> FunctionsEntrances { get; private set; } = new();

        public Dictionary<string, int> FunctionCallCounts { get; private set; } = new();

        private StreamWriter _writer;

        private string _vmFileName;


        public CodeWriter(StreamWriter writer)
        {
            _writer = writer;
        }

        public void SetFileName(string fileName)
        {
            _vmFileName = fileName;
        }

        public void WriteProgramStartLabel() => 
            _writer.Write(VmToAssemblyStandardFunctions.ProgramStartDefinition);

        public void WriteArithmetic(string command)
        {
            var code = AssemblyFunctionDefinitionCreator.GetGoToArithmeticCommand(command, FunctionCallCounts, FunctionsEntrances);
            _writer.Write(code);
        }

        public void WritePushPop(CommandType command, string segment, int index)
        {
            if(command == CommandType.C_Push)
            {
                if (segment == "constant")
                    _writer.Write(VmToAssemblyStandardFunctions.PutConstantIntoD(index));

                else if (CodeTranslationUtil.SegmentsVMToAssemblyMap.TryGetValue(segment, out var asmSegment))
                    _writer.Write(VmToAssemblyStandardFunctions.PutSegmentIndexValueIntoD(asmSegment, index));

                else if (CodeTranslationUtil.PointersValues.TryGetValue(segment, out var ptr))
                    _writer.Write(VmToAssemblyStandardFunctions.PutPointerIndexValueIntoD(ptr, index));
                else
                    throw new Exception($"Cannot handle segment {segment}");

                _writer.Write(VmToAssemblyStandardFunctions.PushDefinition);
            }
            else if(command == CommandType.C_Pop)
            {
                _writer.Write(VmToAssemblyStandardFunctions.PopDefinition);
                
                if (segment == "constant")
                    _writer.Write(VmToAssemblyStandardFunctions.PopDIntoConstant(index));

                else if (CodeTranslationUtil.SegmentsVMToAssemblyMap.TryGetValue(segment, out var asmSegment))
                    _writer.Write(VmToAssemblyStandardFunctions.PopDIntoSegment(asmSegment, index));

                else if (CodeTranslationUtil.PointersValues.TryGetValue(segment, out var ptr))
                    _writer.Write(VmToAssemblyStandardFunctions.PopDIntoPointerIndex(ptr, index));
                else
                    throw new Exception($"Cannot handle segment {segment}");
            }
            else
            {
                throw new Exception($"Try to call WritePushPop for command {command}");
            }
        }

        public void WriteInit()
        {
            _writer.Write(VmToAssemblyStandardFunctions.ProgramVariablesInitialization);
            _writer.Write(VmToAssemblyStandardFunctions.WriteGoToSysInit);
        }

        public void WriteLabel(string label)
        {
            // _writer.Write($"{_vmFileName}.{label}");
            _writer.Write($@"
({ label})
");
        }

        public void WriteGoTo(string label)
        {
            // _writer.Write(AssemblyFunctionDefinitionCreator.GoTo($"{_vmFileName}.{label}"));
            _writer.Write(AssemblyFunctionDefinitionCreator.GoTo(label));
        }

        public void WriteIf(string label)
        {
            _writer.Write(AssemblyFunctionDefinitionCreator.IfGoTo(label));
        }

        public void WriteCall(string funcName, int argumentsNumber)
        {
            // var toWriteName = funcName == "Sys.Init" ? funcName : $"{_vmFileName}.{funcName}";
            var callCode = AssemblyFunctionDefinitionCreator.CallFunction(funcName, argumentsNumber, FunctionCallCounts, FunctionsEntrances);
            _writer.Write(callCode);
        }
      

        public void WriteFunction(string funcName, int localsNumer)
        {
            // var toWriteName = funcName == "Sys.Init" ? funcName : $"{_vmFileName}.{funcName}";
            var functionEntrance = new FunctionEntrance(funcName);
            FunctionsEntrances.Add(funcName, functionEntrance);

            var funcCode = AssemblyFunctionDefinitionCreator.GetFuncAssemblyCode(functionEntrance, localsNumer);
            _writer.Write(funcCode);
        }

        public void WriteReturn()
        {
            var returnCode = AssemblyFunctionDefinitionCreator.ReturnFromFunction();
            _writer.Write(returnCode);
        }

        public void WriteStandardFunctions()
        {
            foreach (var kvp in CodeTranslationUtil.FunctionAndVmImplementations)
            {
                var functionEntrance = new FunctionEntrance(kvp.Key);
                FunctionsEntrances.Add(kvp.Key, functionEntrance);
                var funcCode = AssemblyFunctionDefinitionCreator.PackAssemblyCode(functionEntrance, kvp.Value);
                _writer.Write(funcCode);
            }
        }
    }
}