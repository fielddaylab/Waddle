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
    [SysUpdate(GameLoopPhase.FixedUpdate, 10000)]
    public class PlayerBodyCenteringSystem : SharedStateSystemBehaviour<PlayerHeadState, PlayerMovementState>
    {
        #region Inspector

        [Header("Config")]
        public float DistanceStartThreshold = 0.3f;
        public float DistanceStopThreshold = 0.3f;
        public float MaxMovePerSecond = 1;
        public float TimeSinceLastStepThreshold = 1;

        #endregion // Inspector

        public override void ProcessWork(float deltaTime) {
            if (!m_StateA.Connected) {
                return;
            }

            if (!PenguinGameManager._headMovementActive) {
                StopShifting();
            }

            double timeSinceLastStep = Time.unscaledTimeAsDouble - m_StateB.LastStepTime;
            if (timeSinceLastStep < TimeSinceLastStepThreshold) {
                return;
            }

            Vector3 headOffsetFromBody = m_StateA.HeadRoot.position - m_StateA.PositionRoot.position;
            headOffsetFromBody.y = 0;

            float absDistance = headOffsetFromBody.magnitude;

            if (!m_StateA.IsShiftingBody) {
                if (absDistance < DistanceStartThreshold) {
                    return;
                }

                StartShifting();
            }

            float dist = absDistance - DistanceStopThreshold;
            if (dist <= 0) {
                StopShifting();
                return;
            }

            PenguinFeetSnapping snapping = m_StateA.GetComponent<PenguinFeetSnapping>();

            float shiftDist = Mathf.Min(MaxMovePerSecond * deltaTime, dist);

            Vector3 shift = headOffsetFromBody.normalized * shiftDist;
            Vector3 newPos = m_StateA.PositionRoot.position + shift;
            if (PenguinFeetUtility.IsSolidGround(snapping, newPos)) {
                m_StateA.PositionRoot.position = newPos;

                PlayerHeadUtility.ShiftHeadOrigin(m_StateA, shift);
                
                if (dist <= shiftDist) {
                    StopShifting();
                }
            }
        }

        private void StartShifting() {
            if (!m_StateA.IsShiftingBody) {
                m_StateA.IsShiftingBody = true;
                // TODO: Cancel movement
            }
        }

        private void StopShifting() {
            if (m_StateA.IsShiftingBody) {
                m_StateA.IsShiftingBody = false;
                // TODO: Cancel animation
            }
        }
    }
}