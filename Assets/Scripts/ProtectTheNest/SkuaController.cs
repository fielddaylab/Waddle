//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System;
using System.Collections.Generic;
using BeauRoutine;
using BeauUtil;
using BeauUtil.Debugger;
using FieldDay.Processes;
using UnityEngine;
using Waddle;

public class SkuaController : ProcessBehaviour, ISlapInteract
{
    #region Inspector

    public Animator AnimController;
    public Rigidbody Rigidbody;

    public SkinnedMeshRenderer Renderer;
    public Material FlashMaterial;

    public AudioSource Sounds;
    public SFXAsset HitSound;
    public SFXAsset MoveSound;
    public SFXAsset ApproachSound;

    #endregion // Inspector

    [NonSerialized] public Transform CachedTransform;
    [NonSerialized] public SkuaSpawner Spawner;
    [NonSerialized] public SkuaSpot CurrentSpot;
	[NonSerialized] public Egg HoldingEgg;
    [NonSerialized] public SkuaSpot TargetSpot;

    #region Unity Events

    private void Awake() {
        this.CacheComponent(ref CachedTransform);
    }

    protected override void Start() {
        MainProcess.Process.OnStateExit += (p, d) => {
            if (TargetSpot && Spawner) {
                Spawner.SetPendingOccupancy(TargetSpot, false);
                TargetSpot = null;
            }
        };
    }

    #endregion // Unity Events

    #region Setters

    public void AssignToSpot(SkuaSpot newSpot) {
        Spawner.AssignToSpot(this, newSpot);
	}
	
	public void SetEggRef(Egg theEgg) {
		HoldingEgg = theEgg;
	}

    #endregion // Setters

    #region State Transitions

    public void WalkToSpot(SkuaSpot spot, SkuaMovementDirection dir, float delay)
	{
        m_MainProcess.TransitionTo(SkuaStates.Walk, new SkuaWalkState.Args() {
            Direction = dir,
            NextSpot = spot,
            InitialDelay = delay,
            TransitionDuration = 0.8f
        });
	}
	
	public void FlyToSpot(SkuaSpot spot)
	{
        m_MainProcess.TransitionTo(SkuaStates.Fly, new SkuaFlyState.Args() {
            NextSpot = spot
        });
	}
	
	public void GoIdle()
	{
        m_MainProcess.TransitionTo(SkuaStates.Idle);
	}
	
	public void GrabEgg(SkuaSpot spot, Egg egg)
	{
        m_MainProcess.TransitionTo(SkuaStates.Grab, new SkuaGrabState.Args() {
            TransitionDuration = 1,
            Egg = egg,
            NextSpot = spot
        });
    }
	
	public void Eat()
	{
        m_MainProcess.TransitionTo(SkuaStates.Eat);
	}
	
	public void SkuaHit(Vector3 hitDirection)
	{
        m_MainProcess.TransitionTo(SkuaStates.Hit, new SkuaHitState.Args() {
            HitDirection = hitDirection
        });

        if (HoldingEgg != null) {
            HoldingEgg.ResetToStart();
            HoldingEgg = null;
        }
    }
	
	public void SkuaRemove()
	{
        m_MainProcess.TransitionTo(SkuaStates.Remove);
	}

    #endregion // State Transitions

    #region Queries

    public bool InHitState() {
        return m_MainProcess.CurrentStateId == "hit";
    }

    #endregion // Queries

    #region Handlers

    public void OnSlapInteract(PlayerHeadState state, SlapTrigger trigger, Collider slappedCollider, Vector3 slapVelocity, Collision collisionInfo) {
        if (slapVelocity.magnitude < 0.5f) {
            return;
        }

        Log.Msg("attempted to hit skua!");

        if (CurrentSpot != null && !CurrentSpot.IsInner && HoldingEgg == null) {
            return;
        }

        Log.Msg("hit skua!");

        if (InHitState()) {
            return;
        }

        //Vector3 toSkua = Vector3.Normalize(transform.position - _mainCamera.transform.position);
        //Vector3 lookDir = _mainCamera.transform.forward;
        //if(Vector3.Dot(toSkua, lookDir) > 0.5f)
        {
            SkuaHit(slapVelocity);
            trigger.PlayHaptics();
            trigger.SetCooldown(slappedCollider, 2);
        }
    }

    #endregion // Handlers

    #region Configuration

    public void SetPhysics(bool enabled) {
        Rigidbody.useGravity = enabled;
        Rigidbody.isKinematic = !enabled;
    }

    #endregion // Configuration

    static public readonly Quaternion RotationAdjust = Quaternion.Euler(0, -90, 0);
}

public enum SkuaMovementDirection {
    FORWARD = 0,
    BACK,
    LEFT,
    RIGHT,
    STAY
}