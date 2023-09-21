using System;
using FieldDay.Components;
using UnityEngine;

namespace Waddle {
    public sealed class PenguinStepState : BatchedComponent {
        public Transform LeftFoot;
        public AudioSource LeftAudio;

        public Transform RightFoot;
        public AudioSource RightAudio;

        public SFXAsset StepSFX;

        [NonSerialized] public FootIndex LastFoot;
        [NonSerialized] public bool Queued;
        [NonSerialized] public StepType Type;

        public enum FootIndex {
            None,
            Left,
            Right,
            Both
        }

        public enum StepType {
            Default,
            Soft
        }
    }
}