using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Tactile
{
    [Serializable]
    public struct Orientation
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public static readonly Orientation Origin = new Orientation(Vector3.zero, Quaternion.identity, Vector3.one);
        
        public Orientation(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        public static Orientation FromTransform(Transform t, bool useLocalOrientation)
        {
            return useLocalOrientation ? LocalFromTransform(t) : WorldFromTransform(t);
        }

        public static Orientation LocalFromTransform(Transform t)
        {
            return new Orientation(t.localPosition, t.localRotation, t.localScale);
        }
        
        public static Orientation WorldFromTransform(Transform t)
        {
            return new Orientation(t.position, t.rotation, t.localScale);
        }

        public void ApplyToTransformLocally(Transform t)
        {
            t.localPosition = position;
            t.localRotation = rotation;
            t.localScale = scale;
        }

        public void ApplyToTransformInWorldSpace(Transform t)
        {
            t.position = position;
            t.rotation = rotation;
            t.localScale = scale;
        }

        public void ApplyToTransform(Transform t, bool useLocalSpace)
        {
            if (useLocalSpace)
            {
                ApplyToTransformLocally(t);
            }
            else
            {
                ApplyToTransformInWorldSpace(t);
            }
        }

        public Orientation SetPosition(Func<Vector3, Vector3> changePositionFunc)
        {
            position = changePositionFunc(position);
            return this;
        }

        public Orientation SetPosition(Vector3 pos)
        {
            position = pos;
            return this;
        }
        
        public Orientation SetScale(Func<Vector3, Vector3> changeScaleFunc)
        {
            scale = changeScaleFunc(scale);
            return this;
        }

        public Orientation SetScale(Vector3 scale)
        {
            this.scale = scale;
            return this;
        }
        
        public Orientation SetRotation(Func<Quaternion, Quaternion> changeRotationFunc)
        {
            rotation = changeRotationFunc(rotation);
            return this;
        }

        public Orientation SetRotation(Quaternion rot)
        {
            rotation = rot;
            return this;
        }
        
        public static IEnumerator LerpOrientationCoroutineWithArc(Orientation from, Orientation to, Vector3 arcAxis, float arcHeight, IEnumerator<float> lerpEnumerator, Action<Orientation> orient)
        {
            yield return LerpOrientationCoroutine(from, to, lerpEnumerator, (orientation, t) =>
            {
                orientation.position += arcAxis * (Mathf.Sin(Mathf.PI * t) * arcHeight);
                orient(orientation);
            });
        }
        
        public static IEnumerator LerpOrientationCoroutine(Orientation from, Orientation to, IEnumerator<float> lerpEnumerator, Action<Orientation> orient)
        {
            yield return LerpOrientationCoroutine(from, to, lerpEnumerator, (orientation, _) => orient(orientation));
        }

        public static IEnumerator LerpOrientationCoroutine(Orientation from, Orientation to, IEnumerator<float> lerpEnumerator, Action<Orientation, float> orient)
        {
            yield return TactileUtility.UseLerpEnumeratorCoroutine(lerpEnumerator, t =>
            {
                Orientation lerp = new Orientation()
                {
                    position = Vector3.Lerp(from.position, to.position, t),
                    rotation = Quaternion.Lerp(from.rotation, to.rotation, t),
                    scale = Vector3.Lerp(from.scale, to.scale, t)
                };

                orient(lerp, t);
            });
        }
        
        public static IEnumerator SlerpOrientationCoroutine(Orientation from, Orientation to, IEnumerator<float> lerpEnumerator, Action<Orientation> orient)
        {
            yield return TactileUtility.UseLerpEnumeratorCoroutine(lerpEnumerator, t =>
            {
                Orientation lerp = new Orientation()
                {
                    position = Vector3.Slerp(from.position, to.position, t),
                    rotation = Quaternion.Slerp(from.rotation, to.rotation, t),
                    scale = Vector3.Slerp(from.scale, to.scale, t)
                };

                orient(lerp);
            });
        }

        public static implicit operator Orientation((Vector3, Quaternion, Vector3) tuple)
        {
            return new Orientation(tuple.Item1, tuple.Item2, tuple.Item3);
        }
    }
}