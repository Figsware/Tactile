using System.Collections;
using UnityEngine;

namespace Tactile.Utility.Logging
{
    /// <summary>
    /// A cancel token allows you to interrupt an executing coroutine and gracefully let it finish. You can also use it
    /// within a coroutine too, rather than having to start/stop the entire coroutine. 
    /// </summary>
    public class CancelToken
    {
        public delegate void CancelDelegate();

        /// <summary>
        /// Called when a token is cancelled.
        /// </summary>
        public event CancelDelegate onTokenCancelled;
        public bool IsCancelled => _cancelled;
        
        private bool _cancelled = false;

        /// <summary>
        /// Cancels the coroutine.
        /// </summary>
        public void Cancel()
        {
            _cancelled = true;
            onTokenCancelled?.Invoke();
        }

        /// <summary>
        /// Executes a coroutine until it is cancelled. 
        /// </summary>
        /// <param name="coroutine">The coroutine to execute</param>
        public IEnumerator DoUntilCancelledCoroutine(IEnumerator coroutine)
        {
            IEnumerator flattened = coroutine.FlattenCoroutine();
            while (!_cancelled && flattened.MoveNext())
                yield return flattened.Current;
        }

        /// <summary>
        /// Executes all remaining steps within a single frame by ignoring "yield return null".
        /// </summary>
        /// <param name="coroutine">The coroutine to wrap</param>
        public IEnumerator SkipNullIfCancelledCoroutine(IEnumerator coroutine)
        {
            IEnumerator flattened = coroutine.FlattenCoroutine();
            while (!_cancelled && flattened.MoveNext())
                yield return flattened.Current;

            // Execute all remaining steps now.
            while (flattened.MoveNext()) { }
        }

        /// <summary>
        /// Waits for a set amount of time or until the token cancels.
        /// </summary>
        /// <param name="time">The time to wait for.</param>
        public IEnumerator WaitForTimeOrUntilCancelledCoroutine(float time)
        {
            float elapsed = 0;
            while (!_cancelled && elapsed <= time)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        /// <summary>
        /// Allows the token to be used as a bool in an expression. It will return true if the token has not been
        /// cancelled yet (so that you can do something like while token ...), and false otherwise. Furthermore, if the
        /// token is null, it will always return true.
        /// </summary>
        /// <param name="token">The token to examine</param>
        /// <returns>True if the token hasn't been cancelled, false otherwise</returns>
        public static implicit operator bool(CancelToken token)
        {
            return token == null || !token.IsCancelled;
        }
    };
}