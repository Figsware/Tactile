using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Tactile.UI.Events
{
    public class ButtonEvents : MonoBehaviour
    {
        [SerializeField] public ButtonBase button;
        public UnityEvent OnPress;
        public UnityEvent OnRelease;
        public UnityEvent OnHoldBegin;
        public UnityEvent OnHoldInterval;
        public UnityEvent OnHoldEnd;
        [SerializeField] private float holdDelay = 2f;
        [SerializeField] private float holdInterval = 0.5f;
        private Coroutine holdCoroutine;
        private bool isHolding = false;

        private void OnEnable()
        {
            AddButtonListeners();
        }

        private void OnDisable()
        {
            RemoveButtonListeners();
        }

        public void SetButton(ButtonBase button)
        {
        }

        private void AddButtonListeners()
        {
            if (!button)
                return;

            button.OnButtonStateChanged += OnButtonStateChanged;
        }

        private void RemoveButtonListeners()
        {
            if (!button)
                return;

            button.OnButtonStateChanged -= OnButtonStateChanged;
        }

        private void OnButtonStateChanged(bool isPressed)
        {
            if (isPressed)
            {
                holdCoroutine = StartCoroutine(HoldCoroutine());
                OnPress.Invoke();
            }
            else
            {
                StopCoroutine(holdCoroutine);
                OnRelease.Invoke();

                if (isHolding)
                {
                    OnHoldEnd.Invoke();
                }
            }
        }

        private IEnumerator HoldCoroutine()
        {
            isHolding = false;
            yield return new WaitForSeconds(holdDelay);

            OnHoldBegin.Invoke();
            isHolding = true;
            while (button.IsPressed)
            {
                OnHoldInterval.Invoke();
                yield return new WaitForSeconds(holdInterval);
            }
        }
    }
}