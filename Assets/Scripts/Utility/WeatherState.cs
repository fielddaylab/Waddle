using System;
using FieldDay.SharedState;
using UnityEngine;

namespace Waddle {
    public sealed class WeatherState : SharedStateComponent {
        [Header("Snow")]
        public Transform WeatherRoot;
        public ParticleSystem Snow;

        [Header("Wind")]
        public WindZone Wind;
    }
}