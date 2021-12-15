using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    // Start is called before the first frame update
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
		
		//_isGameUnlocked = false;
	
		//if(_lockable)
		//{
			//transform.GetChild((int)MiniGameUnlocker.MiniGameCommonObjects.ICON).GetComponent<MeshRenderer>().sharedMaterial = _lockMaterials[0];
			
			for(int i = 0; i < transform.GetChild((int)MiniGameUnlocker.MiniGameCommonObjects.NEST).childCount; ++i)
			{
				transform.GetChild((int)MiniGameUnlocker.MiniGameCommonObjects.NEST).GetChild(i).gameObject.SetActive(false);
			}
		//}
		
		/*if(transform.childCount > 7)
		{
			transform.GetChild((int)MiniGameCommonObjects.SPOT_LIGHT).gameObject.SetActive(true);
			transform.GetChild((int)MiniGameCommonObjects.RAY_OF_LIGHT).gameObject.SetActive(true);
		}*/
		
		transform.GetChild((int)MiniGameUnlocker.MiniGameCommonObjects.ICON).gameObject.SetActive(true);
		transform.GetChild((int)MiniGameUnlocker.MiniGameCommonObjects.POLE).gameObject.SetActive(true);
		
		PenguinPlayer.Instance.transform.GetChild(3).GetChild(0).GetComponent<WaddleTrigger>().Speed = 20f;
		PenguinPlayer.Instance.transform.GetChild(3).GetChild(1).GetComponent<WaddleTrigger>().Speed = 20f;
	}
}
