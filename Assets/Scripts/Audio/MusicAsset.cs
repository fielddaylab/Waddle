using System;
using BeauRoutine.Extensions;
using BeauUtil;
using FieldDay;
using FieldDay.SharedState;
using UnityEngine;

namespace Waddle {
    [CreateAssetMenu(menuName = "Waddle/Music Asset")]
    public class MusicAsset : ScriptableObject {
        public AudioClip Clip;
        [Range(0, 1)] public float Volume = 1;

        [Header("BPM")]
        public float BPM;
        public int Measure = 4;
    }
}