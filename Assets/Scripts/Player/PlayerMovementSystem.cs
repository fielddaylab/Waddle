using BeauUtil;
using FieldDay.Debugging;
using FieldDay.SharedState;
using FieldDay.Systems;
using UnityEngine;

namespace Waddle
{
    [SysUpdate(FieldDay.GameLoopPhase.Update, 1000)]
    public class PlayerMovementSystem : SharedStateSystemBehaviour<PlayerMovementState, PlayerHeadState>
    {
        #region Inspector

        [SerializeField] private float m_MoveSpeedMultiplier = 1f / 90f;
        [SerializeField] private float m_MoveCheckIterations = 8;

        #endregion // Inspector

        public override void ProcessWork(float deltaTime)
        {
            if (!m_StateA.Queued) {
                return;
            }

            Vector3 originalPos = m_StateB.PositionRoot.position;
            PlayerMoveResult result = TryMove(m_StateA, m_StateB);
            Vector3 finalPos = m_StateB.PositionRoot.position;

            if (result == PlayerMoveResult.Allowed) {
                PenguinAnalytics.Instance.LogMove(originalPos, finalPos, m_StateB.HeadRotation, m_StateA.FromRight ? 1 : 0);

                if (m_StateA.FootAudioSource) {
                    m_StateA.FootAudioSource.PlayOneShot(RNG.Instance.Choose(m_StateA.StepAudioClips));
                }

                m_StateA.ConsecutiveSteps++;
            } else {
                if (m_StateA.FootAudioSource) {
                    m_StateA.FootAudioSource.PlayOneShot(RNG.Instance.Choose(m_StateA.CollideAudioClips));
                }
            }

            DebugDraw.AddLine(originalPos, finalPos, Color.green, 1, 1, false);

            m_StateA.Queued = false;
        }

        private PlayerMoveResult TryMove(PlayerMovementState moveState, PlayerHeadState headState) {
            Vector3 moveDirection = moveState.MoveDirection;
            moveDirection.y = 0;

            float moveDist = moveState.MoveSpeed * m_MoveSpeedMultiplier;

            Vector3 headStart = headState.HeadRoot.position;
            Vector3 bodyStart = headState.PositionRoot.position;

            Ray bodyCast = new Ray(bodyStart, moveDirection);

            float low = 0;
            float high = moveDist;
            float checkDist = moveDist;

            // TODO: more efficient method, and allow lightly redirecting player displacement to slide against surfaces

            // yeah let's do a binary search for where the best movement position is, that seems like it'll work
            for(int i = 0; i < m_MoveCheckIterations && low < high; i++) {
                checkDist = (low + high) / 2;
                if (!PlayerMovementUtility.IsSolidGround(m_StateA, bodyCast.GetPoint(checkDist))) {
                    high = checkDist;
                } else {
                    low = checkDist;
                }
            }

            if (checkDist <= moveDist * m_StateA.MinMovePercentage) {
                return PlayerMoveResult.Blocked_Solid;
            }

            Vector3 targetPos = bodyCast.GetPoint(checkDist);
            headState.PositionRoot.position = targetPos;
            return PlayerMoveResult.Allowed;
        }
    }

    public enum PlayerMoveResult {
        Allowed,
        Blocked_Solid,
        Blocked_Invisible
    }
}