using System;
using BeauRoutine.Extensions;
using BeauUtil;
using FieldDay;
using FieldDay.SharedState;
using UnityEngine;

namespace Waddle {
    public class SFXState : SharedStateComponent {
        public RingBuffer<QueuedSFX> Queue = new RingBuffer<QueuedSFX>(32, RingBufferMode.Expand);
    }

    public struct QueuedSFX {
        public SFXAsset Asset;
        public AudioClip Clip;
        public AudioSource Source;
        public float Volume;
    }

    static public class SFXUtility {
        static public void Play(AudioSource source, AudioClip clip, float volume = 1) {
            if (source == null || clip == null) {
                return;
            }

            Game.SharedState.Get<SFXState>().Queue.PushBack(new QueuedSFX() {
                Clip = clip,
                Source = source,
                Volume = volume
            });
        }

        static public void Play(AudioSource source, SFXAsset asset, float volume = 1) {
            if (source == null || asset == null) {
                return;
            }

            Game.SharedState.Get<SFXState>().Queue.PushBack(new QueuedSFX() {
                Asset = asset,
                Source = source,
                Volume = volume
            });
        }
    }
}