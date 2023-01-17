using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tactile.Utility
{
    /// <summary>
    /// An arrow points to objects within the game world. It can oscillate back and forth to grab the user's attention.
    /// The target position and approach rotation are defined relative to the Transform's parent. The arrow will oscillate
    /// in the direction of this transform's forward vector.
    /// </summary>
    public class Arrow : MonoBehaviour
    {
        [Tooltip("The transform to manipulate. If unspecified, this GameObject's transform is used instead.")]
        [SerializeField] private Transform arrowTransform;
        
        [Header("Target")]
        [Tooltip("The position relative to the parent transform to point at.")]
        [SerializeField] private Vector3 targetPosition;

        [Tooltip("The angle at which to approach the target.")]
        [SerializeField] private Quaternion approachRotation;

        [Tooltip("The distance to travel back and forth.")]
        [SerializeField] private float travelDistance = 1f;

        [Tooltip("The minimum distance between the target and the arrow.")]
        [SerializeField] private float targetGap;
        
        [Header("Motion")]
        [Tooltip("The curve that the arrow will follow.")]
        public AnimationCurve motionCurve = CreateDefaultCurve();
        
        [Tooltip("The rate at which to traverse the curve.")]
        public float curveRate = 1f;

        /// <summary>
        /// The position relative to the parent transform to point at.
        /// </summary>
        public Vector3 TargetPosition
        {
            get => targetPosition;
            set
            {
                targetPosition = value;
                CalculateStartAndEndPositions();
            }
        }
        
        /// <summary>
        /// The angle at which to approach the target.
        /// </summary>
        public Quaternion ApproachRotation
        {
            get => approachRotation;
            set
            {
                approachRotation = value;
                CalculateStartAndEndPositions();
            }
        }
        
        /// <summary>
        /// The distance to travel back and forth.
        /// </summary>
        public float TravelDistance
        {
            get => travelDistance;
            set
            {
                travelDistance = value;
                CalculateStartAndEndPositions();
            }
        }
        
        /// <summary>
        /// The minimum distance between the target and arrow.
        /// </summary>
        public float TargetGap
        {
            get => targetGap;
            set
            {
                targetGap = value;
                CalculateStartAndEndPositions();
            }
        }

        public Transform ArrowTransform
        {
            get => arrowTransform ? arrowTransform : transform;
            set => arrowTransform = value;
        }

        private Vector3 _startPos;
        private Vector3 _endPos;

        private void Start()
        {
            StartCoroutine(MoveArrowCoroutine());
        }

        /// <summary>
        /// Given the current approach rotation and target position, this calculates the 
        /// </summary>
        private void CalculateStartAndEndPositions()
        {
            Vector3 arrowVector = approachRotation * Vector3.forward;
            _startPos = targetPosition - (targetGap + travelDistance) * arrowVector;
            _endPos = targetPosition - (targetGap) * arrowVector;
        }

        /// <summary>
        /// Moves the arrow back and forth according to the motion curve, target position, and local rotation.
        /// </summary>
        /// <returns></returns>
        private IEnumerator MoveArrowCoroutine()
        {
            while (true)
            {
                Transform currentArrowTransform = ArrowTransform;
                float t = motionCurve.Evaluate(Time.time * curveRate);
                Vector3 newPos = Vector3.Lerp(_startPos, _endPos, t);
                currentArrowTransform.localPosition = newPos;
                currentArrowTransform.localRotation = approachRotation;

                yield return null;
            }
        }

        private void OnValidate()
        {
            CalculateStartAndEndPositions();
        }

        private void OnDrawGizmosSelected()
        {
            if (ArrowTransform.parent)
            {
                Gizmos.matrix = ArrowTransform.parent.localToWorldMatrix;
            }

            float sphereSize = 0.05f;
            float halfSphereSize = sphereSize / 2f;

            // Draw target
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(targetPosition, sphereSize);

            // Draw path start/end
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_startPos, halfSphereSize);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_endPos, halfSphereSize);

            // Draw path
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_startPos, _endPos);
        }

        /// <summary>
        /// Creates the default animation curve for an arrow, which is to oscillate back and forth with sinusoidal
        /// motion.
        /// </summary>
        /// <returns></returns>
        private static AnimationCurve CreateDefaultCurve()
        {
            AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
            curve.postWrapMode = WrapMode.PingPong;

            return curve;
        }
    }
}