//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System;
using System.Collections;
using System.Collections.Generic;
using BeauPools;
using BeauRoutine;
using BeauRoutine.Splines;
using FieldDay.Processes;
using UnityEngine;
using Waddle;

public class SkuaBrain : ProcessBehaviour, IPoolAllocHandler {
    [Flags]
    public enum AnimId {
        Forward,
        Back,
        Left,
        Right,
        Idle,
        Walk,
        Slapped,
        Break,
        Grab,
        FlyEgg
    }

    public Animator Animator;
    public Rigidbody Rigidbody;
    public AudioSource Sounds;

    [Header("Resources")]
    public SFXAsset HitSound;
    public SFXAsset HopSound;
    public SFXAsset SpawnSound;

    [NonSerialized] public SkuaSpot Spot;

    #region IPoolAllocHandler

    void IPoolAllocHandler.OnAlloc() {
    }

    void IPoolAllocHandler.OnFree() {
        if (Spot != null) {
            Spot.Occupant = null;
            Spot = null;
        }
    }

    #endregion // IPoolAllocHandler
}

namespace Waddle.Skua {
    public abstract class SkuaState : IProcessStateEnterExit {
        public virtual void OnEnter(Process process) { }
        public virtual void OnExit(Process process) { }

        static protected SkuaBrain Brain(Process process) {
            return process.Context<SkuaBrain>();
        }

        static protected void SetAnimation(SkuaBrain brain, SkuaBrain.AnimId anim) {
            brain.Animator.SetBool("forward", anim == SkuaBrain.AnimId.Forward);
            brain.Animator.SetBool("back", anim == SkuaBrain.AnimId.Back);
            brain.Animator.SetBool("left", anim == SkuaBrain.AnimId.Left);
            brain.Animator.SetBool("right", anim == SkuaBrain.AnimId.Right);
            brain.Animator.SetBool("idle", anim == SkuaBrain.AnimId.Idle);
            brain.Animator.SetBool("walk", anim == SkuaBrain.AnimId.Walk);
            brain.Animator.SetBool("slapped", anim == SkuaBrain.AnimId.Slapped);
            brain.Animator.SetBool("break", anim == SkuaBrain.AnimId.Break);
            brain.Animator.SetBool("grab", anim == SkuaBrain.AnimId.Grab);
            brain.Animator.SetBool("flyegg", anim == SkuaBrain.AnimId.FlyEgg);
        }

        static protected void SetRigidbodyPhysics(SkuaBrain brain, bool enabled) {
            brain.Rigidbody.useGravity = enabled;
            brain.Rigidbody.isKinematic = !enabled;
        }
    }

    public class SkuaSpawnState : SkuaState {

    }

    public class SkuaHopState : SkuaState, IProcessStateSequence {
        public override void OnEnter(Process process) {
            var brain = Brain(process);
            SetAnimation(brain, process.Data<Args>().Anim);
            SetRigidbodyPhysics(brain, false);
        }

        public IEnumerator Sequence(Process process) {
            var brain = Brain(process);
            SkuaSpot spot = process.Data<Args>().SpotTarget.Cast<SkuaSpot>();
            SimpleSpline spline = Spline.Simple(brain.transform.position, spot.transform.position, 0.5f, spot.IsUp ? new Vector3(0, 2, 0) : new Vector3(0, 1, 0));
            SFXUtility.Play(brain.Sounds, brain.HopSound);
            yield return Routine.Combine(
                brain.transform.MoveAlong(spline, 1),
                brain.transform.RotateQuaternionTo(spot.transform.rotation, 1)
            );
            process.TransitionTo(SkuaStates.Idle);
        }

        public struct Args {
            public RuntimeObjectHandle SpotTarget;
            public SkuaBrain.AnimId Anim;
        }
    }

    public class SkuaHitState : SkuaState, IProcessStateSequence {

        public override void OnEnter(Process process) {
            var brain = Brain(process);
            SetAnimation(brain, SkuaBrain.AnimId.Slapped);
            PenguinAnalytics.Instance.LogFlipperBash(brain.gameObject.name, false);
            brain.Rigidbody.AddForceAtPosition(process.Data<Args>().Force, process.Data<Args>().ForcePos);
            SFXUtility.Play(brain.Sounds, brain.HitSound);
        }

        public IEnumerator Sequence(Process process) {
            var brain = Brain(process);
            SetRigidbodyPhysics(brain, false);
            yield return 2;
        }

        public struct Args {
            public Vector3 Force;
            public Vector3 ForcePos;
        }
    }

    public class SkuaIdleState : SkuaState {
        public override void OnEnter(Process process) {
            var brain = Brain(process);
            SetAnimation(brain, SkuaBrain.AnimId.Idle);
            SetRigidbodyPhysics(brain, false);
        }
    }

    static public class SkuaStates {
        static public readonly ProcessStateDefinition Idle = ProcessStateDefinition.FromCallbacks("idle", new SkuaIdleState());
        static public readonly ProcessStateDefinition Hop = ProcessStateDefinition.FromCallbacks("hop", new SkuaHopState());
        static public readonly ProcessStateDefinition Hit = ProcessStateDefinition.FromCallbacks("hit", new SkuaIdleState());
        static public readonly ProcessStateDefinition Spawn = ProcessStateDefinition.FromCallbacks("spawn", new SkuaSpawnState());
    }
}