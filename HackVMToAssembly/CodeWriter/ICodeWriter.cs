namespace HackVMToAssembly.CodeWriter
{
    public interface ICodeWriter
    {
        void SetFileName(string fileName);

        void WriteInit();

        void WriteLabel(string label);

        void WriteGoTo(string goToLabel);

        void WriteIf(string label);

        void WriteCall(string funcName, int argumentsNumber);

        void WriteReturn();

        void WriteFunction(string funcName, int localsNumer);

        void WriteArithmetic(string command);

        void WritePushPop(CommandType command, string segment, int index);

        void Close();
    }
}
