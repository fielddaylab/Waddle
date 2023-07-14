using System.Collections;
using UnityEngine;
using BeauUtil;
using FieldDay.Processes;

namespace Waddle {
    public class PenguinThoughtWander : PenguinThoughtState {
        public override IEnumerator Sequence(Process process) {
            PenguinBrain brain = Brain(process);
            while(true) {
                yield return RNG.Instance.Next(4, 8);
                Vector3 nearbyPoint = FindNearbyPoint(brain);
                brain.SetMainState(PenguinStates.Walk, new PenguinWalkData() { TargetPosition = nearbyPoint });
                yield return null;
                while(brain.Steering.HasTarget) {
                    yield return null;
                }
            }
        }

        static public Vector3 FindNearbyPoint(PenguinBrain brain) {
            Vector3 newPoint = brain.Position.position;
            newPoint.x += RNG.Instance.NextFloat(-4, 4);
            newPoint.z += RNG.Instance.NextFloat(-4, 4);
            return newPoint;
        }
    }
}