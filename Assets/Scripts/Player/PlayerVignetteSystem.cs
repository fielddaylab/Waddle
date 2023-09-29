using System;
using BeauRoutine;
using BeauUtil;
using BeauUtil.Debugger;
using FieldDay;
using FieldDay.SharedState;
using FieldDay.Systems;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Waddle {

    [SysUpdate(GameLoopPhase.UnscaledLateUpdate, 1000)]
    public class PlayerVignetteSystem : SharedStateSystemBehaviour<PlayerHeadState, PlayerVignetteState, PlayerMovementState> {
        public override void ProcessWork(float deltaTime) {
            if (PenguinGameManager._isGamePaused) {
                return;
            }

            Vector3 headOffset = PlayerHeadUtility.CalculateHeadBodyVector(m_StateA);
            Vector3 localHeadOffset = m_StateA.Rig.trackingSpace.InverseTransformVector(headOffset);
            float lean = Math.Abs(m_StateA.HeadLook.y) * 1 - Mathf.Clamp01(Mathf.Abs(localHeadOffset.x) / 0.5f);
            lean *= lean;
            float radius = headOffset.magnitude;

            float radiusOffset = m_StateB.BoundsRadiusLeanOffset * lean;
            radiusOffset += Mathf.Clamp01(m_StateC.WalkCooldown / 0.2f) * m_StateB.BoundsRadiusWalkOffset;
            radiusOffset += Mathf.Clamp01(PlayerHeadUtility.CalculateAverageVelocity(m_StateA, 8).magnitude / 3) * m_StateB.BoundsRadiusWalkOffset;
            float min = m_StateB.BoundsRadiusStart + radiusOffset;
            float max = m_StateB.BoundsRadiusEnd + radiusOffset;

            float amount = Curve.CubeOut.Evaluate(Mathf.Clamp01(MathUtils.Remap(radius, min, max, 0, 1)));
            m_StateB.Fader.SetAlpha(amount);
            AudioListener.volume = 1 - amount;
            m_StateB.ReturnAudio.volume = amount * m_StateB.ReturnAudioVolume;
            if (amount > 0 && !m_StateB.ReturnAudio.isPlaying) {
                m_StateB.ReturnAudio.Play();
            } else if (amount == 0 && m_StateB.ReturnAudio.isPlaying) {
                m_StateB.ReturnAudio.Pause();
            }
        }
    }
}