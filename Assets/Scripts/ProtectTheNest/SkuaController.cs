//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System;
using System.Collections.Generic;
using BeauRoutine;
using UnityEngine;
using Waddle;

public class SkuaController : MonoBehaviour, ISlapInteract
{
	ISkuaState _walkState, _hitState, _eatState, _idleState, _removeState, _grabState, _flyState;
	
	SkuaContext _skuaStateContext;
	
	[SerializeField] Animator _animController;
    [SerializeField] Rigidbody _rigidbody;

    [NonSerialized]
	SkuaSpot _currentSpot;
	
	public SkuaSpot CurrentSpot => _currentSpot;
	
	[SerializeField]
	float _moveFrequency = 0.70588235f;
	
	public float MoveFrequency => _moveFrequency;
	
	Egg _theEgg;
	
	bool _wasUp = false;
	
	public bool WasUp => _wasUp;
	
	public Egg GetEgg => _theEgg;
	
	private SkuaSpot _centerSpot = null;
	
	public SkuaSpot CenterSpot => _centerSpot;
	
	private SkuaWalkState.WalkDirection _walkDir;
	
	public SkuaWalkState.WalkDirection WalkDir => _walkDir;

    public SkinnedMeshRenderer Renderer;
    public Material FlashMaterial;

    public AudioSource Sounds;
    public SFXAsset HitSound;
    public SFXAsset MoveSound;
    public SFXAsset ApproachSound;
	
	[NonSerialized] GameObject _mainCamera;
    [NonSerialized] Material _defaultMaterial;

    private Routine m_FlashRoutine;

    // Start is called before the first frame update
    void Start()
    {
		_skuaStateContext = new SkuaContext(this);
		
		_walkState = gameObject.AddComponent<SkuaWalkState>();
		_idleState = gameObject.AddComponent<SkuaIdleState>();
		_eatState = gameObject.AddComponent<SkuaEatState>();
		_hitState = gameObject.AddComponent<SkuaHitState>();
		_removeState = gameObject.AddComponent<SkuaRemoveState>();
		_grabState = gameObject.AddComponent<SkuaGrabState>();
		_flyState = gameObject.AddComponent<SkuaFlyState>();
		
		_skuaStateContext.Transition(_idleState);
		
		_mainCamera = Camera.main.gameObject;
        _defaultMaterial = Renderer.sharedMaterial;
    }

    public Animator GetAnimController() {
        if (_animController == null) {
            _animController = transform.GetChild(0).GetComponent<Animator>();
        }

        return _animController;
    }

    public Rigidbody GetRigidbody() {
        if (_rigidbody == null) {
            _rigidbody = GetComponent<Rigidbody>();
        }
        return _rigidbody;
    }

    #region Setters

    public void SetNewSpot(SkuaSpot newSpot)
	{
		
		if(_currentSpot != null)
		{
			_wasUp = _currentSpot.IsUp;
			_currentSpot.CurrentSkua = null;
		}
		
		_currentSpot = newSpot;
		
		/*if(_centerSpot == null)
		{
			GameObject parentObject = _currentSpot.gameObject.transform.parent.gameObject;
			SkuaSpot[] spots = parentObject.GetComponentsInChildren<SkuaSpot>();
			_centerSpot = spots[0];
		}*/
		
		_currentSpot.CurrentSkua = gameObject;
	}
	
	public void SetEggRef(Egg theEgg)
	{
		_theEgg = theEgg;
	}

    #endregion // Setters

    #region State Transitions

    public void WalkToSpot(SkuaWalkState.WalkDirection eDir)
	{
		_walkDir = eDir;
		
		_skuaStateContext.Transition(_walkState);
	}
	
	public void FlyToSpot(SkuaWalkState.WalkDirection eDir)
	{
		_walkDir = eDir;
		
		_skuaStateContext.Transition(_flyState);
	}
	
	public void GoIdle()
	{
		_skuaStateContext.Transition(_idleState);
	}
	
	public void GrabEgg()
	{
		_skuaStateContext.Transition(_grabState);
	}
	
	public void Eat()
	{
		_skuaStateContext.Transition(_eatState);
	}
	
	public void SkuaHit()
	{
		_skuaStateContext.Transition(_hitState);
	}
	
	public void SkuaRemove()
	{
		_skuaStateContext.Transition(_removeState);
	}

    #endregion // State Transitions

    #region Queries

    public bool InHitState() {
        return (_skuaStateContext.CurrentState == _hitState);
    }

    public SkuaSpot SearchForOuterSpot()
	{
		//currently assuming this will only be called from a skua on the center after grabbing an egg...
		SkuaSpot newSpot = null;
		
		GameObject parentObject = _currentSpot.gameObject.transform.parent.gameObject;
		SkuaSpot[] spots = parentObject.GetComponentsInChildren<SkuaSpot>();
		List<SkuaSpot> outerSpots = new List<SkuaSpot>();
		foreach(SkuaSpot s in spots)
		{
			if(s.IsOuter && !s.IsBlocked && s.CurrentSkua == null)
			{
				outerSpots.Add(s);
			}
		}
		
		if(outerSpots.Count > 0)
		{	
			int randomIndex = UnityEngine.Random.Range(0, outerSpots.Count-1);
			newSpot = outerSpots[randomIndex];
		}
		
		return newSpot;
	}
	
	public SkuaWalkState.WalkDirection WhichDirection(SkuaSpot potentialSpot)
	{
		if(potentialSpot == _currentSpot.SpotOut)
		{
			return SkuaWalkState.WalkDirection.eBACK;
		}
		else if(potentialSpot == _currentSpot.SpotIn)
		{
			return SkuaWalkState.WalkDirection.eFORWARD;
		}
		else if(potentialSpot == _currentSpot.SpotLeft)
		{
			return SkuaWalkState.WalkDirection.eLEFT;
		}
		else if(potentialSpot == _currentSpot.SpotRight)
		{
			return SkuaWalkState.WalkDirection.eRIGHT;
		}
		
		return SkuaWalkState.WalkDirection.eSTAY;
	}
	
	public SkuaSpot SearchForNewSpot()
	{
		SkuaSpot newSpot = null;
		
		while(newSpot == null)
		{
			float r = UnityEngine.Random.value;
			if(r < 0.1)
			{
				if(_currentSpot != null)
				{
					if(_currentSpot.SpotOut != null && _currentSpot.SpotOut.CurrentSkua == null)
					{
						newSpot = _currentSpot.SpotOut;
					}
				}
			}
			else if (r >= 0.1 && r < 0.2)
			{
				//stay in same place...
				newSpot = _currentSpot;
			}
			else if(r >= 0.2 && r < 0.5)
			{
				//move left
				if(_currentSpot != null)
				{
					if(_currentSpot.SpotLeft != null && _currentSpot.SpotLeft.CurrentSkua == null)
					{
						newSpot = _currentSpot.SpotLeft;
					}
				}
			}
			else if( r >= 0.5 && r < 0.8)
			{
				//move right
				if(_currentSpot != null)
				{
					if(_currentSpot.SpotRight != null && _currentSpot.SpotRight.CurrentSkua == null)
					{
						newSpot = _currentSpot.SpotRight;
					}
				}
			}
			else
			{
				//move in
				if(_currentSpot != null)
				{
					if(_currentSpot.SpotIn != null && _currentSpot.SpotIn.CurrentSkua == null)
					{
						newSpot = _currentSpot.SpotIn;
					}
				}
			}
		}
		
		return newSpot;
	}

    #endregion // Queries

    #region Handlers

    public void OnSlapInteract(PlayerHeadState state, SlapTrigger trigger, Collider slappedCollider, Vector3 slapVelocity, Collision collisionInfo) {
        if (_currentSpot != null && (_currentSpot.IsInner || _theEgg != null)) {
            if (_theEgg != null) {
                _theEgg.Reset();
                _theEgg = null;
            }

            if (InHitState()) {
                return;
            }

            //Vector3 toSkua = Vector3.Normalize(transform.position - _mainCamera.transform.position);
            //Vector3 lookDir = _mainCamera.transform.forward;
            //if(Vector3.Dot(toSkua, lookDir) > 0.5f)
            {
                SkuaHit();
                trigger.PlayHaptics();
            }
        }
    }

    #endregion // Handlers
}
