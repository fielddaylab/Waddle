//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimBob : MonoBehaviour
{
	bool _swimBobComplete = false;
	bool _checkingSwimBob = false;
	bool _checkingLookDown = false;
	bool _swimMoveDone = true;
	
	float _checkLookDownFrameLimit = 120;
	float _swimMoveTimeLimit = 1.0f;
	float _swimBobTimeLimit = 5.0f;
	
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
			//Debug.Log("Angle: " + ((thisOrientation.x + 360.0f) - (lastOrientation.x + 360.0f)).ToString("F4"));
			//Debug.Log((thisFramePos.y - lastFramePos.y).ToString("F4"));
			if((Mathf.Abs(((thisOrientation.x + 360.0f) - (lastOrientation.x + 360.0f))) > 12.0f) && (thisFramePos.y - lastFramePos.y) < -0.03f)
			{
				if(!_checkingSwimBob)
				{
					//Debug.Log("Starting swim bob!");
					_swimBobComplete = false;
					StartCoroutine(CheckLookMoveForward());
					_checkingLookDown = false;
					yield break;
				}
			}
			lookDownFrameCount += 1.0f;
			yield return null;
		}
		
		//Debug.Log("Reset");
		_checkingLookDown = false;
	}
	
	IEnumerator CheckLookMoveForward()
	{
		_checkingSwimBob = true;
		float swimBobTime = 0f;
		Vector3 thisOrientation = _mainCam.transform.rotation.eulerAngles;
		Vector3 thisFramePos = _mainCam.transform.position;
		
		while(swimBobTime < _swimBobTimeLimit)
		{
			Vector3 lastFramePos = _mainCam.transform.position;
			Vector3 lastOrientation = _mainCam.transform.rotation.eulerAngles;
			//Debug.Log("Angle: " + ((thisOrientation.x + 360.0f) - (lastOrientation.x + 360.0f)).ToString("F4"));
			//Debug.Log((thisFramePos.y - lastFramePos.y).ToString("F4"));
			
			if((Mathf.Abs(((thisOrientation.x + 360.0f) - (lastOrientation.x + 360.0f))) > 10.0f) && (thisFramePos.y - lastFramePos.y) < -0.02f)
			{
				_swimBobComplete = true;
				_checkingSwimBob = false;
				yield break;
			}
			swimBobTime += UnityEngine.Time.deltaTime;
			
			yield return null;
		}
		
		_checkingSwimBob = false;
	}
	
	IEnumerator PerformSwimMove()
	{
		float swimMoveTime = 0f;
		
		_swimMoveDone = false;
			
		GetComponent<OVRPlayerController>().OverrideOculusForward = true;
		GetComponent<OVRPlayerController>().Acceleration = 0.2f;
		
		while(swimMoveTime < _swimMoveTimeLimit)
		{
			swimMoveTime += UnityEngine.Time.deltaTime;
			yield return null;
		}
		
		GetComponent<OVRPlayerController>().OverrideOculusForward = false;
		GetComponent<OVRPlayerController>().Acceleration = 0.02f;
		
		_swimMoveDone = true;
		_checkingLookDown = false;
		_checkingSwimBob = false;
	}
	
    // Update is called once per frame
    void Update()
    {
		//looking for a "looking down, moving down, then looking back forward, moving forward" head motion...
		//all over a couple of seconds time...
		//change in local x rotation, change in y down, change in local x rotation up, change in z forward...
		
		if(!_checkingLookDown && !_checkingSwimBob && _swimMoveDone && !_swimBobComplete)
		{
			StartCoroutine(CheckLookMoveDown());
		}
		
		//_lastOrientation = thisOrientation;
		//_lastFramePos = thisFramePos;
    }
	
	void LateUpdate()
	{
		if(_swimBobComplete)
		{
			if(_swimMoveDone)
			{
				GetComponent<AudioSource>().Play();
				StartCoroutine(PerformSwimMove());
			}
			//Debug.Log("Did a swim bob!");
			_swimBobComplete = false;
		}
	}
}
