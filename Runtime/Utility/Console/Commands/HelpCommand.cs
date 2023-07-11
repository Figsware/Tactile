using System;

namespace Tactile.Utility.Console.Commands
{
    [RegisterCommand]
    public class HelpCommand: Command
    {
        public HelpCommand() : base("help", "Shows a list of all available commands.", ShowHelp)
        {
        }

        private static void ShowHelp(ExecuteInfo executedCommand)
        {
            foreach (var command in executedCommand.ExecutingConsole.GetCommands())
            {
                executedCommand.ExecutingConsole.Log(command.ToString());
            }
        }
    }
}