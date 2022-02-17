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
	
	[SerializeField]
	GameObject _waddleIndicatorLeft;
	
	[SerializeField]
	GameObject _waddleIndicatorRight;
	
	LineRenderer _lineRenderer;
	
	bool _showingUI = false;
	public bool ShowingUI => _showingUI;
	
	bool _wasShowingUserMessage = false;
	
    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = transform.GetChild(5).gameObject.GetComponent<LineRenderer>();
    }
	
	public void StopBackgroundMusic()
	{
		AudioSource aSource = GetComponent<AudioSource>();
		if(aSource != null)
		{
			aSource.Stop();
		}
	}
	
	public void StartBackgroundMusic()
	{
		AudioSource aSource = GetComponent<AudioSource>();
		if(aSource != null)
		{
			aSource.Play();
		}
	}
	
	public void HideUserMessage()
	{
		//just for pause screen..
		if(_userMessageUI != null)
		{
			_userMessageUI.SetActive(false);
		}
	}
	
	public void ShowUserMessage()
	{
		if(_userMessageUI != null)
		{
			_userMessageUI.SetActive(true);
		}
	}
	
	public void ForceUserMessageOff()
	{
		//for restarting the game
		if(_userMessageUI != null)
		{
			_userMessageUI.GetComponent<UserMessage>().ForceMessageOff();
		}
	}
	
	public bool UserMessageActive()
	{
		if(_userMessageUI != null)
		{
			return _userMessageUI.GetComponent<UserMessage>().ShowingMessage;
		}
		
		return false;
	}
	
	public void StopShowingUI()
	{
		_showingUI = false;
		UnityEngine.Time.timeScale = 1;
		AudioListener.pause = false;
		if(_wasShowingUserMessage)
		{
			ShowUserMessage();
			_wasShowingUserMessage = false;
		}
		
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
		//hand reticle
		transform.GetChild(6).gameObject.GetComponent<MeshRenderer>().enabled = false;
		
		_lineRenderer.enabled = false;
	}
	
	IEnumerator BlinkIndicators(float blinkFreq, float totalTime)
	{
		float startTime = UnityEngine.Time.time;
		
		Color cStart = Color.black;
		if(_waddleIndicatorLeft != null)
		{
			cStart = _waddleIndicatorLeft.GetComponent<MeshRenderer>().sharedMaterial.color;
			_waddleIndicatorLeft.GetComponent<MeshRenderer>().enabled = true;
		}
		
		if(_waddleIndicatorRight != null)
		{
			_waddleIndicatorRight.GetComponent<MeshRenderer>().enabled = true;
		}
		
		while(UnityEngine.Time.time - startTime < totalTime)
		{
			Color c = _waddleIndicatorLeft.GetComponent<MeshRenderer>().sharedMaterial.color;
			c.a = Mathf.PingPong(UnityEngine.Time.time, 1f);
			_waddleIndicatorLeft.GetComponent<MeshRenderer>().sharedMaterial.color = c;

			yield return null;
		}
		
		if(_waddleIndicatorLeft != null)
		{
			_waddleIndicatorLeft.GetComponent<MeshRenderer>().enabled = false;
			
			_waddleIndicatorLeft.GetComponent<MeshRenderer>().sharedMaterial.color = cStart;
		}
		
		if(_waddleIndicatorRight != null)
		{
			_waddleIndicatorRight.GetComponent<MeshRenderer>().enabled = false;
		}
	}
	
	public void StartShowingUI()
	{
		_showingUI = true;
		_mainUI.SetActive(true);
		UnityEngine.Time.timeScale = 0;
		AudioListener.pause = true;
		
		_wasShowingUserMessage = UserMessageActive();
		if(_wasShowingUserMessage)
		{
			HideUserMessage();
		}

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
		
		//hand reticle
		transform.GetChild(6).gameObject.GetComponent<MeshRenderer>().enabled = true;
		
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
			
			StartCoroutine(BlinkIndicators(0.5f, showDuration));
		}
	}
	
	public void ShowEndGamePrefab()
	{
		//play the snow storm...
		transform.GetChild(4).gameObject.SetActive(true);
	}
}
