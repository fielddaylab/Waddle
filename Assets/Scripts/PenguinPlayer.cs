using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinPlayer : Singleton<PenguinPlayer>
{
	[SerializeField]
	GameObject _userMessageUI;
	
	[SerializeField]
	GameObject _mainUI;
	
	[SerializeField]
	GameObject _leftHandController;
	
	[SerializeField]
	GameObject _rightHandController;
	
	LineRenderer _lineRenderer;
	
	bool _showingUI = false;
	public bool ShowingUI => _showingUI;
	
    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = transform.GetChild(5).gameObject.GetComponent<LineRenderer>();
    }

	public void StopShowingUI()
	{
		_showingUI = false;
					
		_mainUI.SetActive(false);
		//switch our hands...
		if(_leftHandController != null)
		{
			_leftHandController.transform.GetChild(1).gameObject.SetActive(true);
			_leftHandController.transform.GetChild(2).gameObject.SetActive(false);
		}
		
		if(_rightHandController != null)
		{
			_rightHandController.transform.GetChild(1).gameObject.SetActive(true);
			_rightHandController.transform.GetChild(2).gameObject.SetActive(false);
		}
		
		EnableMovement();
		
		//need a way to stop physics for everything but colliding with the pause screen buttons...
		
		_lineRenderer.enabled = false;
	}
	
	public void StartShowingUI()
	{
		_showingUI = true;
		_mainUI.SetActive(true);
		if(_leftHandController != null)
		{
			_leftHandController.transform.GetChild(1).gameObject.SetActive(false);
			_leftHandController.transform.GetChild(2).gameObject.SetActive(true);
		}
		
		if(_rightHandController != null)
		{
			_rightHandController.transform.GetChild(1).gameObject.SetActive(false);
			_rightHandController.transform.GetChild(2).gameObject.SetActive(true);
		}
		
		DisableMovement();
		
		_lineRenderer.enabled = true;	
	}
	
    // Update is called once per frame
    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Start))
		{
			//this should bring up the UI...
			//
			if(_mainUI != null)
			{
				if(_mainUI.activeSelf)
				{
					StopShowingUI();
				}
				else
				{
					StartShowingUI();
				}
			}
		}
    }
	
	public void DisableMovement()
	{
		transform.GetChild(3).gameObject.SetActive(false);
	}
	
	public void EnableMovement()
	{
		transform.GetChild(3).gameObject.SetActive(true);
	}
	
	public void ShowWaddleMessage(float showDuration)
	{
		//store a game object for the UserMessage object..
		if(_userMessageUI != null)
		{
			_userMessageUI.GetComponent<UserMessage>().StartShowMessage("", showDuration);
		}
	}
	
	public void ShowEndGamePrefab()
	{
		//play the snow storm...
		transform.GetChild(4).gameObject.SetActive(true);
	}
}
