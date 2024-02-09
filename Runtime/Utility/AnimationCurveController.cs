using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Tactile.Utility
{
    /// <summary>
    /// An animation curve controller allows you to animate a value along a curve over time, as well as being able to
    /// interrupt it to set a different value.
    /// </summary>
    [Serializable]
    public class AnimationCurveController
    {
        [Tooltip("The current t value")]
        [SerializeField] private float t;
        [Tooltip("The duration over which a transition will take place")]
        [SerializeField] private float duration = 1f;
        [Tooltip("The curve that the values will be animated on")]
        [SerializeField] private AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
        public UnityEvent<float> onNewValue;

        private int _numExecutions = 0;
        
        public float T
        {
            get => t;
            set
            {
                t = value;
                onNewValue.Invoke(Value);
            }
        }

        public float Value => curve.Evaluate(t);

        private float TPerSec => 1f / duration;
        
        public IEnumerator SetTCoroutine(float targetT)
        {
            var sign = targetT >= t ? 1 : -1;
            var tPerSec = sign * TPerSec;
            var id = ++_numExecutions;
            
            while (sign * (targetT - T) > 0f && id == _numExecutions)
            {
                T += tPerSec * Time.deltaTime;
                yield return null;
            }

            // Make sure that our execution wasn't interrupted.
            if (id == _numExecutions)
                T = targetT;
        }

        public float Evaluate() => curve.Evaluate(t);
    }
}