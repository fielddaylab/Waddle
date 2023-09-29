using System;
using BeauRoutine;
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
        public AnimationCurve FadeCurve;
        public float UnsafeTimeThreshold = 1;
        public float FullAlphaTime = 3;
        public float FadeInTime = 2;
        public float FadeOutTime = 1;
        public float TeleportDistance = 1;

        [NonSerialized] public bool FadeEnabled = true;
        [NonSerialized] public float Fade;
    }
}