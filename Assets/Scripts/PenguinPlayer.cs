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
	
    // Start is called before the first frame update
    void Start()
    {
        
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
				}
				else
				{
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
