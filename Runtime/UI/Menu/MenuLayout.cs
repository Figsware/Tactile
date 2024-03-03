using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tactile.UI.Menu
{
    [CreateAssetMenu(fileName = "Menu", menuName = "Tactile/Action Menu")]
    public class MenuLayout : ScriptableObject
    {
        public ItemConfig[] items;
        
        [Serializable]
        public class ItemConfig
        {
            [SerializeField] public MenuStyle style;
            [SerializeField] public MenuLayout subLayout;
            [SerializeField] public string stateKey;
        }
    }
}