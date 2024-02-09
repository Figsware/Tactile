using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Tactile.Editor
{
    public static class TactileEditorUtility
    {
        [MenuItem("Tactile/Open Path/Data")]
        static void OpenDataPath()
        {
            EditorUtility.OpenWithDefaultApp(Application.dataPath);
        }

        [MenuItem("Tactile/Open Path/Console Log")]
        static void OpenConsoleLogPath()
        {
            EditorUtility.OpenWithDefaultApp(Application.consoleLogPath);
        }

        [MenuItem("Tactile/Open Path/Persistent Data")]
        static void OpenPersistentDataPath()
        {
            EditorUtility.OpenWithDefaultApp(Application.persistentDataPath);
        }

        [MenuItem("Tactile/Open Path/Streaming Assets")]
        static void OpenStreamingAssetsPath()
        {
            EditorUtility.OpenWithDefaultApp(Application.streamingAssetsPath);
        }

        [MenuItem("Tactile/Open Path/Temporary Cache")]
        static void OpenTemporaryCachePath()
        {
            EditorUtility.OpenWithDefaultApp(Application.temporaryCachePath);
        }

        public static T CastProperty<T>(this SerializedProperty property)
        {
            var targetObject = property.serializedObject.targetObject;
            var targetObjectType = targetObject.GetType();
            var field = targetObjectType.GetField(property.propertyPath,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null) return default;
            var cast = (T)field.GetValue(targetObject);

            return cast;
        }

        /// <summary>
        /// Gets the object the property represents.
        /// </summary>
        /// <see href="https://forum.unity.com/threads/get-a-general-object-value-from-serializedproperty.327098/#post-7569286"/>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static object GetTargetObjectOfProperty(this SerializedProperty prop)
        {
            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');
            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "")
                        .Replace("]", ""));
                    obj = GetValue_Imp(obj, elementName, index);
                }
                else
                {
                    obj = GetValue_Imp(obj, element);
                }
            }

            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see href="https://forum.unity.com/threads/get-a-general-object-value-from-serializedproperty.327098/#post-7569286"/>
        /// <param name="source"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static object GetValue_Imp(object source, string name)
        {
            if (source == null)
                return null;
            var type = source.GetType();

            while (type != null)
            {
                var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (f != null)
                    return f.GetValue(source);

                var p = type.GetProperty(name,
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p != null)
                    return p.GetValue(source, null);

                type = type.BaseType;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see href="https://forum.unity.com/threads/get-a-general-object-value-from-serializedproperty.327098/#post-7569286"/>
        /// <param name="source"></param>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static object GetValue_Imp(object source, string name, int index)
        {
            var enumerable = GetValue_Imp(source, name) as System.Collections.IEnumerable;
            if (enumerable == null) return null;
            var enm = enumerable.GetEnumerator();
            //while (index-- >= 0)
            //    enm.MoveNext();
            //return enm.Current;

            for (int i = 0; i <= index; i++)
            {
                if (!enm.MoveNext()) return null;
            }

            return enm.Current;
        }
    }
}