//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;

public class HandRaycast : MonoBehaviour
{
	public enum MenuPanel {
		eMAIN,
		eSURVEY_CODE
	};
	
	MenuPanel _panel;
	
	[SerializeField]
	GameObject _rightHand;
	
	[SerializeField]
	GameObject _leftHand;
	
	[SerializeField]
	LayerMask _mask;
	
	[SerializeField]
	MainPanel _mainPanel;
	
	[SerializeField]
	SurveyCodePanel _surveyCodePanel;
	
	LineRenderer _lineRenderer;

	GameObject _reticleObject;
	
	const int NUM_LAST_POSITIONS = 14;
	
	Vector3 _avgPosition = Vector3.zero;
	Vector3 _avgDirection = Vector3.zero;
	
	Vector3[] _accumDirection = new Vector3[NUM_LAST_POSITIONS];
	Vector3[] _accumPosition = new Vector3[NUM_LAST_POSITIONS];

	int _currPosition = 0;
	
	OVRHand _handTracker = null;
	
	bool _isLeftUsingController = false;
	bool _isRightUsingController = false;
	
    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = transform.GetChild((int)PenguinPlayer.PenguinPlayerObjects.SELECT_LINE).gameObject.GetComponent<LineRenderer>();
		_reticleObject = transform.GetChild((int)PenguinPlayer.PenguinPlayerObjects.SELECT_DOT).gameObject;
		
		if(_rightHand != null)
		{
			_handTracker = _rightHand.transform.GetChild(1).GetComponent<OVRHand>();
		}

		for(int i = 0; i < NUM_LAST_POSITIONS; ++i)
		{
			_accumDirection[i] = Vector3.zero;
			_accumPosition[i] = Vector3.zero;
		}
    }

	public void SwitchPanel(MenuPanel p)
	{
		if(p == MenuPanel.eMAIN)
		{
			_surveyCodePanel.gameObject.SetActive(false);
			_mainPanel.gameObject.SetActive(true);		
		}
		else if(p == MenuPanel.eSURVEY_CODE)
		{
			_surveyCodePanel.gameObject.SetActive(true);
			_surveyCodePanel.Reset();
			_mainPanel.gameObject.SetActive(false);
		}
		
		_panel = p;
	}
	
	public void SetRightHand(GameObject hand, bool isController=false)
	{
		_rightHand = hand;
		_isRightUsingController = isController;
	}
	
	public void SetLeftHand(GameObject hand, bool isController=false)
	{
		_leftHand = hand;
		_isLeftUsingController = isController;
	}
	
    // Update is called once per frame
    void Update()
    {
		//add check for hand tracking being active...
		if(PenguinPlayer.Instance.ShowingUI && _rightHand != null && _handTracker != null)
		{
			if((_handTracker.IsTracked && _handTracker.IsDataHighConfidence) || _isRightUsingController)
			{
				RaycastHit hitInfo;
				
				Vector3 castOrigin = _rightHand.transform.position - _rightHand.transform.forward*0.025f;// - _rightHand.transform.right*0.11f - _rightHand.transform.forward*0.025f - _rightHand.transform.up * 0.075f;
				
				//_avgPosition += castOrigin;
				_accumPosition[_currPosition] = castOrigin;
				_accumDirection[_currPosition] = Vector3.Normalize((-_rightHand.transform.right - _rightHand.transform.up) * 0.5f);
				
				//_avgPosition = castOrigin;//(float)_currPosition;
				//_avgDirection /= (float)_currPosition;
				
				Vector3 avgDir = Vector3.zero;
				Vector3 avgPos = Vector3.zero;

				for(int i = 0; i < NUM_LAST_POSITIONS; ++i)
				{
					avgDir += _accumDirection[i];
					avgPos += _accumPosition[i];
				}

				_avgPosition = avgPos / (float)NUM_LAST_POSITIONS;
				_avgDirection = avgDir / (float)NUM_LAST_POSITIONS; //_accumDirection[_currPosition];
				//_avgDirection = Vector3.Normalize(_avgDirection);
				
				_currPosition++;
				
				if(_currPosition == NUM_LAST_POSITIONS)
				{
					_currPosition = 0;
				}
				
				//Debug.Log(_avgPosition.ToString("F3") + " " + _avgDirection.ToString("F3"));
				
				//if(OVRInput.Get(OVRInput.Button.One))
				if(_handTracker.GetFingerIsPinching(OVRHand.HandFinger.Index))
				{
					if(Physics.Raycast(_avgPosition, _avgDirection, out hitInfo, Mathf.Infinity, _mask, QueryTriggerInteraction.Ignore))
					{
						
						if(_panel == HandRaycast.MenuPanel.eMAIN)
						{
							_mainPanel.HandleButtonDown(hitInfo);
						}
						else if(_panel == HandRaycast.MenuPanel.eSURVEY_CODE)
						{
							_surveyCodePanel.HandleButtonDown(hitInfo);
						}
						
						if(_reticleObject != null)
						{
							_reticleObject.transform.position = hitInfo.point;
						}
						
						if(_lineRenderer != null)
						{
							_lineRenderer.SetPosition(0, _avgPosition);
							_lineRenderer.SetPosition(1, hitInfo.point);
						}
						//Debug.Log("Hit " + hitInfo.collider.transform.gameObject.name);
					}
					else
					{
						if(_lineRenderer != null)
						{
							_lineRenderer.SetPosition(0, _avgPosition);
							_lineRenderer.SetPosition(1, _avgPosition + _avgDirection * 5f);// - _rightHand.transform.right*0.11f - _rightHand.transform.forward*0.025f - _rightHand.transform.up * 10f);
						}
						
						if(_reticleObject != null)
						{
							_reticleObject.transform.position = Vector3.zero;
						}
					}
				}
				else if(OVRInput.GetUp(OVRInput.Button.One))
				{
					if(Physics.Raycast(_avgPosition, _avgDirection, out hitInfo, Mathf.Infinity, _mask, QueryTriggerInteraction.Ignore))
					{

						//raycast the UI...
						//Debug.Log("Pressing button with hand");
						
						if(_panel == HandRaycast.MenuPanel.eMAIN)
						{
							_mainPanel.HandleButtonUp(hitInfo);
						}
						else if(_panel == HandRaycast.MenuPanel.eSURVEY_CODE)
						{
							_surveyCodePanel.HandleButtonUp(hitInfo);
						}
						
						if(_lineRenderer != null)
						{
							_lineRenderer.SetPosition(0, Vector3.zero);
							_lineRenderer.SetPosition(1, Vector3.zero);
						}
						
						if(_reticleObject != null)
						{
							_reticleObject.transform.position = Vector3.zero;
						}
					}
					else
					{
						if(_lineRenderer != null)
						{
							_lineRenderer.SetPosition(0, Vector3.zero);
							_lineRenderer.SetPosition(1, Vector3.zero);
						}
						
						if(_reticleObject != null)
						{
							_reticleObject.transform.position = Vector3.zero;
						}
					}
				}
				else
				{
					if(_reticleObject != null)
					{
						if(Physics.Raycast(_avgPosition, _avgDirection, out hitInfo, Mathf.Infinity, _mask, QueryTriggerInteraction.Ignore))
						{
							if(_panel == HandRaycast.MenuPanel.eMAIN)
							{
								_mainPanel.HandleHover(hitInfo);
							}
							else if(_panel == HandRaycast.MenuPanel.eSURVEY_CODE)
							{
								_surveyCodePanel.HandleHover(hitInfo);
							}
							
							
							if(_lineRenderer != null)
							{
								_lineRenderer.SetPosition(0, _avgPosition);
								_lineRenderer.SetPosition(1, hitInfo.point);
							}
							
							_reticleObject.transform.position = hitInfo.point;
							
						}
						else
						{
							if(_panel == HandRaycast.MenuPanel.eMAIN)
							{
								_mainPanel.HandleNoHit(hitInfo);
							}
							else if(_panel == HandRaycast.MenuPanel.eSURVEY_CODE)
							{
								_surveyCodePanel.HandleNoHit(hitInfo);
							}
							
							_reticleObject.transform.position = Vector3.zero;
							
							if(_lineRenderer != null)
							{
								_lineRenderer.SetPosition(0, _avgPosition);
								_lineRenderer.SetPosition(1, _avgPosition + _avgDirection * 5f);// - _rightHand.transform.right*0.11f - _rightHand.transform.forward*0.025f - _rightHand.transform.up * 10f);
							}
						}
					}
				}
			}
		}
    }
}
