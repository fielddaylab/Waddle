using FieldDay.Components;
using UnityEngine;

namespace Waddle {
    public class PenguinFeetSnapping : BatchedComponent {
        public Transform RootTransform;
        public float RootYOffset;

        [Header("Raycast")]
        public float RaycastDistance = 10;
        public LayerMask SolidLayers;
        public LayerMask WallLayers;
        [Range(0, 1)] public float AngleStrictness = 0.5f;
        public float CheckRadius;
    }

    static public class PenguinFeetUtility {
        static public bool IsSolidGround(PenguinFeetSnapping snapping, Vector3 newPos) {
            return IsSolidGround(snapping, newPos, out Vector3 _);
        }

        static public bool IsSolidGround(PenguinFeetSnapping snapping, Vector3 newPos, out Vector3 groundNormal) {
            newPos.y += snapping.RaycastDistance;

            if (Physics.Raycast(newPos, Vector3.down, out RaycastHit hit, Mathf.Infinity, snapping.SolidLayers, QueryTriggerInteraction.Ignore)) {
                groundNormal = hit.normal;
                return groundNormal.y >= snapping.AngleStrictness && !Physics.CheckSphere(hit.point, snapping.CheckRadius, snapping.WallLayers);
            }

            groundNormal = Vector3.up;
            return false;
        }
    }
}