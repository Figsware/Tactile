using System;
using System.Collections;
using System.Collections.Generic;
using Tactile.UI;
using UnityEditor;
using UnityEngine;

namespace Tactile.Editor.UI
{
    [CustomEditor(typeof(InfoCard), true)]
    public class InfoCardEditor : UnityEditor.Editor
    {
        private InfoCard infoCard;
        
        private void Awake()
        {
            infoCard = (InfoCard)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            TactileLayout.Horizontal("box", () =>
            {
                if (GUILayout.Button("Open"))
                    infoCard.Open();
                    
                if(GUILayout.Button("Close"))
                    infoCard.Close();
            });
        }
    }
}

