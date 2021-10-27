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
	
	MiniGame _currentMiniGame = MiniGame.ProtectTheNest;

	ProtectTheNest _nestGame = null;
	MatingDance _matingDance = null;
	
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
		float newRate = 90.0f;
		
		if (Unity.XR.Oculus.Performance.TrySetDisplayRefreshRate(newRate))
		{
			Time.fixedDeltaTime = 1f / newRate;
			Time.maximumDeltaTime = 1f / newRate;
		}
			//}
		//}
		
		OVRManager.HMDUnmounted += HandleHMDUnmounted;
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
			_nestGame.RestartGame();
		}
		
		SetMatingDance();
			
		if(_matingDance != null)
		{
			_matingDance.RestartGame();
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
