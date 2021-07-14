using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectSqwauk : MonoBehaviour
{
	public GameObject _headRef;
	public Vector3 _upOffset;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if(_headRef != null)
		{
			transform.position = _headRef.transform.position + _upOffset;
		}
    }
}
