using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeakTrigger : MonoBehaviour
{
	public GameObject _playerObject;
	
	bool _isInNav = false;
	
	public float _leftWaddleTimer = 0f;
	public float _rightWaddleTimer = 0f;
	public float _leftWaddleStart = 0f;
	public float _rightWaddleStart = 0f;
	public float _waddleMovementThreshold = 5f;
	//bool _waddleInThreshold = false;
	public int _whenToWaddle = 2;
	public int _waddleCount = 0;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_leftWaddleTimer != 0f)
		{
			_leftWaddleTimer += UnityEngine.Time.deltaTime;
			if(_leftWaddleTimer - _leftWaddleStart > _waddleMovementThreshold)
			{
				_waddleCount = 0;
				_leftWaddleTimer = 0f;
			}
		}
		
		if(_rightWaddleTimer != 0f)
		{
			_rightWaddleTimer += UnityEngine.Time.deltaTime;
			if(_rightWaddleTimer - _rightWaddleStart > _waddleMovementThreshold)
			{
				_waddleCount = 0;
				_rightWaddleTimer = 0f;
			}
		}
		
		if(_waddleCount >= _whenToWaddle)
		{
			if(_playerObject != null)
			{
				OVRPlayerController ovrPC = _playerObject.GetComponent<OVRPlayerController>();
				ovrPC.OverrideOculusForward = true;
				_playerObject.GetComponent<OVRPlayerController>().UpdateMovement();
				//StartCoroutine(MoveForward());
			}
		}
		else
		{
			if(_playerObject != null)
			{
				OVRPlayerController ovrPC = _playerObject.GetComponent<OVRPlayerController>();
				ovrPC.OverrideOculusForward = false;
			}
		}
    }
	
	IEnumerator MoveForward()
	{
		while(_isInNav)
		{
			_playerObject.GetComponent<OVRPlayerController>().UpdateMovement();
			yield return null;
		}
	}
	
	void OnTriggerEnter(Collider otherCollider)
	{
		if(otherCollider.gameObject.name == "NavigationTrigger")
		{
			//trigger a constant forward navigation motion..
			//Debug.Log("Beak hit navigation trigger");
			//if(gameObject.transform.childCount == 0)
			{
				_isInNav = true;
				if(_playerObject != null)
				{
					OVRPlayerController ovrPC = _playerObject.GetComponent<OVRPlayerController>();
					ovrPC.OverrideOculusForward = true;
					StartCoroutine(MoveForward());
				}
			}
		}
		else if(otherCollider.gameObject.name == "WaddleTriggerLeft")
		{
			if(_leftWaddleTimer == 0f)
			{
				_leftWaddleTimer = UnityEngine.Time.time;
				_leftWaddleStart = _leftWaddleTimer;
				
				if(_rightWaddleTimer != 0f)
				{
					float timeSinceRight = _rightWaddleTimer - _rightWaddleStart;
					if(timeSinceRight < _waddleMovementThreshold)
					{
						_waddleCount++;
					}
					else
					{
						_waddleCount = 0;
					}
					
					_rightWaddleTimer = 0f;
				}
			}
		}
		else if(otherCollider.gameObject.name == "WaddleTriggerRight")
		{
			if(_rightWaddleTimer == 0f)
			{
				_rightWaddleTimer = UnityEngine.Time.time;
				_rightWaddleStart = _rightWaddleTimer;
				
				if(_leftWaddleTimer != 0f)
				{
					float timeSinceLeft = _leftWaddleTimer - _leftWaddleStart;
					if(timeSinceLeft < _waddleMovementThreshold)
					{
						_waddleCount++;
					}
					else
					{
						_waddleCount = 0;
					}
					
					_leftWaddleTimer = 0f;
				}
			}
		}
		else if(otherCollider.gameObject.name.StartsWith("rock"))
		{
			//pick up a rock with your beak
			if(gameObject.transform.childCount == 0)
			{
				otherCollider.gameObject.transform.parent = gameObject.transform;
				Rigidbody rb = otherCollider.gameObject.GetComponent<Rigidbody>();
				if(rb != null)
				{
					rb.isKinematic = true;
					rb.detectCollisions = false;
				}
				//enable the navigationtrigger collider... so that we can drop the rock..
				/*if(navigationTrigger != null)
				{
					navigationTrigger.GetComponent<Collider>().enabled = true;
					navigationTrigger.GetComponent<Rigidbody>().detectCollisions = true;
				}*/
			}
			Debug.Log(otherCollider.gameObject.name);
		}
	}
	
	void OnTriggerExit(Collider otherCollider)
	{
		if(otherCollider.gameObject.name == "NavigationTrigger")
		{
			//Debug.Log("Beak left navigation trigger");
			_isInNav = false;
			if(_playerObject != null)
			{
				OVRPlayerController ovrPC = _playerObject.GetComponent<OVRPlayerController>();
				ovrPC.OverrideOculusForward = false;
			}
		}
		
		//have on trigger exit cause movement?
	}
}
