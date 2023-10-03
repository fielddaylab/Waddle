using System.Collections;
using System.Collections.Generic;
using FieldDay.Processes;
using UnityEngine;
using Waddle;

public class SkuaEatState : SkuaStateBase {
	public override void Handle(Process process, SkuaController sc)
	{
		Animator a = sc.AnimController;
		if(!a.GetCurrentAnimatorStateInfo(0).IsName("eat"))
		{
			sc.CachedTransform.rotation = sc.CurrentSpot.CachedTransform.rotation * SkuaController.RotationAdjust;
            sc.HoldingEgg.transform.localPosition = Vector3.zero;
			sc.HoldingEgg.transform.SetParent(sc.CachedTransform.GetChild(1).transform, false);
		}
	}
}
