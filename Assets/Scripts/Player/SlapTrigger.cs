//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using BeauUtil.Debugger;
using FieldDay;
using UnityEngine;
using Waddle;

public class SlapTrigger : MonoBehaviour
{
    public SlapFlipper Flipper;
    public Rigidbody RB;
    public Collider Collider;

    public CollisionCooldowns TempIgnored = new CollisionCooldowns(8);

    public void PlayHaptics() {
        if (PenguinPlayer.SlapHaptics.Ready) {
            if (Flipper == SlapFlipper.Right) {
                OVRHaptics.RightChannel?.Mix(PenguinPlayer.SlapHaptics);
            } else {
                OVRHaptics.LeftChannel?.Mix(PenguinPlayer.SlapHaptics);
            }
        }
    }

    public void SetCooldown(Collider collider, float duration) {
        TempIgnored.Add(collider, duration);
    }

    public void SetCooldown(GameObject go, float duration) {
        TempIgnored.Add(go, duration);
    }

    public void SetCooldown(ISlapInteract interact, float duration) {
        TempIgnored.Add((MonoBehaviour) interact, duration);
    }

    void OnTriggerEnter(Collider otherCollider) {
        if (TempIgnored.Contains(otherCollider)) {
            return;
        }

        ISlapInteract interact = FindInteractForCollider(otherCollider, out GameObject go);
        if (interact != null) {
            if (TempIgnored.Contains(go) || TempIgnored.Contains((MonoBehaviour) interact)) {
                return;
            }

            Vector3 contactPointEstimate = (otherCollider.ClosestPoint(RB.position) + Collider.ClosestPoint(otherCollider.transform.position)) / 2;
            Vector3 velocity = RB.GetPointVelocity(contactPointEstimate);
            velocity = StrongestVelocity(RB.velocity, velocity);
            Log.Msg("[SlapTrigger] slap velocity {0} -> {1}", velocity, velocity.magnitude);
            interact.OnSlapInteract(Game.SharedState.Get<PlayerHeadState>(), this, otherCollider, velocity, null);
        }
	}

    void OnCollisionEnter(Collision collision) {
        Collider otherCollider = collision.collider;
        if (TempIgnored.Contains(otherCollider)) {
            return;
        }

        ISlapInteract interact = FindInteractForCollider(otherCollider, out GameObject go);
        if (interact != null) {
            if (TempIgnored.Contains(go) || TempIgnored.Contains((MonoBehaviour) interact)) {
                return;
            }

            Vector3 velocity = RB.GetPointVelocity(collision.GetContact(0).point);
            velocity = StrongestVelocity(RB.velocity, velocity);
            Log.Msg("[SlapTrigger] slap velocity {0} -> {1}", velocity, velocity.magnitude);
            interact.OnSlapInteract(Game.SharedState.Get<PlayerHeadState>(), this, collision.collider, velocity, collision);
        }
    }

    private void LateUpdate() {
        TempIgnored.Update(Frame.DeltaTime);
    }

    static public ISlapInteract FindInteractForCollider(Collider collider, out GameObject go) {
        ISlapInteract interact = collider.GetComponent<ISlapInteract>();
        go = collider.gameObject;
        if (interact == null && collider.attachedRigidbody) {
            interact = collider.attachedRigidbody.GetComponent<ISlapInteract>();
            go = collider.attachedRigidbody.gameObject;
        }
        return interact;
    }

    static private Vector3 StrongestVelocity(Vector3 a, Vector3 b) {
        Vector3 c;
        c.x = Mathf.Abs(a.x) > Mathf.Abs(b.x) ? a.x : b.x;
        c.y = Mathf.Abs(a.y) > Mathf.Abs(b.y) ? a.y : b.y;
        c.z = Mathf.Abs(a.z) > Mathf.Abs(b.z) ? a.z : b.z;
        return c;
    }
}

public enum SlapFlipper {
    Left,
    Right
}
