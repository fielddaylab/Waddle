using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavRing : MonoBehaviour
{
	public GameObject _ovrPlayer;
	public GameObject _centerEye;
	
	bool _needsUpdate = false;
	int _lr = -1;
	
	//let's make the radius based on the distance between the two children...
	//public float _colliderRadius = 0.06f;
	
    // Start is called before the first frame update
    void Start()
    {
        transform.position = _centerEye.transform.position;
		transform.rotation = _centerEye.transform.rotation;
		//Debug.Log(_colliderRadius);
    }

	public void ForceUpdate(int lr)
	{
		_needsUpdate = true;
		_lr = lr;
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
		float radius = Vector3.Distance(transform.GetChild(0).transform.position, transform.GetChild(1).transform.position) * 0.25f;
		//Debug.Log(radius);
		if(_needsUpdate)
		{
			transform.position = _centerEye.transform.position;
			if(_lr != -1)
			{
				if(_lr == 0)
				{
					//Debug.Log("Right");
					transform.position = transform.position - _centerEye.transform.right * radius;
				}
				else
				{
					//Debug.Log("Left");
					transform.position = transform.position + _centerEye.transform.right * radius;
				}
				
				_lr = -1;
			}
			
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
			//Debug.Log(fLen + " " + radius);
			if(fLen > radius*3f)
			{
				transform.position = _centerEye.transform.position;
			}
		}
	}
}
