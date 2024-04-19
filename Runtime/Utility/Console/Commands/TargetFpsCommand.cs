using System;
using UnityEngine;
using Tactile.Utility.Logging.Console.Parameters;

namespace Tactile.Utility.Logging.Console.Commands
{
    [RegisterCommand]
    public class TargetFpsCommand : Command<int>
    {
        public TargetFpsCommand() : base("fps", "Sets the target fps.", SetTargetFPS, new IntegerParameter("fps", "The desired FPS", true))
        {
        }

        static void SetTargetFPS(ExecutionContext info)
        {
            Application.targetFrameRate = info.Arg1;
        }
    }
}