using FieldDay.Processes;
using UnityEngine;

namespace Waddle {
    public class PenguinBrain : ProcessBehaviour {
        public Transform Position;
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

            StartMainProcess(PenguinStates.Idle, "PenguinMain");
        }

        #region Look State

        public void SetLookState(ProcessStateDefinition lookState) {
            m_LookProcess.TransitionTo(lookState);
        }

        public void SetLookState(ProcessStateDefinition lookState, PenguinLookData lookData) {
            m_LookProcess.TransitionTo(lookState, lookData);
        }

        #endregion // Look State

        #region Main State

        public void SetMainState(ProcessStateDefinition mainState) {
            m_MainProcess.TransitionTo(mainState);
        }

        public void SetMainState<T>(ProcessStateDefinition mainState, in T data) where T : unmanaged {
            m_MainProcess.TransitionTo(mainState, data);
        }

        #endregion // Main State
    }
}