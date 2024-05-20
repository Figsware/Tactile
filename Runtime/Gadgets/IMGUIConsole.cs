using Tactile.Utility.Console;
using UnityEngine;

namespace Tactile.Gadgets
{
    public class IMGUIConsole : ConsoleAttacher
    {
        [SerializeField] private bool showConsole;
        [SerializeField] private Rect consoleRect;

        private string _consoleInput = string.Empty;
        private Vector2 _scrollVector = Vector2.zero;

        private void ExecuteCommand()
        {
            _consoleInput = _consoleInput.Replace("\n", string.Empty);
            // Don't execute an empty command.
            if (_consoleInput.Length == 0)
                return;
            
            console.ExecuteCommand(_consoleInput);
            _consoleInput = string.Empty; 
        }

        protected override void OnNewConsoleText(string text)
        {
            _scrollVector = new Vector2(0, float.PositiveInfinity);
        }

        private void OnGUI()
        {
            if (!showConsole)
                return;
            
            GUILayout.BeginArea(consoleRect);
            GUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
            GUILayout.Box("Console");

            if (console)
            {
                var consoleTextStyle = new GUIStyle(GUI.skin.label);
                consoleTextStyle.alignment = TextAnchor.LowerLeft;
                
                // Console text
                _scrollVector = GUILayout.BeginScrollView(_scrollVector);
                
                GUILayout.Label(console.GetConsoleText(), consoleTextStyle, GUILayout.ExpandHeight(true));
                GUILayout.EndScrollView();
                
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