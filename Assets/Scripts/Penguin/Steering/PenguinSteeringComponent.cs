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

    static public class PenguinSteeringUtility {
        static public void SteerTowards(PenguinSteeringComponent steering, Vector3 position, float tolerance = 0.2f) {
            steering.HasTarget = true;
            steering.TargetPos = position;
            steering.TargetObject = null;
            steering.TargetPosTolerance = tolerance;
        }

        static public void SteerTowards(PenguinSteeringComponent steering, Transform node, float tolerance = 0.2f) {
            steering.HasTarget = true;
            steering.TargetPos = default;
            steering.TargetObject = node;
            steering.TargetPosTolerance = tolerance;
        }
    }
}