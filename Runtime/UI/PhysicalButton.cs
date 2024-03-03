using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Tactile.UI
{
    public class PhysicalButton : ButtonBase
    {
        [SerializeField] private Transform buttonTransform;
        [SerializeField] private Vector3 buttonAxis = Vector3.up;
        [SerializeField] private Vector3 buttonOrigin = Vector3.zero;
        [SerializeField] private float activationDistance = 0.5f;
        

        public override event IButton.ButtonPressHandler OnButtonStateChanged;

        /// <summary>
        /// Whether the button is currently pressed.
        /// </summary>
        public override bool IsPressed => _pressed;

        public float PressDistance => GetPressDistance();

        private bool _pressed = false;

        private void FixedUpdate()
        {
            if (!buttonTransform)
                return;
            
            var distance = PressDistance;

            // Determine button pressed.
            if (distance <= activationDistance && !_pressed)
            {
                OnButtonStateChanged?.Invoke(true);
                _pressed = true;
            }
            else if (distance > activationDistance && _pressed)
            {
                OnButtonStateChanged?.Invoke(false);
                _pressed = false;
            }
        }

        public float GetPressDistance()
        {
            var distance = Vector3.Dot(buttonTransform.transform.localPosition - buttonOrigin, buttonAxis.normalized);
            return distance;
        }

        private Vector3 GetButtonVector(float distance)
        {
            return buttonOrigin + buttonAxis * distance;
        }

        #if UNITY_EDITOR
        [SerializeField] [Header("Gizmos")] private float sphereRadius = 0.1f;
        private void OnDrawGizmosSelected()
        {
            if (!buttonTransform) return;
            
            Gizmos.matrix = buttonTransform.transform.GetParentOrWorldMatrix().localToWorldMatrix;
            Gizmos.color = IsPressed ? Color.green : Color.red;
            Gizmos.DrawSphere(buttonOrigin, sphereRadius);
            Gizmos.DrawLine(buttonOrigin, GetButtonVector(activationDistance));
        }
        #endif
    }
}