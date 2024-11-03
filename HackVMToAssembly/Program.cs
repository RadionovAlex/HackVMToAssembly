using HackVMToAssembly;
using HackVMToAssembly.CodeWriter;
using HackVMToAssembly.Parser;

string hackVMDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HackDirectory");
string hackAssemblyFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Hack.hasm");
var streamWriter = new StreamWriter(hackAssemblyFilePath, append: false);
var dirFiles = System.IO.Directory.EnumerateFiles(hackVMDirectoryPath, "*.vm");

bool sysInitFound = IsAnyFilesContainsSysInit();

var codeWriter = new CodeWriter(streamWriter);

codeWriter.WriteInit();
if (!sysInitFound)
    streamWriter.Write(VmToAssemblyStandardFunctions.WriteCustomSysInit);

codeWriter.WriteStandardFunctions();
codeWriter.WriteProgramStartLabel();

foreach (var vmFilePath in dirFiles)
{
    var fileContent = System.IO.File.ReadAllText(vmFilePath);
    var parser = new ParserModule(fileContent);

    var fileName = Path.GetFileName(vmFilePath);
    codeWriter.SetFileName(fileName);

    var staticSegmentUsageCount = 0;

    while (parser.HasMoreCommands())
    {
        parser.ReadNext();
        var commandType = parser.GetCommandType();
        switch (commandType)
        {
            case CommandType.C_Arithmetic:
                codeWriter.WriteArithmetic(parser.Arg1);
                break;

            case CommandType.C_Push:
            case CommandType.C_Pop:
                if(parser.Arg1 == "static")
                {
                    var index = parser.Arg2;
                    index++;
                    if (index > staticSegmentUsageCount)
                        staticSegmentUsageCount = index;
                }
                codeWriter.WritePushPop(commandType, parser.Arg1, parser.Arg2);
                break;

            case CommandType.C_Label:
                codeWriter.WriteLabel(parser.Arg1);
                break;

            case CommandType.C_GoTo:
                codeWriter.WriteGoTo(parser.Arg1);
                break;

            case CommandType.C_IfGoTo:
                codeWriter.WriteIf(parser.Arg1);
                break;

            case CommandType.C_Function:
                codeWriter.WriteFunction(parser.Arg1, parser.Arg2);
                break;

            case CommandType.C_Return:
                codeWriter.WriteReturn();
                break;

            case CommandType.C_Call:
                codeWriter.WriteCall(parser.Arg1, parser.Arg2);
                break;

            default: throw new Exception($"Unhandled command type: {commandType}");
        }
    }

    //each .vm file should have it`s own static segment space. So, in case of 3 files, there should be 3 different spaces.
    var staticPointerValue = CodeTranslationUtil.PointersValues["static"];
    staticPointerValue += staticSegmentUsageCount;
    CodeTranslationUtil.PointersValues["static"] = staticPointerValue;
}

streamWriter?.Flush();
streamWriter?.Close();

bool IsAnyFilesContainsSysInit()
{
    foreach (var vmFilePath in dirFiles)
    {
        var fileContent = System.IO.File.ReadAllText(vmFilePath);
        var contentParser = new ParserModule(fileContent);
        while (contentParser.HasMoreCommands())
        {
            contentParser.ReadNext();
            var commandType = contentParser.GetCommandType();
            if (commandType == CommandType.C_Function && contentParser.Arg1 == "Sys.init")
            {
                sysInitFound = true;
                Console.WriteLine("Found Sys.Init function");
                return true;
            }
        }
    }

    Console.WriteLine("Any of .vm files does not contains Sys.Init function");
    return false;
}
