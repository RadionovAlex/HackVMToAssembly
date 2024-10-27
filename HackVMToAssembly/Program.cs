using HackVMToAssembly.CodeWriter;
using HackVMToAssembly.Parser;

string hackVMFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Hack.vm");
string hackAssemblyFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Hack.hasm");
var text = System.IO.File.ReadAllText(hackVMFilePath);
var parser = new ParserModule(text);
var codeWriter = new CodeWriter();
codeWriter.SetFileName(hackAssemblyFilePath);
codeWriter.WriteProgramStartLabel();
codeWriter.WriteProgramVariablesInitialization();

while (parser.HasMoreCommands())
{
    parser.ReadNext();
    var commandType = parser.GetCommandType();
    switch (commandType)
    {
        case HackVMToAssembly.CommandType.C_Arithmetic:
            codeWriter.WriteArithmetic(parser.Arg1);
            break;

        case HackVMToAssembly.CommandType.C_Push:
        case HackVMToAssembly.CommandType.C_Pop:
            codeWriter.WritePushPop(commandType, parser.Arg1, parser.Arg2);
            break;

        case HackVMToAssembly.CommandType.C_Label:
            codeWriter.WriteLabel(parser.Command);
            break;

        case HackVMToAssembly.CommandType.C_GoTo:
            codeWriter.WriteGoTo(parser.Arg1);
            break;

        case HackVMToAssembly.CommandType.C_IfGoTo:
            codeWriter.WriteIf(parser.Arg1);
            break;

        case HackVMToAssembly.CommandType.C_Function:
            codeWriter.WriteFunction(parser.Arg1, parser.Arg2);
            break;

        case HackVMToAssembly.CommandType.C_Return:
            codeWriter.WriteReturn();
            break;

        case HackVMToAssembly.CommandType.C_Call:
            codeWriter.WriteCall(parser.Arg1, parser.Arg2);
            break;

        default: throw new Exception($"Unhandled command type: {commandType}");
    }
}
codeWriter.Close();
