using System;

namespace Tactile.UI.Menu.Builder
{
    public static class Surface
    {
        public struct Parameters : IViewParameters
        {
            public void PopulateWithDefaultValues()
            {
                
            }
        }

        public static View<TMessage, Parameters> Height<TMessage>(this View<TMessage, Parameters> view, float height) where TMessage : Message
        {
            return view;
        }
    }
    
    public class Surface<TMessage>: View<TMessage, Surface.Parameters> where TMessage : Message
    {
        public Surface(params View<TMessage>[][] children) : base(children)
        {
            
        }
        
        public override void Build(Action<TMessage> update)
        {
            throw new NotImplementedException();
        }
    }
}