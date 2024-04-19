using System;
using System.Collections.Generic;
using Tactile.Utility.Logging;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tactile.UI.Menu.Keyboard
{
    [CreateAssetMenu(fileName = "KeyboardLayout", menuName = "Tactile/Keyboard Layout", order = 0)]
    public class KeyboardLayout : ScriptableObject
    {
        public Key keyPrefab;
        public UnityDictionary<KeyCode, KeyStyle> styles;
        public Row[] rows;
        public float rowSpacing;

        [Serializable]
        public class Row
        {
            public KeyLayout[] keys;
            public float spacing;
        }

        [Serializable]
        public class KeyLayout
        {
            public KeyCode key;
            public KeyCode shiftKey = KeyCode.None;
            public UnityDictionary<KeyboardManager.Modifier, KeyCode> modifiers;
            [FormerlySerializedAs("flex")] public float ratio = 1;

            public KeyLayout()
            {
                modifiers = new UnityDictionary<KeyboardManager.Modifier, KeyCode>();
                modifiers[KeyboardManager.Modifier.Default] = KeyCode.None;
            }
        }

        [Serializable]
        public class KeyStyle
        {
            public string text;
        }
    }
}