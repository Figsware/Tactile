using System;

namespace Tactile.Utility.Logging.Console.Commands
{
    [RegisterCommand]
    public class ClearCommand: Command
    {
        public ClearCommand() : base("clear", "Clears the screen.", Clear)
        {
        }

        private static void Clear(ExecutionContext info)
        {
            info.ExecutingConsole.ClearConsole();
        }
    }
}