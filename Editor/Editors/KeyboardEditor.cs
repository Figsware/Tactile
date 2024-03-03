using System;
using System.Collections;
using System.Collections.Generic;
using Tactile.UI.Menu.Keyboard;
using UnityEditor;
using UnityEngine;

namespace Tactile.Editor.Editors
{
    [CustomEditor(typeof(KeyboardManager), true)]
    public class KeyboardEditor : UnityEditor.Editor
    {
        private KeyboardManager _keyboardManager;
        
        private void Awake()
        {
            _keyboardManager = (KeyboardManager)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (EditorApplication.isPlaying)
            {
                
                if (GUILayout.Button("Paste"))
                    _keyboardManager.Paste();
                
                GUILayout.Label("Modifiers");
                GUILayout.BeginVertical("box");
                foreach (var modifier in KeyboardManager.Modifiers)
                {
                    var state = _keyboardManager[modifier];
                    var toggle = GUILayout.Toggle(state, modifier.ToString());
                    if (toggle != state)
                    {
                        _keyboardManager[modifier] = toggle;
                    }
                }
                
                GUILayout.EndVertical();
                
                GUILayout.Label("Keys");
                GUILayout.BeginVertical("box");
                
                foreach (var key in KeyboardManager.Keys)
                {
                    var state = _keyboardManager[key];
                    var toggle = GUILayout.Toggle(state, key.ToString());
                    if (toggle != state)
                    {
                        _keyboardManager[key] = toggle;
                    }
                }
                
                GUILayout.EndVertical();
            }
        }
    }
}