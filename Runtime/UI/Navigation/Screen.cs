using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Tactile.UI.Navigation
{
    [AddComponentMenu("Tactile/UI/Screen")]
    public class Screen : MonoBehaviour, IScreen
    {
        [Header("Metadata")]
        [SerializeField] private string key;
        [SerializeField] private string title;
        [SerializeField] private Texture icon;

        public string Key => key;
        public string Title => title;
        public Texture Icon => icon;
        
        [Header("Events")]
        public UnityEvent onAppear;
        public UnityEvent onDisappear;
    }
}