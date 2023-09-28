using System;
using BeauUtil;
using BeauUtil.Debugger;
using FieldDay;
using FieldDay.SharedState;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Waddle
{
    public class PlayerHeadState : SharedStateComponent, IRegistrationCallbacks
    {
        public Transform PositionRoot;
        public Transform BodyRoot;
        public Transform HeadRoot;
        public Transform FootRoot;
        public Transform CenterOfMassRoot;
        public OVRCameraRig Rig;
        public Animator BodyAnimator;
        public int VelocityAveragingFrames = 32;

        [NonSerialized] public bool Connected;
        [NonSerialized] public float ReconnectDelay;
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

        [NonSerialized] public bool IsShiftingBody;

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

        static public Vector3 CalculateHeadBodyVector(PlayerHeadState state) {
            Vector3 vec = state.HeadRoot.position - state.BodyRoot.position;
            vec.y = 0;
            return vec;
        }

        static public void ResetHeadToBody(PlayerHeadState state) {
            Vector3 diff = CalculateHeadBodyVector(state);
            ShiftHeadOrigin(state, diff);
        }

        static public void ShiftHeadOrigin(PlayerHeadState state, Vector3 worldShift) {
            Transform trackingSpace = state.Rig.trackingSpace;
            Vector3 localShift = trackingSpace.InverseTransformVector(worldShift);
            state.Rig.TrackingPositionOffset -= localShift;
            trackingSpace.localPosition -= localShift;
            Log.Msg("[PlayerHeadUtility] Shifted tracking offset by {0}", localShift);
        }
    }
}