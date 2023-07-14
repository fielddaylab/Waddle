using System;
using FieldDay.Components;
using UnityEngine;

namespace Waddle {
    public sealed class PenguinSteeringComponent : BatchedComponent {
        public Transform PositionRoot;
        public Transform RotationRoot;
        public PenguinBrain Brain;

        public float MovementSpeed = 2;
        public float TurnSpeed = 30;
        public float MaxAngleDiffToMove = 60;

        [NonSerialized] public Vector3 LinearVelocity;
        [NonSerialized] public float AngularVelocity;

        [NonSerialized] public bool HasTarget;
        [NonSerialized] public Vector3 TargetPos;
        [NonSerialized] public Transform TargetObject;
        [NonSerialized] public float TargetPosTolerance = 0.2f;
    }
}