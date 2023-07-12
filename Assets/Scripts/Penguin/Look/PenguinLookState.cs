using UnityEngine;
using FieldDay.Processes;

namespace Waddle {
    public struct PenguinLookData {
        public RuntimeObjectHandle TargetTransform;
    }

    public abstract class PenguinLookState : IProcessStateEnterExit, IProcessStateUpdate {
        public virtual Space LookSpace {
            get { return Space.World; }
        }

        public virtual bool TryGetWorldLookVector(Process process, PenguinBrain brain, PenguinLookSmoothing smoothing, out Vector3 look) {
            look = Vector3.zero;
            return false;
        }

        public virtual void OnEnter(Process p) {

        }

        public virtual void OnExit(Process p) {

        }

        public virtual void OnUpdate(Process p, float deltaTime) {
            PenguinBrain brain = Brain(p);
            PenguinLookSmoothing smoothing = brain.LookSmoothing;
            smoothing.LookSpace = LookSpace;
            smoothing.IsLooking = TryGetWorldLookVector(p, brain, smoothing, out smoothing.LookVector);
        }

        static protected PenguinBrain Brain(Process p) {
            return p.Context<PenguinBrain>();
        }
    }

    public class PenguinStareAheadState : PenguinLookState { }

    public class PenguinLookAtTransformState : PenguinLookState {
        public override bool TryGetWorldLookVector(Process process, PenguinBrain brain, PenguinLookSmoothing smoothing, out Vector3 look) {
            var lookData = process.Data<PenguinLookData>();
            if (lookData.TargetTransform) {
                Transform t = lookData.TargetTransform.Cast<Transform>();
                look = t.position - smoothing.LookFrom.position;
                return true;
            }

            look = default;
            return false;

        }
    }

    static public class PenguinLookStates {
        static public readonly ProcessStateDefinition Default = ProcessStateDefinition.FromCallbacks("stare-ahead", new PenguinStareAheadState());
        static public readonly ProcessStateDefinition LookAtTransform = ProcessStateDefinition.FromCallbacks("look-at-transform", new PenguinLookAtTransformState());
    }
}