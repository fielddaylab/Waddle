using BeauRoutine;
using BeauUtil;
using BeauUtil.Debugger;
using FieldDay;
using FieldDay.Debugging;
using FieldDay.SharedState;
using FieldDay.Systems;
using UnityEngine;
using UnityEngine.UIElements;

namespace Waddle
{
    [SysUpdate(GameLoopPhase.LateFixedUpdate)]
    public class PlayerCollisionEscapeSystem : SharedStateSystemBehaviour<PlayerHeadState, PlayerMovementState>
    {
        public override void ProcessWork(float deltaTime) {
            if (!m_StateA.Connected) {
                return;
            }

            PenguinFeetSnapping snapping = m_StateA.GetComponent<PenguinFeetSnapping>();
            if (!PenguinFeetUtility.IsSolidGround(snapping, snapping.RootTransform.position)) {
                // TODO: alarm alarm the player cannot move
                Log.Warn("[PlayerCollisionEscapeSystem] cannot move from spot!");
            }
        }
    }
}