using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScale : MonoBehaviour
{
	float _defaultHeight = 0.6096f;
	
	Camera _mainCam = null;
    // Start is called before the first frame update
    void Start()
    {
       OVRManager.TrackingAcquired += RescaleHeight;
    }
	
	void RescaleHeight()
	{
		 _mainCam = Camera.main;
		float currHeight = _mainCam.transform.localPosition.y;
		Debug.Log("Starting height: " + currHeight);
		float scale = _defaultHeight / currHeight;
		transform.localScale = Vector3.one * scale;
	}
	
    // Update is called once per frame
    void Update()
    {
        
    }
}
