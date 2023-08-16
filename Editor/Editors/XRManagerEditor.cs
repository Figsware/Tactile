using System;
using UnityEditor;
using Tactile.XR;
using UnityEngine;

namespace Tactile.Editor.Editors
{
    [CustomEditor(typeof(XRManager))]
    public class XRManagerEditor : UnityEditor.Editor
    {
        private XRManager.XROverrideMode currentMode = XRManager.XROverrideMode.Default;

        private void Awake()
        {
            currentMode = XRManager.GetXROverrideMode();
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.BeginVertical("box");

            GUILayout.Label("Editor Settings", EditorStyles.boldLabel);
            GUILayout.Space(8);

            GUILayout.BeginHorizontal();
            GUILayout.Label("XR Startup Override: ");
            var mode = currentMode.ToolbarFromEnum();
            if (mode != currentMode)
            {
                XRManager.SetXROverrideMode(mode);
                currentMode = mode;
            }
            GUILayout.EndHorizontal();
            
            GUILayout.EndVertical();
        }
    }
}