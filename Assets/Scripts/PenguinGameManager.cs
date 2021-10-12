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

	//temporary for demo as we're just restarting one game.
	ProtectTheNest _nestGame = null;

    void Start()
    {
		//temporary for demo as we're just restarting one game.
		//this appears to not function with multiple scenes at the moment
        _nestGame = GameObject.Find("ProtectTheNest").GetComponent<ProtectTheNest>();
		OVRManager.HMDUnmounted += HandleHMDUnmounted;
    }

	void HandleHMDUnmounted()
	{
		if(_nestGame != null)
		{
			_nestGame.RestartGame();
		}
	}

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Start))
		{
			_nestGame.RestartGame();
		}
    }

	public void LoadMiniGame(MiniGame mg)
	{
		if(_nestGame == null)
		{
			_nestGame = GameObject.Find("ProtectTheNest").GetComponent<ProtectTheNest>();
		}

		if(_nestGame != null)
		{
			_nestGame.StartGame();
		}
	}
}
