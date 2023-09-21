using System;
using BeauRoutine;
using UnityEngine;

namespace Waddle {
    [SharedBetweenAnimators]
    public class AdditiveBlendMaskBehavior : StateMachineBehaviour {
        public AnimationClip Clip;
        public int LayerIndex;
        public float OverrideLerpSpeed = 0;
        [Range(0, 1)] public float DefaultWeight = 1;
        [Range(0, 1)] public float InRangeWeight = 0;
        public AnimFrameRange[] Ranges;

        [NonSerialized] private int m_FrameCount;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            m_FrameCount = (int) Math.Round(Clip.frameRate * Clip.length);

            AdditiveBlendState state = animator.GetComponent<AdditiveBlendState>();
            float layerWeight = AnimFrameRange.InRange(stateInfo, Ranges, m_FrameCount) ? InRangeWeight : DefaultWeight;
            state.StateMachineMaskingWeights.Target[LayerIndex] = layerWeight;
            if (OverrideLerpSpeed > 0) {
                state.StateMachineMaskingWeights.Lerp[LayerIndex] = OverrideLerpSpeed;
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            AdditiveBlendState state = animator.GetComponent<AdditiveBlendState>();
            float layerWeight = AnimFrameRange.InRange(stateInfo, Ranges, m_FrameCount) ? InRangeWeight : DefaultWeight;
            state.StateMachineMaskingWeights.Target[LayerIndex] = layerWeight;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            AdditiveBlendState state = animator.GetComponent<AdditiveBlendState>();
            state.StateMachineMaskingWeights.Target[LayerIndex] = DefaultWeight;
        }
    }
}