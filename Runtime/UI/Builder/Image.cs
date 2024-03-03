using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tactile.UI.Menu.Builder
{
    public class Image<TMessage> : View<TMessage> where TMessage : Message
    {
        public Image(Texture2D tex)
        {
            
        }
        
        public override void Build(Action<TMessage> update)
        {
            GetComponent<RawImage>();
        }
    }
}