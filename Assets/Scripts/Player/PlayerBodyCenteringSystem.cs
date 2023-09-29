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
    [SysUpdate(GameLoopPhase.LateUpdate, 10000)]
    public class PlayerBodyCenteringSystem : SharedStateSystemBehaviour<PlayerHeadState, PlayerMovementState>
    {
        public bool CheckForSafeLocation = true;

        public override void ProcessWork(float deltaTime) {
            if (!m_StateA.Connected) {
                return;
            }

            Vector3 headOffsetFromBody = PlayerHeadUtility.CalculateHeadBodyVector(m_StateA);

            //if (!m_StateA.IsShiftingBody) {
            //    if (absDistance < DistanceStartThreshold) {
            //        return;
            //    }

            //    StartShifting();
            //}

            //float dist = absDistance - DistanceStopThreshold;
            //if (dist <= 0) {
            //    StopShifting();
            //    return;
            //}

            Vector3 shift = headOffsetFromBody;
            Vector3 newPos = m_StateA.PositionRoot.position + shift;

            bool isSafe = PlayerMovementUtility.IsSolidGround(m_StateB, newPos, out Vector3 safeNormal);
            m_StateB.IsOnSafeGround = isSafe;

            if (!CheckForSafeLocation || isSafe) {
                m_StateA.PositionRoot.position = newPos;
                PlayerHeadUtility.ShiftHeadOrigin(m_StateA, shift);
            }

            if (isSafe) {
                m_StateB.SafeLocationBuffer.PushBack(new SafeLocationRecord() {
                    Location = newPos,
                    Normal = safeNormal.y
                });
                if (safeNormal.y > 0.7f) {
                    m_StateB.HighQualitySafeLocationBuffer.PushBack(new SafeLocationRecord() {
                        Location = newPos,
                        Normal = safeNormal.y
                    });
                }
                m_StateB.LastSafeTime = Time.unscaledTime;
            }
        }
    }
}