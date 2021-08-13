//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkuaState : MonoBehaviour
{
	[SerializeField]
	SkuaSpot _currentSpot;
	
	public SkuaSpot CurrentSpot
	{
		get { return _currentSpot; }
		set { _currentSpot = value; }
	}
	
	[SerializeField]
	bool _hasEgg = false;
	
	public bool HasEgg => _hasEgg;
	
	[SerializeField]
	bool _eatingEgg = false;
	
	[SerializeField]
	bool _isHit = false;
	
	public bool IsHit => _isHit;
	
	[SerializeField]
	bool _isMoving = false;
	
	[SerializeField]
	bool _isRecovering = false;
	
	[SerializeField]
	bool _isRecovered = true;
	
	SkuaSpawner _skuaSpawner;
	
	public SkuaSpawner Spawner
	{
		get { return _skuaSpawner; }
		set { _skuaSpawner = value; }
	}
	
	Animator _skuaController;
	
    // Start is called before the first frame update
    void Start()
    {
      
    }
	
	IEnumerator Recover(float recoverTime)
	{
		
		yield return new WaitForSeconds(recoverTime);
		
		GetComponent<Rigidbody>().useGravity = false;
		GetComponent<Rigidbody>().isKinematic = true;
		_skuaController.enabled = true;
		
		GoIdle();
		
		transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
		
		GameObject parentObject = _currentSpot.gameObject.transform.parent.gameObject;
		SkuaSpot[] spots = parentObject.GetComponentsInChildren<SkuaSpot>();
		List<SkuaSpot> outerSpots = new List<SkuaSpot>();
		float minDist = 9999f;
		
		SkuaSpot closestSpot = null;
		
		foreach(SkuaSpot s in spots)
		{
			if(s.IsOuter)
			{
				float dist = Vector3.Distance(transform.position, s.gameObject.transform.position);
				if(dist < minDist)
				{
					closestSpot = s;
					minDist = dist;
				}
				outerSpots.Add(s);
			}
		}
		
		//int randomIndex = UnityEngine.Random.Range(0, outerSpots.Count-1);
		
		_isRecovered = false;
		
		MoveToNewSpot(closestSpot);
	}
	
	void OnCollisionEnter(Collision otherCollision)
	{
		//Debug.Log("Skua collided");
		//Debug.Log("Impulse: " + otherCollision.impulse);
		//Debug.Log("Relative Velocity: " + otherCollision.relativeVelocity);
		if(otherCollision.gameObject.name.StartsWith("Flipper"))
		{
			_isHit = true;
			
			if(_hasEgg)
			{
				//reset egg to middle...
				ResetEgg();
			}
			
			GetComponent<Rigidbody>().useGravity = true;
			GetComponent<Rigidbody>().isKinematic = false;
			//GoIdle();
			_skuaController.enabled = false;
			GetComponent<AudioSource>().Play();
			GetComponent<Rigidbody>().AddForce((-transform.forward + transform.up)*5.0f);
			if(!_isRecovering)
			{
				_isRecovering = true;
		
				StartCoroutine(Recover(3f));
			}
		}
	}
	
	public void GoIdle()
	{
		if(_skuaController == null)
		{
			_skuaController = GetComponent<Animator>();
		}
		
		if(_skuaController != null)
		{
			//Debug.Log("Setting fly");
			_skuaController.SetBool("takeoff", false);
			_skuaController.SetBool("fly", false);
			_skuaController.SetBool("walkleft", false);
			_skuaController.SetBool("walkright", false);
			_skuaController.SetBool("walk", false);
			_skuaController.SetBool("idle", true);
		}
	}
	
	public void FlyIn()
	{	
		if(_skuaController == null)
		{
			_skuaController = GetComponent<Animator>();
		}
		
		if(_skuaController != null)
		{
			//Debug.Log("Setting fly");
			_skuaController.SetBool("idle", false);
			_skuaController.SetBool("takeoff", true);
			_skuaController.SetBool("fly", true);
		}
		/*else
		{
			Debug.Log("Couldn't set fly");
		}*/
	}
	
	public void Eat()
	{
		if(_skuaController == null)
		{
			_skuaController = GetComponent<Animator>();
		}
		
		if(_skuaController != null && !_skuaController.GetCurrentAnimatorStateInfo(0).IsName("eat"))
		{
			_skuaController.SetBool("idle", false);
			_skuaController.SetBool("walkleft", false);
			_skuaController.SetBool("walkright", false);
			_skuaController.SetBool("walk", false);
			_skuaController.SetBool("eat", true);
		}
	}
	
	public void WalkForward(bool bForce=false)
	{
		if(_skuaController == null)
		{
			_skuaController = GetComponent<Animator>();
		}
		
		if(_skuaController != null && (!_skuaController.GetCurrentAnimatorStateInfo(0).IsName("walk") || bForce))
		{
			_skuaController.SetBool("idle", false);
			_skuaController.SetBool("walkleft", false);
			_skuaController.SetBool("walkright", false);
			_skuaController.SetBool("walk", true);
		}
		/*else
		{
			Debug.Log("Couldn't set walk");
		}*/
	}
	
	public void WalkLeft()
	{
		if(_skuaController == null)
		{
			_skuaController = GetComponent<Animator>();
		}
		
		if(_skuaController != null)
		{
			_skuaController.SetBool("idle", false);
			_skuaController.SetBool("walk", false);
			_skuaController.SetBool("walkleft", true);
		}
	}
	
	public void WalkRight()
	{
		if(_skuaController == null)
		{
			_skuaController = GetComponent<Animator>();
		}
		
		if(_skuaController != null)
		{
			_skuaController.SetBool("idle", false);
			_skuaController.SetBool("walk", false);
			_skuaController.SetBool("walkright", true);
		}
	}
	
    // Update is called once per frame
    void Update()
    {
		//_lastPosition = transform.position;
		//_lastRotation = transform.rotation;
    }
	
	public void ResetEgg()
	{
		_hasEgg = false;
		_skuaSpawner.TheEgg.transform.SetParent(null, false);
		_skuaSpawner.TheEgg.GetComponent<Egg>().Reset();
		//move all skuas back one ring...
		
	}
	
	public void MoveSkua()
	{
		if(!_isMoving && !_isRecovering)
		{
			if(!_isHit)
			{
				if(!_hasEgg)
				{
					float r = Random.value;
					if(r < 0.1)
					{
						//move out
						if(_currentSpot != null)
						{
							if(_currentSpot.SpotOut != null && _currentSpot.SpotOut.CurrentSkua == null)
							{
								MoveToNewSpot(_currentSpot.SpotOut);
							}
						}
					}
					else if (r >= 0.1 && r < 0.2)
					{
						//stay
					}
					else if(r >= 0.2 && r < 0.5)
					{
						//move left
						if(_currentSpot != null)
						{
							if(_currentSpot.SpotLeft != null && _currentSpot.SpotLeft.CurrentSkua == null)
							{
								MoveToNewSpot(_currentSpot.SpotLeft);
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
								MoveToNewSpot(_currentSpot.SpotRight);
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
								MoveToNewSpot(_currentSpot.SpotIn);
							}
						}
					}
				}
				else
				{
					if(!_eatingEgg)
					{
						//only do this once...
						GameObject parentObject = _currentSpot.gameObject.transform.parent.gameObject;
						SkuaSpot[] spots = parentObject.GetComponentsInChildren<SkuaSpot>();
						List<SkuaSpot> outerSpots = new List<SkuaSpot>();
						foreach(SkuaSpot s in spots)
						{
							if(s.IsOuter)
							{
								outerSpots.Add(s);
							}
						}
						
						int randomIndex = UnityEngine.Random.Range(0, outerSpots.Count-1);
						
						MoveToNewSpot(outerSpots[randomIndex]);
						
						_eatingEgg = true;
					}
					else
					{
						Eat();
					}
				}
			}
		}
	}
	
	IEnumerator StartMove(Vector3 newSpot, Quaternion newRot, float duration)
	{
		_isMoving = true;
		float t = 0f;
		
		Vector3 startPosition = transform.position;

		WalkForward(!_isRecovered);
		
		bool grabbedEgg = false;
		
		if((_currentSpot.gameObject.name == "SkuaSpot0") && !_hasEgg && !_skuaSpawner.EggIsTaken() && !_isHit)
		{
			Debug.Log("Grabbing egg");
			_hasEgg = true;
			grabbedEgg = true;
		}
		
		while(t < duration)
		{
			transform.position = Vector3.Lerp(startPosition, newSpot, (t/duration));
			//transform.rotation = Quaternion.Slerp(startRot, newRot, (t/duration));
			
			/*float heading = Vector3.Dot(Vector3.Normalize(newSpot - transform.position), transform.forward);
			Debug.Log(heading);
			if(heading > 0.5 && heading < 0.9)
			{
				gameObject.GetComponent<Skua>().WalkLeft();
			}
			else if(heading < 0.5 && heading > 0.1)
			{
				gameObject.GetComponent<Skua>().WalkRight();
			}
			else
			{
				
			}*/
			/*if(_hasEgg)
			{
				Debug.Log(t);
			}*/
			
			t += (Time.deltaTime);	
			
			yield return null;
		}
		
		if(grabbedEgg)
		{
			_skuaSpawner.TheEgg.transform.SetParent(gameObject.transform.GetChild(0).transform, false);
			_skuaSpawner.TheEgg.GetComponent<Egg>().IsTaken = true;
		}

		if(!_isRecovered)
		{
			_isHit = false;
			_isRecovering = false;
			_isRecovered = true;
		}
		
		GoIdle();
		
		transform.position = newSpot;
		transform.rotation = newRot;	
		
		_isMoving = false;
	}
	
	/*IEnumerator StartTurn(Vector3 newSpot, Quaternion newRot, float duration)
	{
		float t = 0f;
		
		Quaternion startRot = transform.rotation;
		
		while(t < duration)
		{
			transform.rotation = Quaternion.Slerp(startRot, newRot, (t/duration));
			
			float heading = Vector3.Dot(Vector3.Normalize(newSpot - transform.position), transform.forward);
			Debug.Log(heading);
			if(heading > 0.5 && heading < 0.9)
			{
				gameObject.GetComponent<Skua>().WalkLeft();
			}
			else if(heading < 0.5 && heading > 0.1)
			{
				gameObject.GetComponent<Skua>().WalkRight();
			}
			else
			{
				//gameObject.GetComponent<Skua>().WalkForward();
			}
			
			t += (Time.deltaTime);	
			
			yield return null;
		}
		
		transform.rotation = newRot;	
	}*/
	
	void MoveToNewSpot(SkuaSpot newSpot)
	{
		
		//set old spot's current skua to null to indicate it's open
		_currentSpot.CurrentSkua = null;
		
		newSpot.CurrentSkua = gameObject;
		
		_currentSpot = newSpot;
		
		//todo - eventually lerp, or something along those lines...
		Vector3 p = newSpot.gameObject.transform.position;
		//p.y += 0.05f;
		//gameObject.transform.position = p;
		
		Vector3 e = newSpot.gameObject.transform.rotation.eulerAngles;
		Quaternion q = Quaternion.LookRotation(Vector3.Normalize(p - gameObject.transform.position), Vector3.up);
		//q.SetFromToRotation(gameObject.transform.forward, );
		//Vector3 e = q.eulerAngles;
		e.y -= 90.0f;//due to skua model's local rotation.
		gameObject.transform.rotation = q;
		
		StartCoroutine(StartMove(p, Quaternion.Euler(e), _skuaSpawner.MoveFrequency));
		//StartCoroutine(StartTurn(p, Quaternion.Euler(e), _skuaSpawner.MoveFrequency));
		

	}
}
