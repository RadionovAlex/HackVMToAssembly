using System.Text.RegularExpressions;

namespace HackVMToAssembly.Parser
{
    public class Command
    {
        private const string LabelPattern = @"\(([^)]+)\)";
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
            if (CommandType == CommandType.C_Label)
                Arg1 = ExtractLabelName(CommandName);


        }

        public CommandType CommandType { get; }
        public string CommandName {get; }
        public string Arg1 { get; }
        public int? Arg2 { get; }

        private CommandType DefineCommandType()
        {
            if (RawsUtil.ArithmeticFunctions.Contains(_raw))
                return CommandType.C_Arithmetic;

            if (_raw.StartsWith("push"))
                return CommandType.C_Push;

            if (_raw.StartsWith("pop"))
                return CommandType.C_Pop;

            if (_raw.StartsWith("(") && _raw.EndsWith(")"))
                return CommandType.C_Label;

            if (_raw.StartsWith("goto"))
                return CommandType.C_GoTo;

            if (_raw.StartsWith("if"))
                return CommandType.C_IfGoTo;

            if (_raw.StartsWith("function"))
                return CommandType.C_Function;

            if (_raw.StartsWith("return"))
                return CommandType.C_Return;

            if (_raw.StartsWith("call"))
                return CommandType.C_Call;


            throw new Exception($"Cannot parse command type from {_raw}");
        }

        private string ExtractLabelName(string label)
        {
            Match match = Regex.Match(label, LabelPattern);

            if (match.Success)
            {
                // Extract and print the value within parentheses
                string extractedValue = match.Groups[1].Value;
                return extractedValue;
            }
            throw new Exception($"Cannot extract label value from {label}");
        }
    }
}
