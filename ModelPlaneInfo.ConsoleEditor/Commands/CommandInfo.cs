namespace ModelPlaneInfo.ConsoleEditor.Commands
{
    public delegate void Command();
    public struct CommandInfo
    {
        public string _name;
        public Command _command;

        public CommandInfo(string name, Command command)
        {
            this._name = name;
            this._command = command;
        }
    }
}

