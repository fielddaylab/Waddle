using System.Collections;
using BeauRoutine;
using BeauRoutine.Splines;
using BeauUtil;
using FieldDay;
using FieldDay.Systems;
using UnityEngine;

namespace Waddle {
    [SysUpdate(GameLoopPhase.LateUpdate, 100000)]
    public sealed class WeatherSystem : SharedStateSystemBehaviour<WeatherState, LODReference> {
        public float MinWindVolume = 0.2f;
        public float WindVolumeIncrease = 0.45f;
        public float WindVolumeEarFacingRatio = 0.65f;
        public float WindPanEarFacing = 0.4f;

        public override void ProcessWork(float deltaTime) {
            Vector3 anchorPos = m_StateB.CachedTransform.position;
            Vector3 windDir = m_StateA.WindDirection;

            anchorPos.x -= windDir.x * m_StateA.WindRootOffsetMultiplier;
            anchorPos.z -= windDir.z * m_StateA.WindRootOffsetMultiplier;
            m_StateA.WeatherRoot.SetPosition(anchorPos, Axis.XZ, Space.World);

            if (windDir.x != 0 || windDir.y != 0 || windDir.z != 0) {
                float strength = windDir.magnitude;
                Vector3 normalized = windDir.normalized;
                m_StateA.Wind.windMain = strength * m_StateA.WindStrengthMultiplier;
                m_StateA.WindTransform.forward = normalized;

                float earsFacing = Vector3.Dot(m_StateB.CachedTransform.right, normalized);
                m_StateA.WindAudio.volume = MinWindVolume + Mathf.Clamp01(strength) * WindVolumeIncrease * ((1 - WindVolumeEarFacingRatio) + Mathf.Abs(earsFacing) * WindVolumeEarFacingRatio);
                m_StateA.WindAudio.panStereo = -earsFacing * WindPanEarFacing;
            } else {
                m_StateA.Wind.windMain = 0;
                m_StateA.WindAudio.volume = MinWindVolume;
                m_StateA.WindAudio.panStereo = 0;
            }

            m_StateA.WindAudio.mute = m_StateA.Mute;

            if (!m_StateA.WindRoutine) {
                m_StateA.WindRoutine.Replace(m_StateA, WindRoutine(m_StateA));
            }
        }

        private void OnReset() {
            if (m_StateA && m_StateB) {
                ProcessWork(0);
                m_StateA.WindDirection = default;
                m_StateA.Mute = false;
                foreach(var ps in m_StateA.WeatherRoot.GetComponentsInChildren<ParticleSystem>()) {
                    ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    ps.Play(true);
                }
                m_StateA.WindRoutine.Replace(m_StateA, WindRoutine(m_StateA));
            }
        }

        static private IEnumerator WindRoutine(WeatherState state) {
            yield return RNG.Instance.NextFloat(2, 8);
            while(true) {
                Vector2 dir2D = RNG.Instance.NextVector2(0.4f, 1.2f);
                Vector3 dir3D = new Vector3(dir2D.x, 0, dir2D.y);
                yield return Tween.Vector(state.WindDirection, dir3D, (v) => state.WindDirection = v, RNG.Instance.NextFloat(1, 6));
                yield return RNG.Instance.NextFloat(8, 15);
                if (RNG.Instance.Chance(0.3f)) {
                    yield return Tween.Vector(state.WindDirection, Vector3.zero, (v) => state.WindDirection = v, RNG.Instance.NextFloat(5, 8));
                    yield return RNG.Instance.NextFloat(4, 12);
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