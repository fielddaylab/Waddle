using BeauRoutine;
using FieldDay;
using FieldDay.SharedState;
using FieldDay.Systems;
using UnityEngine;

namespace Waddle {
    [SysUpdate(GameLoopPhase.LateUpdate, 100000)]
    public sealed class WeatherSystem : SharedStateSystemBehaviour<WeatherState, LODReference> {
        public override void ProcessWork(float deltaTime) {
            Vector3 anchorPos = m_StateB.CachedTransform.position;

            m_StateA.WeatherRoot.SetPosition(anchorPos, Axis.XZ, Space.World);
        }

        private void OnReset() {
            if (m_StateA && m_StateB) {
                ProcessWork(0);
                foreach(var ps in m_StateA.WeatherRoot.GetComponentsInChildren<ParticleSystem>()) {
                    ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    ps.Play(true);
                }
            }
        }

        public override void Initialize() {
            base.Initialize();

            PenguinGameManager.OnReset += OnReset;
        }

        public override void Shutdown() {
            PenguinGameManager.OnReset -= OnReset;

            base.Shutdown();
        }
    }
}