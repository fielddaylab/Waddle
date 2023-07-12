using System;
using FieldDay.Components;
using UnityEngine;

namespace Waddle {
    public sealed class PenguinTurnAnimState : MonoBehaviour {
        public Transform Root;

        [NonSerialized] public Vector3 StartRotation;
        [NonSerialized] public Vector3 TargetRotation;

#if UNITY_EDITOR
        private void Reset() {
            Root = transform.parent;
        }
#endif // UNITY_EDITOR
    }
}