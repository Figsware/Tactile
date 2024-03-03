using System.Collections;
using UnityEngine;

namespace Tactile.UI.Menu
{
    /// <summary>
    /// An info card can quickly display information to the user by creating a card in 3D space and populating it with
    /// text, an icon, and a color. This is similar to what Android does with their "toast" cards.
    /// </summary>
    public abstract class InfoCard: MonoBehaviour
    {
        public abstract Color Color { set; }
        public abstract string Message { set; }

        public void Open()
        {
            StartCoroutine(OpenCoroutine());
        }

        public void Close()
        {
            StartCoroutine(CloseCoroutine());
        }

        public Coroutine ShowAndDestroy(float seconds)
        {
            return StartCoroutine(ShowAndDestroyCoroutine(seconds));
        }

        protected abstract IEnumerator OpenCoroutine();

        protected abstract IEnumerator CloseCoroutine();
        
        private IEnumerator ShowAndDestroyCoroutine(float seconds)
        {
            yield return OpenCoroutine();
            yield return new WaitForSeconds(seconds);
            yield return CloseAndDestroy();
        }
        
        private IEnumerator CloseAndDestroy()
        {
            yield return CloseCoroutine();
            Destroy(gameObject);
        }
    }
}