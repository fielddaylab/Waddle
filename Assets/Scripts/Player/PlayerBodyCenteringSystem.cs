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

            PenguinFeetSnapping snapping = m_StateA.GetComponent<PenguinFeetSnapping>();
            if (!CheckForSafeLocation || PenguinFeetUtility.IsSolidGround(snapping, newPos)) {
                m_StateA.PositionRoot.position = newPos;
                PlayerHeadUtility.ShiftHeadOrigin(m_StateA, shift);
            }
        }
    }
}