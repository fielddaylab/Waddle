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
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    /*void FixedUpdate()
    {
		
    }*/
		
	void LateUpdate()
	{
		RaycastHit hitInfo;
		
		if(Physics.Raycast(_eyeObject.transform.position, Vector3.down, out hitInfo, Mathf.Infinity, _layerMask, QueryTriggerInteraction.Ignore))
		{
			//Debug.Log("Hit point: " + hitInfo.point);
			transform.position = hitInfo.point;	
			Vector3 flatForward = _eyeObject.transform.forward;
			flatForward.y = 0f;
			flatForward = flatForward.normalized;
			transform.position -= (flatForward * 0.05f);
			
			Quaternion q = Quaternion.identity;
			q.eulerAngles = _rotationToApply;
			transform.rotation = _rotationTransform.transform.rotation;
			transform.rotation *= q;
			//to-do, need to back this up some...
		}
	}
}
