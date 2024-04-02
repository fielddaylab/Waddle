//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using BeauUtil.Debugger;
using FieldDay;
using UnityEngine;
using Waddle;

public class BodyTrigger : MonoBehaviour
{
    public Collider Collider;

    public CollisionCooldowns TempIgnored = new CollisionCooldowns(8);

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

        IBodyInteract interact = FindInteractForCollider(otherCollider, out GameObject go);
        if (interact != null) {
            if (TempIgnored.Contains(go) || TempIgnored.Contains((MonoBehaviour) interact)) {
                return;
            }

             interact.OnBodyEnter(this, otherCollider);
        }
	}
	
	void OnTriggerStay(Collider otherCollider) {
		
	    IBodyInteract interact = FindInteractForCollider(otherCollider, out GameObject go);
        if (interact != null) {
            interact.OnBodyStay(this, otherCollider);
        }	
	}
	
	void OnTriggerExit(Collider otherCollider) {
		
	    IBodyInteract interact = FindInteractForCollider(otherCollider, out GameObject go);
        if (interact != null) {
            interact.OnBodyExit(this, otherCollider);
        }	
	}

    void OnCollisionEnter(Collision collision) {
        Collider otherCollider = collision.collider;
        if (TempIgnored.Contains(otherCollider)) {
            return;
        }

        IBodyInteract interact = FindInteractForCollider(otherCollider, out GameObject go);
        if (interact != null) {
            if (TempIgnored.Contains(go) || TempIgnored.Contains((MonoBehaviour) interact)) {
                return;
            }

            interact.OnBodyEnter(this, otherCollider);
        }
    }
	
	void OnCollisionStay(Collision collision) {
		Collider otherCollider = collision.collider;
	    IBodyInteract interact = FindInteractForCollider(otherCollider, out GameObject go);
        if (interact != null) {

            interact.OnBodyStay(this, otherCollider);
        }	
	}
	
	void OnCollisionExit(Collision collision) {
		Collider otherCollider = collision.collider;
	    IBodyInteract interact = FindInteractForCollider(otherCollider, out GameObject go);
        if (interact != null) {

            interact.OnBodyExit(this, otherCollider);
        }	
	}
	
    private void LateUpdate() {
        TempIgnored.Update(Frame.DeltaTime);
    }

    static public IBodyInteract FindInteractForCollider(Collider collider, out GameObject go) {
        IBodyInteract interact = collider.GetComponent<IBodyInteract>();
        go = collider.gameObject;
        if (interact == null && collider.attachedRigidbody) {
            interact = collider.attachedRigidbody.GetComponent<IBodyInteract>();
            go = collider.attachedRigidbody.gameObject;
        }
        return interact;
    }

}