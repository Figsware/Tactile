using System;
using UnityEngine;

namespace Tactile.Gadgets
{
    public class SpringButton : MonoBehaviour
    {
        [SerializeField] private Rigidbody buttonBaseRigidBody;
        [SerializeField] private Rigidbody buttonRigidBody;
        [SerializeField] private float pushForce = 1f;
        [SerializeField] private float buttonRange = 1f;

        private void Start()
        {
            UpdateRigidBodies();
        }

        private void OnValidate()
        {
            UpdateRigidBodies();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.matrix = buttonBaseRigidBody.transform.localToWorldMatrix;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Vector3.zero, Vector3.up * buttonRange);
        }

        private void UpdateRigidBodies()
        {
            if (!(buttonRigidBody && buttonBaseRigidBody))
                return;

            var joint = buttonRigidBody.gameObject.GetOrAddComponent<ConfigurableJoint>();
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Limited;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Locked;
            var limit = joint.linearLimit;
            limit.limit = buttonRange * 0.5f;
            joint.linearLimit = limit;

            joint.connectedBody = buttonBaseRigidBody;
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = buttonRange * 0.5f * Vector3.up;
            joint.anchor = Vector3.zero;

            var constantForce = buttonRigidBody.gameObject.GetOrAddComponent<ConstantForce>();
            constantForce.force = pushForce * Vector3.up;
        }
    }
}