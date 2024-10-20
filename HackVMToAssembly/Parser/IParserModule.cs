namespace HackVMToAssembly.Parser
{
    public interface IParserModule
    {
        bool HasMoreCommands();
        void ReadNext();
        CommandType GetCommandType();

        string Arg1 { get; }

        int Arg2 { get; }
    }
}
