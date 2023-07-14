using System.Collections;
using BeauUtil;
using FieldDay.Processes;

namespace Waddle {
    public class PenguinThoughtPebbles : PenguinThoughtState {
        public override IEnumerator Sequence(Process process) {
            PenguinBrain brain = Brain(process);
            PenguinPebbleData pebbleData = brain.GetComponent<PenguinPebbleData>();
            while(pebbleData.PebblesToGather > 0) {
                yield return RNG.Instance.Next(2, 4);
                PebbleSource nearbySource = FindRandomSource(pebbleData);
                brain.SetMainState(PenguinStates.Walk, new PenguinWalkData() { TargetPosition = nearbySource.transform.position });
                yield return null;
                while(brain.Steering.HasTarget) {
                    yield return null;
                }

                // TODO: Pick up pebble
                yield return 1;

                brain.SetMainState(PenguinStates.Walk, new PenguinWalkData() { TargetPosition = pebbleData.PebbleDropOff.position });
                yield return null;
                while(brain.Steering.HasTarget) {
                    yield return null;
                }

                pebbleData.PebblesToGather -= 1;
            }
        }

        static public PebbleSource FindRandomSource(PenguinPebbleData data) {
            return RNG.Instance.Choose(data.PebbleSources);
        }
    }
}