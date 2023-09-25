//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021
//This class sits in the overworld, one for each mini game, and handles the scene loading of the mini games
//after collecting enough pebbles

using System.Collections;
using System.Collections.Generic;
using BeauRoutine;
using UnityEngine;

public class MiniGameUnlocker : MonoBehaviour
{
	public enum MiniGameCommonObjects
	{
		NEST,
		SNOW,
		PENGUINS,
		RAY_OF_LIGHT,
		MISC2
	}
	
    [SerializeField]
    string _sceneName;

    public string SceneName => _sceneName;

	[SerializeField]
	bool _lockable;

    [SerializeField]
    bool _autoUnlock = false;
	
	public bool Lockable => _lockable;
	
    MiniGameController _miniGame = null;
	
	[SerializeField]
	int _numPebblesToUnlock = 10;
	
	public int NumPebblesToUnlock => _numPebblesToUnlock;
	
	int _numPebblesCollected = 0;
	public int NumPebblesCollected => _numPebblesCollected;

	bool _isGameUnlocked = false;
	public bool IsGameUnlocked => _isGameUnlocked;
	
	
    public MiniGameController MiniGame
    {
        get { return _miniGame; }
        set { _miniGame = value; }
    }

	void OnEnable()
	{
		PenguinGameManager.OnReset += ResetMiniGame;
        if (_autoUnlock && !_isGameUnlocked) {
            AutoUnlock();
        }
    }
	
	void OnDisable()
	{
		PenguinGameManager.OnReset -= ResetMiniGame;
	}
	
	void ResetMiniGame()
	{
		Debug.Log("Reseting mini game: " + _sceneName);
		
		transform.GetChild((int)MiniGameCommonObjects.SNOW).gameObject.SetActive(false);
		
		_isGameUnlocked = false;
	
		if(_lockable)
		{
			_numPebblesCollected = 0;
			
			//transform.GetChild((int)MiniGameCommonObjects.ICON).GetComponent<MeshRenderer>().sharedMaterial = _lockMaterials[0];
			
			for(int i = 0; i < transform.GetChild((int)MiniGameCommonObjects.NEST).childCount; ++i)
			{
				transform.GetChild((int)MiniGameCommonObjects.NEST).GetChild(i).gameObject.SetActive(false);
			}
		}
		
		if(transform.childCount > 7)
		{
			transform.GetChild((int)MiniGameCommonObjects.RAY_OF_LIGHT).gameObject.SetActive(true);
			transform.GetChild((int)MiniGameCommonObjects.MISC2).gameObject.SetActive(true);
		}
		
		//transform.GetChild((int)MiniGameCommonObjects.ICON).gameObject.SetActive(true);
		//transform.GetChild((int)MiniGameCommonObjects.POLE).gameObject.SetActive(true);

        PenguinPlayer.Instance.SpeedUpMovement();

        Collider c = GetComponent<Collider>();
		if(c != null)
		{
			c.enabled = true;
		}
		
		//10/19/2022 - ensure protect the nest audio turns off right away if user restarts during it
		if(_sceneName == "ProtectTheNest")
		{
			AudioSource audio = GetComponent<AudioSource>();
			if(audio != null)
			{
				audio.Stop();
			}
		
		}

        if (_autoUnlock) {
            AutoUnlock();
        }
	}

    private void AutoUnlock() {
        Routine.StartDelay(PebbleUnlock, 0.2f);
    }
	
	public void PebbleUnlock()
	{
		if(_lockable && !_isGameUnlocked)
		{
            _isGameUnlocked = true;
            PenguinAnalytics.Instance.LogMatingDanceIndicator(100f);

            //slow down the speed of the user, so harder for them to leave...

            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
	}
}
