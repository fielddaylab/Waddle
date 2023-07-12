using FieldDay.Processes;

namespace Waddle {
    public abstract class PenguinState : IProcessStateEnterExit {
        public virtual void OnEnter(Process process) { }
        public virtual void OnExit(Process process) { }

        static protected PenguinBrain Brain(Process process) {
            return process.Context<PenguinBrain>();
        }
    }

    public abstract class ParameterizedPenguinState<TParam> : PenguinState where TParam : unmanaged {
        public override void OnEnter(Process process) {
            OnEnter(process, process.Data<TParam>());
        }

        public virtual void OnEnter(Process process, TParam param) {

        }

        static protected ref TParam Data(Process process) {
            return ref process.Data<TParam>();
        }
    }

    static public class PenguinStates {
        static public readonly ProcessStateDefinition Idle = ProcessStateDefinition.FromCallbacks("idle", new PenguinIdleState());
        static public readonly ProcessStateDefinition Walk = ProcessStateDefinition.FromCallbacks("walk", new PenguinWalkState());
    }
}