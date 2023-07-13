using BeauRoutine;
using FieldDay;
using FieldDay.SharedState;
using FieldDay.Systems;
using UnityEngine;

namespace Waddle
{
    [SysUpdate(GameLoopPhase.Update, -100)]
    public class PlayerHeadSystem : SharedStateSystemBehaviour<PlayerHeadState>
    {
        #region Inspector

        [Header("Config")]
        [SerializeField] private float m_OffsetLerp;

        #endregion // Inspector

        public override void ProcessWork(float deltaTime) {
            Vector3 newHeadPos = m_State.HeadRoot.localPosition;
            Vector3 delta = newHeadPos - m_State.CurrentHeadPos;
            m_State.CurrentHeadPos = newHeadPos;

            Vector3 vel = delta / deltaTime;
            m_State.CurrentHeadVelocity = vel;

            m_State.PositionBuffer.PushFront(newHeadPos);
            m_State.VelocityBuffer.PushFront(vel);

            m_State.HeadReference = Vector3.Lerp(m_State.HeadReference, m_State.CurrentHeadPos, TweenUtil.Lerp(m_OffsetLerp));
        }
    }
}