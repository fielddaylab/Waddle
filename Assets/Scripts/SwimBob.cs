using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimBob : MonoBehaviour
{
	//Vector3 _lastFramePos;
	//Vector3 _lastOrientation;
	
	bool _swimBobComplete = false;
	bool _checkingSwimBob = false;
	bool _checkingLookDown = false;
	
	float _checkLookDownFrameLimit = 120;
	
	float _swimBobTimeLimit = 2.0f;
	
	Camera _mainCam;
	
    // Start is called before the first frame update
    void Start()
    {
		_mainCam = Camera.main;
    }
	
	IEnumerator CheckLookMoveDown()
	{
		_checkingLookDown = true;
		float lookDownFrameCount = 0;
		Vector3 thisOrientation = _mainCam.transform.rotation.eulerAngles;
		Vector3 thisFramePos = _mainCam.transform.position;
		
		while(lookDownFrameCount < _checkLookDownFrameLimit)
		{
			Vector3 lastFramePos = _mainCam.transform.position;
			Vector3 lastOrientation = _mainCam.transform.rotation.eulerAngles;
			if((thisOrientation.x - lastOrientation.x > 5.0f) && (thisFramePos.y - lastFramePos.y) > 0.1f)
			{
				if(!_checkingSwimBob)
				{
					Debug.Log("Starting swim bob!");
					_swimBobComplete = false;
					StartCoroutine(CheckLookMoveForward());
				}
			}
			lookDownFrameCount += 1.0f;
			yield return null;
		}
		
		_checkingLookDown = false;
	}
	
	IEnumerator CheckLookMoveForward()
	{
		_checkingSwimBob = true;
		float swimBobTime = 0f;
		
		while(swimBobTime < _swimBobTimeLimit)
		{
			swimBobTime += UnityEngine.Time.deltaTime;
			
			yield return null;
		}
		
		_checkingSwimBob = false;
	}
	
    // Update is called once per frame
    void Update()
    {
		//looking for a "looking down, moving down, then looking back forward, moving forward" head motion...
		//all over a couple of seconds time...
		//change in local x rotation, change in y down, change in local x rotation up, change in z forward...
		
		/*if(!_checkingLookDown)
		{
			StartCoroutine(CheckLookMoveDown());
		}*/
		
		//_lastOrientation = thisOrientation;
		//_lastFramePos = thisFramePos;
    }
	
	void LateUpdate()
	{
		if(_swimBobComplete)
		{
			
		}
	}
}
