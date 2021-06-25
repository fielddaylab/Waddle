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
		}
    }
	
	void LateUpdate()
	{
		
	}
}
