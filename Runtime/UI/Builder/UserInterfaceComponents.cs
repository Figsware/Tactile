using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace Tactile.UI.Builder
{
    /// <summary>
    /// Provides a bunch of component definitions so that you don't have to keep typing "new" or Create methods.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public abstract partial class UserInterface<TMessage> where TMessage : Message
    {
        protected Stack<TMessage> Row(params View<TMessage>[][] children)
        {
            return Row(IViewParameters.CreateDefault<StackParameters>(), children);
        }
        
        protected Stack<TMessage> Row(StackParameters parameters, params View<TMessage>[][] children)
        {
            parameters.direction = StackParameters.Direction.Horizontal;
            return new Stack<TMessage>(parameters, children);
        }
        
        protected Stack<TMessage> Col(params View<TMessage>[][] children)
        {
            return Col(IViewParameters.CreateDefault<StackParameters>(), children);
        }
        
        protected Stack<TMessage> Col(StackParameters parameters, params View<TMessage>[][] children)
        {
            parameters.direction = StackParameters.Direction.Vertical;
            return new Stack<TMessage>(parameters, children);
        }
        
        protected Text<TMessage> Text(string text)
        {
            return new Text<TMessage>(text);
        }
        
        protected Text<TMessage> Text(Text.Parameters parameters, string text)
        {
            return new Text<TMessage>(parameters, text);
        }

        protected Button<TMessage> Button(View<TMessage> view)
        {
            return new Button<TMessage>(view);
        }
        
        protected Box<TMessage> Box(BoxParameters parameters, params View<TMessage>[][] children)
        {
            return new Box<TMessage>(parameters, children);
        }
        
        protected Box<TMessage> Box(params View<TMessage>[][] children)
        {
            return new Box<TMessage>(children);
        }

        protected Surface<TMessage> Surface(params View<TMessage>[][] children)
        {
            return new Surface<TMessage>(children);
        }
    }
}