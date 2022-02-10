using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObject : MonoBehaviour
{

	Vector3 _startingPosition;
	Quaternion _startingOrientation;
	Vector3 _startingScale;
	Transform _parentTransform;
	string _startingName;
	
    void Start()
    {
        _startingPosition = transform.position;
		_startingOrientation = transform.rotation;
		_startingScale = transform.localScale;
		_startingName = gameObject.name;
		if(transform.parent != null)
		{
			_parentTransform = transform.parent;
		}
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
		 
		 if(_parentTransform != null)
		 {
			transform.parent = _parentTransform;
		 }
		 
		 transform.position = _startingPosition;
		 transform.rotation = _startingOrientation;
		 transform.localScale = _startingScale;
		 gameObject.name = _startingName;
		 
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
