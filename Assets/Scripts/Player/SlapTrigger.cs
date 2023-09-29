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
    public Rigidbody RB;
    public Collider Collider;

    public void PlayHaptics() {
        if (PenguinPlayer.SlapHaptics.Ready) {
            if (Flipper == SlapFlipper.Right) {
                OVRHaptics.RightChannel?.Mix(PenguinPlayer.SlapHaptics);
            } else {
                OVRHaptics.LeftChannel?.Mix(PenguinPlayer.SlapHaptics);
            }
        }
    }

    void OnTriggerEnter(Collider otherCollider) {
        ISlapInteract interact = FindInteractForCollider(otherCollider);
        if (interact != null) {
            Vector3 contactPointEstimate = (otherCollider.ClosestPoint(RB.position) + Collider.ClosestPoint(otherCollider.transform.position)) / 2;
            Vector3 velocity = RB.GetPointVelocity(contactPointEstimate);
            interact.OnSlapInteract(Game.SharedState.Get<PlayerHeadState>(), this, otherCollider, velocity, null);
        }
	}

    void OnCollisionEnter(Collision collision) {
        ISlapInteract interact = FindInteractForCollider(collision.collider);
        if (interact != null) {
            Vector3 velocity = RB.GetPointVelocity(collision.GetContact(0).point);
            interact.OnSlapInteract(Game.SharedState.Get<PlayerHeadState>(), this, collision.collider, velocity, collision);
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
