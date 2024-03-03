using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tactile.UI.Menu.Builder
{
    public static class View
    {
        public static View<TMessage, TParameters> With<TMessage, TParameters>(this View<TMessage, TParameters> view, TParameters parameters)
            where TMessage : Message where TParameters : struct, IViewParameters
        {
            view.ViewParameters = IViewParameters.FillMissing(view.ViewParameters, parameters);
            return view;
        }
        
        public static View<TMessage>[][] AsChildren<TMessage>(this View<TMessage> view) where TMessage : Message
        {
            return new[] { new[] { view } };
        }
    }
    
    public abstract class View<TMessage, TParameters> : View<TMessage>, IViewWithParameters, IEquatable<View<TMessage, TParameters>>
        where TMessage : Message where TParameters : struct, IViewParameters
    {
        internal TParameters ViewParameters;
        internal string key;

        protected View()
        {
            ViewParameters = IViewParameters.CreateDefault<TParameters>();
        }

        protected View(View<TMessage>[][] children) : base(children)
        {
            ViewParameters = IViewParameters.CreateDefault<TParameters>();
        }

        protected View(TParameters parameters)
        {
            ViewParameters = IViewParameters.FillMissing(parameters);
        }

        protected View(TParameters? parameters)
        {
            ViewParameters = parameters != null ? IViewParameters.FillMissing(parameters.Value) : IViewParameters.CreateDefault<TParameters>();
        }

        protected View(TParameters parameters, View<TMessage>[][] children) : base(children)
        {
            ViewParameters = IViewParameters.FillMissing(parameters);
        }

        protected View(TParameters? parameters, View<TMessage>[][] children) : base(children)
        {
            ViewParameters = parameters != null ? IViewParameters.FillMissing(parameters.Value) : IViewParameters.CreateDefault<TParameters>();
        }
        
        public bool Equals(View<TMessage, TParameters> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && ViewParameters.Equals(other.ViewParameters);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && Equals((View<TMessage, TParameters>)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), ViewParameters);
        }

        public IViewParameters GetParameters()
        {
            return ViewParameters;
        }
    }

    public abstract class View<TMessage> : IEnumerable<View<TMessage>> where TMessage : Message
    {
        protected RectTransform ViewRectTransform;
        private readonly View<TMessage>[] _children;
        private View<TMessage> this[int index] => index < _children.Length ? _children[index] : null;
        private int NumChildren => _children.Length;

        protected View()
        {
            _children = Array.Empty<View<TMessage>>();
        }

        protected View(params View<TMessage>[][] children)
        {
            _children = children.SelectMany(c => c).ToArray();
        }

        public T GetComponent<T>() where T : MonoBehaviour
        {
            T component = ViewRectTransform.gameObject.GetComponent<T>();
            if (!component)
            {
                component = ViewRectTransform.gameObject.AddComponent<T>();
            }

            return component;
        }
        
        public void RemoveComponent<T>() where T : MonoBehaviour
        {
            T component = ViewRectTransform.gameObject.GetComponent<T>();
            if (component)
            {
                GameObject.Destroy(component);
            }
        }

        public abstract void Build(Action<TMessage> update);

        public virtual void Build(View<TMessage> previousView, RectTransform parent, Action<TMessage> update)
        {
            if (previousView == null || previousView.GetType() != GetType())
            {
                CreateRectTransform(parent);
            }
            else
            {
                ViewRectTransform = previousView.ViewRectTransform;
            }

            for (int i = 0; i < NumChildren; i++)
            {
                View<TMessage> childPreviousView = previousView?[i];
                View<TMessage> child = this[i];
                child.Build(childPreviousView, ViewRectTransform, update);
            }

            // Destroy extra children
            int prevNumChildren = previousView?.NumChildren ?? 0;
            for (int i = _children.Length; i < prevNumChildren; i++)
            {
                Object.Destroy(previousView![i].ViewRectTransform.gameObject);
            }

            if (!Equals(previousView))
            {
                Build(update);
            }
        }

        public void CreateRectTransform(RectTransform parent)
        {
            GameObject go = new GameObject(GetType().Name);
            var rt = go.AddComponent<RectTransform>();
            rt.SetParent(parent);
            
            rt.localRotation = Quaternion.identity;
            rt.localPosition = Vector3.zero;
            rt.localScale = Vector3.one;
            
            if (this is IViewWithParameters paramView && paramView.GetParameters() is ISizeable sizable)
            {
                Vector2 xAnchor = new Vector2(rt.anchorMin.x, rt.anchorMax.x);
                Vector2 yAnchor = new Vector2(rt.anchorMin.y, rt.anchorMax.y);
                Vector2 xOffset = new Vector2(rt.offsetMin.x, rt.offsetMax.x);
                Vector2 yOffset = new Vector2(rt.offsetMin.y, rt.offsetMax.y);
                
                sizable.XSize.GetAnchorAndOffset(ref xAnchor, ref xOffset);
                sizable.YSize.GetAnchorAndOffset(ref yAnchor, ref yOffset);
                
                rt.anchorMin = new Vector2(xAnchor.x, yAnchor.x);
                rt.anchorMax = new Vector2(xAnchor.y, yAnchor.y);
                rt.offsetMin = new Vector2(xOffset.x, yOffset.x);
                rt.offsetMax = new Vector2(xOffset.y, yOffset.y);
            }

            
            ViewRectTransform = rt;
        }

        public static RectTransform FillParent(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;

            return rt;
        }

        public IEnumerator<View<TMessage>> GetEnumerator()
        {
            foreach (var child in _children)
            {
                yield return child;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType();
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<View<TMessage>> DepthFirstTraverse()
        {
            foreach (var child in _children)
            {
                foreach (var childView in child.DepthFirstTraverse())
                {
                    yield return childView;
                }
            }

            yield return this;
        }

        public IEnumerable<View<TMessage>> BreadthFirstTraverse()
        {
            yield return this;

            foreach (var child in _children)
            {
                foreach (var childView in child.BreadthFirstTraverse())
                {
                    yield return childView;
                }
            }
        }
        
        public static implicit operator View<TMessage>[](View<TMessage> view) => new [] { view };
    }

    public interface IViewWithParameters
    {
        IViewParameters GetParameters();
    }
    
    public interface IViewParameters
    {
        public void PopulateWithDefaultValues();

        public static T CreateDefault<T>() where T : struct, IViewParameters
        {
            T parameters = default;
            parameters.PopulateWithDefaultValues();

            return parameters;
        }

        public static T FillMissing<T>(T parameters, T? with = null) where T: struct, IViewParameters
        {
            object baseParameters = with ?? CreateDefault<T>();
            object filledIn = parameters;
            
            var nullableFields = typeof(T).GetFields().Where(fi => fi.FieldType.IsGenericType && fi.FieldType.GetGenericTypeDefinition() == typeof(Nullable<>));
            foreach (var info in nullableFields)
            {
                object val = info.GetValue(parameters);
                if (val == null)
                {
                    info.SetValue(filledIn, info.GetValue(baseParameters));
                }
            }

            var nullableProperties = typeof(T).GetProperties().Where(p => p.CanRead && p.CanWrite);
            foreach (var info in nullableProperties)
            {
                object val = info.GetValue(parameters);
                if (val == null)
                {
                    info.SetValue(filledIn, info.GetValue(baseParameters));
                }
            }

            return (T) filledIn;
        }
    }
}

namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit {}
}