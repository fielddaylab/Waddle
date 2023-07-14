using UnityEngine;
using FieldDay.Processes;
using BeauUtil;

namespace Waddle {
    public struct PenguinWalkData {
        public Vector3 TargetPosition;
        public RuntimeObjectHandle TargetObject;
        public float TargetDistanceThreshold;
    }

    public class PenguinWalkState : ParameterizedPenguinState<PenguinWalkData>, IProcessStateSignal {
        public override void OnEnter(Process process, PenguinWalkData param) {
            PenguinBrain brain = Brain(process);
            brain.Animator.SetBool("Waddle", true);
            brain.Steering.HasTarget = true;
            brain.Steering.TargetPos = param.TargetPosition;
            brain.Steering.TargetObject = param.TargetObject.Cast<Transform>();
            if (param.TargetDistanceThreshold > 0) {
                brain.Steering.TargetPosTolerance = param.TargetDistanceThreshold;
            }
        }

        public override void OnExit(Process process) {
            PenguinBrain brain = Brain(process);
            brain.Animator.SetBool("Waddle", false);
            brain.Steering.HasTarget = false;
        }

        public virtual void OnSignal(Process process, StringHash32 signalId, object signalArgs) {
            if (signalId == "steering-completed") {
                process.TransitionToDefault();
            }
        }
    }
}