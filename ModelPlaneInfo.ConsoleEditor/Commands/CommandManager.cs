using Common.ConsoleIO;


namespace ModelPlaneInfo.ConsoleEditor.Commands
{
    public abstract class CommandManager
    {
        protected abstract void IniCommandInfoArray();
        protected CommandInfo[] commandInfoArray;
        public CommandManager()
        {
            IniCommandInfoArray();
        }
        protected static void ShowCommandsMenu(CommandInfo[] commandInfoArray, string prompt)
        {
            Console.WriteLine(prompt);
            for (int i = 0; i < commandInfoArray?.Length; i++)
            {
                Console.WriteLine("\t{0,2} - {1}", i, commandInfoArray[i]._name);
            }
        }
        protected static Command EnterCommand(CommandInfo[] commandInfoArray, string prompt)
        {
            Console.WriteLine();
            int number = Entering.EnterInt(prompt, 0, commandInfoArray.Length - 1);
            return commandInfoArray[number]._command;
        }
        public void Run()
        {
            while (true)
            {
                PrepareScreen();
                ShowCommandsMenu(commandInfoArray, "\n\tList of commands: ");
                Command command = EnterCommand(commandInfoArray, "Enter the menu command number");
                if (command == null) { return; }
                command();
            }
        }
        protected abstract void PrepareScreen();
    }
}
