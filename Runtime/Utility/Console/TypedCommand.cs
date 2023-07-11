using System;

namespace Tactile.Utility.Console
{
    public class Command<TFirst> : Command
    {
        private Action<ExecuteInfo> onExecute;

        public Command(string name, string description, Action<ExecuteInfo> onExecute, Parameter<TFirst> firstParameter)
            : base(name, description, CreateTypedExecutor(info => new ExecuteInfo(info), onExecute), firstParameter)
        {
        }

        public new class ExecuteInfo : Command.ExecuteInfo
        {
            public readonly TFirst Arg1;

            public ExecuteInfo(Command.ExecuteInfo other) : base(other)
            {
                Arg1 = (TFirst)Arguments[0].value;
            }
        }
    }

    public class Command<TFirst, TSecond> : Command
    {
        private Action<ExecuteInfo> onExecute;

        public Command(string name, string description, Action<ExecuteInfo> onExecute, Parameter<TFirst> firstParameter,
            Parameter<TSecond> secondParameter)
            : base(name, description, CreateTypedExecutor(info => new ExecuteInfo(info), onExecute), firstParameter, secondParameter)
        {
        }

        public new class ExecuteInfo : Command<TFirst>.ExecuteInfo
        {
            public readonly TSecond Arg2;

            public ExecuteInfo(Command.ExecuteInfo other) : base(other)
            {
                Arg2 = (TSecond)Arguments[1].value;
            }
        }
    }

    public class Command<TFirst, TSecond, TThird> : Command
    {
        private Action<ExecuteInfo> onExecute;

        public Command(string name, string description, Action<ExecuteInfo> onExecute, Parameter<TFirst> firstParameter,
            Parameter<TSecond> secondParameter, Parameter<TThird> thirdParameter)
            : base(name, description, CreateTypedExecutor(info => new ExecuteInfo(info), onExecute), firstParameter,
                secondParameter, thirdParameter)
        {
        }

        public new class ExecuteInfo : Command<TFirst, TSecond>.ExecuteInfo
        {
            public readonly TThird Arg3;

            public ExecuteInfo(Command.ExecuteInfo other) : base(other)
            {
                Arg3 = (TThird)Arguments[2].value;
            }
        }
    }

    public class Command<TFirst, TSecond, TThird, TFourth> : Command
    {
        private Action<ExecuteInfo> onExecute;

        public Command(string name, string description, Action<ExecuteInfo> onExecute, Parameter<TFirst> firstParameter,
            Parameter<TSecond> secondParameter, Parameter<TThird> thirdParameter, Parameter<TFourth> fourthParameter)
            : base(name, description, CreateTypedExecutor(info => new ExecuteInfo(info), onExecute), firstParameter, secondParameter, thirdParameter,
                fourthParameter)
        {
        }

        public new class ExecuteInfo : Command<TFirst, TSecond, TThird>.ExecuteInfo
        {
            public readonly TFourth Arg4;

            public ExecuteInfo(Command.ExecuteInfo other) : base(other)
            {
                Arg4 = (TFourth)Arguments[3].value;
            }
        }
    }
    
    public class Command<TFirst, TSecond, TThird, TFourth, TFifth> : Command
    {
        private Action<ExecuteInfo> onExecute;

        public Command(string name, string description, Action<ExecuteInfo> onExecute, Parameter<TFirst> firstParameter,
            Parameter<TSecond> secondParameter, Parameter<TThird> thirdParameter, Parameter<TFourth> fourthParameter, Parameter<TFifth> fifthParameter)
            : base(name, description, CreateTypedExecutor(info => new ExecuteInfo(info), onExecute), firstParameter, secondParameter, thirdParameter,
                fourthParameter)
        {
        }

        public new class ExecuteInfo : Command<TFirst, TSecond, TThird, TFourth>.ExecuteInfo
        {
            public readonly TFifth Arg5;

            public ExecuteInfo(Command.ExecuteInfo other) : base(other)
            {
                Arg5 = (TFifth)Arguments[4].value;
            }
        }
    }
}