using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Tactile.UI.Builder
{
    public abstract partial class UserInterface<TMessage>: IUserInterface where TMessage : Message
    {
        public bool LogBuilds = false;
        public bool LogUpdates = false;
        private View<TMessage> _previousView = null; 
        private Dictionary<Type, Action<TMessage>> _messageHandlers = new Dictionary<Type, Action<TMessage>>();
        
        protected UserInterface()
        {
            FindMessageHandlers();
        }

        public virtual void UpdateState(TMessage message)
        {
            if (_messageHandlers.TryGetValue(message.GetType(), out var handler))
            {
                handler(message);
                SendViewUpdateEvent();
            }
        }

        protected void SendViewUpdateEvent() => OnViewUpdated?.Invoke();

        private void FindMessageHandlers()
        {
            _messageHandlers.Clear();

            var handlerMethods = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m => m.IsDefined(typeof(MessageHandlerAttribute), false));
            foreach (var info in handlerMethods)
            {
                var handlerAttribute =
                    info.GetCustomAttributes(typeof(MessageHandlerAttribute), false).FirstOrDefault() as
                        MessageHandlerAttribute;

                var parameters = info.GetParameters();

                if (handlerAttribute!.MessageType != null && parameters.Length == 0)
                {
                    _messageHandlers.Add(handlerAttribute.MessageType, msg => info.Invoke(this, new object[] { }));
                }
                else if (handlerAttribute.MessageType == null && parameters.Length == 1 &&
                         parameters[0].ParameterType == handlerAttribute.MessageType)
                {
                    _messageHandlers.Add(parameters[0].ParameterType, msg => info.Invoke(this, new object[] { msg }));
                }
                else
                {
                    throw new Exception("The MessageHandler attribute for " + info.Name + " is invalid!");
                }
            }
        }

        protected View<TMessage>[] ExpandEnum(IEnumerable<View<TMessage>> enumerable)
        {
            return enumerable.ToArray();
        }

        protected abstract View<TMessage> BuildView();
        public event IUserInterface.ViewUpdatedHandler OnViewUpdated;

        public void Build(RectTransform parent)
        {
            var view = BuildView();
            view.Build(_previousView, parent, UpdateState);
            _previousView = view;
        }
    }

    public class MessageHandlerAttribute : Attribute
    {
        public Type MessageType;
        public bool TriggerUpdate;

        public MessageHandlerAttribute()
        {
        }

        public MessageHandlerAttribute(Type messageType, bool triggerUpdate = true)
        {
            MessageType = messageType;
            TriggerUpdate = triggerUpdate;
        }
    }

    public abstract record Message;

    public interface IUserInterface
    {
        public delegate void ViewUpdatedHandler();
        public event ViewUpdatedHandler OnViewUpdated;
        public void Build(RectTransform parent);
    }
}