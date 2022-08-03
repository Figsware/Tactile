using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Plastic.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

namespace Tactile
{
    public static class TactileExtensions
    {
        /// <summary>
        /// Linearly lerps a transform to a specified position, orientation, and scale over a given time frame.
        /// </summary>
        /// <param name="transform">The transform to manipulate</param>
        /// <param name="targetLocalPosition">The target location position</param>
        /// <param name="targetLocalRotation">The target location rotation</param>
        /// <param name="targetLocalScale">The target scale</param>
        /// <param name="time">The time to translate</param>
        public static IEnumerator LocallyLerpToCoroutine(this Transform transform, Vector3 targetLocalPosition, Quaternion targetLocalRotation, Vector3 targetLocalScale, float time, bool slerp = false)
        {
            if (transform != null && transform)
            {
                // Store starting position and orientation.
                Vector3 startPos = transform.localPosition;
                Quaternion startRot = transform.localRotation;
                Vector3 startScale = transform.localScale;

                yield return LinearLerpOverTimeCoroutine(time, t =>
                {
                    transform.localPosition = slerp ? Vector3.Slerp(startPos, targetLocalPosition, t) : Vector3.Lerp(startPos, targetLocalPosition, t);
                    transform.localRotation = slerp ? Quaternion.Slerp(startRot, targetLocalRotation, t) : Quaternion.Lerp(startRot, targetLocalRotation, t);
                    transform.localScale = slerp ? Vector3.Slerp(startScale, targetLocalScale, t) : Vector3.Lerp(startScale, targetLocalScale, t);
                });
            }
            else
            {
                Debug.LogError("Tried to lerp a non-existent Transform!");
            }
        }

        /// <summary>
        /// Scales a GameObject to its original scale (assuming its disabled) over a specified amount of time to make it appear.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static IEnumerator ScaleToAppearCoroutine(this Transform transform, float time)
        {
            Vector3 startingScale = transform.localScale;
            transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            transform.gameObject.SetActive(true);

            // Perform the scale.
            yield return transform.LocallyLerpToCoroutine(transform.localPosition, transform.localRotation, startingScale, time, true);
        }

        /// <summary>
        /// Scales a GameObject to zero over a specified amount of time to make it disappear. 
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static IEnumerator ScaleToDisappearCoroutine(this Transform transform, float time)
        {
            Vector3 startingScale = transform.localScale;

            // Perform the scale.
            yield return transform.LocallyLerpToCoroutine(transform.localPosition, transform.localRotation, Vector3.zero, time, true);

            // Disable the game object and reset its scale.
            transform.gameObject.SetActive(false);
            transform.localScale = startingScale;
        }

        /// <summary>
        /// Waits for multiple coroutines to execute.
        /// </summary>
        /// <param name="executor"></param>
        /// <param name="enumerators"></param>
        /// <returns></returns>
        public static IEnumerator WaitForAllCoroutine(this MonoBehaviour executor, params IEnumerator[] enumerators)
        {
            Coroutine[] coroutines = new Coroutine[enumerators.Length];

            for (int i = 0; i < enumerators.Length; i++)
            {
                coroutines[i] = executor.StartCoroutine(enumerators[i]);
            }

            foreach (Coroutine coroutine in coroutines)
            {
                yield return coroutine;
            }
        }

        /// <summary>
        /// Performs an action over time using a linear lerp function.
        /// </summary>
        /// <param name="time">The time to perform the action</param>
        /// <param name="action">The action to receive the linear lerp value</param>
        public static IEnumerator LinearLerpOverTimeCoroutine(float time, Action<float> action)
        {
            yield return LerpFuncOverTimeCoroutine(time, elapsed => elapsed / time, action);
        }

        /// <summary>
        /// Performs an action over time using a specified lerp function.
        /// </summary>
        /// <param name="time">The time to call the lerp function over</param>
        /// <param name="lerpFunction">The function to calculate the lerp value based on the elapsed time.</param>
        /// <param name="action">The action to receive the lerp value</param>
        public static IEnumerator LerpFuncOverTimeCoroutine(float time, System.Func<float, float> lerpFunction, Action<float> action)
        {
            float elapsed = 0f;
            while (elapsed <= time)
            {
                // Calculate lerp from lerp function.
                float t = lerpFunction(elapsed);
                
                // Perform action using lerp.
                action(t);
                
                // Wait extra frame and elapse time.
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Give a lerp value of 1 so that it always finishes in a completed state.
            action(1f);
        }

        public static void RefreshLayout(this RectTransform rt)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
        }
        
        /// <summary>
        /// A routine to refresh the layout at the end of the frame.
        /// </summary>
        public static IEnumerator RefreshLayoutAtEndOfFrameCoroutine(this RectTransform rt)
        {
            yield return new WaitForEndOfFrame();
            rt.RefreshLayout();
        }

        /// <summary>
        /// Refreshes a RectTransform layout at the end of the frame using a coroutine.
        /// </summary>
        /// <param name="rt">RectTransform to update</param>
        /// <param name="executor">The executor of the refresh coroutine</param>
        /// <returns>The started refresh coroutine</returns>
        public static Coroutine RefreshLayoutAtEndOfFrame(this RectTransform rt, MonoBehaviour executor)
        {
            return executor.StartCoroutine(RefreshLayoutAtEndOfFrameCoroutine(rt));
        }
    }
}
