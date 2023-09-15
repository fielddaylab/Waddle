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
            if (m_StateA.WalkCooldown <= 0) {
                if (Input.GetKey(KeyCode.UpArrow) || OVRInput.Get(OVRInput.Button.Up)) {
                    PlayerMovementUtility.QueueMovement(m_StateA, m_StateB.BodyRoot.forward, PlayerFoot.Invalid, 0.2f);
                } else if (Input.GetKey(KeyCode.DownArrow)) {
                        PlayerMovementUtility.QueueMovement(m_StateA, -m_StateB.BodyRoot.forward, PlayerFoot.Invalid, 0.2f);
                    }
            }

            if (Input.GetKey(KeyCode.LeftArrow)) {
                m_StateB.BodyRoot.Rotate(0, -90 * deltaTime, 0);
            }
            if (Input.GetKey(KeyCode.RightArrow)) {
                m_StateB.BodyRoot.Rotate(0, 90 * deltaTime, 0);
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R)) {
                PenguinGameManager.Instance.RestartGame();
            }
        }
    }
}