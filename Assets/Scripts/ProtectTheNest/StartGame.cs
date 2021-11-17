//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021
//Temporary class for demo.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
	public PenguinGameManager.MiniGame _miniGame;
	
	[SerializeField]
	bool _loadScene = false;
	
    // Start is called before the first frame update
    void Start()
    {
        MiniGameController._endGameDelegate += OnEndGame;
		MiniGameController._startGameDelegate += OnStartGame;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnDisable()
	{
		MiniGameController._endGameDelegate -= OnEndGame;
		MiniGameController._startGameDelegate -= OnStartGame;
	}
	
	void OnStartGame()
	{
		//todo - figure out how to combine this with the same code that runs in MiniGameUnlocker
		if(_miniGame != PenguinGameManager.MiniGame.MatingDance)
		{
			//turn on the borders...
			transform.GetChild(3).gameObject.SetActive(true);
			
			//turn off icon and Pole...eventually fade and fade back in when leaving
			transform.GetChild(1).gameObject.SetActive(false);
			transform.GetChild(4).gameObject.SetActive(false);
			
			//if protect the nest, turn off ray of light...
			if(_miniGame == PenguinGameManager.MiniGame.ProtectTheNest)
			{
				transform.GetChild(8).gameObject.SetActive(false);
				transform.GetChild(9).gameObject.SetActive(false);
			}
		}
		
		//slow down the player...
		PenguinPlayer.Instance.transform.GetChild(3).GetChild(0).GetComponent<WaddleTrigger>().Speed = 5f;
		PenguinPlayer.Instance.transform.GetChild(3).GetChild(1).GetComponent<WaddleTrigger>().Speed = 5f;
	}
	
	void OnEndGame()
	{
		if(_miniGame != PenguinGameManager.MiniGame.MatingDance)
		{
			//turn on the borders...
			transform.GetChild(3).gameObject.SetActive(false);
			
			//turn off icon and Pole...eventually fade and fade back in when leaving
			transform.GetChild(1).gameObject.SetActive(true);
			transform.GetChild(4).gameObject.SetActive(true);
			
			//if protect the nest, turn off ray of light...
			if(_miniGame == PenguinGameManager.MiniGame.ProtectTheNest)
			{
				transform.GetChild(8).gameObject.SetActive(true);
				transform.GetChild(9).gameObject.SetActive(true);
			}
		}
		
		//return the the player to default speed...
		PenguinPlayer.Instance.transform.GetChild(3).GetChild(0).GetComponent<WaddleTrigger>().Speed = 20f;
		PenguinPlayer.Instance.transform.GetChild(3).GetChild(1).GetComponent<WaddleTrigger>().Speed = 20f;
	}
	
	void OnTriggerEnter(Collider otherCollider)
	{
		if(otherCollider.gameObject.layer == 3)
		{
			gameObject.GetComponent<Collider>().enabled = false;
			
			if(_loadScene)
			{
				StartCoroutine(LoadMiniGameAsync(_miniGame.ToString()));
			}
			else
			{
				PenguinGameManager.Instance.LoadMiniGame(_miniGame);
			}
		}
	}
	
	
	IEnumerator LoadMiniGameAsync(string _miniGameName)
	{
		AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_miniGameName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
		
		while(!asyncLoad.isDone)
		{
			yield return null;
		}
		
		PenguinGameManager.Instance.LoadMiniGame(_miniGame);
	}
}
