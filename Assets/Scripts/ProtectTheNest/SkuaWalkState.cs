//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeauRoutine.Splines;
using BeauRoutine;
using Waddle;
using FieldDay.Processes;
using FieldDay;

public class SkuaWalkState : SkuaStateBase, IProcessStateSequence {
    public struct Args {
        public RuntimeObjectHandle NextSpot;
        public SkuaMovementDirection Direction;
        public float TransitionDuration;
        public float InitialDelay;
    }

    public override void Handle(Process process, SkuaController sc) {
        
    }

    public IEnumerator Sequence(Process process) {
        Args args = process.Data<Args>();
        if (args.InitialDelay > 0) {
            yield return args.InitialDelay;
        }

        SkuaController sc = Brain(process);
        sc.CachedTransform.GetPositionAndRotation(out Vector3 startPos, out Quaternion startRot);
        
        SkuaSpot newSpot = args.NextSpot.Cast<SkuaSpot>();
        sc.AssignToSpot(null);
        sc.TargetSpot = newSpot;
        sc.Spawner.SetPendingOccupancy(newSpot, true);

        Animator a = sc.AnimController;
        a.SetBool("right", args.Direction == SkuaMovementDirection.RIGHT);
        a.SetBool("left", args.Direction == SkuaMovementDirection.LEFT);
        a.SetBool("back", args.Direction == SkuaMovementDirection.BACK);
        a.SetBool("forward", args.Direction == SkuaMovementDirection.FORWARD);
        a.SetBool("idle", false);
        a.SetBool("slapped", false);
        a.SetBool("walk", false);

        if (args.Direction == SkuaMovementDirection.FORWARD) {
            SFXUtility.Play(sc.Sounds, sc.ApproachSound);
        } else {
            SFXUtility.Play(sc.Sounds, sc.MoveSound);
        }

        Quaternion rotNewSpot = newSpot.CachedRotation * SkuaController.RotationAdjust;
        yield return HopToPosition(sc, newSpot.CachedPosition, new Vector3(0, newSpot.IsUp ? 1 : 0.5f, 0), rotNewSpot, args.TransitionDuration, Curve.Smooth);

        PenguinAnalytics.Instance.LogSkuaMove(sc.gameObject.name, startPos, newSpot.CachedPosition);
        sc.GoIdle();
        sc.AssignToSpot(newSpot);
    }
}
