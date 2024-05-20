using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Tactile.Editor.Windows
{
    /// <summary>
    /// Enables you to prompt the editor user for text. Unity currently does not have a built in method of prompting for
    /// text, so this script allows you to do so.
    /// </summary>
    public class TextPromptWindow : EditorWindow
    {
        private string _text;
        private static readonly Vector2 WindowSize = new Vector2(400, 100);
        private TaskCompletionSource<string> _taskCompletionSource;
        private string _prompt = string.Empty;
        
        private void Awake()
        {
            _taskCompletionSource = new TaskCompletionSource<string>();
        }

        private void OnDestroy()
        {
            _taskCompletionSource.TrySetResult(null);
        }

        private void OnGUI()
        {
            GUILayout.Label(_prompt);
            GUILayout.Space(8);
            _text = GUILayout.TextArea(_text, GUILayout.ExpandHeight(true));
            GUILayout.Space(8);
            GUILayout.BeginHorizontal();

            var didCancel = GUILayout.Button("Cancel");
            var didSubmit = GUILayout.Button("Submit");

            if (didCancel)
                _taskCompletionSource.TrySetResult(null);
            else if (didSubmit)
                _taskCompletionSource.TrySetResult(_text);
            
            if (didCancel || didSubmit)
                Close();
            
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Prompts the user for text and returns the result.
        /// </summary>
        /// <param name="prompt">The prompt of the window</param>
        /// <returns>The prompted text</returns>
        public static Task<string> Prompt(string prompt)
        {
            var window = GetWindow<TextPromptWindow>();
            window.titleContent = new GUIContent("Enter Text");
            window.minSize = window.maxSize = WindowSize;
            window._prompt = prompt;
            window.ShowPopup();

            return window._taskCompletionSource.Task;
        }
    }
}