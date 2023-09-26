using System;
using BeauRoutine;
using FieldDay.Processes;
using UnityEngine;

namespace Waddle {
    public class PlayerNestChunk : MonoBehaviour {
        public ParticleSystem Effect;
        public Renderer Renderer;
        public Material ShineMaterial;
        public Material PlacedMaterial;

        public void ResetEffects() {
            Effect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            Renderer.enabled = false;
            Renderer.sharedMaterial = ShineMaterial;
        }
    }
}