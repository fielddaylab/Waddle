//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using FieldDay;
using UnityEngine;
using Waddle;

public class BeakTrigger : MonoBehaviour
{		
	void OnTriggerEnter(Collider otherCollider)
	{
        IBeakInteract interact = FindInteractForCollider(otherCollider);
        if (interact != null) {
            interact.OnBeakInteract(Game.SharedState.Get<PlayerBeakState>(), this, otherCollider);
        }
	}

    static public IBeakInteract FindInteractForCollider(Collider collider) {
        IBeakInteract interact = collider.GetComponent<IBeakInteract>();
        if (interact == null && collider.attachedRigidbody) {
            interact = collider.attachedRigidbody.GetComponent<IBeakInteract>();
        }
        return interact;
    }

}
