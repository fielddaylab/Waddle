using FieldDay.Systems;
using UnityEngine;

namespace Waddle {
    public class PenguinFeetSnapSystem : ComponentSystemBehaviour<PenguinFeetSnapping> {
        public override void ProcessWorkForComponent(PenguinFeetSnapping component, float deltaTime) {
            if (component.SolidLayers == 0) {
                return;
            }

            Vector3 testPos = component.RootTransform.position;
            testPos.y += component.RaycastDistance;

            RaycastHit hit;
            if (Physics.Raycast(testPos, Vector3.down, out hit, Mathf.Infinity, component.SolidLayers, QueryTriggerInteraction.Ignore)) {
                testPos.y = hit.point.y + component.RootYOffset;
                component.RootTransform.position = testPos;
            }
        }
    }
}