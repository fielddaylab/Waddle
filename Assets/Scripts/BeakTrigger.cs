using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeakTrigger : MonoBehaviour
{
	public GameObject _playerObject;
	
	bool _isInNav = false;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
	}
}
