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
	
	Vector3 _lastPosition = Vector3.zero;
	
	float _penguinCapsuleHeight = 0.7112f;
	
    // Start is called before the first frame update
    void Start()
    {
        if(_eyeObject != null)
		{
			//grab height value of capsule...
			_penguinCapsuleHeight = _eyeObject.transform.parent.parent.parent.GetComponent<CharacterController>().height;
			//Debug.Log(_penguinCapsuleHeight);
		}
		
		//OVRManager.TrackingAcquired += SetHeights;
    }

	void LateUpdate()
	{
		//if(Vector3.Distance(_eyeObject.transform.position, _lastPosition) > 0.1f)
		{
			RaycastHit hitInfo;
			
			if(Physics.Raycast(_eyeObject.transform.position, Vector3.down, out hitInfo, Mathf.Infinity, _layerMask, QueryTriggerInteraction.Ignore))
			{
				//Debug.Log("Hit point: " + hitInfo.point);
				Vector3 pos = transform.position;
				pos.x = hitInfo.point.x;
				pos.z = hitInfo.point.z;
				//pos.y = hitInfo.point.y;//_eyeObject.transform.position.y - _penguinCapsuleHeight + (_penguinCapsuleHeight - _eyeObject.transform.localPosition.y);	//also take into account difference between tracked height and capsule height...
				transform.position = pos;

				Vector3 flatForward = _eyeObject.transform.forward;
				flatForward.y = 0f;
				flatForward = flatForward.normalized;

				//Jack's changes
				Quaternion q = Quaternion.identity;
				q.eulerAngles = _rotationToApply;
			
				if( Vector3.Dot(_eyeObject.transform.up, Vector3.down) > 0){
					Vector3 tmp = q.eulerAngles;
					tmp.y = q.eulerAngles.y - 180;
					q.eulerAngles = tmp;
					transform.position += (flatForward * 0.15f);
				}
				else{
					transform.position -= (flatForward * 0.1f);	//this should be related to scale
				}

				transform.rotation = _rotationTransform.transform.rotation;
				transform.rotation *= q;
			}
			
			_lastPosition = _eyeObject.transform.position;
		}
	}
}
