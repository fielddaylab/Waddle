//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullController : MonoBehaviour
{
	ISkuaState _walkState, _hitState, _eatState, _idleState, _removeState;
	
	SkuaContext _skuaStateContext;
	
	Animator _animController;
	
	[SerializeField]
	SkuaSpot _currentSpot;
	
	public SkuaSpot CurrentSpot => _currentSpot;
	
	[SerializeField]
	float _moveFrequency = 0.70588235f;
	
	public float MoveFrequency => _moveFrequency;
	
	Egg _theEgg;
	
	public Egg GetEgg => _theEgg;
	
    // Start is called before the first frame update
    void Start()
    {
		_skuaStateContext = new SkuaContext(this);
		
		_walkState = gameObject.AddComponent<SkuaWalkState>();
		_idleState = gameObject.AddComponent<SkuaIdleState>();
		_eatState = gameObject.AddComponent<SkuaEatState>();
		_hitState = gameObject.AddComponent<SkuaHitState>();
		_removeState = gameObject.AddComponent<SkuaRemoveState>();
		
		_skuaStateContext.Transition(_idleState);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void SetNewSpot(SkuaSpot newSpot)
	{
		if(_currentSpot != null)
		{
			_currentSpot.CurrentSkua = null;
		}
		
		_currentSpot = newSpot;
		
		_currentSpot.CurrentSkua = gameObject;
	}
	
	public void SetEggRef(Egg theEgg)
	{
		_theEgg = theEgg;
	}
	
	public Animator GetAnimController() 
	{
		if(_animController == null)
		{
			_animController = GetComponent<Animator>();
		}
		
		return _animController; 
	}
	
	public bool InHitState()
	{
		return (_skuaStateContext.CurrentState == _hitState);
	}
	
	public void WalkToSpot()
	{
		_skuaStateContext.Transition(_walkState);
	}
	
	public void GoIdle()
	{
		_skuaStateContext.Transition(_idleState);
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
	
	public SkuaSpot SearchForNewSpot()
	{
		SkuaSpot newSpot = null;
		
		while(newSpot == null)
		{
			float r = Random.value;
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
	
	void OnCollisionEnter(Collision otherCollision)
	{
		//Debug.Log("Skua collided");
		//Debug.Log("Impulse: " + otherCollision.impulse);
		//Debug.Log("Relative Velocity: " + otherCollision.relativeVelocity);
		if(otherCollision.gameObject.name.StartsWith("Flipper"))
		{
			if(_currentSpot != null && (_currentSpot.IsInner || _theEgg != null))
			{
				if(_theEgg != null)
				{
					_theEgg.Reset();
					_theEgg = null;
				}
				
				SkuaHit();
			}
		}
	}
}
