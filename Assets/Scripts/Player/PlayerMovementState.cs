using System;
using BeauUtil;
using FieldDay.Debugging;
using FieldDay.SharedState;
using UnityEngine;

namespace Waddle
{
    public class PlayerMovementState : SharedStateComponent
    {
        public float MoveSpeed = 20;
        public LayerMask TerrainMask;
        public LayerMask InvisibleColliderMask;
        [Range(0, 1)] public float TerrainAngleStrictness = 0.5f;
        public float ColliderCheckRadius;
        [Range(0, 1)] public float MinMovePercentage = 0.5f;

        [NonSerialized] public bool Queued;
        [NonSerialized] public bool FromRight;
        [NonSerialized] public Vector3 MoveDirection;

        [NonSerialized] public float WalkCooldown;
        [NonSerialized] public PlayerFoot LastStepSide;
        [NonSerialized] public int ConsecutiveSteps;
    }

    static public class PlayerMovementUtility {
        static public void QueueMovement(PlayerMovementState state, Vector3 moveIn, PlayerFoot foot, float cooldown) {
            state.Queued = true;
            state.FromRight = (foot == PlayerFoot.Right);
            state.MoveDirection = moveIn;
            state.WalkCooldown = cooldown;
            state.LastStepSide = foot;
        }

        static public bool IsSolidGround(PlayerMovementState snapping, Vector3 newPos) {
            return IsSolidGround(snapping, newPos, out Vector3 _);
        }

        static public bool IsSolidGround(PlayerMovementState snapping, Vector3 newPos, out Vector3 groundNormal) {
            newPos.y += 10;
            DebugDraw.AddLine(newPos, newPos - Vector3.up * 20, Color.blue, 0.25f, 8);

            if (Physics.Raycast(newPos, Vector3.down, out RaycastHit hit, Mathf.Infinity, snapping.TerrainMask, QueryTriggerInteraction.Ignore)) {
                groundNormal = hit.normal;
                return groundNormal.y >= snapping.TerrainAngleStrictness && !Physics.CheckSphere(hit.point, snapping.ColliderCheckRadius, snapping.InvisibleColliderMask, QueryTriggerInteraction.Ignore);
            }

            groundNormal = Vector3.up;
            return false;
        }
    }

    public enum PlayerFoot {
        Invalid,
        Left,
        Right
    }
}