using FieldDay;
using FieldDay.Components;
using FieldDay.Systems;
using UnityEngine;

namespace Waddle {
    public class WaterAudioSystem : ComponentSystemBehaviour<WaterAudioSource> {
        public override void ProcessWork(float deltaTime) {
            LODReference refRoot = Game.SharedState.Get<LODReference>();

            Vector3 refPos = refRoot.CachedTransform.position;

            int idx = 0;
            foreach(var component in m_Components) {
                if (!Frame.Interval(10, idx++)) {
                    return;
                }

                component.transform.position = component.Region.ClosestPoint(refPos);
            }
        }
    }
}