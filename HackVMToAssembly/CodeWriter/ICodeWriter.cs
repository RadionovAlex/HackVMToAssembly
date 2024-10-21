namespace HackVMToAssembly.CodeWriter
{
    public interface ICodeWriter
    {
        void SetFileName(string fileName);

        void WriteArithmetic(string command);

        void WritePushPop(CommandType command, string segment, int index);

        void Close();
    }
}
