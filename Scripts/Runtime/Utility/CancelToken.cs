using System.Collections;
using UnityEngine;

namespace Tactile.Utility
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
            while (!_cancelled && coroutine.MoveNext())
                yield return coroutine.Current;
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