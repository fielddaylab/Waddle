using System;
using BeauRoutine;
using FieldDay;
using FieldDay.Processes;
using UnityEngine;

namespace Waddle {
    public class PlayerNest : MonoBehaviour, IBeakInteract {
        public PlayerNestChunk[] Chunks;
        public SFXAsset DropSFX;
        public AudioSource CompleteSound;
        public AudioSource ShimmerSound;

        [NonSerialized] private int m_ChunksFull;
        [NonSerialized] private float m_OriginalShimmerVolume;
        [NonSerialized] private Routine m_ShimmerFade;

        private void Start() {
            PenguinGameManager.OnReset += () => {
                foreach (var chunk in Chunks) {
                    chunk.ResetEffects();
                }
                m_ChunksFull = 0;
                m_ShimmerFade.Stop();
                ShimmerSound.Pause();
            };

            foreach(var chunk in Chunks) {
                chunk.ResetEffects();
            }

            m_OriginalShimmerVolume = ShimmerSound.volume;
            ShimmerSound.volume = 0;
            ShimmerSound.Pause();

            Game.Events.Register(PlayerBeakUtility.Event_PickedUp, OnPlayerPickedUp)
                .Register(PlayerBeakUtility.Event_Dropped, OnPlayerDropped);
        }

        private void OnPlayerPickedUp() {
            for(int i = m_ChunksFull; i < Chunks.Length; i++) {
                Chunks[i].Renderer.enabled = true;
            }

            m_ShimmerFade.Replace(this, Tween.ZeroToOne(UpdateShimmerVolume, 0.3f));
        }

        private void OnPlayerDropped() {
            for (int i = m_ChunksFull; i < Chunks.Length; i++) {
                Chunks[i].Renderer.enabled = false;
            }

            m_ShimmerFade.Replace(this, Tween.OneToZero(UpdateShimmerVolume, 0.3f));
        }

        public void OnBeakInteract(PlayerBeakState state, BeakTrigger trigger, Collider beakedCollider) {
            if (m_ChunksFull == Chunks.Length) {
                return;
            }

            if (state.HoldingPebble == null) {
                return;
            }

            state.HoldingPebble.Hide();
            state.HoldingPebble = null;

            Game.Events.Queue(PlayerBeakUtility.Event_Dropped);

            PlayerNestChunk chunk = Chunks[m_ChunksFull++];
            chunk.Renderer.sharedMaterial = chunk.PlacedMaterial;
            chunk.Renderer.enabled = true;
            SFXUtility.Play(chunk.GetComponent<AudioSource>(), DropSFX);

            if (m_ChunksFull == Chunks.Length) {
                foreach (PlayerNestChunk nestChunk in Chunks) {
                    nestChunk.Effect.Play();
                }
                CompleteSound.Play();
                PenguinAnalytics.Instance.LogNestComplete();
            } else {
                chunk.Effect.Play();
            }
        }

        private void UpdateShimmerVolume(float vol) {
            ShimmerSound.volume = vol * m_OriginalShimmerVolume;
            if (vol > 0) {
                ShimmerSound.UnPause();
            } else {
                ShimmerSound.Pause();
            }
        }
    }
}