//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021
//Temporary class for demo.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{

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
			PenguinGameManager.Instance.LoadMiniGame(PenguinGameManager.MiniGame.ProtectTheNest);
		}
	}
}
