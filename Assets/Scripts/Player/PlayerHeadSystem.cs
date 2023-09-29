using BeauRoutine;
using BeauUtil;
using FieldDay;
using FieldDay.Debugging;
using FieldDay.SharedState;
using FieldDay.Systems;
using UnityEngine;
using UnityEngine.UIElements;

namespace Waddle
{
    [SysUpdate(GameLoopPhase.Update, -100)]
    public class PlayerHeadSystem : SharedStateSystemBehaviour<PlayerHeadState>
    {
        #region Inspector

        [Header("Config")]
        [SerializeField] private float m_OffsetLerp;
        [SerializeField] private float m_ReconnectMovementDelay;

        #endregion // Inspector

        public override void ProcessWork(float deltaTime) {
            bool connected = !PenguinGameManager._isGamePaused;
            if (m_State.Connected != connected) {
                m_State.Connected = connected;
                m_State.ReconnectDelay = m_ReconnectMovementDelay;
            }

            if (!connected) {
                return;
            }

            if (m_State.ReconnectDelay > 0) {
                m_State.ReconnectDelay -= deltaTime;
            }

            bool firstFrame = m_State.VelocityBuffer.Count == 0;

            Quaternion rotate = Quaternion.Inverse(m_State.BodyRoot.localRotation);

            Vector3 newHeadPos = rotate * (m_State.HeadRoot.localPosition + m_State.Rig.TrackingOrigin);
            Vector3 delta;
            if (firstFrame) {
                delta = default;
            } else {
                delta = newHeadPos - m_State.CurrentHeadPos;
            }
            m_State.CurrentHeadPos = newHeadPos;

            Vector3 vel = delta / deltaTime;
            if (float.IsFinite(vel.x) && float.IsFinite(vel.y) && float.IsFinite(vel.z)) {
                m_State.CurrentHeadVelocity = vel;

                m_State.PositionBuffer.PushFront(newHeadPos);
                m_State.VelocityBuffer.PushFront(vel);

                if (firstFrame) {
                    m_State.HeadReference = m_State.CurrentHeadPos;
                    m_State.HeadRotationReference = m_State.HeadRotation;
                } else {
                    float lerp = TweenUtil.Lerp(m_OffsetLerp, 1, deltaTime);
                    m_State.HeadReference = Vector3.Lerp(m_State.HeadReference, m_State.CurrentHeadPos, lerp);
                    m_State.HeadRotationReference = Quaternion.Slerp(m_State.HeadRotationReference, m_State.HeadRotation, lerp);
                }
            }

            m_State.RootPos = m_State.BodyRoot.position;
            m_State.RootRotation = m_State.BodyRoot.rotation;

            m_State.HeadRotation = m_State.HeadRoot.rotation;
            m_State.HeadLook = m_State.HeadRoot.forward;
            m_State.HeadUp = m_State.HeadRoot.up;
        }
    }
}