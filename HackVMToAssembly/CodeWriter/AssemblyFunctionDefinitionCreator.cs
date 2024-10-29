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

        public static string GetFuncAssemblyCode(FunctionEntrance entrance, int localsNumber)
        {
            StringBuilder _sb = new StringBuilder();
            _sb.Append("\n");
            _sb.Append($"// {entrance.FunctionName} definition \n");
            _sb.Append($"({entrance.FunctionLabel}) \n");
            _sb.Append("\n");
            for(int i = 0; i < localsNumber; i++)
            {
                _sb.Append(VmToAssemblyStandardFunctions.PutConstantIntoD(0));
                _sb.Append(VmToAssemblyStandardFunctions.PushDefinition);
            }

            return _sb.ToString();
        }

        public static string GetGoToArithmeticCommand(string toGoCommand, Dictionary<string, int> funcCallsCount, Dictionary<string, FunctionEntrance> funcEntrances)
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

        public static string CallFunction(string funcName, int argumentsNumber, Dictionary<string, int> funcCallsCount, Dictionary<string, FunctionEntrance> funcEntrances)
        {
            if (funcCallsCount.TryGetValue(funcName, out var count))
                funcCallsCount[funcName] = count + 1;
            else
                funcCallsCount[funcName] = 0;

            var callsCount = funcCallsCount[funcName];
            var callLabel = @$"{funcName}Call_{callsCount}";

            var functionEntrance = funcEntrances[funcName];
            var code = @$"
// 1- push return address
@{callLabel}  
D=A
{VmToAssemblyStandardFunctions.PushDefinition}

// 2 - push current LCL
@LCL
D=M
{VmToAssemblyStandardFunctions.PushDefinition}

// 3 - push current ARG
@ARG
D=M
{VmToAssemblyStandardFunctions.PushDefinition};

// 4 - push current THIS
@THIS
D=M
{VmToAssemblyStandardFunctions.PushDefinition};

// 5 - push current THAT
@THAT
D=M
{VmToAssemblyStandardFunctions.PushDefinition};

// 6 - reposition current ARG
@5
D=A
@{argumentsNumber}
D=D+A
@SP
D=M-D

@ARG
M=D

// 7 - reposition LCL
@SP
D=M
@LCL
M=D

// 8 - transfer control
@{functionEntrance.FunctionLabel}
0;JMP

// 9 - label for return address
({callLabel})
";
            return code;
        }

        public static string ReturnFromFunction()
        {
            var code = @$"

// 3 - *ARG = pop() - reposition the return value for the caller
// changed order because of Pop uses registers R13 and R14, which 1,2 uses also for temporary needs
{VmToAssemblyStandardFunctions.PopDefinition}
{VmToAssemblyStandardFunctions.PopDIntoSegment("ARG", 0)}

// 1 - FRAME=LCL
@LCL
D=M
@R13 // FRAME
M=D

// 2 - RET = *(FRAME-5) put the return address in a temporary value
@5
D=A
@R13
A=M-D
D=M
@R14 // put return address to the R14
M=D

// 4 - SP = ARG+1
@ARG
D=M
@SP
M=D+1

// 5 - THAT = *(FRAME-1)
{FillRegisterFromStack("THAT", 1)}

// 6 - THIS = *(FRAME-2)
{FillRegisterFromStack("THIS", 2)}

// 7 - ARG = *(FRAME-3)
{FillRegisterFromStack("ARG", 3)}

// 8 - LCL = *(FRAME-4)
{FillRegisterFromStack("LCL", 4)}

// 9 - goto RET
@R14
0;JMP
";

            return code;

            string FillRegisterFromStack(string registerPointerName, int frameOffset) =>
             $@"
@{frameOffset}
D=A
@R13
A=M-D
D=M
@{registerPointerName} // put return address to the register ( THAT|THIS|ARG|LCL)
M=D
";
            
        }

        public static string GoTo(string funcName) =>
            $@"
@{funcName}
0;JMP
";

        public static string IfGoTo(string funcName) =>
            $@"
{VmToAssemblyStandardFunctions.PopDefinition}
@{funcName}
D;JGT
";
    }
}
