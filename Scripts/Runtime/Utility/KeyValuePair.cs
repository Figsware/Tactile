using System;

namespace Tactile
{
    [Serializable]
    public class KeyValuePair<T>
    {
        public string key;
        public T value;
    }
}