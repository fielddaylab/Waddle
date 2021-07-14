using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimBob : MonoBehaviour
{
	Vector3 _lastFramePos;
	Vector3 _lastOrientation;
	
	bool _swimBobComplete = false;
	
	Camera _mainCam;
	
    // Start is called before the first frame update
    void Start()
    {
		_mainCam = Camera.main;
    }

	IEnumerator CheckMoveDown()
	{
		yield return null;
	}
	
	IEnumerator CheckLookForward()
	{
		yield return null;
	}
	
	IEnumerator CheckMoveForward()
	{
		yield return null;
	}
	
    // Update is called once per frame
    void Update()
    {
		//looking for a "looking down, moving down, then looking back forward, moving forward" head motion...
		//all over a couple of seconds time...
		//change in local x rotation, change in y down, change in local x rotation up, change in z forward...
		Vector3 thisOrientation = _mainCam.transform.rotation.eulerAngles;
		
		if(thisOrientation.x - _lastOrientation.x > 5.0f)
		{
			StartCoroutine(CheckMoveDown());
		}
		
		_lastOrientation = thisOrientation;
    }
}
