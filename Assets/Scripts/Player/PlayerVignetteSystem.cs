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
    public class PlayerVignetteSystem : SharedStateSystemBehaviour<PlayerVignetteState, PlayerHeadState, PlayerMovementState> {
        public override void ProcessWork(float deltaTime) {
            if (PenguinGameManager._isGamePaused) {
                return;
            }

            float timeSinceLastSafe = Time.unscaledTime - m_StateC.LastSafeTime;
            if (timeSinceLastSafe > m_StateA.UnsafeTimeThreshold) {
                m_StateA.Fade = Math.Min(1, m_StateA.Fade + deltaTime / m_StateA.FadeOutTime);
                m_StateA.Fader.SetAlpha(m_StateA.FadeCurve.Evaluate(m_StateA.Fade));
                if (m_StateA.Fade >= 1 && timeSinceLastSafe > m_StateA.UnsafeTimeThreshold + m_StateA.FadeOutTime + m_StateA.FullAlphaTime) {
                    m_StateB.PositionRoot.position = PlayerMovementUtility.GetBestSafeLocation(m_StateC, m_StateB.PositionRoot.position, m_StateA.TeleportDistance);
                }
            } else if (m_StateA.Fade > 0) {
                m_StateA.Fade = Math.Max(0, m_StateA.Fade - deltaTime / m_StateA.FadeInTime);
                m_StateA.Fader.SetAlpha(m_StateA.FadeCurve.Evaluate(m_StateA.Fade));
            }
        }
    }
}