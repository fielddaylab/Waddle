using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavRing : MonoBehaviour
{
	public GameObject _ovrPlayer;
	public GameObject _centerEye;
	
	bool _needsUpdate = false;
	float _colliderRadius = 0.1f;
	
    // Start is called before the first frame update
    void Start()
    {
        transform.position = _ovrPlayer.transform.position;
		transform.rotation = _ovrPlayer.transform.rotation;
		_colliderRadius = GetComponent<CapsuleCollider>().radius;
		//Debug.Log(_colliderRadius);
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
		transform.rotation = _centerEye.transform.rotation;
		
		if(_needsUpdate)
		{
			transform.position = _centerEye.transform.position;
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
