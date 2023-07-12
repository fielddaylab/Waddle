using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsControllerSwitcher : MonoBehaviour
{
	[SerializeField]
	GameObject _leftHand;
	
	[SerializeField]
	GameObject _rightHand;
	
	[SerializeField]
	GameObject _leftController;
	
	[SerializeField]
	GameObject _rightController;
	
	[SerializeField]
	GameObject _leftIK;
	
	[SerializeField]
	GameObject _rightIK;
	
	bool _wasHandTrackingEnabled = true;
	bool _wasControllersEnabled = false;
	
	HandRaycast _rayCaster = null;
	
	
    // Start is called before the first frame update
    void Start()
    {
        _rayCaster = GetComponent<HandRaycast>();
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRPlugin.GetHandTrackingEnabled())
		{
			if(!_wasHandTrackingEnabled)
			{
				_leftHand.SetActive(true);
				_rightHand.SetActive(true);
				_leftController.SetActive(false);
				_rightController.SetActive(false);
				_wasHandTrackingEnabled = true;
				_wasControllersEnabled = false;
				
				if(_rayCaster != null)
				{
					_rayCaster.SetLeftHand(_leftHand);
					_rayCaster.SetRightHand(_rightHand);
				}
				
				if(_leftIK != null)
				{
					_leftIK.GetComponent<FollowTransform>()._followTransform = _leftHand;
				}
				
				if(_rightIK != null)
				{
					_rightIK.GetComponent<FollowTransform>()._followTransform = _rightHand;
				}
			}
		}
		else
		{
			if(!_wasControllersEnabled)
			{
				_leftHand.SetActive(false);
				_rightHand.SetActive(false);
				_leftController.SetActive(true);
				_rightController.SetActive(true);
				_wasControllersEnabled = true;
				_wasHandTrackingEnabled = false;
				
				if(_rayCaster != null)
				{
					_rayCaster.SetLeftHand(_leftController, true);
					_rayCaster.SetRightHand(_rightController, true);
				}
				
				if(_leftIK != null)
				{
					_leftIK.GetComponent<FollowTransform>()._followTransform = _leftController;
				}
				
				if(_rightIK != null)
				{
					_rightIK.GetComponent<FollowTransform>()._followTransform = _rightController;
				}
			}
		}
    }
}
