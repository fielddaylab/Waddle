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
        private const OVRInput.Controller DisableInputWhileControllers = OVRInput.Controller.LTouch | OVRInput.Controller.RTouch;

        public override void ProcessWork(float deltaTime) {
            if (m_StateA.WalkCooldown > 0) {
                m_StateA.WalkCooldown -= deltaTime;
                if (m_StateA.WalkCooldown <= 0) {
                    m_StateA.LastStepSide = PlayerFoot.Invalid;
                    m_StateA.ConsecutiveSteps = 0;
                }
            }

            if (!m_StateB.Connected || m_StateB.ReconnectDelay > 0) {
                return;
            }

            WaddleDetectionParams detect = m_StateA.CurrentDetectionParams();

            //OVRInput.Controller connectedControllers = OVRInput.GetActiveController();
            //Log.Msg("Connected controllers {0}", connectedControllers);
            //if ((connectedControllers & DisableInputWhileControllers) != 0) {
            //    return;
            //}

            Vector3 currentHeadOffset = m_StateB.CurrentHeadPos - m_StateB.HeadReference;
            Vector3 currentHeadLocalLook = m_StateB.BodyRoot.InverseTransformDirection(m_StateB.HeadLook);
            Vector3 currentHeadVelocity = PlayerHeadUtility.CalculateAverageVelocity(m_StateB, detect.VelocityAveragingFrames);
            Vector3 currentHeadVelocityNormalized = currentHeadVelocity.normalized;
            float currentHeadSpeed = currentHeadVelocity.magnitude;

            float desiredXOffset = detect.HorizontalSensitivity;
            if (m_StateA.ConsecutiveSteps >= detect.ConsecutiveStepEasingThreshold) {
                desiredXOffset *= detect.ConsecutiveStepEasingMultiplier;
            }

            //DebugDraw.AddViewportText(new Vector2(0, 0), new Vector2(8, 8),
            //    string.Format("offset {0} | velocity {1} | look {2}", currentHeadOffset, currentHeadVelocity, currentHeadLocalLook),
            //    Color.yellow, 0, TextAnchor.LowerLeft, DebugTextStyle.BackgroundDark);

            if (Vector3.Dot(m_StateB.HeadUp, Vector3.up) > 1 - detect.HeadTiltSensitivity) {
                return;
            }

            if (currentHeadSpeed >= detect.MinVelocitySensitivity) {
                if (m_StateA.LastStepSide != PlayerFoot.Left) {
                    if (currentHeadOffset.x <= -desiredXOffset && currentHeadLocalLook.z >= detect.LookSensitivity) {
                        if (Vector3.Dot(currentHeadVelocityNormalized, Vector3.left) > detect.VelocitySensitivity) {
                            PlayerMovementUtility.QueueMovement(m_StateA, m_StateB.HeadLook, PlayerFoot.Left, detect.WalkCooldown, PlayerMovementSource.Motion);
                            m_StateB.HeadReference = Vector2.Lerp(m_StateB.HeadReference, m_StateB.CurrentHeadPos, detect.HeadReferenceSnap);
                        }
                    }
                }

                if (m_StateA.LastStepSide != PlayerFoot.Right) {
                    if (currentHeadOffset.x >= desiredXOffset && currentHeadLocalLook.z >= detect.LookSensitivity) {
                        if (Vector3.Dot(currentHeadVelocityNormalized, Vector3.right) > detect.VelocitySensitivity) {
                            PlayerMovementUtility.QueueMovement(m_StateA, m_StateB.HeadLook, PlayerFoot.Right, detect.WalkCooldown, PlayerMovementSource.Motion);
                            m_StateB.HeadReference = Vector2.Lerp(m_StateB.HeadReference, m_StateB.CurrentHeadPos, detect.HeadReferenceSnap);
                        }
                    }
                }
            }
        }
    }
}