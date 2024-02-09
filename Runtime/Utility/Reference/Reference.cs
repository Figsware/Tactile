using System;

namespace Tactile.Utility.Reference
{
    [Serializable]
    public class Reference<T>
    {
        public bool isReference;
        public T value;

        public abstract class Foo
        {
            
        }
    }
    
    // public Reference<Color> myColor;
    // --> color value
    // --> asset reference --> color value
    
    
}