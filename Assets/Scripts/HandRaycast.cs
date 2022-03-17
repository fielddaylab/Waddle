using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;

public class HandRaycast : MonoBehaviour
{
	[SerializeField]
	GameObject _rightHand;
	
	[SerializeField]
	GameObject _leftHand;
	
	[SerializeField]
	LayerMask _mask;
	
	[SerializeField]
	GameObject _leftButton;
	
	[SerializeField]
	GameObject _rightButton;
	
	[SerializeField]
	GameObject _middleButton;
	
	[SerializeField]
	GameObject _creditsBack;
	
	[SerializeField]
	GameObject _rayStartPoint;
	
	LineRenderer _lineRenderer;

	GameObject _reticleObject;
	
	const int NUM_LAST_POSITIONS = 10;
	
	Vector3 _avgPosition = Vector3.zero;
	Vector3 _avgDirection = Vector3.zero;
	
	int _currPosition = 0;
	
	OVRHand _handTracker = null;
	
    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = transform.GetChild((int)PenguinPlayer.PenguinPlayerObjects.SELECT_LINE).gameObject.GetComponent<LineRenderer>();
		_reticleObject = transform.GetChild((int)PenguinPlayer.PenguinPlayerObjects.SELECT_DOT).gameObject;
		
		if(_rightHand != null)
		{
			_handTracker = _rightHand.transform.GetChild(1).GetComponent<OVRHand>();
		}
    }

	void SelectButton(GameObject button, bool bOn)
	{
		//assumes highlight is in child slot 1
		button.transform.GetChild(0).gameObject.SetActive(!bOn);
		button.transform.GetChild(1).gameObject.SetActive(bOn);
	}
	
    // Update is called once per frame
    void Update()
    {
		//add check for hand tracking being active...
		if(PenguinPlayer.Instance.ShowingUI && _rightHand != null && _handTracker != null)
		{
			//if(_handTracker.IsTracked && _handTracker.IsDataHighConfidence)
			{
				RaycastHit hitInfo;
				
				Vector3 castOrigin = _rightHand.transform.position - _rightHand.transform.forward*0.025f;// - _rightHand.transform.right*0.11f - _rightHand.transform.forward*0.025f - _rightHand.transform.up * 0.075f;
				
				//_avgPosition += castOrigin;
				_avgDirection += ((-_rightHand.transform.right - _rightHand.transform.up) * 0.5f);
				_currPosition++;
				if(_currPosition == 10)
				{
					_currPosition = 1;
				}
				
				_avgPosition = castOrigin;//(float)_currPosition;
				_avgDirection /= (float)_currPosition;
				_avgDirection = Vector3.Normalize(_avgDirection);
				
				//Debug.Log(_avgPosition.ToString("F3") + " " + _avgDirection.ToString("F3"));
				
				if(OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.Hands))
				{
					if(Physics.Raycast(_avgPosition, _avgDirection, out hitInfo, Mathf.Infinity, _mask, QueryTriggerInteraction.Ignore))
					{
					
						if(hitInfo.collider.transform.gameObject == _rightButton)
						{
							SelectButton(_leftButton, false);
							SelectButton(_rightButton, true);
							SelectButton(_middleButton, false);
							SelectButton(_creditsBack, false);
						}
						else if(hitInfo.collider.transform.gameObject == _leftButton)
						{
							SelectButton(_leftButton, true);
							SelectButton(_rightButton, false);
							SelectButton(_middleButton, false);	
							SelectButton(_creditsBack, false);							
						}
						else if(hitInfo.collider.transform.gameObject == _middleButton)
						{
							SelectButton(_leftButton, false);
							SelectButton(_rightButton, false);
							SelectButton(_middleButton, true);
							SelectButton(_creditsBack, false);	
						}
						else if(hitInfo.collider.transform.gameObject == _creditsBack)
						{
							SelectButton(_leftButton, false);
							SelectButton(_rightButton, false);
							SelectButton(_middleButton, false);
							SelectButton(_creditsBack, true);		
						}
						else
						{
							SelectButton(_leftButton, false);
							SelectButton(_rightButton, false);
							SelectButton(_middleButton, false);
							SelectButton(_creditsBack, false);	
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
				else if(OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.Hands))
				{
					if(Physics.Raycast(_avgPosition, _avgDirection, out hitInfo, Mathf.Infinity, _mask, QueryTriggerInteraction.Ignore))
					{

						//raycast the UI...
						//Debug.Log("Pressing button with hand");
						if(PenguinMenuSystem.Instance.GetCurrentMenu() == PenguinMenuSystem.MenuType.MainMenu)
						{
							if(hitInfo.collider.transform.gameObject == _leftButton)
							{
								PenguinGameManager.Instance.BeginTheGame(PenguinGameManager.GameMode.ShowMode);
							}
							else if(hitInfo.collider.transform.gameObject == _rightButton)
							{
								PenguinGameManager.Instance.BeginTheGame(PenguinGameManager.GameMode.HomeMode);
							}
							else if(hitInfo.collider.transform.gameObject == _middleButton)
							{
								//credits
								PenguinGameManager.Instance.ShowCredits(true);
							}
							else if(hitInfo.collider.transform.gameObject == _creditsBack)
							{
								PenguinGameManager.Instance.ShowCredits(false);
							}
						}
						else if(PenguinMenuSystem.Instance.GetCurrentMenu() == PenguinMenuSystem.MenuType.PauseMenu)
						{
							if(hitInfo.collider.transform.gameObject == _leftButton)
							{
								//resume (just close the menu)
								//PenguinPlayer.Instance.StopShowingUI();
								//restart
								PenguinGameManager.Instance.RestartGame();
							}
							else if(hitInfo.collider.transform.gameObject == _rightButton)
							{
								//Resume
								PenguinPlayer.Instance.StopShowingUI();
								//PenguinGameManager.Instance.HandleHMDUnmounted();
								//PenguinMenuSystem.Instance.ChangeMenuTo(PenguinMenuSystem.MenuType.MainMenu);
							}
							else if(hitInfo.collider.transform.gameObject == _middleButton)
							{
								//credits
								PenguinGameManager.Instance.ShowCredits(true);
							}
							else if(hitInfo.collider.transform.gameObject == _creditsBack)
							{
								PenguinGameManager.Instance.ShowCredits(false);
							}
						}
						else if(PenguinMenuSystem.Instance.GetCurrentMenu() == PenguinMenuSystem.MenuType.EndMenu)
						{
							if(hitInfo.collider.transform.gameObject == _leftButton)
							{
								//resume (just close the menu)
								//PenguinPlayer.Instance.StopShowingUI();
								//restart
								PenguinGameManager.Instance.RestartGame();
							}
							else if(hitInfo.collider.transform.gameObject == _rightButton)
							{
								//show survey
								
							}
							else if(hitInfo.collider.transform.gameObject == _middleButton)
							{
								PenguinGameManager.Instance.ShowCredits(true);
							}
							else if(hitInfo.collider.transform.gameObject == _creditsBack)
							{
								PenguinGameManager.Instance.ShowCredits(false);
							}
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
							//Debug.Log(hitInfo.collider.transform.gameObject.name);
							if(hitInfo.collider.transform.gameObject == _rightButton)
							{
								SelectButton(_leftButton, false);
								SelectButton(_rightButton, true);
								SelectButton(_middleButton, false);
								SelectButton(_creditsBack, false);
							}
							else if(hitInfo.collider.transform.gameObject == _leftButton)
							{
								SelectButton(_leftButton, true);
								SelectButton(_rightButton, false);
								SelectButton(_middleButton, false);	
								SelectButton(_creditsBack, false);							
							}
							else if(hitInfo.collider.transform.gameObject == _middleButton)
							{
								SelectButton(_leftButton, false);
								SelectButton(_rightButton, false);
								SelectButton(_middleButton, true);
								SelectButton(_creditsBack, false);	
							}
							else if(hitInfo.collider.transform.gameObject == _creditsBack)
							{
								SelectButton(_leftButton, false);
								SelectButton(_rightButton, false);
								SelectButton(_middleButton, false);
								SelectButton(_creditsBack, true);		
							}
							else
							{
								SelectButton(_leftButton, false);
								SelectButton(_rightButton, false);
								SelectButton(_middleButton, false);
								SelectButton(_creditsBack, false);	
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
							SelectButton(_leftButton, false);
							SelectButton(_rightButton, false);
							SelectButton(_middleButton, false);
							SelectButton(_creditsBack, false);	
						
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
