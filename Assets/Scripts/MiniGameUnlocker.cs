//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021
//This class sits in the overworld, one for each mini game, and handles the scene loading of the mini games
//after collecting enough pebbles

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameUnlocker : MonoBehaviour
{
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
		transform.GetChild(3).gameObject.SetActive(false);
		
		_isGameUnlocked = false;
	
		if(_lockable)
		{
			transform.GetChild(1).GetComponent<MeshRenderer>().sharedMaterial = _lockMaterials[0];
			
			for(int i = 0; i < transform.GetChild(2).childCount; ++i)
			{
				transform.GetChild(2).GetChild(i).gameObject.SetActive(false);
			}
		}
		
		if(transform.childCount > 7)
		{
			transform.GetChild(8).gameObject.SetActive(true);
			transform.GetChild(9).gameObject.SetActive(true);
		}
		
		transform.GetChild(1).gameObject.SetActive(true);
		transform.GetChild(4).gameObject.SetActive(true);
		
		PenguinPlayer.Instance.transform.GetChild(3).GetChild(0).GetComponent<WaddleTrigger>().Speed = 20f;
		PenguinPlayer.Instance.transform.GetChild(3).GetChild(1).GetComponent<WaddleTrigger>().Speed = 20f;
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
				transform.GetChild(1).GetComponent<MeshRenderer>().sharedMaterial = _unlockedMaterial;
				_isGameUnlocked = true;
				
				//show border
				transform.GetChild(3).gameObject.SetActive(true);
				
				if(!Lockable)
				{
					//if a lockable attraction, don't hide poles and icon until game actually running
					transform.GetChild(1).gameObject.SetActive(false);
					transform.GetChild(4).gameObject.SetActive(false);
				}
				
				//slow down the speed of the user, so harder for them to leave...
				
				//accumulate a pebble onto the nest...
				//every 2 pebbles, so a new version of the nest...
				if(_numPebblesCollected % 2 == 0)
				{
					transform.GetChild(2).GetChild((_numPebblesCollected/2)-1).gameObject.SetActive(true);
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
					transform.GetChild(2).GetChild((_numPebblesCollected/2)-1).gameObject.SetActive(true);
				}
			}
		}
	}
}
