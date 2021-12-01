using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinPlayer : Singleton<PenguinPlayer>
{
	[SerializeField]
	GameObject _userMessageUI;
	
	[SerializeField]
	GameObject _endGamePrefab;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void ShowWaddleMessage(float showDuration)
	{
		//store a game object for the UserMessage object..
		if(_userMessageUI != null)
		{
			_userMessageUI.GetComponent<UserMessage>().StartShowMessage("", showDuration);
		}
	}
	
	public void ShowEndGamePrefab()
	{
		if(_endGamePrefab != null)
		{
			//play the snow storm...
			
		}
	}
}
