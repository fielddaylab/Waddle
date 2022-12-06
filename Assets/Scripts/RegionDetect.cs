using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionDetect : MonoBehaviour
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
		if(otherCollider.gameObject.name == "AdelieBody")
		{
			//Debug.Log(gameObject.name);
			PenguinAnalytics.Instance.LogEnterRegion(gameObject.name);
		}
	}
	
	void OnTriggerExit(Collider otherCollider)
	{
		if(otherCollider.gameObject.name == "AdelieBody")
		{
			//Debug.Log("Leaving " + gameObject.name);
			PenguinAnalytics.Instance.LogEnterRegion(gameObject.name);
		}
	}
}
