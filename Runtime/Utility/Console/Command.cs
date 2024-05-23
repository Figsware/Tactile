using System;
using System.Collections.Generic;
using System.Linq;

namespace Tactile.Utility.Console
{
    public class Command
    {
        public readonly string Name;
        public readonly string Description;
        public readonly Parameter[] Parameters;
        
        private Action<ExecutionContext> executeAction;

        public Command(string name, string description, Action<ExecutionContext> onExecute, params Parameter[] parameters)
        {
            executeAction = onExecute;
            Name = name;
            Description = description;
            Parameters = parameters;
        }

        protected Command(string name, string description, params Parameter[] parameters)
        {
            Name = name;
            Description = description;
            Parameters = parameters;
        }

        public virtual bool Execute(Console console, string[] arguments)
        {
            var info = CreateExecuteInfo(console, arguments);
            if (info == null)
                return false;

            executeAction?.Invoke(info);
            return true;
        }

        protected ExecutionContext CreateExecuteInfo(Console console, string[] arguments)
        {
            List<(string name, object value)> parsedArguments = new List<(string name, object value)>();
            bool successful = true;
            for (int i = 0; i < Parameters.Length && successful; i++)
            {
                var param = Parameters[i];
                if (i >= arguments.Length)
                {
                    if (param.IsRequired)
                    {
                        console.LogError($"Missing required argument: {param.Name}");
                        successful = false;
                    }
                    else
                    {
                        parsedArguments.Add((param.Name, null));
                    }
                }
                else
                {
                    var arg = arguments[i];
                    var match = param.Match(arg);
                    if (match.TryGetValue(out var value))
                    {
                        parsedArguments.Add((param.Name, value));
                    }
                    else
                    {
                        console.LogError($"Failed to parse argument: {param.Name}");
                        successful = false;
                    }
                }
            }

            if (successful)
            {
                return new ExecutionContext(console, parsedArguments.ToArray());
            }

            return null;
        }
        
        public class ExecutionContext
        {
            public readonly Console ExecutingConsole;
            public readonly (string name, object value)[] Arguments;
            public object this[int index] => Arguments[index].value;
            public object this[string name] => Arguments.First(a => a.name.Equals(name)).value;

            public ExecutionContext(Console executingConsole, (string name, object value)[] arguments)
            {
                Arguments = arguments;
                ExecutingConsole = executingConsole;
            }

            protected ExecutionContext(ExecutionContext other)
            {
                Arguments = other.Arguments;
                ExecutingConsole = other.ExecutingConsole;
            }
        }

        public override string ToString()
        {
            var description = string.IsNullOrEmpty(Description) ? "No description provided." : Description;
            var parameters = string.Join(" ", Parameters.Select(p => p.IsRequired ? $"({p.Name})" : $"[{p.Name}]").ToArray());
            return $"{Name} {parameters}: {description}";
        }

        protected static Action<ExecutionContext> CreateTypedExecutor<T>(Func<ExecutionContext, T> converter,
            Action<T> onExecute) where T: ExecutionContext
        {
            return info => onExecute(converter(info));
        }
    }
}