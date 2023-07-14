using BeauUtil;
using UnityEngine;

namespace Waddle {
    [RequireComponent(typeof(Collider))]
    public class PenguinSensor : MonoBehaviour {
        [Required] public PenguinBrain Brain;
        [Required] public TriggerListener Listener;
        public SerializedHash32 EnterSignal;
        public SerializedHash32 ExitSignal;

        private void Awake() {
            Listener.onTriggerEnter.AddListener(OnEnter);
            Listener.onTriggerExit.AddListener(OnExit);
        }

        private void OnEnter(Collider collider) {
            Brain.Signal(EnterSignal, collider);
        }

        private void OnExit(Collider collider) {
            Brain.Signal(ExitSignal, collider);
        }
    }
}