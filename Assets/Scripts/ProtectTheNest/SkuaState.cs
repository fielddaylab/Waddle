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
	
	const float UPDATE_FREQ = 0.70588235f;
	
	float _updateTime;
	
    // Start is called before the first frame update
    void Start()
    {
        _updateTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
		float currTime = Time.time;
		//should move 85 times / minute...
		if(currTime - _updateTime < UPDATE_FREQ)
		{
			return;
		}
		
        float r = Random.value;
		if(r < 0.1)
		{
			//move out
			if(_currentSpot != null)
			{
				if(_currentSpot.SpotOut != null)
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
				if(_currentSpot.SpotLeft != null)
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
				if(_currentSpot.SpotRight != null)
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
				if(_currentSpot.SpotIn != null)
				{
					MoveToNewSpot(_currentSpot.SpotIn);
				}
			}
		}
		
		_updateTime = currTime;
    }
	
	void MoveToNewSpot(SkuaSpot newSpot)
	{
		//set old spot's current skua to null to indicate it's open
		_currentSpot.CurrentSkua = null;
		
		//todo - eventually lerp, or something along those lines...
		Vector3 p = newSpot.gameObject.transform.position;
		p.y += 0.5f;
		gameObject.transform.position = p;
		
		Vector3 e = newSpot.gameObject.transform.rotation.eulerAngles;
		e.y -= 90.0f;//due to skua model's local rotation.
		gameObject.transform.rotation = Quaternion.Euler(e);
		
		newSpot.CurrentSkua = gameObject;
		
		_currentSpot = newSpot;
	}
}
