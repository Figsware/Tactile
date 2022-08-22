using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tactile
{
    /// <summary>
    /// An item of the template. Every item has a template key associated with it as well as a fallback value.
    /// </summary>
    /// <typeparam name="T">The type of the item</typeparam>
    [Serializable]
    public class TemplateItem<T> 
    {
        /// <summary>
        /// The key from the template associated with this item.
        /// </summary>
        public string templateKey;

        /// <summary>
        /// The fallback value to use if no template is found.
        /// </summary>
        public T fallbackValue;

        /// <summary>
        /// The value of the item, which is either from the template or a fallback value.
        /// </summary>
        public T Value => fallbackValue;

        /// <summary>
        /// Allows a template item to be used wherever its corresponding type can be used.
        /// </summary>
        /// <param name="templateItem">The template item to cast to its value type</param>
        public static implicit operator T(TemplateItem<T> templateItem)
        {
            return templateItem.Value;
        }
        
        [Serializable]
        public class Descriptor
        {
            public string key;
            public T value;
        }
    }
}