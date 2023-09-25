using UnityEngine;
using FieldDay.Processes;
using BeauUtil;

namespace Waddle {
    public struct PenguinLookData {
        public RuntimeObjectHandle TargetTransform;
    }

    public abstract class PenguinLookState : IProcessStateEnterExit, IProcessStateUpdate, IProcessStateSignal {
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

        public virtual void OnSignal(Process process, StringHash32 signalId, object signalArgs) {
        }
    }

    public class PenguinStareAheadState : PenguinLookState {
        public override void OnSignal(Process process, StringHash32 signalId, object signalArgs) {
            if (signalId == "player-near") {
                Collider c = (Collider) signalArgs;
                process.TransitionTo(PenguinLookStates.LookAtPlayer, new PenguinLookData() { TargetTransform = c.transform });
            }
        }
    }

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

    public class PenguinLookAtPlayerState : PenguinLookAtTransformState {
        public override void OnSignal(Process process, StringHash32 signalId, object signalArgs) {
            if (signalId == "player-leave") {
                process.TransitionToDefault();
            }
        }
    }

    static public class PenguinLookStates {
        static public readonly ProcessStateDefinition Default = ProcessStateDefinition.FromCallbacks("stare-ahead", new PenguinStareAheadState());
        static public readonly ProcessStateDefinition LookAtTransform = ProcessStateDefinition.FromCallbacks("look-at-transform", new PenguinLookAtTransformState());
        static public readonly ProcessStateDefinition LookAtPlayer = ProcessStateDefinition.FromCallbacks("look-at-player", new PenguinLookAtPlayerState());
    }
}