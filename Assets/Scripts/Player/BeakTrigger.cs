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
        IBeakInteract interact = otherCollider.GetComponent<IBeakInteract>();
        if (interact != null) {
            interact.OnBeakInteract(Game.SharedState.Get<PlayerBeakState>(), this);
        }
	}

}
