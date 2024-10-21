using HackVMToAssembly.FunctionsTable;

namespace HackVMToAssembly.CodeWriter
{
    public class CodeWriter : ICodeWriter
    {
        private Dictionary<string, FunctionEntrance> _functionsEntrances = new();

        private Dictionary<string, int> _functionCallCounts = new();

        private StreamWriter _writer;

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
                if (segment == "constant")
                    _writer.Write(VmToAssemblyStandardFunctions.PutConstantIntoR14(index));

                else if (CodeTranslationUtil.SegmentsVMToAssemblyMap.TryGetValue(segment, out var asmSegment))
                    _writer.Write(VmToAssemblyStandardFunctions.PutSegmentIndexValueIntoR14(asmSegment, index));

                else if (CodeTranslationUtil.PointersValues.TryGetValue(segment, out var ptr))
                    _writer.Write(VmToAssemblyStandardFunctions.PutPointerIndexValueIntoR14(ptr, index));
                else
                    throw new Exception($"Cannot handle segment {segment}");

                WriteGoToCommand("push");
            }
            else if(command == CommandType.C_Pop)
            {
                WriteGoToCommand("pop");
                
                if (segment == "constant")
                    _writer.Write(VmToAssemblyStandardFunctions.PopR14IntoConstant(index));

                else if (CodeTranslationUtil.SegmentsVMToAssemblyMap.TryGetValue(segment, out var asmSegment))
                    _writer.Write(VmToAssemblyStandardFunctions.PopR14IntoSegment(asmSegment, index));

                else if (CodeTranslationUtil.PointersValues.TryGetValue(segment, out var ptr))
                    _writer.Write(VmToAssemblyStandardFunctions.PopR14IntoPointerIndex(ptr, index));
                else
                    throw new Exception($"Cannot handle segment {segment}");
            }
            else
            {
                throw new Exception($"Try to call WritePushPop for command {command}");
            }
        }

        private void WriteStandardFunctions()
        {
            _writer.Write(VmToAssemblyStandardFunctions.GoToProgramStartDefinition);
            foreach(var kvp in CodeTranslationUtil.FunctionAndVmImplementations)
            {
                var functionEntrance = new FunctionEntrance(kvp.Key);
                _functionsEntrances.Add(kvp.Key, functionEntrance);
                var funcCode = AssemblyFunctionDefinitionCreator.PackAssemblyCode(functionEntrance, kvp.Value);
                _writer.Write(funcCode);
            }   
        }

        private void WriteGoToCommand(string command)
        {
            var code = AssemblyFunctionDefinitionCreator.GetGoToCommand(command, _functionCallCounts, _functionsEntrances);
            _writer.Write(code);
        }
    }
}