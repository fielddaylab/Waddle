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
	
	bool _hasEgg = false;
	
	SkuaSpawner _skuaSpawner;
	
	public SkuaSpawner Spawner
	{
		get { return _skuaSpawner; }
		set { _skuaSpawner = value; }
	}
	
	Vector3 _lastPosition;
	Quaternion _lastRotation;
	
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
		if((_currentSpot.gameObject.name == "SkuaSpot0") && !_hasEgg && !_skuaSpawner.EggIsTaken())
		{
			//have skua grab the egg and move it to a random outer location...
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
			
			Debug.Log("Grabbing egg");
			_skuaSpawner.TheEgg.transform.SetParent(gameObject.transform.GetChild(0).transform, false);
			_skuaSpawner.TheEgg.GetComponent<Egg>().IsTaken = true;
			
			MoveToNewSpot(outerSpots[randomIndex]);
			_hasEgg = true;
		}
		
		_lastPosition = transform.position;
		_lastRotation = transform.rotation;
    }
	
	public void MoveSkua()
	{
		if(!_hasEgg && !gameObject.GetComponent<Skua>().IsHit)
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
	}
	
	IEnumerator StartMove(Vector3 newSpot, Quaternion newRot, float duration)
	{
		float t = 0f;
		
		Vector3 startPosition = transform.position;
		//Quaternion startRot = transform.rotation;
		
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
			
			gameObject.GetComponent<Skua>().WalkForward();
			
			t += (Time.deltaTime);	
			
			yield return null;
		}
		
		gameObject.GetComponent<Skua>().GoIdle();
		
		transform.position = newSpot;
		transform.rotation = newRot;
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
		
		newSpot.CurrentSkua = gameObject;
		
		_currentSpot = newSpot;
	}
}