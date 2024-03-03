using System;

namespace Tactile.UI.Menu.Builder
{
    public class TextInput<TMessage> : View<TMessage> where TMessage : Message
    {
        public override void Build(Action<TMessage> update)
        {
            throw new NotImplementedException();
        }
    }
}