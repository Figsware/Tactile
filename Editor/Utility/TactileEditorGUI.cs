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
        
        /// <summary>
        /// Given an enum, this will create a toolbar using the enum type's values as options and will use the passed
        /// in value as the currently selected option.
        /// </summary>
        /// <param name="selectedEnum">The enum value to be selected</param>
        /// <typeparam name="T">The type of the Enum (to derive options from)</typeparam>
        /// <returns>The selected enum from the toolbar</returns>
        public static T ToolbarFromEnum<T>(this T selectedEnum) where T: Enum
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
        

        /// <summary>
        /// Specifying GUIContent.none should remove all label spacing, but it doesn't. Use this to offset the rect so
        /// that it is truly without a label.
        /// </summary>
        /// <param name="rect">The rect to offset</param>
        /// <returns>Adjusted rect</returns>
        public static Rect OffsetPrefixLabelIndent(this Rect rect)
        {
            float offsetAmount = EditorGUI.indentLevel * 15f;
            Rect newRect = new Rect(rect.x - offsetAmount, rect.y, rect.width + offsetAmount, rect.height);
            
            return newRect;
        }
    }
}