using System.Collections;
using System.Collections.Generic;
using Tactile.Editor.Utility.PropertyShelves;
using Tactile.Utility.Logging.Folder;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Tactile.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(Folder<,>))]
    public class FolderPropertyDrawer : ShelfPropertyDrawer
    {
        public FolderPropertyDrawer()
        {
            AddShelf(new Shelf((rect, property, label) => GUI.Label(rect, label)));
            AddShelf(new NodeShelf("root"));
        }

        private class NodeShelf : IShelf
        {
            private readonly string _rootProperty;
            private ShelfGroup _group;
            private int _directoryId;
            private ReorderableList _list;

            public NodeShelf(string rootProperty)
            {
                _rootProperty = rootProperty;
                _group = new ShelfGroup();
                _list = new ReorderableList((IList)null, null);
                _group.AddShelf(new ReorderableListShelf(_list));
                //list.onAddCallback = list => { Debug.Log("Adding!"); };
                _list.drawElementCallback = ((rect, index, active, focused) =>
                {
                    EditorGUI.IntField(rect, 0);
                });
            }

            public void Render(Rect rect, SerializedProperty property, GUIContent label)
            {
                var directory = property.FindPropertyRelative(_rootProperty);
                var children = directory.FindPropertyRelative("children");
                if (_list.serializedProperty?.propertyPath != children.propertyPath)
                {
                    _list.serializedProperty = children;
                }

                _group.Render(rect, property, label);
            }

            public float GetHeight(SerializedProperty property, GUIContent label)
            {
                return _group.GetHeight(property, label);
            }
        }
    }
}