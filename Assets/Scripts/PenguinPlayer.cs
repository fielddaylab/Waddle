//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinPlayer : Singleton<PenguinPlayer>
{
	public enum PenguinPlayerObjects
	{
		CONTROLLER,
		PENGUIN_BODY,
		MIRROR_BODY,
		NAV_RING,
		SNOW_RING,
		SELECT_LINE,
		SELECT_DOT,
		PAUSE_UI
	}
	
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
	
	bool _showingUI = true;
	public bool ShowingUI => _showingUI;
	
	bool _wasShowingUserMessage = false;
	
    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = transform.GetChild((int)PenguinPlayerObjects.SELECT_LINE).gameObject.GetComponent<LineRenderer>();
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
	
	public void SlowDownMovement()
	{
		transform.GetChild(3).GetChild(0).GetComponent<WaddleTrigger>().Speed = 2f;
		transform.GetChild(3).GetChild(1).GetComponent<WaddleTrigger>().Speed = 2f;
	}
	
	public void SpeedUpMovement()
	{
		transform.GetChild(3).GetChild(0).GetComponent<WaddleTrigger>().Speed = 20f;
		transform.GetChild(3).GetChild(1).GetComponent<WaddleTrigger>().Speed = 20f;
	}
	
	public void HideMenu()
	{
		_mainUI.SetActive(false);
		//_showingUI = false;
	}
	
	public void ResetHeight()
	{	
		transform.GetChild(0).GetChild(1).GetComponent<HeightSetter>().ResetHeight();
	}
	
	public void ShowMenu()
	{
		_mainUI.SetActive(true);
		//_showingUI = false;
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
		
		if(_mainUI != null)
		{
			_mainUI.SetActive(false);
		}
		//switch our hands...
		/*if(_leftHandController != null)
		{
			_leftHandController.transform.GetChild(1).gameObject.SetActive(true);
			_leftHandController.transform.GetChild(2).gameObject.SetActive(false);
		}
		
		if(_rightHandController != null)
		{
			_rightHandController.transform.GetChild(1).gameObject.SetActive(true);
			_rightHandController.transform.GetChild(2).gameObject.SetActive(false);
		}*/
		
		EnableMovement();
		
		//need a way to stop physics for everything but colliding with the pause screen buttons...
		//hand reticle
		transform.GetChild((int)PenguinPlayerObjects.SELECT_DOT).gameObject.GetComponent<MeshRenderer>().enabled = false;
		
		if(_lineRenderer != null)
		{
			_lineRenderer.enabled = false;
		}
		
		Physics.autoSimulation = true;
		PenguinGameManager._isGamePaused = false;
	}
	
	/*void OnTriggerEnter(Collider otherCollider)
	{
		Debug.Log(otherCollider.gameObject.name);
	}*/
	
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
	
	public void StartShowingUI(bool isStartMenu=false, bool skipUI=false)
	{
		if(!skipUI)
		{
			_showingUI = true;
			
			if(_mainUI != null)
			{
				_mainUI.SetActive(true);
			}
		}
		
		PenguinGameManager._isGamePaused = true;
		
		_wasShowingUserMessage = UserMessageActive();
		if(_wasShowingUserMessage)
		{
			HideUserMessage();
		}

		/*if(_leftHandController != null)
		{
			_leftHandController.transform.GetChild(1).gameObject.SetActive(false);
			_leftHandController.transform.GetChild(2).gameObject.SetActive(true);
		}
		
		if(_rightHandController != null)
		{
			_rightHandController.transform.GetChild(1).gameObject.SetActive(false);
			_rightHandController.transform.GetChild(2).gameObject.SetActive(true);
		}*/
		
		DisableMovement();
		
		//hand reticle
		transform.GetChild((int)PenguinPlayerObjects.SELECT_DOT).gameObject.GetComponent<MeshRenderer>().enabled = true;
		
		if(_lineRenderer != null)
		{
			_lineRenderer.enabled = true;	
		}
		
		if(!isStartMenu)
		{
			UnityEngine.Time.timeScale = 0;
			Physics.autoSimulation = false;
			AudioListener.pause = true;
		}
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
		transform.GetChild((int)PenguinPlayerObjects.NAV_RING).gameObject.SetActive(false);
	}
	
	public void EnableMovement()
	{
		transform.GetChild((int)PenguinPlayerObjects.NAV_RING).gameObject.SetActive(true);
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
		transform.GetChild((int)PenguinPlayerObjects.SNOW_RING).gameObject.SetActive(true);
	}
	
	public void HideEndGamePrefab()
	{
		//play the snow storm...
		transform.GetChild((int)PenguinPlayerObjects.SNOW_RING).gameObject.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);
		transform.GetChild((int)PenguinPlayerObjects.SNOW_RING).gameObject.SetActive(false);
	}
}
