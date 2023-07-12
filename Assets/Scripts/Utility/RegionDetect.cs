using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionDetect : MonoBehaviour
{
	[SerializeField]
	bool _isInnerRegion = false;
	
	bool _isInRegion = false;
    // Start is called before the first frame update
    void Start()
    {
        _isInRegion = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter(Collider otherCollider)
	{
		if(_isInnerRegion && !_isInRegion)
		{
			if(otherCollider.gameObject.name == "AdelieBody")
			{
				//Debug.Log(gameObject.name);
				PenguinPlayer.Instance.CurrentRegion = gameObject.name;
				PenguinAnalytics.Instance.LogEnterRegion(gameObject.name);
				_isInRegion = true;
			}
		}
	}
	
	void OnTriggerStay(Collider otherCollider)
	{
		if(_isInnerRegion && !_isInRegion)
		{
			if(otherCollider.gameObject.name == "AdelieBody")
			{
				if(PenguinPlayer.Instance.CurrentRegion == "none")
				{
					if(gameObject.name != "none")
					{
						PenguinPlayer.Instance.CurrentRegion = gameObject.name;
						PenguinAnalytics.Instance.LogEnterRegion(gameObject.name);
					}
				}
			}
		}
	}
	
	void OnTriggerExit(Collider otherCollider)
	{
		if(!_isInnerRegion)
		{
			if(otherCollider.gameObject.name == "AdelieBody")
			{
				//Debug.Log("Leaving " + gameObject.name);
				PenguinAnalytics.Instance.LogExitRegion(gameObject.name);
				PenguinPlayer.Instance.CurrentRegion = "none";
				_isInRegion = false;
			}
		}
	}
}
