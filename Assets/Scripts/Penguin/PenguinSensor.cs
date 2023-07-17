using BeauUtil;
using BeauUtil.Debugger;
using UnityEngine;

namespace Waddle {
    [RequireComponent(typeof(Collider), typeof(TriggerListener))]
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
            if (!EnterSignal.IsEmpty) {
                Brain.Signal(EnterSignal, collider);
            }
        }

        private void OnExit(Collider collider) {
            if (!ExitSignal.IsEmpty) {
                Brain.Signal(ExitSignal, collider);
            }
        }
    }
}