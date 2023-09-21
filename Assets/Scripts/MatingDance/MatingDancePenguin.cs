using UnityEngine;

namespace Waddle {
    [RequireComponent(typeof(PenguinBrain))]
    public class MatingDancePenguin : MonoBehaviour {
        public ParticleSystem HeartParticles;
        public Transform FinalHeartTransform;
        public SFXAsset BopFeedback;
    }
}