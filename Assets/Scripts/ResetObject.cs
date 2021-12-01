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
		PenguinGameManager._resetGameDelegate += DoReset;
	}
	
	void OnDisable()
	{
		PenguinGameManager._resetGameDelegate -= DoReset;
	}
	
	public void DoReset()
	{
		 Rigidbody rb = GetComponent<Rigidbody>();
		 /*if(rb != null)
		 {
			 rb.velocity = Vector3.zero;
		 }*/
		 
		 transform.position = _startingPosition;
		 transform.rotation = _startingOrientation;
		 transform.localScale = _startingScale;
		 
		 if(rb != null)
		 {
			 StartCoroutine(TurnPhysicsOff());
		 }
	}

	IEnumerator TurnPhysicsOff()
	{
		yield return null;
		
		Rigidbody rb = GetComponent<Rigidbody>();
		if(rb != null)
		{
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}
	}
	
    // Update is called once per frame
    void Update()
    {
        
    }
}
