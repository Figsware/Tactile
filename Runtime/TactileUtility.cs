using System;
using System.Collections;
using System.Collections.Generic;
using Tactile.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Tactile
{
    public static class TactileUtility
    {
        /// <summary>
        /// Linearly lerps a transform to a specified position, orientation, and scale over a given time frame.
        /// </summary>
        /// <param name="transform">The transform to manipulate</param>
        /// <param name="targetLocalPosition">The target location position</param>
        /// <param name="targetLocalRotation">The target location rotation</param>
        /// <param name="targetLocalScale">The target scale</param>
        /// <param name="time">The time to translate</param>
        public static IEnumerator LocallyLerpToCoroutine(this Transform transform, Vector3 targetLocalPosition,
            Quaternion targetLocalRotation, Vector3 targetLocalScale, float time, bool slerp = false,
            CancelToken token = null)
        {
            yield return LocallyLerpToCoroutine(transform,
                new Orientation(targetLocalPosition, targetLocalRotation, targetLocalScale), time, slerp, token);
        }

        /// <summary>
        /// Linearly lerps a transform to a specified position, orientation, and scale over a given time frame.
        /// </summary>
        /// <param name="transform">The transform to manipulate</param>
        /// <param name="targetOrientation">The target orientation</param>
        /// <param name="time">The time to translate</param>
        /// <param name="slerp">Whether to slerp or not</param>
        /// <param name="token">The cancel token to use</param>
        public static IEnumerator LocallyLerpToCoroutine(this Transform transform, Orientation targetOrientation,
            float time, bool slerp = false,
            CancelToken token = null)
        {
            yield return LerpToCoroutine(transform, targetOrientation, time, true, slerp, token);
        }

        public static IEnumerator LocallyLerpToWithArcCoroutine(this Transform transform, Orientation targetOrientation,
            Vector3 arcAxis, float arcHeight, float time, CancelToken token = null)
        {
            yield return LerpToWithArcCoroutine(transform, targetOrientation, arcAxis, arcHeight, time, true, token);
        }

        public static IEnumerator LerpToWithArcCoroutine(this Transform transform, Orientation targetOrientation,
            Vector3 arcAxis, float arcHeight, float time, bool local = false, CancelToken token = null)
        {
            Orientation from = Orientation.FromTransform(transform, local);
            var linerLerpEnumerator = CreateLinearLerpEnumerator(time, token);

            void Orient(Orientation orientation)
            {
                orientation.ApplyToTransform(transform, local);
            }

            yield return Orientation.LerpOrientationCoroutineWithArc(from, targetOrientation, arcAxis, arcHeight, linerLerpEnumerator, Orient);
        }

        /// <summary>
        /// Linearly lerps a transform to a specified position, orientation, and scale over a given time frame.
        /// </summary>
        /// <param name="transform">The transform to manipulate</param>
        /// <param name="targetOrientation">The target orientation</param>
        /// <param name="time">The time to translate</param>
        /// <param name="local">Whether to lerp in the local transform space</param>
        /// <param name="slerp">Whether to slerp or not</param>
        /// <param name="token">The cancel token to use</param>
        public static IEnumerator LerpToCoroutine(this Transform transform, Orientation targetOrientation,
            float time, bool local = false, bool slerp = false,
            CancelToken token = null)
        {
            if (transform != null && transform)
            {
                void Orient(Orientation orientation)
                {
                    orientation.ApplyToTransform(transform, local);
                }

                Orientation from = Orientation.FromTransform(transform, local);
                
                var linearLerpEnumerator = CreateLinearLerpEnumerator(time, token);

                if (slerp)
                {
                    yield return Orientation.SlerpOrientationCoroutine(from, targetOrientation, linearLerpEnumerator,
                        Orient);
                }
                else
                {
                    yield return Orientation.LerpOrientationCoroutine(from, targetOrientation, linearLerpEnumerator,
                        Orient);
                }
            }
            else
            {
                Debug.LogError("Tried to lerp a non-existent Transform!");
            }
        }
        
        public static IEnumerator LinearScaleCoroutine(this Transform transform, Vector3 targetLocalScale, float time,
            CancelToken token = null)
        {
            if (transform != null && transform)
            {
                Vector3 startScale = transform.localScale;
                yield return LinearLerpOverTimeCoroutine(time,
                    t => transform.localScale = Vector3.Lerp(startScale, targetLocalScale, t), token);
            }
        }

        /// <summary>
        /// Scales a GameObject to its original scale (assuming its disabled) over a specified amount of time to make it appear.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static IEnumerator ScaleToAppearCoroutine(this Transform transform, float time, CancelToken token = null)
        {
            Vector3 startingScale = transform.localScale;
            transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            transform.gameObject.SetActive(true);

            // Perform the scale.
            yield return transform.LocallyLerpToCoroutine(transform.localPosition, transform.localRotation,
                startingScale, time, true, token);
        }

        /// <summary>
        /// Scales a GameObject to zero over a specified amount of time to make it disappear. 
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static IEnumerator ScaleToDisappearCoroutine(this Transform transform, float time,
            CancelToken token = null)
        {
            Vector3 startingScale = transform.localScale;

            // Perform the scale.
            yield return transform.LocallyLerpToCoroutine(transform.localPosition, transform.localRotation,
                Vector3.zero, time, true, token);

            // Disable the game object and reset its scale.
            transform.gameObject.SetActive(false);
            transform.localScale = startingScale;
        }

        /// <summary>
        /// Waits for multiple coroutines to execute.
        /// </summary>
        /// <param name="enumerators">The coroutines to execute simultaneously</param>
        /// <returns></returns>
        public static IEnumerator WaitForAllCoroutine(params IEnumerator[] enumerators)
        {
            bool allDone = false;
            IEnumerator[] flattenedCoroutines = new IEnumerator[enumerators.Length];
            bool[] routineProgress = new bool[enumerators.Length];

            // Flatten coroutines
            for (int i = 0; i < flattenedCoroutines.Length; i++)
                flattenedCoroutines[i] = FlattenCoroutine(enumerators[i]);

            while (!allDone)
            {
                allDone = true;
                for (var i = 0; i < flattenedCoroutines.Length; i++)
                {
                    var e = flattenedCoroutines[i];

                    // If the coroutine isn't already done, execute it and check whether it is done (MoveNext returns false).
                    if (!routineProgress[i])
                        routineProgress[i] = !e.MoveNext();

                    allDone = allDone && routineProgress[i];
                }

                yield return null;
            }
        }

        /// <summary>
        /// "Flattens" a coroutine by stepping through the provided coroutine and any nested coroutines.
        /// </summary>
        /// <param name="coroutine">The coroutine to flatten</param>
        public static IEnumerator FlattenCoroutine(this IEnumerator coroutine)
        {
            Stack<IEnumerator> coroutines = new Stack<IEnumerator>();
            coroutines.Push(coroutine);

            while (coroutines.Count > 0)
            {
                IEnumerator c = coroutines.Peek();
                bool done = !c.MoveNext();
                object current = c.Current;

                if (done)
                {
                    coroutines.Pop();
                }
                else if (current is IEnumerator nestedC)
                {
                    coroutines.Push(nestedC);
                }
                else
                {
                    yield return current;
                }
            }
        }

        /// <summary>
        /// Performs an action over time using a linear lerp function.
        /// </summary>
        /// <param name="time">The time to perform the action</param>
        /// <param name="action">The action to receive the linear lerp value</param>
        public static IEnumerator LinearLerpOverTimeCoroutine(float time, Action<float> action,
            CancelToken token = null)
        {
            yield return LerpFuncOverTimeCoroutine(time, elapsed => elapsed / time, action, token);
        }

        public static IEnumerator LerpFunctionCoroutine(float time, Func<float, float> lerpFunction,
            Action<float> action, CancelToken token = null)
        {
            float elapsed = 0f;
            while (token && elapsed <= time)
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

        /// <summary>
        /// Creates a linear lerp enumerator.
        /// </summary>
        /// <param name="time">The time over which to perform the lerp</param>
        /// <param name="token">A cancel token to use for the enumerator</param>
        public static IEnumerator<float> CreateLinearLerpEnumerator(float time, CancelToken token = null)
        {
            return CreateTimeBasedLerpEnumerator(time, elapsed => elapsed / time, token);
        }

        /// <summary>
        /// Performs an action over time using a specified lerp function.
        /// </summary>
        /// <param name="time">The time to call the lerp function over</param>
        /// <param name="lerpFunction">The function to calculate the lerp value based on the elapsed time.</param>
        /// <param name="action">The action to receive the lerp value</param>
        public static IEnumerator LerpFuncOverTimeCoroutine(float time, Func<float, float> lerpFunction,
            Action<float> action, CancelToken token = null)
        {
            float elapsed = 0f;
            while (token && elapsed <= time)
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

        /// <summary>
        /// Uses a lerp enumerator and performs a specified action with the lerp value until the lerp is complete.
        /// </summary>
        /// <param name="lerpEnumerator">The enumerator to generate lerp values.</param>
        /// <param name="action">The action to perform with the lerp value</param>
        public static IEnumerator UseLerpEnumeratorCoroutine(IEnumerator<float> lerpEnumerator, Action<float> action)
        {
            while (lerpEnumerator.MoveNext())
            {
                // Get the t value from the lerp function.
                float t = lerpEnumerator.Current;

                // Perform the action.
                action(t);

                // Wait for next frame.
                yield return null;
            }
        }

        public static IEnumerator<float> CreateTimeBasedLerpEnumerator(float time, Func<float, float> lerpFunction,
            CancelToken token = null)
        {
            float elapsed = 0f;
            while (token && elapsed <= time)
            {
                // Calculate lerp from lerp function.
                float t = lerpFunction(elapsed);

                // Add delta time
                elapsed += Time.deltaTime;

                // Yield the lerp value.
                yield return t;
            }

            // Give a lerp value of 1 so that it always finishes in a completed state.
            yield return 1f;
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

        public static Color ChooseTextColorForBackgroundColor(this Color backgroundColor, Color lightTextColor,
            Color darkTextColor)
        {
            float r = backgroundColor.r;
            float g = backgroundColor.g;
            float b = backgroundColor.b;
            bool useDarkColor = r * 0.299f + g * 0.587f + b * 0.114f > 186;
            return useDarkColor ? darkTextColor : lightTextColor;
        }

        public static Orientation ToLocalOrientation(this Transform transform)
        {
            return Orientation.LocalFromTransform(transform);
        }

        public static Orientation ToWorldOrientation(this Transform transform)
        {
            return Orientation.WorldFromTransform(transform);
        }

        public static Texture2D CreateSolidColorTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();

            return texture;
        }

        /// <summary>
        /// no survivors
        /// </summary>
        /// <param name="transform">The transform whose children will be destroyed</param>
        public static void DestroyAllChildren(this Transform transform)
        {
            int totalChildren = transform.childCount;
            for (int i = 0; i < totalChildren; i++)
            {
                var child = transform.GetChild(i).gameObject;
                GameObject.Destroy(child);
            }
        }

        public static void DestroyAllChildren(this GameObject gameObject) {
            gameObject.transform.DestroyAllChildren();
        }

        public static T[] GetComponentsInDirectChildren<T>(this GameObject gameObject, bool includeInactive = false) where T: MonoBehaviour
        {
            List<T> components = new List<T>();
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i).gameObject;
                if (child.GetComponent<T>() is { } component && (includeInactive || child.activeSelf))
                {
                    components.Add(component);
                }
            }

            return components.ToArray();
        }
    }
}