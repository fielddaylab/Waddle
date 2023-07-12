using System;
using BeauRoutine;
using UnityEngine;

namespace Waddle {
    [SharedBetweenAnimators]
    public class PenguinTurnAnimBehavior : StateMachineBehaviour {
        public AnimationClip Clip;
        public AnimFrameRange FrameRange;
        public float RotateAmount;
        public Curve RotateCurve;

        [NonSerialized] private int m_FrameCount;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            m_FrameCount = (int) Math.Round(Clip.frameRate * Clip.length);

            PenguinTurnAnimState state = animator.GetComponent<PenguinTurnAnimState>();
            state.StartRotation = state.Root.localEulerAngles;
            state.TargetRotation = state.StartRotation + new Vector3(0, RotateAmount, 0);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            PenguinTurnAnimState state = animator.GetComponent<PenguinTurnAnimState>();
            if (FrameRange.InRange(stateInfo, m_FrameCount, out float lerp)) {
                state.Root.localEulerAngles = Vector3.Lerp(state.StartRotation, state.TargetRotation, RotateCurve.Evaluate(lerp));
            }
        }
    }
}