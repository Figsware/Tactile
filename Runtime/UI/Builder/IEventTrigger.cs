using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Tactile.UI.Menu.Builder
{
    public interface IEventTrigger<TMessage> where TMessage : Message
    {
        public TMessage PointerEnter { get; set; }
        public TMessage PointerExit { get; set; }
        public TMessage PointerDown { get; set; }
        public TMessage PointerUp { get; set; }
        public TMessage PointerClick { get; set; }
        public TMessage Drag { get; set; }
        public TMessage Drop { get; set; }
        public TMessage Scroll { get; set; }
        public TMessage UpdateSelected { get; set; }
        public TMessage Select { get; set; }
        public TMessage Deselect { get; set; }
        public TMessage Move { get; set; }
        public TMessage InitializePotentialDrag { get; set; }
        public TMessage BeginDrag { get; set; }
        public TMessage EndDrag { get; set; }
        public TMessage Submit { get; set; }
        public TMessage Cancel { get; set; }
    }

    public static class EventTriggerUtil
    {
        public static View<TMessage, TParameters> OnPointerEnter<TMessage, TParameters>(this View<TMessage, TParameters> view, TMessage message) where TMessage : Message where TParameters : struct, IViewParameters, IEventTrigger<TMessage>
        {
            view.ViewParameters.PointerEnter = message;
            return view;
        }
        
        public static View<TMessage, TParameters> OnPointerExit<TMessage, TParameters>(this View<TMessage, TParameters> view, TMessage message) where TMessage : Message where TParameters : struct, IViewParameters, IEventTrigger<TMessage>
        {
            view.ViewParameters.PointerExit = message;
            return view;
        } 
        
        public static View<TMessage, TParameters> OnPointerDown<TMessage, TParameters>(this View<TMessage, TParameters> view, TMessage message) where TMessage : Message where TParameters : struct, IViewParameters, IEventTrigger<TMessage>
        {
            view.ViewParameters.PointerDown = message;
            return view;
        } 
        
        public static View<TMessage, TParameters> OnPointerUp<TMessage, TParameters>(this View<TMessage, TParameters> view, TMessage message) where TMessage : Message where TParameters : struct, IViewParameters, IEventTrigger<TMessage>
        {
            view.ViewParameters.PointerUp = message;
            return view;
        } 
        
        public static View<TMessage, TParameters> OnPointerClick<TMessage, TParameters>(this View<TMessage, TParameters> view, TMessage message) where TMessage : Message where TParameters : struct, IViewParameters, IEventTrigger<TMessage>
        {
            view.ViewParameters.PointerClick = message;
            return view;
        } 
        
        public static View<TMessage, TParameters> OnDrag<TMessage, TParameters>(this View<TMessage, TParameters> view, TMessage message) where TMessage : Message where TParameters : struct, IViewParameters, IEventTrigger<TMessage>
        {
            view.ViewParameters.Drag = message;
            return view;
        } 
        
        public static View<TMessage, TParameters> OnDrop<TMessage, TParameters>(this View<TMessage, TParameters> view, TMessage message) where TMessage : Message where TParameters : struct, IViewParameters, IEventTrigger<TMessage>
        {
            view.ViewParameters.Drop = message;
            return view;
        } 
        
        public static View<TMessage, TParameters> OnScroll<TMessage, TParameters>(this View<TMessage, TParameters> view, TMessage message) where TMessage : Message where TParameters : struct, IViewParameters, IEventTrigger<TMessage>
        {
            view.ViewParameters.Scroll = message;
            return view;
        } 
        
        public static View<TMessage, TParameters> OnUpdateSelected<TMessage, TParameters>(this View<TMessage, TParameters> view, TMessage message) where TMessage : Message where TParameters : struct, IViewParameters, IEventTrigger<TMessage>
        {
            view.ViewParameters.UpdateSelected = message;
            return view;
        } 
        
        public static View<TMessage, TParameters> OnSelect<TMessage, TParameters>(this View<TMessage, TParameters> view, TMessage message) where TMessage : Message where TParameters : struct, IViewParameters, IEventTrigger<TMessage>
        {
            view.ViewParameters.Select = message;
            return view;
        } 
        
        public static View<TMessage, TParameters> OnDeselect<TMessage, TParameters>(this View<TMessage, TParameters> view, TMessage message) where TMessage : Message where TParameters : struct, IViewParameters, IEventTrigger<TMessage>
        {
            view.ViewParameters.Deselect = message;
            return view;
        } 
        
        public static View<TMessage, TParameters> OnMove<TMessage, TParameters>(this View<TMessage, TParameters> view, TMessage message) where TMessage : Message where TParameters : struct, IViewParameters, IEventTrigger<TMessage>
        {
            view.ViewParameters.Move = message;
            return view;
        } 
        
        public static View<TMessage, TParameters> OnInitializePotentialDrag<TMessage, TParameters>(this View<TMessage, TParameters> view, TMessage message) where TMessage : Message where TParameters : struct, IViewParameters, IEventTrigger<TMessage>
        {
            view.ViewParameters.InitializePotentialDrag = message;
            return view;
        } 
        
        public static View<TMessage, TParameters> OnBeginDrag<TMessage, TParameters>(this View<TMessage, TParameters> view, TMessage message) where TMessage : Message where TParameters : struct, IViewParameters, IEventTrigger<TMessage>
        {
            view.ViewParameters.BeginDrag = message;
            return view;
        } 
        
        public static View<TMessage, TParameters> OnEndDrag<TMessage, TParameters>(this View<TMessage, TParameters> view, TMessage message) where TMessage : Message where TParameters : struct, IViewParameters, IEventTrigger<TMessage>
        {
            view.ViewParameters.EndDrag = message;
            return view;
        } 
        
        public static View<TMessage, TParameters> OnSubmit<TMessage, TParameters>(this View<TMessage, TParameters> view, TMessage message) where TMessage : Message where TParameters : struct, IViewParameters, IEventTrigger<TMessage>
        {
            view.ViewParameters.Submit = message;
            return view;
        } 
        
        public static View<TMessage, TParameters> OnCancel<TMessage, TParameters>(this View<TMessage, TParameters> view, TMessage message) where TMessage : Message where TParameters : struct, IViewParameters, IEventTrigger<TMessage>
        {
            view.ViewParameters.Cancel = message;
            return view;
        }

        public static TParameters ApplyToEventTrigger<TParameters, TMessage>(this TParameters parameters,
            EventTrigger trigger, Action<TMessage> update) where TMessage : Message
            where TParameters : struct, IViewParameters, IEventTrigger<TMessage>
        {
            var triggers = trigger.triggers;
            triggers.Clear();

            if (parameters.PointerEnter != null)
            {
                var te = new EventTrigger.TriggerEvent();
                te.AddListener(_ => update(parameters.PointerEnter));
                triggers.Add(new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerEnter,
                    callback = te
                });
            }
            
            if (parameters.PointerExit != null)
            {
                var te = new EventTrigger.TriggerEvent();
                te.AddListener(_ => update(parameters.PointerExit));
                triggers.Add(new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerExit,
                    callback = te
                });
            }
            
            return parameters;
        }
    }
}