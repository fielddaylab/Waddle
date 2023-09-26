using System.Collections;
using UnityEngine;
using BeauUtil;
using FieldDay.Processes;

namespace Waddle {
    public class PenguinThoughtPebbles : PenguinThoughtState {
        public override IEnumerator Sequence(Process process) {
            PenguinBrain brain = Brain(process);
            PenguinPebbleData pebbleData = brain.GetComponent<PenguinPebbleData>();
            pebbleData.HeldPebbleRenderer.enabled = false;
            while (true) {
                yield return RNG.Instance.Next(2, 4);
                PebbleSource nearbySource = FindRandomSource(pebbleData);
                brain.SetMainState(PenguinStates.Walk, new PenguinWalkData() { TargetPosition = nearbySource.transform.position });
                yield return null;
                while(brain.Steering.HasTarget) {
                    yield return null;
                }
                brain.Animator.SetTrigger("T_PebblePickup");
                brain.Animator.SetBool("PebbleCarried", true);
                yield return 1;
                yield return 1.5f;
                pebbleData.HeldPebbleRenderer.enabled = true;
                yield return 0.5f;
                brain.SetMainState(PenguinStates.Walk, new PenguinWalkData() { TargetPosition = pebbleData.PebbleDropOff.position });
                yield return null;
                while(brain.Steering.HasTarget) {
                    yield return null;
                }

                brain.Animator.SetTrigger("T_PebbleDropOff");
                brain.Animator.SetBool("PebbleCarried", false);
                yield return 1;
                pebbleData.HeldPebbleRenderer.enabled = false;
                yield return 2;

                //pebbleData.PebblesToGather -= 1;
            }
        }

        static public PebbleSource FindRandomSource(PenguinPebbleData data) {
            return RNG.Instance.Choose(data.PebbleSources);
        }
    }
}