using System;
using Tactile.Utility.Console;
using UnityEditor;
using UnityEngine;
using Console = Tactile.Utility.Console.Console;

namespace Tactile.Gadgets
{
    public class IMGUIConsole : ConsoleAttacher
    {
        [SerializeField] private bool showConsole;
        [SerializeField] private Rect consoleRect;

        private string _consoleInput = string.Empty;

        private void Awake()
        {
            
        }

        private void ExecuteCommand()
        {
            _consoleInput = _consoleInput.Replace("\n", string.Empty);
            // Don't execute an empty command.
            if (_consoleInput.Length == 0)
                return;
            
            console.ExecuteCommand(_consoleInput);
            _consoleInput = string.Empty;
        }

        private void OnGUI()
        {
            if (!showConsole)
                return;
            
            GUILayout.BeginArea(consoleRect);
            GUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
            GUILayout.Box("Console");
            // GUILayout.Label("Console");

            if (console)
            {
                var consoleTextStyle = new GUIStyle(GUI.skin.label);
                consoleTextStyle.alignment = TextAnchor.LowerLeft;
                GUILayout.Label(console.GetConsoleText(), consoleTextStyle, GUILayout.ExpandHeight(true));
                GUILayout.BeginHorizontal();
                _consoleInput = GUILayout.TextArea(_consoleInput, GUILayout.ExpandWidth(true));
                
                bool hasNewline = _consoleInput.Contains("\n");
                if (hasNewline || GUILayout.Button("Execute", GUILayout.ExpandWidth(false)))
                    ExecuteCommand();
                
                
                GUILayout.EndHorizontal();
            }
            else
            {
                var textStyle = new GUIStyle(GUI.skin.label);
                textStyle.alignment = TextAnchor.MiddleCenter;
                textStyle.fontSize = 32;
                GUILayout.Label("<color=red>No console attached!</color>", textStyle, GUILayout.ExpandHeight(true));
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}