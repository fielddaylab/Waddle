using FieldDay;
using FieldDay.SharedState;
using FieldDay.Systems;
using UnityEngine;

namespace Waddle
{
    [SysUpdate(GameLoopPhase.Update, -100)]
    public class PlayerHeadSystem : SharedStateSystemBehaviour<PlayerHeadState>
    {
        #region Inspector

        [Header("Config")]
        [SerializeField] private float m_OffsetLerp;

        #endregion // Inspector

        public override void ProcessWork(float deltaTime) {

        }
    }
}