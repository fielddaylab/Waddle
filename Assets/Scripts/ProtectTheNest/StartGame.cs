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
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
		
		//turn on the borders...
		transform.GetChild(3).gameObject.SetActive(true);
	}
}
