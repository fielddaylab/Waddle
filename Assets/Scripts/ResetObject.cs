//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

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
	bool _wasDetectingCollisions;
	bool _wasKinematic;
	
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
		
		Rigidbody rb = GetComponent<Rigidbody>();
		if(rb != null)
		{
			_wasDetectingCollisions = rb.detectCollisions;
			_wasKinematic = rb.isKinematic;
		}
		
    }

	void OnEnable()
	{
		PenguinGameManager.OnReset += DoReset;
	}
	
	void OnDisable()
	{
		PenguinGameManager.OnReset -= DoReset;
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
		 
		 MeshRenderer mr = GetComponent<MeshRenderer>();
		 if(mr != null)
		 {
			 mr.enabled = true;
		 }
		 
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

			rb.isKinematic = _wasKinematic;
			rb.detectCollisions = _wasDetectingCollisions;
		}
	}
	
    // Update is called once per frame
    void Update()
    {
        
    }
}
