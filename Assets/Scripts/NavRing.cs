//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavRing : MonoBehaviour
{
	public GameObject _ovrPlayer;
	public GameObject _centerEye;
	public GameObject _positionTransform;
	
	bool _needsUpdate = false;

	bool _hitOnce = false;
	
	//let's make the radius based on the distance between the two children...
	private float _colliderRadius;
	
    // Start is called before the first frame update
    void Start()
    {
        transform.position = _centerEye.transform.position;
		transform.rotation = _ovrPlayer.transform.rotation;
		BoxCollider _rightTrigger = transform.GetChild(0).GetComponent<BoxCollider>();
		BoxCollider _leftTrigger = transform.GetChild(1).GetComponent<BoxCollider>();
		_colliderRadius = Vector3.Distance(_rightTrigger.center, _leftTrigger.center);
		
		Debug.Log(_colliderRadius);
    }

	public void ForceUpdate()
	{
		_needsUpdate = true;
	}
	
    void Update()
    {
        //transform.rotation = _centerEye.transform.rotation;
    }
	
	/*void OnTriggerExit(Collider otherCollider)
	{
		if(otherCollider.gameObject.name == "BeakRight")
		{
			//Debug.Log("Left nav ring");
			if(!_needsUpdate)
			{
				//don't want this to happen if we've hit either of the side boxes...
				transform.position = otherCollider.gameObject.transform.position;
			}
		}
	}*/
	
	void LateUpdate()
	{
		transform.rotation = _ovrPlayer.transform.rotation;
		
		//this radius calculation shouldn't be the position - it needs to be the centers of the colliders...
		float radius = _colliderRadius * 0.25f;
		//Debug.Log(radius);
		if(_needsUpdate)
		{
			Vector3 flatEye = _centerEye.transform.position;
			flatEye.y = transform.position.y;
			transform.position = flatEye;
			/*if(_lr != -1)
			{
				//this code fudges the centering of the box in the opposite direction, assuming the user's momentum is going the other way
				if(_lr == 0)
				{
					//Debug.Log("Right");
					transform.position = transform.position - _centerEye.transform.right * 0.5f;
				}
				else
				{
					//Debug.Log("Left");
					transform.position = transform.position + _centerEye.transform.right * 0.5f;
				}
				
				_lr = -1;
			}*/
			//_positionTransform.transform.position -= _ovrPlayer.transform.forward * Time.deltaTime;
			//transform.position = _positionTransform.transform.position;			
			_needsUpdate = false;
		}
		else
		{
			//the problem - this sometimes gets hit instead of the trigger first...
			
			//this updates the ring only if we move outside of it's radius, and only if we haven't triggered a waddle collider.
			Vector3 flatEye = _centerEye.transform.position;
			flatEye.y = 0f;
			Vector3 flatPos = transform.position;
			flatPos.y = 0f;
			float fLen = (flatEye - flatPos).magnitude;
			
			if(fLen > radius*4f)
			{
				transform.position = _centerEye.transform.position;
			}
		}
	}
}
