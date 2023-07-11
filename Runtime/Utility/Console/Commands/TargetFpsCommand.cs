using System;
using UnityEngine;
using Tactile.Utility.Console.Parameters;

namespace Tactile.Utility.Console.Commands
{
    [RegisterCommand]
    public class TargetFpsCommand : Command<int>
    {
        public TargetFpsCommand() : base("fps", "Sets the target fps.", SetTargetFPS, new IntegerParameter("fps", "The desired FPS", true))
        {
        }

        static void SetTargetFPS(ExecuteInfo info)
        {
            Application.targetFrameRate = info.Arg1;
        }
    }
}