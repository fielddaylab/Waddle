//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetRaycast : MonoBehaviour
{
	public GameObject _eyeObject;
	public LayerMask _layerMask;
	
	[SerializeField]
	GameObject _rotationTransform;
	GameObject RotationTransform => _rotationTransform;
	
	[SerializeField]
	Vector3 _rotationToApply;
	Vector3 RotationToApply => _rotationToApply;
	
	float _penguinCapsuleHeight = 0.7112f;
	
	bool _resetHeights = false;
	
    // Start is called before the first frame update
    void Start()
    {
        if(_eyeObject != null)
		{
			//grab height value of capsule...
			_penguinCapsuleHeight = _eyeObject.transform.parent.parent.parent.GetComponent<CharacterController>().height;
		}
		
		//OVRManager.TrackingAcquired += SetHeights;
    }

    // Update is called once per frame
    /*void FixedUpdate()
    {
		
    }*/
	
	/*void SetHeights()
	{
		if(!_resetHeights)
		{
			Time.timeScale = 0;
			GameObject[] heightSets = GameObject.FindGameObjectsWithTag("HeightSet");
			for(int i = 0; i < heightSets.Length; ++i)
			{
				Debug.Log( Camera.main.transform.position.y );
				Vector3 posHeight = heightSets[i].transform.position;
				posHeight.y = Camera.main.transform.position.y - 0.6096f;
				heightSets[i].transform.position = posHeight;
			}
			_resetHeights = true;
			Time.timeScale = 1;
		}	
	}*/
	
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
			pos.x = hitInfo.point.x;
			pos.z = hitInfo.point.z;
			pos.y = _eyeObject.transform.position.y - _penguinCapsuleHeight + (_penguinCapsuleHeight - _eyeObject.transform.localPosition.y);	//also take into account difference between tracked height and capsule height...
			transform.position = pos;

			Vector3 flatForward = _eyeObject.transform.forward;
			flatForward.y = 0f;
			flatForward = flatForward.normalized;
			transform.position -= (flatForward * 0.1f);	//this should be related to scale
			
			Quaternion q = Quaternion.identity;
			q.eulerAngles = _rotationToApply;
			transform.rotation = _rotationTransform.transform.rotation;
			transform.rotation *= q;
			//to-do, need to back this up some...
		}
	}
}
