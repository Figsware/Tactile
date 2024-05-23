using Tactile.Utility;
using UnityEditor;

namespace Tactile.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(UnityDictionary<,>))]
    public class UnityDictionaryPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(UnityEngine.Rect rect, SerializedProperty property,
            UnityEngine.GUIContent label)
        {
           // rect = EditorGUI.PrefixLabel(rect, label);
           var items = GetItemsProperty(property);
           EditorGUI.PropertyField(rect, items, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, UnityEngine.GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(GetItemsProperty(property));
        }
        
        private SerializedProperty GetItemsProperty(SerializedProperty property) => property.FindPropertyRelative("items");
       
    }
}

