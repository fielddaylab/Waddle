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

	[SerializeField]
	bool _flier = false;
	
	Vector3 _startingPosition;

	private SkuaState _currentState;

	private enum SkuaState : byte {
		Idling,
		ReadyToMove,
		Moving
	}

    // Start is called before the first frame update
    void Start()
    {
        _startingPosition = transform.position;
		StartCoroutine(StartIdle(NewIdleTime()));
    }

    // Update is called once per frame
    void Update()
    {
		if (_currentState.Equals(SkuaState.ReadyToMove))
		{
			Vector3 newLoc = FindNewLocation();
			Vector3 toNewSpot = newLoc - transform.position;
			toNewSpot = Vector3.Normalize(toNewSpot);
			Quaternion newRot = Quaternion.LookRotation(toNewSpot, Vector3.up);
			
			float distToSpot = Vector3.Distance(newLoc, transform.position);

            //Debug.Log("[Skua] Starting move...");
			_currentState = SkuaState.Moving;
            StartCoroutine(StartMove(newLoc, newRot, distToSpot / _speed, _flier));	
		}
    }
	
	private float NewIdleTime() {
		return _idleTime + Random.Range(-3, 3);
    }
	IEnumerator StartIdle(float duration)
	{
		Animator a = transform.GetChild(0).gameObject.GetComponent<Animator>();
		a.SetBool("flying", false);
        a.SetBool("walking", false);

        yield return new WaitForSeconds(duration);
        _currentState = SkuaState.ReadyToMove;
		yield break;
    }

    IEnumerator StartMove(Vector3 newSpot, Quaternion newRot, float duration, bool flying)
	{
		Animator a = transform.GetChild(0).gameObject.GetComponent<Animator>();
		if (flying) {
			a.SetBool("flying", true);
		} else {
			a.SetBool("walking", true);
		}

		float t = 0f;
		Vector3 startPosition = transform.position;
		Quaternion startRotation = transform.rotation;
			
		while(t < duration)
		{
			transform.position = Vector3.Lerp(startPosition, newSpot, (t/duration));
			if (Quaternion.Angle(transform.rotation, newRot) > 1) {
                transform.rotation = Quaternion.Slerp(startRotation, newRot, (t / 0.5f));
            } else transform.rotation = newRot;

            t += (Time.deltaTime);	
			yield return null;
		}

		//transform.rotation = newRot;

		//Debug.Log("[Skua] Starting idle...");
		if (_flier) {
			_currentState = SkuaState.ReadyToMove;
		} else {
            _currentState = SkuaState.Idling;
            StartCoroutine(StartIdle(NewIdleTime()));
        }	
		yield break;
	}
	
	Vector3 FindNewLocation()
	{
		//Debug.Log("[Skua] Finding new location...");
		Vector3 newLoc = _startingPosition;
		if (_flier) {
            newLoc.y += Random.Range(-1f, 1f);
        }
        newLoc.x += Random.Range(-_wanderRadius, _wanderRadius);
		newLoc.z += Random.Range(-_wanderRadius, _wanderRadius);
		
		return newLoc;
	}
}
