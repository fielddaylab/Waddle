using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
	public GameObject _followTransform;

	Camera _mainCam = null;

    // Start is called before the first frame update
    void Start()
    {
		_mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(_followTransform != null)
		{
			if(_followTransform.transform.childCount > 1)
			{
				OVRHand h = _followTransform.transform.GetChild(1).GetComponent<OVRHand>();
				if(!h.IsDataHighConfidence)
				{
					//if losing tracking - force the hand IK end straight out from the 
					if(h.HandType == OVRHand.Hand.HandLeft)
					{
						transform.position = _mainCam.transform.position - _mainCam.transform.up * 2.0f;//- _mainCam.transform.right * 3.0f;
					}
					else
					{
						transform.position = _mainCam.transform.position - _mainCam.transform.up * 2.0f;// _mainCam.transform.right * 3.0f;
					}
				}
				else
				{
					transform.position = _followTransform.transform.position;
				}
			}
			else
			{
				transform.position = _followTransform.transform.position;
			}
		}
    }
	
	void LateUpdate()
	{
		
	}
}
