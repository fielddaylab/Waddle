using System;
using BeauUtil;
using FieldDay;
using FieldDay.SharedState;
using UnityEngine;

namespace Waddle
{
    public class PlayerHeadState : SharedStateComponent, IRegistrationCallbacks
    {
        public Transform PositionRoot;
        public Transform BodyRoot;
        public Transform HeadRoot;
        public Transform FootRoot;
        public OVRCameraRig Rig;
        public int VelocityAveragingFrames = 32;

        [NonSerialized] public bool Connected;
        [NonSerialized] public Vector3 RootPos;
        [NonSerialized] public Quaternion RootRotation;

        [NonSerialized] public Vector3 HeadReference;
        [NonSerialized] public Quaternion HeadRotation;
        [NonSerialized] public Quaternion HeadRotationReference;
        [NonSerialized] public Vector3 HeadLook;
        [NonSerialized] public Vector3 HeadUp;

        [NonSerialized] public RingBuffer<Vector3> VelocityBuffer;
        [NonSerialized] public RingBuffer<Vector3> PositionBuffer;

        [NonSerialized] public Vector3 CurrentHeadPos;
        [NonSerialized] public Vector3 CurrentHeadVelocity;

        void IRegistrationCallbacks.OnDeregister() {
        }

        void IRegistrationCallbacks.OnRegister() {
            VelocityBuffer = new RingBuffer<Vector3>(VelocityAveragingFrames, RingBufferMode.Overwrite);
            PositionBuffer = new RingBuffer<Vector3>(VelocityAveragingFrames, RingBufferMode.Overwrite);
        }
    }

    static public class PlayerHeadUtility {
        static public Vector3 CalculateAverageVelocity(PlayerHeadState state, int maxFrames) {
            int historyCount = Math.Min(state.VelocityBuffer.Count, maxFrames);
            Vector3 accum = default;
            if (historyCount > 0) {
                for (int i = 0; i < historyCount; i++) {
                    accum += state.VelocityBuffer[i];
                }
                accum /= historyCount;
            }
            return accum;
        }
    }
}