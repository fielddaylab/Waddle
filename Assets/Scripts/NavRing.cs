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
	bool _firstUpdate = true;
	int _lr = -1;
	
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

	public void ForceUpdate(int lr)
	{
		_needsUpdate = true;
		_lr = lr;
	}
	
    void Update()
    {
        //transform.rotation = _centerEye.transform.rotation;
		if(_firstUpdate)
		{
			transform.position = _centerEye.transform.position;
			_firstUpdate = false;
		}
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
		//Debug.Log(_colliderRadius);	//0.58
		//Debug.Log(radius);	//0.144...
		if(_needsUpdate)
		{
			Vector3 flatEye = _centerEye.transform.position;
			flatEye.y = transform.position.y;
			transform.position = flatEye;
			if(_lr != -1)
			{
				//this code fudges the centering of the boxes in the opposite direction, assuming the user's momentum is going the other way
				Vector3 vR = _centerEye.transform.right;
				vR.y = 0f;
				vR = Vector3.Normalize(vR);
				
				if(_lr == 0)
				{
					//Debug.Log("Right" + transform.position.ToString("F3"));
					transform.position = transform.position - vR * radius*0.75f;
					//Debug.Log(transform.position.ToString("F3"));
				}
				else
				{
					//Debug.Log("Left" + transform.position.ToString("F3"));
					transform.position = transform.position + vR * radius*0.75f;
					//Debug.Log(transform.position.ToString("F3"));
				}
				
				_lr = -1;
			}
			//_positionTransform.transform.position -= _ovrPlayer.transform.forward * Time.deltaTime;
			//transform.position = _positionTransform.transform.position;			
			_needsUpdate = false;
		}
		else
		{
			//this updates the ring only if we move outside of it's radius, and only if we haven't triggered a waddle collider.
			Vector3 flatEye = _centerEye.transform.position;
			flatEye.y = 0f;
			Vector3 flatPos = transform.position;
			flatPos.y = 0f;
			float fLen = (flatEye - flatPos).magnitude;
			
			if(fLen > radius*2f)
			{
				transform.position = _centerEye.transform.position;
			}
		}
	}
}
