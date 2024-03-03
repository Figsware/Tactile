using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tactile.UI.Menu.Builder
{
    public static class Button
    {
        public struct Parameters<TMessage> : IEventTrigger<TMessage>, IViewParameters where TMessage : Message
        {
            public Color? buttonColor;
            public Color? hoverColor;
            public Color? disabledColor;
            public bool? interactable;
            internal TMessage onClickMessage;
            
            public void PopulateWithDefaultValues()
            {
                interactable = true;
                buttonColor = Color.white;
                hoverColor = Color.yellow;
                disabledColor = Color.gray;
            }

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
        
        public static View<TMessage, Parameters<TMessage>> OnClick<TMessage>(this View<TMessage, Parameters<TMessage>> view, TMessage message) where TMessage : Message
        {
            view.ViewParameters.onClickMessage = message;
            return view;
        } 
        
        public static View<TMessage, Parameters<TMessage>> ButtonColor<TMessage>(this View<TMessage, Parameters<TMessage>> view, Color color) where TMessage : Message
        {
            view.ViewParameters.buttonColor = color;
            return view;
        } 
        
        public static View<TMessage, Parameters<TMessage>> HoverColor<TMessage>(this View<TMessage, Parameters<TMessage>> view, Color color) where TMessage : Message
        {
            view.ViewParameters.hoverColor = color;
            return view;
        } 
        
        public static View<TMessage, Parameters<TMessage>> DisabledColor<TMessage>(this View<TMessage, Parameters<TMessage>> view, Color color) where TMessage : Message
        {
            view.ViewParameters.disabledColor = color;
            return view;
        } 
        
        public static View<TMessage, Parameters<TMessage>> Interactable<TMessage>(this View<TMessage, Parameters<TMessage>> view, bool interactable) where TMessage : Message
        {
            view.ViewParameters.interactable = interactable;
            return view;
        } 
        
        
    }

    public class Button<TMessage>: View<TMessage, Button.Parameters<TMessage>> where TMessage: Message
    {
        public Button(View<TMessage> view): base(view.AsChildren())
        {
            
        }
        
        public Button(Button.Parameters<TMessage> parameters, View<TMessage> view): base(parameters, view.AsChildren())
        {
            
        }
        
        public override void Build(Action<TMessage> update)
        {
            var button = GetComponent<UnityEngine.UI.Button>();
            var graphic = GetComponent<RawImage>();
            button.targetGraphic = graphic;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => update(ViewParameters.onClickMessage));
            button.interactable = ViewParameters.interactable.GetValueOrDefault();
            var colors = button.colors;
            colors.normalColor = ViewParameters.buttonColor.GetValueOrDefault();
            colors.highlightedColor = ViewParameters.hoverColor.GetValueOrDefault();
            colors.disabledColor = ViewParameters.disabledColor.GetValueOrDefault();
            var trigger = GetComponent<UnityEngine.EventSystems.EventTrigger>();
            ViewParameters.ApplyToEventTrigger(trigger, update);
            button.colors = colors;
        }
    }
}