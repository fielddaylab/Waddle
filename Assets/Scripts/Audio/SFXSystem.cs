using System;
using BeauRoutine.Extensions;
using BeauUtil;
using FieldDay;
using FieldDay.SharedState;
using FieldDay.Systems;
using UnityEngine;

namespace Waddle {
    [SysUpdate(GameLoopPhase.ApplicationPreRender, 50000)]
    public class SFXSystem : SharedStateSystemBehaviour<SFXState> {
        public override void ProcessWork(float deltaTime) {
            if (AudioListener.pause) {
                return;
            }

            while(m_State.Queue.TryPopFront(out QueuedSFX item)) {
                if (item.Source == null) {
                    continue;
                }

                AudioClip clip = item.Clip;
                float volume = item.Volume;
                float pitch = 1;
                if (clip == null && item.Asset != null) {
                    clip = GetRandomClip(item.Asset);
                    volume *= item.Asset.Volume.Generate();
                    pitch *= item.Asset.PitchRange.Generate();
                }

                if (clip != null) {
                    item.Source.Stop();
                    item.Source.volume = volume;
                    item.Source.pitch = pitch;
                    item.Source.clip = clip;
                    item.Source.Play();
                }
            }
        }

        private AudioClip GetRandomClip(SFXAsset asset) {
            if (asset.Random == null) {
                asset.Random = new RandomDeck<AudioClip>(asset.Clips);
            }

            return asset.Random.Next();
        }
    }
}