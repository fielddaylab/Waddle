using BeauUtil.Debugger;
using FieldDay.Debugging;
using FieldDay.SharedState;
using FieldDay.Systems;
using UnityEngine;

namespace Waddle
{
    [SysUpdate(FieldDay.GameLoopPhase.Update, 100)]
    public class PlayerMovementDebugSystem : SharedStateSystemBehaviour<PlayerMovementState, PlayerHeadState>
    {
        #region Inspector

        #endregion // Inspector

        public override void ProcessWork(float deltaTime) {
            CheckDebugKeys(deltaTime);
            CheckOVRKeys();
        }

        private void CheckDebugKeys(float deltaTime) {
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.LeftArrow)) {
                m_StateB.BodyRoot.Rotate(0, -90 * deltaTime, 0);
            }
            if (Input.GetKey(KeyCode.RightArrow)) {
                m_StateB.BodyRoot.Rotate(0, 90 * deltaTime, 0);
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R)) {
                PenguinGameManager.Instance.RestartGame();
            }

            if (m_StateA.WalkCooldown <= 0) {
                if (Input.GetKey(KeyCode.UpArrow)) {
                    PlayerMovementUtility.QueueMovement(m_StateA, m_StateB.BodyRoot.forward, PlayerFoot.Invalid, 0.1f, PlayerMovementSource.Button);
                } else if (Input.GetKey(KeyCode.DownArrow)) {
                    PlayerMovementUtility.QueueMovement(m_StateA, -m_StateB.BodyRoot.forward, PlayerFoot.Invalid, 0.1f, PlayerMovementSource.Button);
                }
            }
#endif // UNITY_EDITOR
        }

        private void CheckOVRKeys() {
            if (m_StateA.WalkCooldown <= 0) {
                if (OVRInput.GetDown(OVRInput.Button.One | OVRInput.Button.Two | OVRInput.Button.Three | OVRInput.Button.Four | OVRInput.Button.SecondaryThumbstickUp | OVRInput.Button.PrimaryThumbstickUp)) {
                    PlayerMovementUtility.QueueMovement(m_StateA, m_StateB.BodyRoot.forward, PlayerFoot.Invalid, 0.5f, PlayerMovementSource.Button);
                }
            }
        }
    }
}