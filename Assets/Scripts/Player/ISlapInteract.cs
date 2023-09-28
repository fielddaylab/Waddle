using UnityEngine;

namespace Waddle {
    public interface ISlapInteract {
        void OnSlapInteract(PlayerHeadState state, SlapTrigger trigger, Collider slappedCollider, Collision collisionInfo);
    }
}