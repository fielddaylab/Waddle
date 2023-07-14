using System;
using BeauUtil;
using FieldDay.SharedState;
using UnityEngine;

namespace Waddle
{
    public class PlayerMovementState : SharedStateComponent
    {
        public float MoveSpeed = 20;

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
    }

    public enum PlayerFoot {
        Invalid,
        Left,
        Right
    }
}