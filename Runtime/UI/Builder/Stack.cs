using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tactile.UI.Menu.Builder
{
    public struct StackParameters : IViewParameters
    {
        public enum Direction
        {
            Vertical,
            Horizontal
        }

        public Direction direction;
        public int paddingTop;
        public int paddingBottom;
        public int paddingLeft;
        public int paddingRight;
        public float spacing;
        public bool? reverseLayout;
        public ContentSizeFitter.FitMode? horizontalFit;
        public ContentSizeFitter.FitMode? verticalFit;

        public void PopulateWithDefaultValues()
        {
            direction = Direction.Vertical;
            spacing = 0;
            reverseLayout = false;
            horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        }

        public StackParameters Spacing(float spacing)
        {
            this.spacing = spacing;
            return this;
        }
    }

    public static class Stack
    {
        public static View<TMessage, StackParameters> Spacing<TMessage>(this View<TMessage, StackParameters> view,
            float spacing) where TMessage : Message
        {
            view.ViewParameters.spacing = spacing;
            return view;
        }

        public static View<TMessage, StackParameters> ReverseLayout<TMessage>(this View<TMessage, StackParameters> view,
            bool reverseLayout) where TMessage : Message
        {
            view.ViewParameters.reverseLayout = reverseLayout;
            return view;
        }

        public static View<TMessage, StackParameters> HorizontalFit<TMessage>(this View<TMessage, StackParameters> view,
            ContentSizeFitter.FitMode fit) where TMessage : Message
        {
            view.ViewParameters.horizontalFit = fit;
            return view;
        }

        public static View<TMessage, StackParameters> VerticalFit<TMessage>(this View<TMessage, StackParameters> view,
            ContentSizeFitter.FitMode fit) where TMessage : Message
        {
            view.ViewParameters.verticalFit = fit;
            return view;
        }
    }

    public class Stack<TMessage> : View<TMessage, StackParameters> where TMessage : Message
    {
        public Stack(params View<TMessage>[][] children) : base(children)
        {
        }

        public Stack(StackParameters parameters, params View<TMessage>[][] children) : base(parameters, children)
        {
        }

        public override void Build(Action<TMessage> update)
        {
            FillParent(ViewRectTransform);

            HorizontalOrVerticalLayoutGroup layoutGroup;

            if (ViewParameters.direction == StackParameters.Direction.Vertical)
            {
                layoutGroup = GetComponent<VerticalLayoutGroup>();
                layoutGroup.childForceExpandWidth = true;
            }
            else
            {
                layoutGroup = GetComponent<HorizontalLayoutGroup>();
                layoutGroup.childForceExpandHeight = true;
            }

            layoutGroup.childControlHeight = true;
            layoutGroup.childControlWidth = true;
            layoutGroup.spacing = ViewParameters.spacing;
            layoutGroup.padding = new RectOffset(ViewParameters.paddingLeft, ViewParameters.paddingRight,
                ViewParameters.paddingTop, ViewParameters.paddingBottom);
            layoutGroup.reverseArrangement = ViewParameters.reverseLayout.GetValueOrDefault();

            var verticalFit = ViewParameters.verticalFit.GetValueOrDefault();
            var horizontalFit = ViewParameters.horizontalFit.GetValueOrDefault();
            if (verticalFit != ContentSizeFitter.FitMode.Unconstrained ||
                horizontalFit != ContentSizeFitter.FitMode.Unconstrained)
            {
                var csf = GetComponent<ContentSizeFitter>();
                csf.verticalFit = verticalFit;
                csf.horizontalFit = horizontalFit;
            }
            else
            {
                RemoveComponent<ContentSizeFitter>();
            }
        }
    }
}