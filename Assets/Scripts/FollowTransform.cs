using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
	public GameObject _followTransform;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_followTransform != null)
		{
			transform.position = _followTransform.transform.position;
			
			if(_followTransform.transform.childCount > 1)
			{
				OVRHand h = _followTransform.transform.GetChild(1).GetComponent<OVRHand>();
				if(!h.IsDataHighConfidence)
				{
					//do something here when we lose tracking...
				}
			}
		}
    }
	
	void LateUpdate()
	{
		
	}
}
