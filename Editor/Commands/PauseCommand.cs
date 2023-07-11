﻿using Tactile.Utility.Console;
using UnityEditor;

namespace Tactile.Editor.Commands
{
    public class PauseCommand: Command
    {
        public PauseCommand() : base("pause", "Pauses play mode.", Pause)
        {
        }

        private static void Pause(ExecuteInfo command)
        {
            EditorApplication.isPaused = true;
        }
    }
}