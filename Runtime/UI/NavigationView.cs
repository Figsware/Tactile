using System;
using System.Collections;
using System.Collections.Generic;

namespace Tactile.UI
{
    public class NavigationView : View, INavigationController, NavigationView.IViewTransitionHandler
    {
        public View CurrentView => views.Count > 0 ? views[^1] : null;

        public string Title
        {
            get => title;
            set => title = value;
        }

        public int ItemCount => views.Count;
        public void GoBack()
        {
            View outgoing = CurrentView;
            views.RemoveAt(views.Count - 1);
            View returning = CurrentView;
            StartCoroutine(transitionHandler.RemoveViewCoroutine(outgoing, returning));
        }

        public void GoToInitialView()
        {
            throw new NotImplementedException();
        }

        protected List<View> views = new List<View>();
        protected string title;
        public IViewTransitionHandler transitionHandler;

        protected virtual void Awake()
        {
            transitionHandler = this;
        }

        public void Present(View view)
        {
            View prev = CurrentView;
            views.Add(view);
            View newView = Instantiate(view);
            StartCoroutine(transitionHandler.PresentViewCoroutine(prev, newView));
        }

        public interface IViewTransitionHandler
        {
            public IEnumerator PresentViewCoroutine(View previousView, View newView);
            public IEnumerator RemoveViewCoroutine(View outgoingView, View returningView);
        }

        public IEnumerator PresentViewCoroutine(View previousView, View newView)
        {
            previousView.gameObject.SetActive(false);
            newView.gameObject.SetActive(true);
            yield return null;
        }

        public IEnumerator RemoveViewCoroutine(View outgoingView, View returningView)
        {
            Destroy(outgoingView.gameObject);
            returningView.gameObject.SetActive(true);
            yield return null;
        }
    }
}