using System;
using UnityEngine;

namespace Tactile
{
    public class TactileLayout
    {
        public static void Horizontal(Action block)
        {
            GUILayout.BeginHorizontal();
            block();
            GUILayout.EndHorizontal();
        }
        
        public static void Horizontal(string style, Action block)
        {
            GUILayout.BeginHorizontal(style);
            block();
            GUILayout.EndHorizontal();
        }
        
        public static void Vertical(Action block)
        {
            GUILayout.BeginVertical();
            block();
            GUILayout.EndVertical();
        }
        
        public static void Vertical(string style, Action block)
        {
            GUILayout.BeginVertical(style);
            block();
            GUILayout.EndVertical();
        }
    }
}