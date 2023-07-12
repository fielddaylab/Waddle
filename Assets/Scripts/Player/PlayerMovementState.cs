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
    }
}