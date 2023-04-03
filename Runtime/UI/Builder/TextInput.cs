using System;

namespace Tactile.UI.Builder
{
    public class TextInput<TMessage> : View<TMessage> where TMessage : Message
    {
        public override void Build(Action<TMessage> update)
        {
            throw new NotImplementedException();
        }
    }
}