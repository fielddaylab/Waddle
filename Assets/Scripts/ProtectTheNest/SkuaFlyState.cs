//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using BeauRoutine;
using FieldDay.Processes;
using UnityEngine;
using Waddle;

public class SkuaFlyState : SkuaStateBase, IProcessStateSequence {
    public struct Args {
        public RuntimeObjectHandle NextSpot;
    }
	
	public override void Handle(Process process, SkuaController sc)
	{
	}

    public IEnumerator Sequence(Process process) {
        SkuaController sc = Brain(process);
        Args args = process.Data<Args>();
        SkuaSpot nextSpot = args.NextSpot.Cast<SkuaSpot>();

        Vector3 targetPos = nextSpot.CachedPosition;
        Quaternion targetRot = GetRotationToLook(sc, nextSpot);

        sc.TargetSpot = nextSpot;
        sc.Spawner.SetPendingOccupancy(sc.TargetSpot, true);
        sc.AssignToSpot(null);

        Animator a = sc.AnimController;
        a.SetBool("grab", false);
        a.SetBool("flyegg", true);

        yield return MoveRotateToPosition(sc, targetPos, targetRot, 5, Curve.Smooth);

        sc.AssignToSpot(nextSpot);
        a.SetBool("flyegg", false);
    }
}
