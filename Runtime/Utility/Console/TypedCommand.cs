using System;

namespace Tactile.Utility.Logging.Console
{
    public class Command<TFirst> : Command
    {
        private Action<ExecutionContext> onExecute;

        public Command(string name, string description, Action<ExecutionContext> onExecute, Parameter<TFirst> firstParameter)
            : base(name, description, CreateTypedExecutor(info => new ExecutionContext(info), onExecute), firstParameter)
        {
        }

        public new class ExecutionContext : Command.ExecutionContext
        {
            public readonly TFirst Arg1;

            public ExecutionContext(Command.ExecutionContext other) : base(other)
            {
                Arg1 = (TFirst)Arguments[0].value;
            }
        }
    }

    public class Command<TFirst, TSecond> : Command
    {
        private Action<ExecutionContext> onExecute;

        public Command(string name, string description, Action<ExecutionContext> onExecute, Parameter<TFirst> firstParameter,
            Parameter<TSecond> secondParameter)
            : base(name, description, CreateTypedExecutor(info => new ExecutionContext(info), onExecute), firstParameter, secondParameter)
        {
        }

        public new class ExecutionContext : Command<TFirst>.ExecutionContext
        {
            public readonly TSecond Arg2;

            public ExecutionContext(Command.ExecutionContext other) : base(other)
            {
                Arg2 = (TSecond)Arguments[1].value;
            }
        }
    }

    public class Command<TFirst, TSecond, TThird> : Command
    {
        private Action<ExecutionContext> onExecute;

        public Command(string name, string description, Action<ExecutionContext> onExecute, Parameter<TFirst> firstParameter,
            Parameter<TSecond> secondParameter, Parameter<TThird> thirdParameter)
            : base(name, description, CreateTypedExecutor(info => new ExecutionContext(info), onExecute), firstParameter,
                secondParameter, thirdParameter)
        {
        }

        public new class ExecutionContext : Command<TFirst, TSecond>.ExecutionContext
        {
            public readonly TThird Arg3;

            public ExecutionContext(Command.ExecutionContext other) : base(other)
            {
                Arg3 = (TThird)Arguments[2].value;
            }
        }
    }

    public class Command<TFirst, TSecond, TThird, TFourth> : Command
    {
        private Action<ExecutionContext> onExecute;

        public Command(string name, string description, Action<ExecutionContext> onExecute, Parameter<TFirst> firstParameter,
            Parameter<TSecond> secondParameter, Parameter<TThird> thirdParameter, Parameter<TFourth> fourthParameter)
            : base(name, description, CreateTypedExecutor(info => new ExecutionContext(info), onExecute), firstParameter, secondParameter, thirdParameter,
                fourthParameter)
        {
        }

        public new class ExecutionContext : Command<TFirst, TSecond, TThird>.ExecutionContext
        {
            public readonly TFourth Arg4;

            public ExecutionContext(Command.ExecutionContext other) : base(other)
            {
                Arg4 = (TFourth)Arguments[3].value;
            }
        }
    }
    
    public class Command<TFirst, TSecond, TThird, TFourth, TFifth> : Command
    {
        private Action<ExecutionContext> onExecute;

        public Command(string name, string description, Action<ExecutionContext> onExecute, Parameter<TFirst> firstParameter,
            Parameter<TSecond> secondParameter, Parameter<TThird> thirdParameter, Parameter<TFourth> fourthParameter, Parameter<TFifth> fifthParameter)
            : base(name, description, CreateTypedExecutor(info => new ExecutionContext(info), onExecute), firstParameter, secondParameter, thirdParameter,
                fourthParameter)
        {
        }

        public new class ExecutionContext : Command<TFirst, TSecond, TThird, TFourth>.ExecutionContext
        {
            public readonly TFifth Arg5;

            public ExecutionContext(Command.ExecutionContext other) : base(other)
            {
                Arg5 = (TFifth)Arguments[4].value;
            }
        }
    }
}