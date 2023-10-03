//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using FieldDay;
using UnityEngine;
using Waddle;

public class BeakTrigger : MonoBehaviour
{
    public CollisionCooldowns TempIgnored = new CollisionCooldowns(8);

    public void SetCooldown(Collider collider, float duration) {
        TempIgnored.Add(collider, duration);
    }

    public void SetCooldown(GameObject go, float duration) {
        TempIgnored.Add(go, duration);
    }

    public void SetCooldown(IBeakInteract interact, float duration) {
        TempIgnored.Add((MonoBehaviour) interact, duration);
    }

    void OnTriggerEnter(Collider otherCollider)
	{
        if (TempIgnored.Contains(otherCollider)) {
            return;
        }

        IBeakInteract interact = FindInteractForCollider(otherCollider, out GameObject go);
        if (interact != null) {
            if (TempIgnored.Contains(go) || TempIgnored.Contains((MonoBehaviour) interact)) {
                return;
            }

            interact.OnBeakInteract(Game.SharedState.Get<PlayerBeakState>(), this, otherCollider);
        }
	}

    private void LateUpdate() {
        TempIgnored.Update(Frame.DeltaTime);
    }

    static public IBeakInteract FindInteractForCollider(Collider collider, out GameObject go) {
        IBeakInteract interact = collider.GetComponent<IBeakInteract>();
        go = collider.gameObject;
        if (interact == null && collider.attachedRigidbody) {
            interact = collider.attachedRigidbody.GetComponent<IBeakInteract>();
            go = collider.attachedRigidbody.gameObject;
        }
        return interact;
    }

}
