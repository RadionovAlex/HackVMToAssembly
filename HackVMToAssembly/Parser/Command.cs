namespace HackVMToAssembly.Parser
{
    public class Command
    {
        private string _raw;
        public Command(string raw)
        {
            _raw = raw;
            CommandType = DefineCommandType();
            var splittedWords = _raw.Split(new[] { " " }, StringSplitOptions.None);

            if(splittedWords.Length > 0)
                CommandName = splittedWords[0];
            if (splittedWords.Length > 1)
                Arg1 = splittedWords[1];
            if (splittedWords.Length > 2)
            {
                if (Int32.TryParse(splittedWords[2], out var value))
                    Arg2 = value;
            }


            if (CommandType == CommandType.C_Arithmetic)
                Arg1 = CommandName;
        }

        public CommandType CommandType { get; }
        public string CommandName {get; }
        public string Arg1 { get; }
        public int? Arg2 { get; }

        private CommandType DefineCommandType()
        {
            if (RawsUtil.ArithmeticFunctions.Contains(_raw))
                return CommandType.C_Arithmetic;
            else if (_raw.Contains("push"))
                return CommandType.C_Push;

            return CommandType.C_If;
        }
    }
}
