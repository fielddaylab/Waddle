using UnityEngine;

namespace Waddle {
    [RequireComponent(typeof(PenguinBrain))]
    public class MatingDancePenguin : MonoBehaviour {
        public Transform DesiredPlayerLook;
        public ParticleSystem HeartParticles;
    }
}