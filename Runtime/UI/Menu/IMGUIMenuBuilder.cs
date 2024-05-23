using System;
using Tactile.Utility;
using Tactile.Utility.Templates;
using UnityEditor;
using UnityEngine;

namespace Tactile.UI.Menu
{
    public class IMGUIMenuBuilder : MenuBuilder
    {
        private Navigator<MenuItem[]> currentItems;


        private void Awake()
        {
          
        }

        public override void SetMenuItems(MenuItem[] newMenuItems)
        {
            currentItems = new Navigator<MenuItem[]>(newMenuItems);
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
                
                if (GUILayout.Button(item.Style.Name))
                {
                    switch (item)
                    {
                        case MenuFolder folder:
                            currentItems.Push(folder.Items);
                            break;
                        case MenuAction action:
                            action.State.Invoke();
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