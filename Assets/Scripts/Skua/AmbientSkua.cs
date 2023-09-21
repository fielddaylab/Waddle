using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSkua : MonoBehaviour
{
	[SerializeField]
	float _wanderRadius = 5f;
	
	[SerializeField]
	float _idleTime = 4f;
	
	[SerializeField]
	float _speed = 0.5f;
	
	Vector3 _startingPosition;
    
	bool _moving = false;
	
	// Start is called before the first frame update
    void Start()
    {
        _startingPosition = transform.position;
		
		StartCoroutine(StartIdle(_idleTime));
    }

    // Update is called once per frame
    void Update()
    {
		if(!_moving)
		{
			Vector3 newLoc = FindNewLocation();
			Vector3 toNewSpot = newLoc - transform.position;
			toNewSpot = Vector3.Normalize(toNewSpot);
			Quaternion newRot = Quaternion.LookRotation(toNewSpot, Vector3.up);
			
			float distToSpot = Vector3.Distance(newLoc, transform.position);

			StartCoroutine(StartMove(newLoc, newRot, distToSpot / _speed));	
		}
    }
	
	IEnumerator StartIdle(float duration)
	{
		Animator a = transform.GetChild(0).gameObject.GetComponent<Animator>();
		//a.SetBool("fly", false);
		
		yield return new WaitForSeconds(duration);
	}
	
	IEnumerator StartMove(Vector3 newSpot, Quaternion newRot, float duration)
	{
		if(!_moving)
		{
			_moving = true;
			
			Animator a = transform.GetChild(0).gameObject.GetComponent<Animator>();
			//a.SetBool("fly", true);
			
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

			//transform.rotation = newRot;
			
			_moving = false;
			//StartCoroutine(StartIdle(_idleTime));
		}
	}
	
	Vector3 FindNewLocation()
	{
		Vector3 newLoc = _startingPosition;
		//newLoc.y += UnityEngine.Random.Range(1f, _wanderRadius);
		newLoc.x += UnityEngine.Random.Range(-_wanderRadius, _wanderRadius);
		newLoc.z += UnityEngine.Random.Range(-_wanderRadius, _wanderRadius);
		
		return newLoc;
	}
}
