//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using BeauRoutine;
using FieldDay.Processes;
using UnityEngine;
using Waddle;

public class SkuaGrabState : SkuaStateBase, IProcessStateSequence {
    public struct Args {
        public RuntimeObjectHandle NextSpot;
        public RuntimeObjectHandle Egg;
        public float TransitionDuration;
    }

    public override void Handle(Process process, SkuaController sc)
	{
	}

    public IEnumerator Sequence(Process process) {
        SkuaController sc = Brain(process);
        Args args = process.Data<Args>();
        SkuaSpot newSpot = args.NextSpot.Cast<SkuaSpot>();

        sc.TargetSpot = args.NextSpot.Cast<SkuaSpot>();
        sc.Spawner.SetPendingOccupancy(sc.TargetSpot, true);
        sc.AssignToSpot(null);

        Animator a = sc.AnimController;
        a.SetBool("idle", false);
        a.SetBool("grab", true);

        yield return MoveToPosition(sc, newSpot.CachedPosition, args.TransitionDuration, Curve.QuadOut);

        sc.AssignToSpot(sc.TargetSpot);

        Egg egg = args.Egg.Cast<Egg>();
        egg.IsTaken = true;
        sc.SetEggRef(egg);

        a.SetBool("grab", false);
        a.SetBool("flyegg", true);
    }
}
