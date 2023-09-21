using System;
using BeauRoutine;
using UnityEngine;

namespace Waddle {
    [SharedBetweenAnimators]
    public class PenguinStepBehaviour : StateMachineBehaviour {
        public AnimationClip Clip;
        public PenguinStepState.StepType StepType;
        public AnimFrameRange[] LeftFrameRanges;
        public AnimFrameRange[] RightFrameRanges;
        public AnimFrameRange[] BothFrameRanges;

        [NonSerialized] private int m_FrameCount;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            m_FrameCount = (int) Math.Round(Clip.frameRate * Clip.length);

            PenguinStepState state = animator.GetComponent<PenguinStepState>();
            state.LastFoot = PenguinStepState.FootIndex.None;
            state.Type = StepType;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            PenguinStepState state = animator.GetComponent<PenguinStepState>();
            if (state.LastFoot != PenguinStepState.FootIndex.Left && AnimFrameRange.InRange(stateInfo, LeftFrameRanges, m_FrameCount)) {
                state.LastFoot = PenguinStepState.FootIndex.Left;
                state.Queued = true;
            } else if (state.LastFoot != PenguinStepState.FootIndex.Right && AnimFrameRange.InRange(stateInfo, RightFrameRanges, m_FrameCount)) {
                state.LastFoot = PenguinStepState.FootIndex.Right;
                state.Queued = true;
            } else if (state.LastFoot != PenguinStepState.FootIndex.Both && AnimFrameRange.InRange(stateInfo, BothFrameRanges, m_FrameCount)) {
                state.LastFoot = PenguinStepState.FootIndex.Both;
                state.Queued = true;
            }
        }
    }
}