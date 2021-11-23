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
	
	//MiniGame _currentMiniGame = MiniGame.ProtectTheNest;

	ProtectTheNest _nestGame = null;
	MatingDance _matingDance = null;
	
	bool _showedWaddleMessage = false;
	
	float _overallStartTime = 0f;
	
	[SerializeField]
	float _showModeTimeLimit = 420f;	//7 minutes
	
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
		
		OVRManager.HMDUnmounted += HandleHMDUnmounted;
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
	
	void HandleHMDUnmounted()
	{
		SetTheNest();
		
		if(_nestGame != null)
		{
			//_nestGame.RestartGame();
		}
		
		SetMatingDance();
			
		if(_matingDance != null)
		{
			//_matingDance.RestartGame();
		}
	}

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Start))
		{
			//SetTheNest();
		
			/*if(_nestGame != null)
			{
				_nestGame.RestartGame();
			}*/
		}
		
		/*if(!_showedWaddleMessage)
		{
			PenguinPlayer.Instance.ShowWaddleMessage();
			_showedWaddleMessage = true;
		}*/
		
		if(_gameMode == GameMode.ShowMode)
		{
			//7 minutes
			if(GetGameTime() > _showModeTimeLimit)
			{
				
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
