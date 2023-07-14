using System.Collections;
using BeauUtil;
using FieldDay.Processes;

namespace Waddle {
    public abstract class PenguinThoughtState : PenguinState, IProcessStateSequence {
        public virtual void OnSignal(Process process, StringHash32 signalId, object signalArgs) {
        }

        public virtual IEnumerator Sequence(Process process) {
            return null;
        }
    }

    static public class PenguinThoughts {
        static public readonly ProcessStateDefinition Wander = ProcessStateDefinition.FromCallbacks("wander", new PenguinThoughtWander());
        static public readonly ProcessStateDefinition PebbleGather = ProcessStateDefinition.FromCallbacks("pebbles", new PenguinThoughtPebbles());
    }
}