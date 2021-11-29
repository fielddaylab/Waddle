//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetRaycastJack : MonoBehaviour
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

	Vector3 _transformationToApply = new Vector3(0,0,-0.3f);

	Quaternion savedRotationTransform;
	Vector3 pos;
	Vector3 flatForward;
	float threashold = 0.1f;
	bool bendedOver;
	
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
			//Vector3 pos = transform.position;
			//pos.x = hitInfo.point.x;
			//pos.z = hitInfo.point.z;
			//Vector3 flatForward = _eyeObject.transform.forward;
			//flatForward.y = 0f;
			//flatForward = flatForward.normalized;
			
			Debug.Log(Vector3.Dot(_eyeObject.transform.up, Vector3.down));
			//|| Vector3.Dot(_eyeObject.transform.up, Vector3.down) > 0.04f 
			if(Vector3.Dot(_eyeObject.transform.up, Vector3.down) < (threashold*-1) ){
				bendedOver = false;
			}
			else if (Vector3.Dot(_eyeObject.transform.up, Vector3.down) > threashold){
				bendedOver = true;
			}

			if(Vector3.Dot(_eyeObject.transform.up, Vector3.down) < (threashold*-1) || Vector3.Dot(_eyeObject.transform.up, Vector3.down) > threashold  )
			{
				savedRotationTransform = _rotationTransform.transform.rotation;
				pos = transform.position;
				pos.x = hitInfo.point.x;
				pos.z = hitInfo.point.z;
				transform.position = pos;
				flatForward = _eyeObject.transform.forward;
				flatForward.y = 0f;
				flatForward = flatForward.normalized;
			}
			else if(bendedOver == true){
				Quaternion tmp2 = Quaternion.identity;
				tmp2.eulerAngles = new Vector3(0,180,0);
				savedRotationTransform *= tmp2;
				pos += (flatForward * 0.2f);
				bendedOver = false;
			}

			//pos.y = hitInfo.point.y;//_eyeObject.transform.position.y - _penguinCapsuleHeight + (_penguinCapsuleHeight - _eyeObject.transform.localPosition.y);	//also take into account difference between tracked height and capsule height...
				transform.position = pos;


			Quaternion q = Quaternion.identity;
			q.eulerAngles = _rotationToApply;


			//_eyeObject.transform.eulerAngles.x > 88
			//Debug.Log(_eyeObject.transform.eulerAngles.x);


			//transform.position -= (flatForward * 0.1f);

			
			if( Vector3.Dot(_eyeObject.transform.up, Vector3.down) > threashold){
				//transform.rotation = _rotationTransform.transform.rotation;
				Vector3 tmp = q.eulerAngles;
				tmp.y = q.eulerAngles.y - 180;
				q.eulerAngles = tmp;
				transform.position += (flatForward * 0.1f);
				//transform.position = transform.position + _transformationToApply;
			}
			else{
					transform.position -= (flatForward * 0.1f);	//this should be related to scale
			}
			
			

			//Debug.Log(_eyeObject.transform.eulerAngles.x);
			//Debug.Log(Vector3.Dot(_eyeObject.transform.up, Vector3.down));
			//if(_eyeObject.transform.eulerAngles.x >70 && _eyeObject.transform.eulerAngles.x < 180){

			transform.rotation = savedRotationTransform;
			//transform.rotation = _rotationTransform.transform.rotation;


			//Vector3 tmp1 = transform.eulerAngles;
			//tmp1.y = _eyeObject.transform.eulerAngles.y;
			//transform.eulerAngles = tmp1;
			//transform.eulerAngles.y = _eyeObject.transform.eulerAngles.y;
			transform.rotation *= q;
			//to-do, need to back this up some...
		}
	}
}
