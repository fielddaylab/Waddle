using System;
using BeauUtil;
using BeauUtil.Debugger;
using FieldDay;
using FieldDay.SharedState;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Waddle
{
    public class PlayerVignetteState : SharedStateComponent {
        public VRScreenFade Fader;
        public AudioSource ReturnAudio;
        public float ReturnAudioVolume;

        [Header("Bounds")]
        public float BoundsRadiusStart;
        public float BoundsRadiusEnd;
        public float BoundsRadiusLeanOffset;
        public float BoundsRadiusWalkOffset;

        public void Awake() {
            ReturnAudio.ignoreListenerVolume = true;
        }
    }
}