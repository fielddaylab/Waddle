//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using FieldDay;
using UnityEngine;
using Waddle;

public class SlapTrigger : MonoBehaviour
{
    public SlapFlipper Flipper;

    void OnTriggerEnter(Collider otherCollider) {
        ISlapInteract interact = FindInteractForCollider(otherCollider);
        if (interact != null) {
            interact.OnSlapInteract(Game.SharedState.Get<PlayerHeadState>(), this, otherCollider, null);
        }
	}

    void OnCollisionEnter(Collision collision) {
        ISlapInteract interact = FindInteractForCollider(collision.collider);
        if (interact != null) {
            interact.OnSlapInteract(Game.SharedState.Get<PlayerHeadState>(), this, collision.collider, collision);
        }
    }

    static public ISlapInteract FindInteractForCollider(Collider collider) {
        ISlapInteract interact = collider.GetComponent<ISlapInteract>();
        if (interact == null && collider.attachedRigidbody) {
            interact = collider.attachedRigidbody.GetComponent<ISlapInteract>();
        }
        return interact;
    }
}

public enum SlapFlipper {
    Left,
    Right
}
