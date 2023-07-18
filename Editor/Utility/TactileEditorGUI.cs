using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Tactile.Editor
{
    public static class TactileEditorGUI
    {
        private static Dictionary<Type, string[]> _enumValues = new Dictionary<Type, string[]>();
        
        public static T ToolbarFromEnum<T>(T selectedEnum) where T: Enum
        {
            if (!_enumValues.ContainsKey(typeof(T)))
            {
                var values = Enum.GetValues(typeof(T))
                    .Cast<T>()
                    .Select(e => e.ToString())
                    .ToArray();

                _enumValues[typeof(T)] = values;
            }
            
            var index = GUILayout.Toolbar(Convert.ToInt32(selectedEnum), _enumValues[typeof(T)]);
            var newEnum = (T)Enum.ToObject(typeof(T), index);

            return newEnum;
        }
    }
}