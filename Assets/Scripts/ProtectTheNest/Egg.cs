//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
	bool _isTaken;
	bool _wasReset;
	Vector3 _startPosition;
	Quaternion _startRotation;
	
	public bool IsTaken
	{
		get { return _isTaken; }
		set { _isTaken = value; }
	}
	
	public bool WasReset
	{
		get { return _wasReset; }
		set { _wasReset = value; }
	}
	
	GameObject _parentObject;
	
    // Start is called before the first frame update
    void Start()
    {
        _isTaken = false;
		_wasReset = false;
		_startPosition = transform.localPosition;
		_startRotation = transform.rotation;
		_parentObject = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void Reset()
	{
		if(_isTaken)
		{
			PenguinAnalytics.Instance.LogEggReturn();
		}
		
		PenguinPlayer.Instance.SlowDownMovement();
		gameObject.transform.SetParent(_parentObject.transform, true);
		//transform.position = _startPosition;
		StartCoroutine(MoveBackToCenter(3f));
		transform.rotation = _startRotation;
		_isTaken = false;
		_wasReset = true;
	}
	
	IEnumerator MoveBackToCenter(float duration)
	{
		float t = 0f;
		Vector3 startPosition = transform.localPosition;
		Vector3 newSpot = _startPosition;
		
		while(t < duration)
		{
			transform.localPosition = Vector3.Lerp(startPosition, newSpot, (t/duration));
			t += (Time.deltaTime);	
			yield return null;
		}
	}
}
