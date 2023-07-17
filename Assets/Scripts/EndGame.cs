//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    // Start is called before the first frame update
	// this script is on the Mating Dance mini game only
	// Could be used for doing specific actions on end of game.
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnEnable()
	{
		MiniGameController._endGameDelegate += OnEnd;
	}
	
	void OnDisable()
	{
		MiniGameController._endGameDelegate -= OnEnd;
	}
	
	public void OnEnd()
	{
		transform.GetChild((int)MiniGameUnlocker.MiniGameCommonObjects.SNOW).gameObject.SetActive(false);
		
		for(int i = 0; i < transform.GetChild((int)MiniGameUnlocker.MiniGameCommonObjects.NEST).childCount; ++i)
		{
			transform.GetChild((int)MiniGameUnlocker.MiniGameCommonObjects.NEST).GetChild(i).gameObject.SetActive(false);
		}

        PenguinPlayer.Instance.SpeedUpMovement();
	}
}
