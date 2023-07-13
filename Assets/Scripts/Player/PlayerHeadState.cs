using System;
using BeauUtil;
using FieldDay;
using FieldDay.SharedState;
using UnityEngine;

namespace Waddle
{
    public class PlayerHeadState : SharedStateComponent, IRegistrationCallbacks
    {
        public Transform BodyRoot;
        public Transform HeadRoot;
        public Transform FootRoot;
        public OVRCameraRig Rig;

        [NonSerialized] public Vector3 RootPos;
        [NonSerialized] public Quaternion RootRotation;

        [NonSerialized] public Vector3 HeadReference;
        [NonSerialized] public Quaternion HeadRotation;
        [NonSerialized] public Vector3 HeadLook;
        [NonSerialized] public Vector3 HeadUp;

        [NonSerialized] public RingBuffer<Vector3> VelocityBuffer;
        [NonSerialized] public RingBuffer<Vector3> PositionBuffer;

        [NonSerialized] public Vector3 CurrentHeadPos;
        [NonSerialized] public Vector3 CurrentHeadVelocity;

        void IRegistrationCallbacks.OnDeregister() {
        }

        void IRegistrationCallbacks.OnRegister() {
            VelocityBuffer = new RingBuffer<Vector3>(32, RingBufferMode.Overwrite);
            PositionBuffer = new RingBuffer<Vector3>(32, RingBufferMode.Overwrite);
        }
    }
}