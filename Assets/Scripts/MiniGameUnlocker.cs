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

    // Update is called once per frame
    void Update()
    {
        
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
					//TODO - fade these out / in...
					transform.GetChild(4).gameObject.SetActive(false);
					
					transform.GetChild(2).gameObject.SetActive(false);
				}
				
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
				transform.GetChild(1).GetComponent<MeshRenderer>().sharedMaterial = _lockMaterials[_numPebblesCollected-1];

				//accumulate a pebble onto the nest...
				if(_numPebblesCollected % 2 == 0)
				{
					transform.GetChild(2).GetChild((_numPebblesCollected/2)-1).gameObject.SetActive(true);
				}
			}
		}
	}
}
