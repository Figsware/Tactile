using System;
using Tactile.Utility;
using UnityEngine;

namespace Tactile.UI.Menu
{
    [RequireComponent(typeof(RectTransform))]
    public class CircularBounds : DraggableBounds
    {
        [SerializeField, Range(0, 1)] private float innerRadius = 0f;
        [SerializeField] private float minAngle;
        [SerializeField] private float maxAngle;

        private RectTransform rt;

        private void Awake()
        {
            rt = GetComponent<RectTransform>();
        }

        public override Vector2 GetInitialPosition(RectTransform draggableRt)
        {
            var initialPosition = rt.GetScreenSpacePosition();
            var rect = rt.rect;
            var radius = Mathf.Min(rect.width, rect.height) / 2f;
            initialPosition += Vector2.right.Rotate(minAngle) * innerRadius * radius;

            return initialPosition;
        }

        public override Vector2 ConstrainPosition(Vector2 position, RectTransform draggableRt)
        {
            var rect = rt.rect;
            var radius = Mathf.Min(rect.width, rect.height) / 2f;
            var center = rt.GetScreenSpacePosition();

            // Constrain the offset to the min/max positions.
            var offset = position - center;
            var offsetDistance = offset.magnitude;
            if (offsetDistance > radius)
            {
                offset = offset.normalized * radius;
            }
            else if (offsetDistance < innerRadius * radius)
            {
                offset = offset.normalized * innerRadius * radius;
            }

            var angle = TactileUtility.Mod(Vector2.right.SignedAngleTo(offset.normalized) * Mathf.Rad2Deg, 360f);

            if (angle < minAngle)
            {
                offset = offset.Rotate((minAngle - angle) * Mathf.Deg2Rad);
            }
            else if (angle > maxAngle)
            {
                offset = offset.Rotate((maxAngle - angle) * Mathf.Deg2Rad);
            }

            var newPosition = center + offset;

            return newPosition;
        }
    }
}