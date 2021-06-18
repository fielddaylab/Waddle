using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavRing : MonoBehaviour
{
	//public GameObject _ovrPlayer;
	public GameObject _centerEye;
	
	bool _needsUpdate = false;
	int _lr = -1;
	
	public float _colliderRadius = 0.06f;
	
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
		transform.rotation = _centerEye.transform.rotation;
		
		if(_needsUpdate)
		{
			transform.position = _centerEye.transform.position;
			if(_lr != -1)
			{
				if(_lr == 0)
				{
					//Debug.Log("Right");
					transform.position = transform.position - _centerEye.transform.right * _colliderRadius * 1.1f;
				}
				else
				{
					//Debug.Log("Left");
					transform.position = transform.position + _centerEye.transform.right * _colliderRadius * 1.1f;
				}
				
				_lr = -1;
			}
			
			_needsUpdate = false;
		}
		else
		{
			//this updates the ring only if we move outside of it's radius, and only if we haven't triggered a waddle collider.
			float fLen = (_centerEye.transform.position - transform.position).magnitude;
			//Debug.Log(fLen);
			if(fLen > _colliderRadius)
			{
				transform.position = _centerEye.transform.position;
			}
		}
	}
}
