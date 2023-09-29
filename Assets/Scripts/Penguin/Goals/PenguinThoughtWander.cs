using System.Collections;
using UnityEngine;
using BeauUtil;
using FieldDay.Processes;
using System;

namespace Waddle {
    [Serializable]
    public struct PenguinWanderData {
        public float IdleWait;
        public float IdleWaitRandom;
        public float WanderRadius;

        [NonSerialized] public Vector3 Tether;
    }

    public class PenguinThoughtWander : PenguinThoughtState {
        public override IEnumerator Sequence(Process process) {
            PenguinBrain brain = Brain(process);
            while(true) {
                SFXUtility.Play(brain.BeakAudio, brain.Vocalizations);
                yield return brain.WanderParameters.IdleWait + RNG.Instance.NextFloat(brain.WanderParameters.IdleWaitRandom);
                Vector3 nearbyPoint = FindNearbyPoint(brain.Feet, brain.WanderParameters.Tether, brain.WanderParameters.WanderRadius);
                brain.SetMainState(PenguinStates.Walk, new PenguinWalkData() { TargetPosition = nearbyPoint });
                yield return null;
                while(brain.Steering.HasTarget) {
                    yield return null;
                }
            }
        }

        static public Vector3 FindNearbyPoint(PenguinFeetSnapping snapping, Vector3 tether, float radius) {
            Vector3 newPoint;
            do {
                newPoint = tether + Geom.SwizzleYZ(RNG.Instance.NextVector2(radius / 2, radius));
            } while (!PenguinFeetUtility.IsSolidGround(snapping, newPoint));
            return newPoint;
        }
    }
}