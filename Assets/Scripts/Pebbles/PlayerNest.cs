using System;
using FieldDay;
using FieldDay.Processes;
using UnityEngine;

namespace Waddle {
    public class PlayerNest : MonoBehaviour, IBeakInteract {
        public PlayerNestChunk[] Chunks;
        public MiniGameUnlocker NextMinigame;

        [NonSerialized] private int m_ChunksFull;

        private void Start() {
            PenguinGameManager.OnReset += () => {
                foreach (var chunk in Chunks) {
                    chunk.ResetEffects();
                }
                m_ChunksFull = 0;
            };

            Game.Events.Register(PlayerBeakUtility.Event_PickedUp, OnPlayerPickedUp)
                .Register(PlayerBeakUtility.Event_Dropped, OnPlayerDropped);
        }

        private void OnPlayerPickedUp() {
            for(int i = m_ChunksFull; i < Chunks.Length; i++) {
                Chunks[i].Renderer.enabled = true;
            }
        }

        private void OnPlayerDropped() {
            for (int i = m_ChunksFull; i < Chunks.Length; i++) {
                Chunks[i].Renderer.enabled = false;
            }
        }

        public void OnBeakInteract(PlayerBeakState state, BeakTrigger trigger) {
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
            chunk.Effect.Play();

            if (m_ChunksFull == Chunks.Length) {
                NextMinigame.PebbleUnlock();
            }
        }
    }
}