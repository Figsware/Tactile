using UnityEngine;

namespace Tactile.Utility.Logging
{
    public class Finder<T> where T: Component
    {
        private T component;
        
        public T GetComponent()
        {
            if (!component)
            {
                component = GameObject.FindObjectOfType<T>();
            }

            return component;
        }

        public static implicit operator T(Finder<T> finder) => finder.GetComponent();
    }
}