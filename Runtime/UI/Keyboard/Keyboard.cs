using System;
using System.Collections.Generic;
using Tactile.UI.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace Tactile.UI.Menu.Keyboard
{
    public class Keyboard : MonoBehaviour
    {
        [SerializeField] private KeyboardManager keyboardManager;
        [SerializeField] private KeyboardLayout layout;
        [SerializeField] private RectTransform keyboardParent;

        private Dictionary<KeyCode, Key> _keys = new Dictionary<KeyCode, Key>();

        private void Awake()
        {
            BuildLayout();
        }
        
        private void BuildLayout()
        {
            _keys.Clear();
            
            var vlg = keyboardParent.gameObject.AddComponent<VerticalLayoutGroup>();
            vlg.spacing = layout.rowSpacing;
            var styles = layout.styles;
            
            foreach (var row in layout.rows)
            {
                GameObject rowObject = new GameObject("Keyboard Row");
                rowObject.transform.SetParent(keyboardParent);
                var rowRt = rowObject.AddComponent<RectTransform>();
                var rowGroup = rowObject.AddComponent<HorizontalLayoutGroup>();
                rowGroup.spacing = row.spacing;
                rowGroup.childControlWidth = true;
                rowGroup.childForceExpandWidth = false;
                rowGroup.childAlignment = TextAnchor.MiddleCenter;
                
                foreach (var keyLayout in row.keys)
                {
                    GameObject keyGameObject;
                    if (keyLayout.key != KeyCode.None)
                    {
                        var key = Instantiate(layout.keyPrefab, rowRt);
                        key.SetKeyText(keyLayout.key.ToString());
                        key.onKeyDown.AddListener(() => keyboardManager[keyLayout.key] = true);
                        key.onKeyUp.AddListener(() => keyboardManager[keyLayout.key] = false);
                        
                        // Apply key style.
                        if (styles.TryGetValue(keyLayout.key, out var style))
                        {
                            if (!string.IsNullOrEmpty(style.text))
                            {
                                key.SetKeyText(style.text);
                            }    
                        }

                        _keys.TryAdd(keyLayout.key, key);
                        keyGameObject = key.gameObject;
                    }
                    else
                    {
                        keyGameObject = new GameObject("Blank Key");
                        keyGameObject.transform.SetParent(rowRt);
                    }
                    
                    var layoutElement = keyGameObject.gameObject.AddComponent<LayoutElement>();
                    layoutElement.flexibleWidth = keyLayout.ratio;
                    layoutElement.minWidth = 0f;
                }
            }
        }
    }
}