//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021
//This class sits in the overworld, one for each mini game, and handles the scene loading of the mini games
//after collecting enough pebbles

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameUnlocker : MonoBehaviour
{
	public enum MiniGameCommonObjects
	{
		PEBBLES,
		ICON,
		NEST,
		SNOW,
		POLE,
		PENGUINS,
		RAY_OF_LIGHT,
		MISC2
	}
	
    [SerializeField]
    string _sceneName;

    public string SceneName => _sceneName;

	[SerializeField]
	bool _lockable;
	
	public bool Lockable => _lockable;
	
    MiniGameController _miniGame = null;

	[SerializeField]
	List<Material> _lockMaterials = new List<Material>();
	
	public List<Material> LockMaterials => _lockMaterials;
	
	[SerializeField]
	Material _unlockedMaterial;
	
	public Material UnlockedMaterial => _unlockedMaterial;
	
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

	void OnEnable()
	{
		PenguinGameManager._resetGameDelegate += ResetMiniGame;
	}
	
	void OnDisable()
	{
		PenguinGameManager._resetGameDelegate -= ResetMiniGame;
	}
	
	void ResetMiniGame()
	{
		Debug.Log("Reseting mini game: " + _sceneName);
		
		transform.GetChild((int)MiniGameCommonObjects.SNOW).gameObject.SetActive(false);
		
		_isGameUnlocked = false;
	
		if(_lockable)
		{
			_numPebblesCollected = 0;
			
			transform.GetChild((int)MiniGameCommonObjects.ICON).GetComponent<MeshRenderer>().sharedMaterial = _lockMaterials[0];
			
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
		
		transform.GetChild((int)MiniGameCommonObjects.ICON).gameObject.SetActive(true);
		transform.GetChild((int)MiniGameCommonObjects.POLE).gameObject.SetActive(true);
		
		PenguinPlayer.Instance.transform.GetChild(3).GetChild(0).GetComponent<WaddleTrigger>().Speed = 20f;
		PenguinPlayer.Instance.transform.GetChild(3).GetChild(1).GetComponent<WaddleTrigger>().Speed = 20f;
		
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
	}
	
    // Update is called once per frame
    void Update()
    {
		/*if(!_lockable)
		{
			const float DIST_TO_BORDER = 4f;
			float d = Vector3.Distance(PenguinPlayer.Instance.transform.position, transform.position);
			if(d < DIST_TO_BORDER)
			{
				//adjust the interpolator component if within the attraction border...
				if(d < 0.1f)
				{
					transform.GetChild(1).gameObject.GetComponent<WIDVE.Utilities.Interpolator>().SetRawValue(0f);
					transform.GetChild(4).gameObject.GetComponent<WIDVE.Utilities.Interpolator>().SetRawValue(0f);
				}
				else
				{
					transform.GetChild(1).gameObject.GetComponent<WIDVE.Utilities.Interpolator>().SetRawValue(1f - ((DIST_TO_BORDER-d)/DIST_TO_BORDER));
					transform.GetChild(4).gameObject.GetComponent<WIDVE.Utilities.Interpolator>().SetRawValue(1f - ((DIST_TO_BORDER-d)/DIST_TO_BORDER));
				}
			}
		}*/
    }
	
	public void CollectPebble()
	{
		if(_lockable)
		{
			_numPebblesCollected++;
			if(_numPebblesCollected == _numPebblesToUnlock)
			{
				//switch to unlock icon, load mini game...
				transform.GetChild((int)MiniGameCommonObjects.ICON).GetComponent<MeshRenderer>().sharedMaterial = _unlockedMaterial;
				_isGameUnlocked = true;
				
				//show border
				transform.GetChild((int)MiniGameCommonObjects.SNOW).gameObject.SetActive(true);
				
				if(!Lockable)
				{
					//if a lockable attraction, don't hide poles and icon until game actually running
					transform.GetChild((int)MiniGameCommonObjects.ICON).gameObject.SetActive(false);
					transform.GetChild((int)MiniGameCommonObjects.POLE).gameObject.SetActive(false);
				}
				
				//slow down the speed of the user, so harder for them to leave...
				
				//accumulate a pebble onto the nest...
				//every 2 pebbles, so a new version of the nest...
				if(_numPebblesCollected % 2 == 0)
				{
					transform.GetChild((int)MiniGameCommonObjects.NEST).GetChild((_numPebblesCollected/2)-1).gameObject.SetActive(true);
				}
				
				UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
			}
			else if(_numPebblesCollected < _numPebblesToUnlock)
			{
				if(_numPebblesCollected < _lockMaterials.Count)
				{
					transform.GetChild(1).GetComponent<MeshRenderer>().sharedMaterial = _lockMaterials[_numPebblesCollected];
				}

				//accumulate a pebble onto the nest...
				if(_numPebblesCollected % 2 == 0)
				{
					transform.GetChild((int)MiniGameCommonObjects.NEST).GetChild((_numPebblesCollected/2)-1).gameObject.SetActive(true);
				}
			}
		}
	}
}
