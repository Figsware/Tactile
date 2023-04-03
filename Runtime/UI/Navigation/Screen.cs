using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Tactile.UI.Navigation
{
    [AddComponentMenu("Tactile/UI/Screen")]
    public class Screen : MonoBehaviour, IScreen
    {
        public string key;
        public string title;
        public Texture2D icon;

        public string Key => key;
        public string Title => title;
        public Texture2D Icon => icon;
        
        [Header("Events")]
        public UnityEvent onAppear;
        public UnityEvent onDisappear;
    }
}