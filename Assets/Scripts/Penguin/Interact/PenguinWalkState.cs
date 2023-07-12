using System.Numerics;
using FieldDay.Processes;

namespace Waddle {
    public struct PenguinWalkData {
        public Vector3 TargetPosition;
    }

    public class PenguinWalkState : ParameterizedPenguinState<PenguinWalkData>, IProcessStateFixedUpdate {
        public override void OnEnter(Process process) {
            base.OnEnter(process);
            PenguinBrain brain = Brain(process);
            brain.Animator.SetBool("Waddle", true);
        }

        public override void OnExit(Process process) {
            PenguinBrain brain = Brain(process);
            brain.Animator.SetBool("Waddle", false);
        }

        public void OnFixedUpdate(Process process, float deltaTime) {
            PenguinBrain brain = Brain(process);
        }
    }
}