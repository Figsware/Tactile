using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

                float elapsed = 0;
                while (elapsed <= time)
                {
                    elapsed += Time.deltaTime;
                    float t = elapsed / time;

                    // Lerp the position and rotation based on how much time has elapsed.
                    transform.localPosition = slerp ? Vector3.Slerp(startPos, targetLocalPosition, t) : Vector3.Lerp(startPos, targetLocalPosition, t);
                    transform.localRotation = slerp ? Quaternion.Slerp(startRot, targetLocalRotation, t) : Quaternion.Lerp(startRot, targetLocalRotation, t);
                    transform.localScale = slerp ? Vector3.Slerp(startScale, targetLocalScale, t) : Vector3.Lerp(startScale, targetLocalScale, t);
                    yield return null;
                }

                // Move the transform to the final position and orietnation.
                transform.localPosition = targetLocalPosition;
                transform.localRotation = targetLocalRotation;
                transform.localScale = targetLocalScale;
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
        public static IEnumerator WaitForAllCoroutine(this MonoBehaviour executor, IEnumerator[] enumerators)
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

    }
}
