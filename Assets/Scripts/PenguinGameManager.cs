//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;

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
		HomeMode
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
	
	[SerializeField]
	float _showModeTimeLimit = 420f;	//7 minutes
	
	public delegate void OnResetDelegate();
	public static event OnResetDelegate _resetGameDelegate;
	
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
		
		_overallStartTime = UnityEngine.Time.time;
		
		StartCoroutine(ShowMessage("", 5f, 10f));
		
		OVRManager.HMDUnmounted += HandleHMDUnmounted;
		
		OVRManager.HMDMounted += HandleHMDMounted;
    }

	IEnumerator ShowMessage(string message, float startDuration, float duration)
	{
		yield return new WaitForSeconds(startDuration);
		
		PenguinPlayer.Instance.ShowWaddleMessage(duration);
	}
	
	float GetGameTime()
	{
		return UnityEngine.Time.time - _overallStartTime;
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
		StartCoroutine(ShowMessage("", 5f, 10f));
		
		_overallStartTime = UnityEngine.Time.time;
		
		PenguinPlayer.Instance.EnableMovement();
		
		UnityEngine.Time.timeScale = 1;
		//AudioListener.pause = false;
		PenguinPlayer.Instance.StartBackgroundMusic();
	}
	
	public void HandleHMDUnmounted()
	{
		UnityEngine.Time.timeScale = 0;
		//AudioListener.pause = true;
		PenguinPlayer.Instance.StopBackgroundMusic();
		PenguinPlayer.Instance.ForceUserMessageOff();
		
		//this should now reset the whole experience in "ShowMode"
		if(_gameMode == GameMode.ShowMode)
		{
			PenguinPlayer.Instance.transform.position = _playerStartLocation.transform.position;
			
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
			
			_resetGameDelegate();
			
			PenguinPlayer.Instance.DisableMovement();
		}
	}

    // Update is called once per frame
    void Update()
    {
        
		if(_gameMode == GameMode.ShowMode)
		{
			//7 minutes
			if(GetGameTime() > _showModeTimeLimit)
			{
				if(!_showingEndGamePrefab)
				{
					PenguinPlayer.Instance.ShowEndGamePrefab();
					_showingEndGamePrefab = true;
					
					//fade out here...
					OVRScreenFade.instance.FadeOut();
					PenguinPlayer.Instance.StopBackgroundMusic();
				}
			}	
		}
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
}
