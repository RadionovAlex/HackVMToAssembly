namespace HackVMToAssembly.Parser
{
    public class ParserModule : IParserModule
    {
        private List<string> _rawTextLines;
        private int _currentRawIndex = -1;

        private Dictionary<int, Command> _commands = new();
        // private Dictionary<int, ComputeInstruction> linesInstructions = new();
        public ParserModule(string rawText)
        {
            _rawTextLines = rawText.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Where(l => !l.StartsWith("//")).ToList();
            _rawTextLines = RawsUtil.RemoveComments(_rawTextLines);
        }

        public string Arg1 => GetRawCommand().Arg1;

        public int Arg2 => GetRawCommand().Arg2 ?? -999;

        public CommandType GetCommandType() => GetRawCommand().CommandType;


        public bool HasMoreCommands() =>
            _currentRawIndex < _rawTextLines.Count - 1;

        public void ReadNext()
        {
            _currentRawIndex++;
        }

        private Command GetRawCommand()
        {
            if (_commands.TryGetValue(_currentRawIndex, out var command))
                return command;

            var newCommand = new Command(_rawTextLines[_currentRawIndex]);
            _commands.Add(_currentRawIndex, newCommand);

            return newCommand;
        }
    }
}