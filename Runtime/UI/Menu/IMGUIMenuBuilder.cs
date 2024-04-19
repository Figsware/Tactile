using System;
using Tactile.Utility.Logging;
using Tactile.Utility.Logging.Templates;
using UnityEditor;
using UnityEngine;

namespace Tactile.UI.Menu
{
    public class IMGUIMenuBuilder : MenuBuilder
    {
        private Navigator<MenuObject[]> currentItems;


        private void Awake()
        {
        }

        public override void SetMenuObjects(MenuObject[] newMenuObjects)
        {
            currentItems = new Navigator<MenuObject[]>(newMenuObjects);
        }

        private void OnGUI()
        {
            if (currentItems == null)
                return;

            GUILayout.BeginVertical("box");
            var items = currentItems.Peek();
            foreach (var item in items)
            {
                var prevColor = GUI.backgroundColor;

                if (item.Style.Color != null)
                    GUI.backgroundColor = item.Style.Color.GetTemplateValue(this);

                if (item is MenuFolder folder && GUILayout.Button(item.Style.Name))
                {
                    currentItems.Push(folder.Items);
                }

                if (item is MenuItem stateItem)
                {
                    GUI.enabled = !stateItem.State.Disabled;
                    switch (stateItem.State)
                    {
                        case MenuActionState actionState:
                            if (GUILayout.Button(item.Style.Name))
                                actionState.Invoke();
                            break;
                    }
                }

                GUI.backgroundColor = prevColor;
            }

            if (currentItems.Count > 1)
            {
                GUILayout.Space(8);
                if (GUILayout.Button("Back"))
                {
                    currentItems.Pop();
                }
            }

            GUILayout.EndVertical();
        }
    }
}