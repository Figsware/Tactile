using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Tactile.Editor
{
    /// <summary>
    /// Allows editing of a template item. This will provide an interface to edit the value of a template item and choose
    /// whether to use a fallback or not.
    /// </summary>
    [CustomPropertyDrawer(typeof(TemplateItem<>))]
    public class TemplateItemDrawer : PropertyDrawer
    {
        private const string NO_CONFIG_FOUND = "There is no config provider in this scene.";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float totalHeight = 0f;
            EditorGUI.BeginProperty(position, label, property);
            SerializedObject configObj = GetTactileConfigObj();
            SerializedProperty templateArr = configObj.FindProperty("templateItems");
            Type propertyType = fieldInfo.FieldType;

            // Draw property label.
            position = EditorGUI.PrefixLabel(position, label);

            // Draw the key prop.
            Rect propertyRect = CreateFieldRect(position, ref totalHeight, position.width,
                EditorGUIUtility.singleLineHeight);

            SerializedProperty templateKeyProp = property.FindPropertyRelative("templateKey");
            string key = templateKeyProp.stringValue;
            // EditorGUI.PropertyField(propertyRect, templateKeyProp, GUIContent.none);
            EditorGUI.Popup(propertyRect, 0, EnumerateSerializedArray(templateArr)
                .Select(ti => ti.FindPropertyRelative("value"))
                .Where(ti => GetTypeOfProperty(ti).IsAssignableFrom(propertyType)).Select(ti => ti.name)
                .ToArray());

            // Draw config item preview.
            Rect configRect = CreateFieldRect(position, ref totalHeight, position.width,
                EditorGUIUtility.singleLineHeight);
            if (TactileConfig.Config is { } config)
            {
                SerializedObject obj = new SerializedObject(config);
                if (obj.FindProperty(key) is { } configProperty)
                {
                    EditorGUI.PropertyField(configRect, configProperty, GUIContent.none);
                    obj.ApplyModifiedProperties();
                }
                else
                {
                    EditorGUI.HelpBox(configRect, $"Cannot find property {key} in config.", MessageType.Error);
                }
            }
            else
            {
                EditorGUI.HelpBox(configRect, NO_CONFIG_FOUND, MessageType.Error);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int items = 2;
            return EditorGUIUtility.singleLineHeight * items + (items - 1) * 2f;
        }

        Rect CreateFieldRect(Rect position, ref float totalHeight, float itemWidth, float itemHeight,
            bool withIndent = true)
        {
            Rect rect = new Rect(position.x, position.y + totalHeight, itemWidth, itemHeight);
            if (withIndent)
                rect = EditorGUI.IndentedRect(rect);
            totalHeight += itemHeight + 2f;
            return rect;
        }

        SerializedObject GetTactileConfigObj()
        {
            SerializedObject obj = null;

            if (TactileConfig.Config is { } config)
            {
                obj = new SerializedObject(config);
            }

            return obj;
        }

        IEnumerable<SerializedProperty> EnumerateSerializedObject(SerializedObject obj, bool visibleOnly = true,
            bool enterChildren = false)
        {
            var property = obj.GetIterator();
            Func<bool, bool> next =
                visibleOnly ? (child) => property.NextVisible(child) : (child) => property.Next(child);
            next(true);
            while (next(enterChildren))
                yield return property;
        }

        IEnumerable<SerializedProperty> EnumerateSerializedArray(SerializedProperty property)
        {
            for (int i = 0; i < property.arraySize; i++)
                yield return property.GetArrayElementAtIndex(i);
        }

        Type GetTypeOfProperty(SerializedProperty property)
        {
            return property.serializedObject.targetObject.GetType().GetField(property.propertyPath).GetType();
        }
    }
}