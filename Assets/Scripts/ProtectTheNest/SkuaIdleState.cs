using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkuaIdleState : MonoBehaviour, ISkuaState
{
	private SkuaController _sc;
	
	public void Handle(SkuaController sc)
	{
		if(_sc == null)
		{
			_sc = sc;
		}
		
		Animator a = sc.GetAnimController();
		if(a != null)
		{
			a.SetBool("forward", false);
			a.SetBool("back", false);
			a.SetBool("left", false);
			a.SetBool("right", false);
			a.SetBool("idle", true);
			a.SetBool("walk", false);
			
			gameObject.GetComponent<Rigidbody>().useGravity = false;
			gameObject.GetComponent<Rigidbody>().isKinematic = true;

		}

		//StartCoroutine(CheckForMove(_sc.MoveFrequency));
	}
}
