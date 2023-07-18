//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

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
		//MiniGameController._startGameDelegate += OnStartGame;
		PenguinGameManager._resetGameDelegate += OnResetGame;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnDisable()
	{
		MiniGameController._endGameDelegate -= OnEndGame;
		//MiniGameController._startGameDelegate -= OnStartGame;
		PenguinGameManager._resetGameDelegate -= OnResetGame;
	}
	
	void OnStartGame()
	{
		Debug.Log("Starting game: " + _miniGame.ToString());
		//todo - figure out how to combine this with the same code that runs in MiniGameUnlocker
		if(_miniGame != PenguinGameManager.MiniGame.MatingDance)
		{
			//turn on the borders...
			transform.GetChild((int)MiniGameUnlocker.MiniGameCommonObjects.SNOW).gameObject.SetActive(true);
			
			//turn off icon and Pole...eventually fade and fade back in when leaving
			transform.GetChild((int)MiniGameUnlocker.MiniGameCommonObjects.ICON).gameObject.SetActive(false);
			transform.GetChild((int)MiniGameUnlocker.MiniGameCommonObjects.POLE).gameObject.SetActive(false);
			
			//if protect the nest, turn off ray of light...
			if(_miniGame == PenguinGameManager.MiniGame.ProtectTheNest)
			{
				transform.GetChild((int)MiniGameUnlocker.MiniGameCommonObjects.RAY_OF_LIGHT).gameObject.SetActive(false);
				transform.GetChild((int)MiniGameUnlocker.MiniGameCommonObjects.MISC2).gameObject.SetActive(false);
			}
			
			AudioSource mainTrack = PenguinPlayer.Instance.GetComponent<AudioSource>();
			if(mainTrack != null)
			{
				mainTrack.Stop();
			}
			
			AudioSource audio = GetComponent<AudioSource>();
			if(audio != null)
			{
				audio.Play();
			}
		}

		if (_miniGame == PenguinGameManager.MiniGame.MatingDance) {
			// disable movement;
			PenguinGameManager._headMovementActive = false;
		}
		
		MeshRenderer mr = GetComponent<MeshRenderer>();
		if(mr != null)
		{
			mr.enabled = false;
		}
		
		//slow down the player...
		PenguinPlayer.Instance.SlowDownMovement();
		
		PenguinGameManager._isInMiniGame = true;
	}
	
	void OnEndGame()
	{
		Debug.Log("Ending game: " + _miniGame.ToString());
		if(_miniGame != PenguinGameManager.MiniGame.MatingDance)
		{
			//we don't do this for mating dance, because the child objects don't exist where this script is for that case at the moment...
			//turn off the borders...
			transform.GetChild((int)MiniGameUnlocker.MiniGameCommonObjects.SNOW).gameObject.SetActive(false);
			
			//update:  keep icon and pole off until the game is reset...
			/*transform.GetChild((int)MiniGameUnlocker.MiniGameCommonObjects.ICON).gameObject.SetActive(true);
			transform.GetChild((int)MiniGameUnlocker.MiniGameCommonObjects.POLE).gameObject.SetActive(true);
			
			//if protect the nest, turn on ray of light...
			if(_miniGame == PenguinGameManager.MiniGame.ProtectTheNest)
			{
				transform.GetChild((int)MiniGameUnlocker.MiniGameCommonObjects.RAY_OF_LIGHT).gameObject.SetActive(true);
				transform.GetChild((int)MiniGameUnlocker.MiniGameCommonObjects.MISC2).gameObject.SetActive(true);
			}*/
			
			AudioSource audio = GetComponent<AudioSource>();
			if(audio != null)
			{
				audio.Stop();
			}
			
			AudioSource mainTrack = PenguinPlayer.Instance.GetComponent<AudioSource>();
			if(mainTrack != null)
			{
				mainTrack.Play();
			}
		}

        if (_miniGame == PenguinGameManager.MiniGame.MatingDance) {
			// enable movement;
			PenguinGameManager._headMovementActive = true;
        }

        //return the the player to default speed...
        PenguinPlayer.Instance.SpeedUpMovement();
		
		//only want to re-enable here when reseting..
		PenguinGameManager._isInMiniGame = false;
	}
	
	void OnResetGame()
	{
		if(_miniGame == PenguinGameManager.MiniGame.MatingDance)
		{
			gameObject.GetComponent<Collider>().enabled = true;
			
			MeshRenderer mr = GetComponent<MeshRenderer>();
			if(mr != null)
			{
				mr.enabled = true;
			}
		}
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
				
				OnStartGame();
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
		
		OnStartGame();
	}
}
