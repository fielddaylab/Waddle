using System;
using FieldDay.Components;
using UnityEngine;

namespace Waddle {
    public class PenguinLookSmoothing : BatchedComponent {
        public Animator Animator;
        public Transform LookFrom;
        public float LookLerpSpeed = 8;

        [NonSerialized] public bool IsLooking;
        [NonSerialized] public Vector3 LookVector;
        [NonSerialized] public Space LookSpace;

        [NonSerialized] public Vector2 LastAppliedLook;

        public Vector2 WorldLookDirectionToLocal(Vector3 worldLook) {
            return transform.InverseTransformDirection(worldLook);
        }
    }
}