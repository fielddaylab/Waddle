using BeauUtil.Debugger;
using FieldDay.Debugging;
using FieldDay.SharedState;
using FieldDay.Systems;
using UnityEngine;

namespace Waddle
{
    [SysUpdate(FieldDay.GameLoopPhase.Update, 100)]
    public class PlayerMovementDetectorSystem : SharedStateSystemBehaviour<PlayerMovementState, PlayerHeadState>
    {
        #region Inspector

        [Header("Sensitivities")]
        [SerializeField] private float m_HorizontalSensitivity = 0.6f;
        [SerializeField] private float m_LookSensitivity = 0.2f;
        [SerializeField] private float m_VelocitySensitivity = 0.6f;
        [SerializeField] private float m_MinVelocitySensitivity = 0.4f;
        [SerializeField] private float m_HeadTiltSensitivity = 0.2f;
        [SerializeField] private int m_VelocityAveragingFrames = 16;

        [Header("Horizontal Threshold Easing")]
        [SerializeField] private int m_ConsecutiveStepEasingThreshold = 2;
        [SerializeField] private float m_ConsecutiveStepEasingMultiplier = 0.8f;

        [Header("Step Reaction")]
        [SerializeField] private float m_WalkCooldown = 0.5f;
        [SerializeField] private float m_HeadReferenceSnap = 0.5f;

        #endregion // Inspector

        public override void ProcessWork(float deltaTime) {
            if (m_StateA.WalkCooldown > 0) {
                m_StateA.WalkCooldown -= deltaTime;
                if (m_StateA.WalkCooldown <= 0) {
                    m_StateA.LastStepSide = PlayerFoot.Invalid;
                    m_StateA.ConsecutiveSteps = 0;
                }
            }

            if (!m_StateB.Connected) {
                return;
            }

            Vector3 currentHeadOffset = m_StateB.CurrentHeadPos - m_StateB.HeadReference;
            Vector3 currentHeadLocalLook = m_StateB.BodyRoot.InverseTransformDirection(m_StateB.HeadLook);
            Vector3 currentHeadVelocity = PlayerHeadUtility.CalculateAverageVelocity(m_StateB, m_VelocityAveragingFrames);
            Vector3 currentHeadVelocityNormalized = currentHeadVelocity.normalized;
            float currentHeadSpeed = currentHeadVelocity.magnitude;

            float desiredXOffset = m_HorizontalSensitivity;
            if (m_StateA.ConsecutiveSteps >= m_ConsecutiveStepEasingThreshold) {
                desiredXOffset *= m_ConsecutiveStepEasingMultiplier;
            }

            DebugDraw.AddViewportText(new Vector2(0, 0), new Vector2(8, -8),
                string.Format("offset {0} | velocity {1} | look {2}", currentHeadOffset, currentHeadVelocity, currentHeadLocalLook),
                Color.yellow, 0, TextAnchor.LowerLeft, DebugTextStyle.BackgroundDark);

            if (Vector3.Dot(m_StateB.HeadUp, Vector3.up) > 1 - m_HeadTiltSensitivity) {
                return;
            }

            if (currentHeadSpeed >= m_MinVelocitySensitivity) {
                if (m_StateA.LastStepSide != PlayerFoot.Left) {
                    if (currentHeadOffset.x <= -desiredXOffset && currentHeadLocalLook.z >= m_LookSensitivity) {
                        if (Vector3.Dot(currentHeadVelocityNormalized, Vector3.left) > m_VelocitySensitivity) {
                            PlayerMovementUtility.QueueMovement(m_StateA, m_StateB.HeadLook, PlayerFoot.Left, m_WalkCooldown);
                            m_StateB.HeadReference = Vector2.Lerp(m_StateB.HeadReference, m_StateB.CurrentHeadPos, m_HeadReferenceSnap);
                        }
                    }
                }

                if (m_StateA.LastStepSide != PlayerFoot.Right) {
                    if (currentHeadOffset.x >= desiredXOffset && currentHeadLocalLook.z >= m_LookSensitivity) {
                        if (Vector3.Dot(currentHeadVelocityNormalized, Vector3.right) > m_VelocitySensitivity) {
                            PlayerMovementUtility.QueueMovement(m_StateA, m_StateB.HeadLook, PlayerFoot.Right, m_WalkCooldown);
                            m_StateB.HeadReference = Vector2.Lerp(m_StateB.HeadReference, m_StateB.CurrentHeadPos, m_HeadReferenceSnap);
                        }
                    }
                }
            }
        }
    }
}