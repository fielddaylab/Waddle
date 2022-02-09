using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientNPCPenguin : MonoBehaviour
{
	[SerializeField]
	float _wanderRadius = 5f;
	
	[SerializeField]
	float _idleTime = 4f;
	
	[SerializeField]
	LayerMask _layerMask;
	
	[SerializeField]
	float _yOffset = 0.235f;
	
	Vector3 _startingPosition;
	
	public enum PenguinState {
		WALKING,
		IDLE,
		NUM_PENGUIN_STATES
	}
		
	PenguinState _currentState;
	
    // Start is called before the first frame update
    void Start()
    {
		_startingPosition = transform.position;
		
        StartCoroutine(StartIdle(_idleTime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	IEnumerator StartIdle(float duration)
	{
		Animator a = transform.GetChild(0).gameObject.GetComponent<Animator>();
		a.SetBool("walk", false);
		
		yield return new WaitForSeconds(duration);
		
		Vector3 newLoc = FindNewLocation();
		Vector3 toNewSpot = newLoc - transform.position;
		toNewSpot = Vector3.Normalize(toNewSpot);
		Quaternion newRot = Quaternion.LookRotation(toNewSpot, Vector3.up);
		
		float distToSpot = Vector3.Distance(newLoc, transform.position);
		//float oneODistToSpot = 1f / distToSpot;
		float speed = 0.25f;//meters per second
		
		
		StartCoroutine(StartMove(newLoc, newRot, distToSpot / speed));
	}
	
	IEnumerator StartMove(Vector3 newSpot, Quaternion newRot, float duration)
	{
		Animator a = transform.GetChild(0).gameObject.GetComponent<Animator>();
		a.SetBool("walk", true);
		
		float t = 0f;
		Vector3 startPosition = transform.position;
		//Quaternion startRotation = transform.rotation;
		transform.rotation = newRot;
		
		while(t < duration)
		{
			transform.position = Vector3.Lerp(startPosition, newSpot, (t/duration));
			//transform.rotation = Quaternion.Lerp(startRotation, newRot, (t/duration));
			
			t += (Time.deltaTime);	
			yield return null;
		}
		
		StartCoroutine(StartIdle(_idleTime));
	}
	
	Vector3 FindNewLocation()
	{
		Vector3 newLoc = Vector3.zero;
		
		RaycastHit hitInfo;
		
		Vector3 castLocation = _startingPosition;
		castLocation.y += 10f;
		castLocation.x += UnityEngine.Random.Range(-_wanderRadius, _wanderRadius);
		castLocation.z += UnityEngine.Random.Range(-_wanderRadius, _wanderRadius);
		
		if(Physics.Raycast(castLocation, Vector3.down, out hitInfo, Mathf.Infinity, _layerMask, QueryTriggerInteraction.Ignore))
		{
			newLoc = hitInfo.point;
		}
		
		return newLoc;
	}
	
	void LateUpdate()
	{
		RaycastHit hitInfo;
		
		if(Physics.Raycast(transform.position, Vector3.down, out hitInfo, Mathf.Infinity, _layerMask, QueryTriggerInteraction.Ignore))
		{
			//Debug.Log("Hit point: " + hitInfo.point);
			Vector3 pos = transform.position;
			pos.y = hitInfo.point.y + _yOffset;// + (_eyeObject.transform.localPosition.y - _penguinCapsuleHeight);
			transform.position = pos;
		}
	}
}
