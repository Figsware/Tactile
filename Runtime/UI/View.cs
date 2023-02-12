using System;
using UnityEngine;
using UnityEngine.Events;

namespace Tactile.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class View : MonoBehaviour
    {
        protected RectTransform rectTransform;

        protected void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void FillParent()
        {
            
        }
    }
}