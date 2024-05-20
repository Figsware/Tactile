using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;

namespace Tactile.Utility.Console
{
    public class Console : MonoBehaviour
    {
        public delegate void NewConsoleTextHandler(string newText);
        public event NewConsoleTextHandler OnNewConsoleText;

        private Dictionary<string, Command> _commands = new Dictionary<string, Command>();
        private string _consoleText = string.Empty;
        private static Regex CommandRegex = new (@"(?:""((?:\\""|[^""])+)""|((?:\\""|[^\s""])+))");

        protected string ConsoleText
        {
            get => _consoleText;
            set
            {
                _consoleText = value;
                OnNewConsoleText?.Invoke(_consoleText);
            }
        }

        private void Awake()
        {
            Application.logMessageReceived += OnLogMessage;

            // Find commands
            var commandTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes()
                    .Where(t => t.IsClass &&
                        !t.IsAbstract &&
                        t.IsSubclassOf(typeof(Command)) &&
                        t.GetCustomAttributes(typeof(RegisterCommandAttribute), false).Length > 0 &&
                        t.GetConstructor(Type.EmptyTypes) != null));
            
            foreach (var commandType in commandTypes)
            {
                Command command = (Command)Activator.CreateInstance(commandType);
                _commands.Add(command.Name, command);
            }
            
            // Find command attributes
            var methods = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes()
                    .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                    .Where(m => m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(Command.ExecutionContext))
                    .Where(m => m.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0)
                    .ToArray());

            foreach (var method in methods)
            {
                var commandAttribute = method.GetCustomAttribute<CommandAttribute>();

                void ExecuteCommand(Command.ExecutionContext executedCommand)
                {
                    method.Invoke(null, new[] { executedCommand });
                }

                var command = new Command(commandAttribute.Name, string.Empty, ExecuteCommand);
                _commands.Add(command.Name, command);
            }
        }

        public Command[] GetCommands() => _commands.Values.OrderBy(c => c.Name).ToArray();

        public void AddCommand(Command command)
        {
            _commands.Add(command.Name, command);
        }

        public string GetConsoleText()
        {
            return ConsoleText;
        }

        public void ExecuteCommand(string command)
        {
            var args = ParseArguments(command);
            ExecuteCommand(args);
        }

        public void ExecuteCommand(string[] args)
        {
            if (args.Length == 0)
                return;
            
            LogWithColor($"> {string.Join(" ", args)}", Color.gray);
            
            var commandName = args[0];
            if (_commands.TryGetValue(commandName, out var foundCommand))
            {
                foundCommand.Execute(this, args[1..]);
            }
            else
            {
                LogError($"Unknown command: {commandName}");
            }
        }

        private void OnLogMessage(string condition, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                case LogType.Assert:
                case LogType.Exception:
                    LogError(condition);
                    break;
                case LogType.Warning:
                    LogWarning(condition);
                    break;
                case LogType.Log:
                    Log(condition);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void ClearConsole()
        {
            ConsoleText = string.Empty;
            
        }

        public void Log(string message, bool withTime = true)
        {
            var now = DateTime.Now;
            var time = withTime ? $"[{now:T}] " : string.Empty;
            var formattedText = $"{time}{message}\n";
            ConsoleText += formattedText;
        }

        public void LogWarning(string message) => LogWithColor(message, Color.yellow);

        public void LogError(string message) => LogWithColor(message, Color.red);

        private static string[] ParseArguments(string input)
        {
            var matches = CommandRegex.Matches(input);
            var args = matches
                .Select(c => c.Groups
                    .Skip(1)
                    .First(g => !string.IsNullOrEmpty(g.Value))
                    .Value
                )
                .ToArray();

            return args;
        }

        private void LogWithColor(string message, Color color, bool withTime = true)
        {
            var hex = $"#{ColorUtility.ToHtmlStringRGB(color)}";
            var colorMessage = $"<color={hex}>{message}</color>";
            Log(colorMessage, withTime);
        }
    }
}