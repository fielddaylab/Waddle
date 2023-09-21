using System;
using BeauRoutine.Extensions;
using BeauUtil;
using FieldDay;
using FieldDay.SharedState;
using UnityEngine;

namespace Waddle {
    [CreateAssetMenu(menuName = "Waddle/SFX Asset")]
    public class SFXAsset : ScriptableObject {
        public AudioClip[] Clips;
        public FloatRange Volume = new FloatRange(1);
        public FloatRange PitchRange = new FloatRange(1);

        [NonSerialized] public RandomDeck<AudioClip> Random;
    }
}