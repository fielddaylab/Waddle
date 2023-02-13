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
	GameObject _waddleIndicatorLeft;
	
	[SerializeField]
	GameObject _waddleIndicatorRight;
	
	[SerializeField]
	GameObject _centerEye;
	
	[SerializeField]
	GameObject _leftHand;
	
	[SerializeField]
	GameObject _rightHand;
	
	[SerializeField]
	LayerMask _mask;
	
	LineRenderer _lineRenderer;
	
	bool _showingUI = true;
	public bool ShowingUI => _showingUI;
	
	bool _wasShowingUserMessage = false;

	GameObject _lastGazeObject = null;
	float _gazeTimer = 0f;
	const float GAZE_TIMER_SEND = 0.5f;
	string _currentRegion = "";
	
	const float GAZE_LOG_TIMER_SEND = 1.0f;
	float _gazeLogTimer = 0f;
	uint _gazeLogFrameCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = transform.GetChild((int)PenguinPlayerObjects.SELECT_LINE).gameObject.GetComponent<LineRenderer>();
		
		ResetGazeLogging();
    }
	
	public void ResetGazeLogging()
	{
		float t = UnityEngine.Time.time;
		_gazeTimer = t;
		_gazeLogTimer = t;
		_gazeLogFrameCount = 0;
	}
	
	public void GetGaze(out Vector3 pos, out Quaternion view)
	{
		pos = transform.position;
		view = Quaternion.identity;
		
		if(_centerEye != null)
		{
			view = _centerEye.transform.rotation;
		}
	}
	
	public void GazeRaycast(Vector3 rayStart, Vector3 rayDir, out GameObject gazeObject)
	{
		RaycastHit hitInfo;
		
		gazeObject = null;
		
		if(Physics.Raycast(rayStart, rayDir, out hitInfo, Mathf.Infinity, _mask, QueryTriggerInteraction.Ignore))
		{
			gazeObject = hitInfo.collider.gameObject;
		}
	}
	
	public void GetHandTransform(bool leftHand, out Vector3 pos, out Quaternion rot)
	{
		if(leftHand)
		{
			if(_leftHand != null)
			{
				pos = _leftHand.transform.position;
				rot = _leftHand.transform.rotation;
			}
			else
			{
				pos = Vector3.zero;
				rot = Quaternion.identity;
			}
		}
		else
		{
			if(_rightHand != null)
			{
				pos = _rightHand.transform.position;
				rot = _rightHand.transform.rotation;
			}
			else
			{
				pos = Vector3.zero;
				rot = Quaternion.identity;	
			}
		}
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
		PenguinAnalytics.Instance.LogMenuAppeared();
		
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
			PenguinAnalytics.Instance.LogCloseMenu();
		}
		
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

				PenguinAnalytics.Instance.LogOpenMenu();
			}
		}
		
		PenguinGameManager._isGamePaused = true;
		
		_wasShowingUserMessage = UserMessageActive();
		if(_wasShowingUserMessage)
		{
			HideUserMessage();
		}

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

		float t = UnityEngine.Time.time;
		//this gaze is for objects...
		if(t - _gazeTimer > GAZE_TIMER_SEND)
		{
			_gazeTimer = t;
			Vector3 pos = Vector3.zero;
			Quaternion quat = Quaternion.identity;
			GetGaze(out pos, out quat);

			//PenguinAnalytics.Instance.LogGaze(pos, quat, true);

			//check if we're in a certain region here...

			GameObject gazeObject = null;
			GazeRaycast(pos, _centerEye.transform.forward, out gazeObject);
			
			if(gazeObject != null)
			{
				if(gazeObject != _lastGazeObject)
				{
					if(_lastGazeObject != null)
					{
						//end gaze log
						PenguinAnalytics.Instance.LogGazeEnd(gazeObject.name);
					}
					
					_lastGazeObject = gazeObject;
					
					PenguinAnalytics.Instance.LogGazeBegin(gazeObject.name);
				}
				else
				{
					//log begin gaze new object - if we reach here it means we've been gazing on it for at least GAZE_TIMER_SEND seconds...
					PenguinAnalytics.Instance.LogGazeBegin(gazeObject.name);
				}
			}
			else
			{
				if(_lastGazeObject != null)
				{
					//log end gaze...
					PenguinAnalytics.Instance.LogGazeEnd(_lastGazeObject.name);
					_lastGazeObject = null;
				}
			}
		}
		
		if(t - _gazeLogTimer > GAZE_LOG_TIMER_SEND)
		{
			_gazeLogTimer = t;
			
			Vector3 pos = Vector3.zero;
			Quaternion quat = Quaternion.identity;
			GetGaze(out pos, out quat);

			PenguinAnalytics.Instance.LogGaze(pos, quat, _gazeLogFrameCount, true);
			
			_gazeLogFrameCount++;
		}
		else
		{
			if(_gazeLogFrameCount % 2 == 0)
			{
				Vector3 pos = Vector3.zero;
				Quaternion quat = Quaternion.identity;
				GetGaze(out pos, out quat);

				bool sentData = PenguinAnalytics.Instance.LogGaze(pos, quat, _gazeLogFrameCount);
				if(sentData)
				{
					_gazeLogTimer = t;
				}
			}
			
			_gazeLogFrameCount++;
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
