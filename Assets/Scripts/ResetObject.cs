using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObject : MonoBehaviour
{
    // Start is called before the first frame update
	Vector3 _startingPosition;
	Quaternion _startingOrientation;
	Vector3 _startingScale;
	
	//use an event for reseting..
	
    void Start()
    {
        _startingPosition = transform.position;
		_startingOrientation = transform.rotation;
		_startingScale = transform.localScale;
    }

	void OnEnable()
	{
		//PenguinGameManager.OnResetDelegate += DoReset;
	}
	
	void OnDisable()
	{
		//PenguinGameManager.OnResetDelegate -= DoReset;
	}
	
	public void DoReset()
	{
		 transform.position = _startingPosition;
		 transform.rotation = _startingOrientation;
		 transform.localScale = _startingScale;
	}
	
    // Update is called once per frame
    void Update()
    {
        
    }
}
