using UnityEngine;

namespace Waddle {
    public interface IBeakInteract {
        void OnBeakInteract(PlayerBeakState state, BeakTrigger trigger, Collider collider);
    }
}