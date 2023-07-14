using FieldDay.Components;
using UnityEngine;

namespace Waddle {
    public class PenguinFeetSnapping : BatchedComponent {
        public Transform RootTransform;
        public float RootYOffset;

        [Header("Raycast")]
        public float RaycastDistance = 10;
        public LayerMask SolidLayers;
    }

    static public class PenguinFeetUtility {
        static public bool IsSolidGround(PenguinFeetSnapping snapping, Vector3 newPos) {
            newPos.y += snapping.RaycastDistance;
            return Physics.Raycast(newPos, Vector3.down, Mathf.Infinity, snapping.SolidLayers, QueryTriggerInteraction.Ignore);
        }
    }
}