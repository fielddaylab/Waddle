using BeauRoutine;
using FieldDay;
using FieldDay.Systems;
using UnityEngine;

namespace Waddle {
    [SysUpdate(GameLoopPhase.LateUpdate, 100)]
    public sealed class AdditiveBlendSystem : ComponentSystemBehaviour<AdditiveBlendState> {
        public override void ProcessWorkForComponent(AdditiveBlendState component, float deltaTime) {
            int layerCount = component.LayerCount;
            ProcessInterpolations(ref component.ScriptLerpWeights, layerCount, deltaTime);
            ProcessInterpolations(ref component.StateMachineMaskingWeights, layerCount, deltaTime);

            AdditiveBlendState.Float8 finalWeights = new AdditiveBlendState.Float8();
            for(int i = 0; i < layerCount; i++) {
                finalWeights[i] = component.ScriptLerpWeights.Current[i] * component.StateMachineMaskingWeights.Current[i];
            }

            ApplyBlend(component, finalWeights);
        }

        static private void ProcessInterpolations(ref AdditiveBlendState.LerpPart lerps, int count, float deltaTime) {
            float target;
            for(int i = 0; i < count; i++) {
                ref float current = ref lerps.Current[i];
                target = lerps.Target[i];
                current = Mathf.Lerp(current, target, TweenUtil.Lerp(lerps.Lerp[i], 1, deltaTime));
                if (Mathf.Approximately(current, target)) {
                    current = target;
                }
            }
        }

        static private void ApplyBlend(AdditiveBlendState state, AdditiveBlendState.Float8 weights) {
            for(int i = 0; i < state.LayerCount; i++) {
                if (state.LastApplied[i] != weights[i]) {
                    state.Animator.SetLayerWeight(i, weights[i]);
                }
            }
            state.LastApplied = weights;
        }
    }
}