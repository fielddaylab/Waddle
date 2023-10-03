using System.Collections;
using System.Collections.Generic;
using FieldDay.Processes;
using UnityEngine;

public class SkuaIdleState : SkuaStateBase {
	
    public override void Handle(Process process, SkuaController sc)
	{
		Animator a = sc.AnimController;
		if(a != null)
		{
			a.SetBool("forward", false);
			a.SetBool("back", false);
			a.SetBool("left", false);
			a.SetBool("right", false);
			a.SetBool("idle", true);
			a.SetBool("walk", false);
		}

        sc.SetPhysics(false);
	}
}
