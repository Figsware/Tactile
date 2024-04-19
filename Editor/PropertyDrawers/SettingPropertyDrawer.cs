using Tactile.Editor.Utility.PropertyShelves;
using Tactile.Utility.Logging;
using Tactile.Utility.Logging.Settings;
using UnityEditor;
using UnityEngine;

namespace Tactile.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(Setting<>))]
    public class SettingPropertyDrawer : ShelfPropertyDrawer
    {
        private const string ValuePropertyName = "value";
        private ShelfController<PropertyShelf> _valueChangeEvent;

        public SettingPropertyDrawer()
        {
            AddShelf(new Shelf((rect, property, label) =>
            {
                var rects = rect.HorizontalLayout(RectLayout.Flex(), RectLayout.SingleLineHeight);
                var valProp = GetValueProperty(property);


                EditorGUI.BeginChangeCheck();
                EditorGUI.PropertyField(rects[0], valProp, label, true);
                var changed = EditorGUI.EndChangeCheck();

                if (changed)
                {
                    property.serializedObject.ApplyModifiedProperties();
                    var setting = (ISettingChangedNotifier)property.GetTargetObjectOfProperty();
                    setting.SendChangeEvent();
                }

                if (EditorGUI.DropdownButton(rects[1], GUIContent.none, FocusType.Passive))
                    _valueChangeEvent.ToggleVisible();
            }, (property, label) => EditorGUI.GetPropertyHeight(GetValueProperty(property))));

            _valueChangeEvent = AddShelf(new ShelfController<PropertyShelf>(new PropertyShelf("onValueChange"), false));
        }

        private SerializedProperty GetValueProperty(SerializedProperty property) =>
            property.FindPropertyRelative(ValuePropertyName);
    }
}