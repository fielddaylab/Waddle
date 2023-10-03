using System;
using BeauRoutine;
using FieldDay.SharedState;
using UnityEngine;

namespace Waddle {
    public sealed class WeatherState : SharedStateComponent {
        [Header("Snow")]
        public Transform WeatherRoot;
        public ParticleSystem Snow;

        [Header("Wind")]
        public WindZone Wind;
        public float WindStrengthMultiplier = 1;
        public Transform WindTransform;
        public float WindRootOffsetMultiplier = 1;
        public AudioSource WindAudio;

        [NonSerialized] public Routine WindRoutine;
        public Vector3 WindDirection;
        [NonSerialized] public bool Mute;
    }
}