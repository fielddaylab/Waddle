//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustHeight : MonoBehaviour
{
	public GameObject _eyeObject;
	public LayerMask _layerMask;
	
	float _penguinCapsuleHeight = 0.7112f;
	
	bool _resetHeights = false;
	
    // Start is called before the first frame update
    void Start()
    {
        if(_eyeObject != null)
		{
			//grab height value of capsule...
			_penguinCapsuleHeight = _eyeObject.transform.parent.parent.parent.GetComponent<CharacterController>().height;
			Debug.Log(_penguinCapsuleHeight);
		}
		
		//OVRManager.TrackingAcquired += SetHeights;
    }

	void Update()
	{
		//SetHeights();
	}
	
	void LateUpdate()
	{
		RaycastHit hitInfo;
		
		if(Physics.Raycast(_eyeObject.transform.position, Vector3.down, out hitInfo, Mathf.Infinity, _layerMask, QueryTriggerInteraction.Ignore))
		{
			//Debug.Log("Hit point: " + hitInfo.point);
			Vector3 pos = transform.position;
			pos.y = hitInfo.point.y;// + (_eyeObject.transform.localPosition.y - _penguinCapsuleHeight);
			transform.position = pos;
		}
	}
}
