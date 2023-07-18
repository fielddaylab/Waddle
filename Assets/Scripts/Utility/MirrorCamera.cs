using System;
using BeauUtil;
using FieldDay;
using UnityEngine;

namespace Waddle {
    [RequireComponent(typeof(Camera))]
    public class MirrorCamera : MonoBehaviour {
        public Transform ReflectPlane;
        [NonSerialized] private Camera m_Camera;
        [NonSerialized] private Transform m_Transform;

        private void Start() {
            this.CacheComponent(ref m_Camera);
            this.CacheComponent(ref m_Transform);
        }

        private void LateUpdate() {
            PlayerHeadState headState = Game.SharedState.Get<PlayerHeadState>();
            if (headState == null) {
                return;
            }

            Plane plane = new Plane(ReflectPlane.forward, ReflectPlane.position);

            Transform eyeCenterTransform = headState.Rig.centerEyeAnchor;
            Vector3 eyeCenterPos = eyeCenterTransform.position;
            if (plane.GetDistanceToPoint(eyeCenterPos) < -0.1f) {
                m_Camera.enabled = false;
                return;
            }

            Vector3 eyeVec = Vector3.Normalize(ReflectPlane.position - eyeCenterPos);

            m_Camera.enabled = true;

            Vector3 eyePosInLocalSpace = ReflectPlane.InverseTransformPoint(eyeCenterPos);
            eyePosInLocalSpace = -eyePosInLocalSpace;

            Vector3 eyeForwardInLocalSpace = ReflectPlane.InverseTransformDirection(eyeVec);
            eyeForwardInLocalSpace = Vector3.Reflect(eyeForwardInLocalSpace, plane.normal);
            
            m_Transform.position = ReflectPlane.TransformPoint(eyePosInLocalSpace);
            m_Transform.forward = ReflectPlane.TransformDirection(eyeForwardInLocalSpace);

            float clipPlane;
            plane.Raycast(new Ray(m_Transform.position, m_Transform.forward), out clipPlane);

            Vector3 reflectPlaneNormalInLocalSpace = m_Transform.InverseTransformDirection(plane.normal);

            m_Camera.projectionMatrix = m_Camera.CalculateObliqueMatrix(new Vector4(reflectPlaneNormalInLocalSpace.x, reflectPlaneNormalInLocalSpace.y, reflectPlaneNormalInLocalSpace.z, clipPlane));
        }
    }
}