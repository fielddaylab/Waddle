using FieldDay.Processes;
using UnityEngine;

namespace Waddle {
    public class PenguinBrain : ProcessBehaviour {
        public Animator Animator;
        public PenguinLookSmoothing LookSmoothing;

        [Header("-- DEBUG -- ")]
        [SerializeField] private Transform m_DEBUGLookAt;

        protected ProcessId m_LookProcess;

        protected override void Start() {
            m_LookProcess = StartProcess(PenguinLookStates.Default, "PenguinLook");
            if (m_DEBUGLookAt != null) {
                m_LookProcess.TransitionTo(PenguinLookStates.LookAtTransform, new PenguinLookData() { TargetTransform = m_DEBUGLookAt });
            }
        }
    }
}