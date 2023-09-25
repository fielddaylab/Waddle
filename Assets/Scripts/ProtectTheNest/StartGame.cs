//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using BeauUtil;
using UnityEngine;
using UnityEngine.UIElements;
using Waddle;

public class StartGame : MonoBehaviour
{
    private const float BlinkTime = 0.6f;

	public PenguinGameManager.MiniGame _miniGame;
	
	[SerializeField]
	bool _loadScene = false;

	[SerializeField]
	bool _strictAngle = false;

	[SerializeField, Range(0, 1)]
	float _facingThreshold;

    [SerializeField]
    private ParticleSystem m_ParticleRing;

    [SerializeField]
    private MeshRenderer m_HighlightRing;
	
    // Start is called before the first frame update
    void OnEnable()
    {
        MiniGameController._endGameDelegate += OnEndGame;
		//MiniGameController._startGameDelegate += OnStartGame;
		PenguinGameManager.OnReset += OnResetGame;
    }
	
	void OnDisable()
	{
		MiniGameController._endGameDelegate -= OnEndGame;
		//MiniGameController._startGameDelegate -= OnStartGame;
		PenguinGameManager.OnReset -= OnResetGame;
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
			//transform.GetChild((int)MiniGameUnlocker.MiniGameCommonObjects.ICON).gameObject.SetActive(false);
			//transform.GetChild((int)MiniGameUnlocker.MiniGameCommonObjects.POLE).gameObject.SetActive(false);
			
			//if protect the nest, turn off ray of light...
			if(_miniGame == PenguinGameManager.MiniGame.ProtectTheNest)
			{
				transform.GetChild((int)MiniGameUnlocker.MiniGameCommonObjects.RAY_OF_LIGHT).gameObject.SetActive(false);
				transform.GetChild((int)MiniGameUnlocker.MiniGameCommonObjects.MISC2).gameObject.SetActive(false);
			}

            MusicUtility.Stop();
			
			AudioSource audio = GetComponent<AudioSource>();
			if(audio != null)
			{
				audio.Play();
			}
		}

		PenguinGameManager._headMovementActive = false;
		
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

            PenguinPlayer.Instance.StartBackgroundMusic();
		}

        PenguinGameManager._headMovementActive = true;

        //return the the player to default speed...
        PenguinPlayer.Instance.SpeedUpMovement();
		
		//only want to re-enable here when reseting..
		PenguinGameManager._isInMiniGame = false;
	}
	
	void OnResetGame()
	{
		gameObject.GetComponent<Collider>().enabled = true;

        if (m_ParticleRing) {
            m_ParticleRing.Play();
        }

        if (m_HighlightRing) {
            m_HighlightRing.enabled = true;
        }
	}
	
	void OnTriggerEnter(Collider otherCollider)
	{
        if (_strictAngle) {
            if (Vector3.Dot(otherCollider.transform.forward, transform.forward) < _facingThreshold) {
                return;
            }
        }

        ValidTriggerEnter();
	}

    private void OnTriggerStay(Collider otherCollider) {
        if (_strictAngle) {
            if (Vector3.Dot(otherCollider.transform.forward, transform.forward) < _facingThreshold) {
                return;
            }
        }

        ValidTriggerEnter();
    }

    private void ValidTriggerEnter() {
        gameObject.GetComponent<Collider>().enabled = false;

        if (m_HighlightRing) {
            m_HighlightRing.enabled = false;
        }
        if (m_ParticleRing) {
            m_ParticleRing.Stop();
        }

        PenguinGameManager._headMovementActive = false;

        if (_loadScene) {
            StartCoroutine(LoadMiniGameAsync(_miniGame.ToString()));
        }
        else {
            OVRScreenFade.instance.Blink(BlinkTime, OnBlinkToStart);
        }
    }
	
	
	IEnumerator LoadMiniGameAsync(string _miniGameName)
	{
		AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_miniGameName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
		
		while(!asyncLoad.isDone)
		{
			yield return null;
		}

        OVRScreenFade.instance.Blink(BlinkTime, OnBlinkToStart);
	}

    private void OnBlinkToStart() {
        PenguinGameManager.Instance.LoadMiniGame(_miniGame);
        PenguinPlayer.Instance.transform.position = transform.position;

        if (m_ParticleRing) {
            m_ParticleRing.Clear();
        }

        OnStartGame();
    }
}
