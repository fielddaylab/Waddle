using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalLocomotion : MonoBehaviour
{
	public GameObject _centerEye;
	public GameObject _leftHand;
	public GameObject _rightHand;
	//could also adjust forward direction based on hands
	public GameObject _forwardDirection;
	
	public float _speed = 20f;
	
	private Vector3 _playerLastFrame;
	private Vector3 _leftHandLastFrame;
	private Vector3 _rightHandLastFrame;
	
	private OVRHand _trackedLeftHand;
	private OVRHand _trackedRightHand;
	
    // Start is called before the first frame update
    void Start()
    {
        SetLastPositions();
		
		if(_leftHand != null)
		{
			_trackedLeftHand = _leftHand.transform.GetChild(1).GetComponent<OVRHand>();
		}
		
		if(_rightHand != null)
		{
			_trackedRightHand = _rightHand.transform.GetChild(1).GetComponent<OVRHand>();
		}
    }

    // Update is called once per frame
    void Update()
    {
        float playerDist = Vector3.Distance(_playerLastFrame, transform.position);
		float leftDist = Vector3.Distance(_leftHandLastFrame, _leftHand.transform.position);
		float rightDist = Vector3.Distance(_rightHandLastFrame, _rightHand.transform.position);
		
		if(_trackedLeftHand.IsTracked && _trackedLeftHand.IsDataHighConfidence && 
			_trackedRightHand.IsTracked && _trackedRightHand.IsDataHighConfidence && 
			(leftDist > 0.01f || rightDist > 0.01f))
		{
			float handSpeed = (leftDist - playerDist) + (rightDist - playerDist);
			
			if(handSpeed > 1f)
			{
				handSpeed = 1f;
			}
			
			if(Time.timeSinceLevelLoad > 1f)
			{
				transform.position -= _forwardDirection.transform.forward * handSpeed * _speed * Time.deltaTime; 
			}
		}
		
		SetLastPositions();
    }
	
	void SetLastPositions()
	{
		_playerLastFrame = transform.position;
		_leftHandLastFrame = _leftHand.transform.position;
		_rightHandLastFrame = _rightHand.transform.position;
	}
}
