using System;
using BeauUtil;
using FieldDay;
using FieldDay.Components;
using FieldDay.Debugging;
using FieldDay.SharedState;
using UnityEngine;

namespace Waddle
{
    public class PlayerMovementState : SharedStateComponent {
        public float MoveSpeed = 20;

        [Header("Terrain Surface")]
        public LayerMask TerrainMask;
        [Range(0, 1)] public float TerrainAngleStrictness = 0.5f;
        public float MaxHeightChange = 0.1f;

        [Header("Invisible Collisions")]
        public LayerMask InvisibleColliderMask;
        public float ColliderCheckRadius;
        [Range(0, 1)] public float MinMovePercentage = 0.5f;

        [Header("Responses")]
        public AudioSource FootAudioSource;
        public SFXAsset StepAudioClips;
        public SFXAsset CollideAudioClips;

        [NonSerialized] public bool Queued;
        [NonSerialized] public bool FromRight;
        [NonSerialized] public Vector3 MoveDirection;

        [NonSerialized] public float WalkCooldown;
        [NonSerialized] public PlayerFoot LastStepSide;
        [NonSerialized] public int ConsecutiveSteps;
    }

    static public class PlayerMovementUtility {
        static public readonly StringHash32 Event_WaddleDetected = "player-waddle-detected";

        static public void QueueMovement(PlayerMovementState state, Vector3 moveIn, PlayerFoot foot, float cooldown) {
            state.Queued = true;
            state.FromRight = (foot == PlayerFoot.Right);
            state.MoveDirection = moveIn;
            state.WalkCooldown = cooldown;
            state.LastStepSide = foot;
            Game.Events.Queue(Event_WaddleDetected);
        }

        static public bool IsSolidGround(PlayerMovementState snapping, Vector3 newPos) {
            return IsSolidGround(snapping, newPos, out Vector3 _);
        }

        static public bool IsSolidGround(PlayerMovementState snapping, Vector3 newPos, out Vector3 groundNormal) {
            newPos.y += 10;

            if (Physics.Raycast(newPos, Vector3.down, out RaycastHit hit, Mathf.Infinity, snapping.TerrainMask, QueryTriggerInteraction.Ignore)) {
                groundNormal = hit.normal;
                if (groundNormal.y < snapping.TerrainAngleStrictness) {
                    //DebugDraw.AddLine(hit.point, hit.point + hit.normal * 10, Color.red.WithAlpha(0.25f), 0.25f, 8);
                    return false;
                }

                //DebugDraw.AddLine(hit.point, hit.point + hit.normal * 10, Color.blue.WithAlpha(0.25f), 0.25f, 8);
                if (Physics.CheckSphere(hit.point, snapping.ColliderCheckRadius, snapping.InvisibleColliderMask, QueryTriggerInteraction.Ignore)) {
                    //DebugDraw.AddSphere(hit.point, snapping.ColliderCheckRadius, Color.red.WithAlpha(0.25f), 8);
                    return false;
                }

                //DebugDraw.AddSphere(hit.point, snapping.ColliderCheckRadius, Color.blue.WithAlpha(0.25f), 8);
                return true;
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