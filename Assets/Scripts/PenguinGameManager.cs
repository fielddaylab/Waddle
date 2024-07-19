//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;
using FieldDay;
using Waddle;

public class PenguinGameManager : Singleton<PenguinGameManager>
{
	public List<MiniGameUnlocker> _miniGames = new List<MiniGameUnlocker>();

	public enum MiniGame
	{
		ProtectTheNest,
		MatingDance
	}
	
	public enum GameMode
	{
		ShowMode,
		HomeMode,
		ResearchMode
	}
	
	[SerializeField]
	GameMode _gameMode;
	
	public GameMode GetGameMode => _gameMode;

    [SerializeField]
	float _targetFrameRate = 72f;
	
	[SerializeField]
	GameObject _playerStartLocation;

	ProtectTheNest _nestGame = null;
	MatingDance _matingDance = null;

	bool _showingEndGamePrefab = false;
	
	float _overallStartTime = 0f;
	
	float _totalGameTime = 0f;
	
	[SerializeField]
	float _showModeTimeLimit = 420f;	//7 minutes
	
	float _homeModeTimeLimit = 999999f;
	
	bool _wasUnmounted = false;
	
	public static bool _isGamePaused = true;
	
	public static bool _isInMiniGame = false;
	
	bool _wasInMiniGame = false;

    public static bool _headMovementActive = true; // disabled during certain minigames

    bool _gameWasStarted = false;
	public bool GameWasStarted => _gameWasStarted;
	
	public delegate void OnResetDelegate();
	public static event OnResetDelegate OnReset;
	
	[SerializeField]
	GameObject _creditsLocation;
	
	[SerializeField]
	GameObject _surveyLocation;
	
	[SerializeField]
	GameObject _sessionIDLocation;
	
	[SerializeField]
	bool _demoMode = false;	//demo mode makes it so the game will auto-restart if taking off and putting on the HMD

	static Vector3 _priorLocation = Vector3.zero;
	
	bool _logAppStarted = false;
	
	float _gamePausedTime = 0f;
	
	const float GAME_RESTART_TIME = 15f;
	
    [InvokePreBoot]
    static private void PreBoot() {
        Game.SetEventDispatcher(new EventDispatcher<object>());
    }

    void Start()
    {
		//temporary for demo as we're just restarting one game.
		//this appears to not function with multiple scenes at the moment
        //_nestGame = GameObject.Find("ProtectTheNest").GetComponent<ProtectTheNest>();
		/*if (Unity.XR.Oculus.Performance.TryGetDisplayRefreshRate(out var rate))
		{
			float newRate = 120f; // fallback to this value if the query fails.
			float[] rates = new float[10];
			if (Unity.XR.Oculus.Performance.TryGetAvailableDisplayRefreshRates(out var rates))
			{
				newRate = rates.Max();
			}
			if (rate < newRate)
			{*/
		//float newRate = 90.0f;

		if(_targetFrameRate != 72f)
		{
			if (Unity.XR.Oculus.Performance.TrySetDisplayRefreshRate(_targetFrameRate))
			{
				Time.fixedDeltaTime = 1f / _targetFrameRate;
				Time.maximumDeltaTime = 1f / _targetFrameRate;
			}
		}
        //}
        //}

        //UnityEngine.Time.timeScale = 0;
        //PenguinPlayer.Instance.StopBackgroundMusic();

        //AudioListener.pause = true;

        //StartCoroutine(ShowMessage("", 5f, 10f));

        //uncomment this if wanting to test things in editor without hmd
        BeginTheGame(PenguinGameManager.GameMode.ShowMode);
		//Debug.Log("GAME BEGUN?");
    }
	
	public void BeginTheGame(PenguinGameManager.GameMode mode)
	{
		PenguinAnalytics.Instance.StartAnalytics();
		PenguinAnalytics.Instance.LogStartGame();
		
		if(_sessionIDLocation != null)
		{
			_sessionIDLocation.GetComponent<TMPro.TextMeshPro>().text = PenguinAnalytics.Instance.GetSessionID().ToString();
		}
		
		PenguinPlayer.Instance.ResetGazeLogging();
		
		if(_gameMode == GameMode.ShowMode)
		{
			PenguinAnalytics.Instance.LogBeginMode("SHOW_MODE");
			PenguinAnalytics.Instance.LogTimerBegin(_showModeTimeLimit);
		}
		else
		{
			if(_gameMode == GameMode.ResearchMode)
			{
				PenguinAnalytics.Instance.LogBeginMode("RESEARCH_MODE");
			}
			else
			{
				PenguinAnalytics.Instance.LogBeginMode("HOME_MODE");
			}
			PenguinAnalytics.Instance.LogTimerBegin(_homeModeTimeLimit);

		}
		
		PenguinPlayer.Instance.StopShowingUI();
		_gameMode = mode;
		_overallStartTime = UnityEngine.Time.time;
		_totalGameTime = 0f;
		
		Physics.autoSimulation = true;
		UnityEngine.Time.timeScale = 1;
		//AudioListener.pause = false;
		PenguinPlayer.Instance.StartBackgroundMusic();
		_isGamePaused = false;
		_gameWasStarted = true;
		PenguinMenuSystem.Instance.ChangeMenuTo(PenguinMenuSystem.MenuType.PauseMenu);
	}
	
	IEnumerator ShowMessage(string message, float startDuration, float duration)
	{
		yield return new WaitForSeconds(startDuration);
		
		PenguinPlayer.Instance.ShowWaddleMessage(duration);
	}
	
	void SetTheNest()
	{
		if(_nestGame == null)
		{
			GameObject ptn = GameObject.Find("ProtectTheNest");
			if(ptn != null)
			{
				_nestGame = ptn.GetComponent<ProtectTheNest>();
			}
		}
	}
	
	void SetMatingDance()
	{
		if(_matingDance == null)
		{
			GameObject md = GameObject.Find("MatingDance");
			if(md != null)
			{
				_matingDance = md.GetComponent<MatingDance>();
			}
		}
	}
	
	public void HandleHMDMounted()
	{
		PenguinAnalytics.Instance.LogHeadsetOn();
		
		PenguinPlayer.Instance.StopShowingUI(false);
		
		//StartCoroutine(ShowMessage("", 5f, 10f));
		if(_wasUnmounted)
		{
			if(_demoMode)
			{
				if(_gameWasStarted)
				{
					RestartGame();
				}
				else
				{
					BeginTheGame(PenguinGameManager.GameMode.ShowMode);
				}
			}
			else
			{
				if(UnityEngine.Time.realtimeSinceStartup - _gamePausedTime > GAME_RESTART_TIME) 
				{
					if(_gameWasStarted)
					{
						RestartGame();
					}
					else
					{
						BeginTheGame(PenguinGameManager.GameMode.ShowMode);
					}
				}
			}
			
			PenguinPlayer.Instance.ResetHeight();
            PlayerHeadUtility.ResetHeadToBody(Game.SharedState.Get<PlayerHeadState>());

            if (_gameMode == GameMode.ResearchMode)
			{
				//show the menu w/ the key entry panel...	
				PenguinPlayer.Instance.gameObject.GetComponent<HandRaycast>().SwitchPanel(HandRaycast.MenuPanel.eSURVEY_CODE);
				PenguinPlayer.Instance.StartShowingUI();
			}
			
			_isGamePaused = false;
		}
		else
		{
			if(_demoMode)
			{
				if(_gameWasStarted)
				{
					RestartGame();
				}
				else
				{
					BeginTheGame(PenguinGameManager.GameMode.ShowMode);
				}
				
				PenguinPlayer.Instance.ResetHeight();
			}
			else
			{
				//if(_gameMode == GameMode.ResearchMode)
				{
					PenguinPlayer.Instance.ResetHeight();
				}
				//PenguinPlayer.Instance.StartShowingUI(true);
			}
		}
		
		//if(_gameMode != GameMode.ResearchMode)
		//{
		//	PenguinPlayer.Instance.ResetHeight();
		//}

		//PenguinAnalytics.Instance.LogStartGame();
	}
	
	public void RestartGame()
	{
		//if we had been playing previously...
		if(_totalGameTime != 0f)
		{
			PenguinPlayer.Instance.HideEndGamePrefab();
			OVRScreenFade.instance.FadeIn();
		}

		_overallStartTime = UnityEngine.Time.time;
		_totalGameTime = 0f;
		if(_gameMode == GameMode.ShowMode)
		{
			PenguinAnalytics.Instance.LogBeginMode("SHOW_MODE");
			PenguinAnalytics.Instance.LogTimerBegin(_showModeTimeLimit);
		}
		else
		{
			if(_gameMode == GameMode.ResearchMode)
			{
				PenguinAnalytics.Instance.LogBeginMode("RESEARCH_MODE");
			}
			else
			{
				PenguinAnalytics.Instance.LogBeginMode("HOME_MODE");
			}
			PenguinAnalytics.Instance.LogTimerBegin(_homeModeTimeLimit);
		}
		
		//AudioListener.pause = false;
		PenguinPlayer.Instance.StartBackgroundMusic();
        PenguinPlayer.Instance.transform.SetPositionAndRotation(_playerStartLocation.transform.position, _playerStartLocation.transform.rotation);
        PlayerHeadUtility.ResetHeadToBody(Game.SharedState.Get<PlayerHeadState>());

        Game.SharedState.Get<ManualCullingReference>().RegionsDirty = true;
        Game.SharedState.Get<PlayerVignetteState>().FadeEnabled = true;

        PenguinAnalytics.Instance.LogStartGame();
		if(_sessionIDLocation != null)
		{
			_sessionIDLocation.GetComponent<TMPro.TextMeshPro>().text = PenguinAnalytics.Instance.GetSessionID().ToString();
		}
		
		PenguinPlayer.Instance.ResetGazeLogging();
		
		if(_nestGame != null)
		{
			_nestGame.RestartGame();
			
			UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(MiniGame.ProtectTheNest.ToString());
		}
		
		if(_matingDance != null)
		{
			_matingDance.RestartGame();
			
			UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(MiniGame.MatingDance.ToString());
		}
		
		OnReset();
		
		PenguinPlayer.Instance.StopShowingUI();
        PenguinGameManager._headMovementActive = true;

        PenguinMenuSystem.Instance.ChangeMenuTo(PenguinMenuSystem.MenuType.PauseMenu);
		
		if(_gameMode != GameMode.ResearchMode)
		{
			PenguinPlayer.Instance.gameObject.GetComponent<HandRaycast>().SwitchPanel(HandRaycast.MenuPanel.eMAIN);
		}
		else
		{
			PenguinPlayer.Instance.gameObject.GetComponent<HandRaycast>().SwitchPanel(HandRaycast.MenuPanel.eSURVEY_CODE);
		}
	}
	
	public void HandleHMDUnmounted()
	{
		//Debug.Log("UNMOUNTED");
		PenguinAnalytics.Instance.LogHeadsetOff();
		
		_wasUnmounted = true;
		
		_gamePausedTime = UnityEngine.Time.realtimeSinceStartup;
		
		PenguinPlayer.Instance.StartShowingUI(false, true);
		/*if(!_demoMode)
		{
			PenguinPlayer.Instance.StartShowingUI(false);
		}
		else
		{
			PenguinPlayer.Instance.StartShowingUI(false, true);
		}*/
		_isGamePaused = true;
		//PenguinAnalytics.Instance.LogTimerPause();
	}
	
	public void ShowEndGameMenu()
	{
		_wasUnmounted = true;
		if(!_demoMode)
		{
			PenguinPlayer.Instance.StartShowingUI(false);
		}
		else
		{
			PenguinPlayer.Instance.StartShowingUI(false, true);
		}
		
		_isGamePaused = true;
		//PenguinAnalytics.Instance.LogTimerPause();	
	}


    public void ShowEndGameText() {
        PenguinPlayer.Instance.StartShowingUI(true, false);
    }

    public void ShowCredits(bool toCredits)
	{
		if(toCredits)
		{
			_priorLocation = PenguinPlayer.Instance.transform.position;
			
			if(_creditsLocation != null)
			{
				PenguinPlayer.Instance.transform.position = _creditsLocation.transform.position;
			}
			
			PenguinPlayer.Instance.HideMenu();
			//PenguinPlayer.Instance.DisableMovement();
		}
		else
		{
			PenguinPlayer.Instance.ShowMenu();
			PenguinPlayer.Instance.transform.position = _priorLocation;
			//if(_gameWasStarted)
			//{
			//	PenguinPlayer.Instance.EnableMovement();
			//}
		}
	}
	
	public void ShowSurvey(bool toSurvey)
	{
		if(toSurvey)
		{
			_priorLocation = PenguinPlayer.Instance.transform.position;
			
			if(_creditsLocation != null)
			{
				//PenguinPlayer.Instance.transform.position = _surveyLocation.transform.position;
			}
			
			PenguinPlayer.Instance.HideMenu();
		}
		else
		{
			PenguinPlayer.Instance.ShowMenu();
			PenguinPlayer.Instance.transform.position = _priorLocation;
		}
	}
	
    // Update is called once per frame
    void Update()
    {
		/*if(!_gameWasStarted)
		{
			PenguinPlayer.Instance.StartShowingUI(true);
			_gameWasStarted = true;
		}*/
		
		if(!_logAppStarted)
		{
			//PenguinAnalytics.Instance.LogApplicationStart();
			_logAppStarted = true;
			
			OVRManager.HMDUnmounted += HandleHMDUnmounted;
			OVRManager.HMDMounted += HandleHMDMounted;
		}

		if(!_isGamePaused)
		{
			_totalGameTime += UnityEngine.Time.deltaTime;
		}
		else
		{
			if(Time.timeScale == 0f)
			{
				Physics.Simulate(Time.fixedDeltaTime);
			}
		}
		
		if(_gameMode == GameMode.ShowMode)
		{
			//7 minutes
			if(_totalGameTime > _showModeTimeLimit)
			{
				if(!_isInMiniGame)
				{
					if(!_showingEndGamePrefab)
					{
						PenguinAnalytics.Instance.LogTimerExpired();

						if(_wasInMiniGame)
						{
							StartCoroutine(StartBlizzard(15f, 6f));
							StartCoroutine(FinishEndGame(25f));
						}
						else
						{
							StartCoroutine(StartBlizzard(0f, 6f));
							StartCoroutine(FinishEndGame(10f));
						}
					}
				}
				else
				{
					_wasInMiniGame = true;
				}
			}	
		}
    }
	
	IEnumerator StartBlizzard(float initialWaitDuration, float approachDuration)
	{
		yield return new WaitForSeconds(initialWaitDuration);
		
		PenguinPlayer.Instance.ShowEndGamePrefab();
		Vector3 startScale = PenguinPlayer.Instance.transform.GetChild((int)PenguinPlayer.PenguinPlayerObjects.SNOW_RING).localScale;
		Vector3 endScale = new Vector3(0.5f, 0.5f, 0.5f);
		
		_showingEndGamePrefab = true;

		float t = 0f;
		
		while(t < approachDuration)
		{
			PenguinPlayer.Instance.transform.GetChild((int)PenguinPlayer.PenguinPlayerObjects.SNOW_RING).localScale = Vector3.Lerp(startScale, endScale, (t/approachDuration));
			t += UnityEngine.Time.deltaTime;
			yield return null;
		}
		
		
		OVRScreenFade.instance.FadeOut();
	}
	
	IEnumerator FinishEndGame(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		
		OVRScreenFade.instance.TurnOff();
		
		yield return null;
		
		PenguinAnalytics.Instance.LogEndGame();
        MusicUtility.Stop();
		//show survey menu..
		PenguinMenuSystem.Instance.ChangeMenuTo(PenguinMenuSystem.MenuType.EndMenu);
		PenguinPlayer.Instance.StartShowingUI();
	}
	
	public void LoadMiniGame(MiniGame mg)
	{
		if(mg == MiniGame.ProtectTheNest)
		{
			SetTheNest();
			
			if(_nestGame != null)
			{
				//Debug.Log("Starting protect the nest");
				_nestGame.StartGame();
			}
		}
		else if(mg == MiniGame.MatingDance)
		{
			SetMatingDance();
			
			if(_matingDance != null)
			{
				_matingDance.StartGame();
			}
		}
	}

    public void StopMiniGame(MiniGame mg) {
        // TODO: Anything needed?
    }
}
