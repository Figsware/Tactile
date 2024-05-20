using UnityEditor;

namespace Tactile.Utility.Console.Commands
{
    [RegisterCommand]
    public class ExitCommand: Command
    {
        public ExitCommand() : base("exit", "Exits the application.", Exit)
        {
        }

        private static void Exit(ExecutionContext executedCommand)
        {
            #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
            #else
            Application.Quit();
            #endif
        }
    }
}