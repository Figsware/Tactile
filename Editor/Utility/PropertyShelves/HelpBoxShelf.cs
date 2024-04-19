using System.Collections.Generic;
using System.Linq;
using Tactile.Utility.Logging;
using UnityEditor;
using UnityEngine;

namespace Tactile.Editor.Utility.PropertyShelves
{
    public class HelpBoxShelf : IShelf
    {
        private List<(string message, MessageType messageType)> _messages = new();
        public float Gap = RectLayout.DefaultGap;
        public float MessageHeight = 2 * EditorGUIUtility.singleLineHeight;

        public void Render(Rect rect, SerializedProperty property, GUIContent label)
        {
            var rects = rect.VerticalLayout(Gap, RectLayout.Repeat(_messages.Count, RectLayout.Flex()));
            for (var i = 0; i < rects.Length; i++)
            {
                var msgRect = rects[i];
                var (message, messageType) = _messages[i];
                EditorGUI.HelpBox(msgRect, message, messageType);
            }
        }

        public float GetHeight(SerializedProperty property, GUIContent label)
        {
            var numMessages = _messages.Count;
            return Mathf.Max(0f, numMessages * MessageHeight + (numMessages - 1) * Gap);
        }

        public void ClearMessages()
        {
            _messages.Clear();
        }

        public void AddMessage(string message, MessageType messageType)
        {
            _messages.Add((message, messageType));
        }
    }
}