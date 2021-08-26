//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeakTrigger : MonoBehaviour
{

	float _gackTimer = 0.0f;

	[SerializeField]
	float _gackTimerLimit = 1.5f;
	float GackTimerLimit => _gackTimerLimit;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
	
	/*IEnumerator MoveForward()
	{
		while(_isInNav)
		{
			_playerObject.GetComponent<OVRPlayerController>().UpdateMovement();
			yield return null;
		}
	}*/
	
	void OnTriggerEnter(Collider otherCollider)
	{
		//Debug.Log(otherCollider.gameObject.name);
		if(otherCollider.gameObject.name.StartsWith("Rocks"))
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
			//Debug.Log(otherCollider.gameObject.name);
		}
		
	}
	
	void OnTriggerStay(Collider otherCollider)
	{
		if(otherCollider.gameObject.name == "SqwaukBox")
		{
			_gackTimer += UnityEngine.Time.deltaTime;
			if(_gackTimer > _gackTimerLimit)
			{
				//play gack sound.
				otherCollider.gameObject.GetComponent<AudioSource>().Play();
				_gackTimer = 0f;
			}
		}
	}
	
	void OnTriggerExit(Collider otherCollider)
	{
		/*if(otherCollider.gameObject.name == "NavigationTrigger")
		{
			//Debug.Log("Beak left navigation trigger");
			_isInNav = false;
			if(_playerObject != null)
			{
				OVRPlayerController ovrPC = _playerObject.GetComponent<OVRPlayerController>();
				ovrPC.OverrideOculusForward = false;
			}
		}*/
		if(otherCollider.gameObject.name == "SqwaukBox")
		{
			_gackTimer = 0f;
		}
		
		//have on trigger exit cause movement?
	}
}
