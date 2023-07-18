using System;
using UnityEditor;
using Tactile.XR;
using UnityEngine;

namespace Tactile.Editor.Editors
{
    [CustomEditor(typeof(XRManager))]
    public class XRManagerEditor : UnityEditor.Editor
    {
        public const string XREnabledKey = "TACTILE_XR_ENABLED";

        private enum XROverrideMode
        {
            Default,
            ForceOff,
            ForceOn
        }

        private XROverrideMode _enableXr = XROverrideMode.Default;

        private void Awake()
        {
            _enableXr = (XROverrideMode)EditorPrefs.GetInt(XREnabledKey, (int)XROverrideMode.Default);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.BeginVertical("box");

            GUILayout.Label("XRManager Editor Settings", EditorStyles.boldLabel);
            GUILayout.Space(8);
            
            // enableXR = GUILayout.Toggle(enableXR, "Enable XR");
            _enableXr = TactileEditorGUI.ToolbarFromEnum(_enableXr);
            
            GUILayout.EndVertical();
        }
    }
}