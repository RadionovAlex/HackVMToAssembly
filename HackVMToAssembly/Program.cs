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
            codeWriter.WritePushPop(commandType, parser.Arg1, parser.Arg2);
            break;

        default: throw new Exception($"Unhandled command type: {commandType}");
    }
}
codeWriter.Close();
