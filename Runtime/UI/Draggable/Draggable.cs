using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Tactile.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class Draggable : MonoBehaviour, IDragHandler
    {
        public UnityEvent<Vector2> OnNewPosition;

        [SerializeField] private DraggableBounds bounds;
        private RectTransform rt;
        private Vector2 _currentPosition;

        public Vector2 CurrentPosition
        {
            get => _currentPosition;
            private set
            {
                _currentPosition = value;
                OnNewPosition.Invoke(_currentPosition);
            }
        }

        private void Awake()
        {
            rt = GetComponent<RectTransform>();
        }

        private void Start()
        {
            if (bounds)
                rt.position = bounds.GetInitialPosition(rt);

            CurrentPosition = rt.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rt.position = bounds ? bounds.ConstrainPosition(eventData.position, rt) : eventData.position;
            CurrentPosition = rt.position;
        }
    }
}