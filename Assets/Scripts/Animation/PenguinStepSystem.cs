using System;
using FieldDay.Components;
using FieldDay.Systems;
using UnityEngine;

namespace Waddle {
    public sealed class PenguinStepSystem : ComponentSystemBehaviour<PenguinStepState> {
        [SerializeField, Range(0, 1)] private float m_SoftVolume = 0.8f;
        
        public override void ProcessWorkForComponent(PenguinStepState component, float deltaTime) {
            if (component.Queued) {
                float volume = 1;
                if (component.Type == PenguinStepState.StepType.Soft) {
                    volume = m_SoftVolume;
                }

                if (component.LastFoot == PenguinStepState.FootIndex.Left || component.LastFoot == PenguinStepState.FootIndex.Both) {
                    SFXUtility.Play(component.LeftAudio, component.StepSFX, volume);
                }
                if (component.LastFoot == PenguinStepState.FootIndex.Right || component.LastFoot == PenguinStepState.FootIndex.Both) {
                    SFXUtility.Play(component.RightAudio, component.StepSFX, volume);
                }

                component.Queued = false;
            }
        }
    }
}