using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScale : MonoBehaviour
{
	float _defaultHeight = 0.6096f;
	
	Camera _mainCam = null;
	
	bool _scaledHeight = false;
	
    // Start is called before the first frame update
    void Start()
    {
	   //UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<UnityEngine.XR.XRInputSubsystem>().TrySetTrackingOriginMode(UnityEngine.XR.TrackingOriginModeFlags.Floor);
       OVRManager.TrackingAcquired += RescaleHeight;
    }
	
	void RescaleHeight()
	{
		 _mainCam = Camera.main;
		float currHeight = _mainCam.transform.localPosition.y;
		Debug.Log("Height: " + currHeight);
		if(currHeight != 0f)
		{
			//float scale = _defaultHeight / currHeight;
			//transform.localScale = Vector3.one * scale;
			Vector3 lp = transform.localPosition;
			lp.y = _defaultHeight - currHeight;
			transform.localPosition = lp;
			_scaledHeight = true;
		}
	}
	
    // Update is called once per frame
    void Update()
    {
        if(!_scaledHeight)
		{
			if(_mainCam == null)
			{
				_mainCam = Camera.main;
			}
			
			float currHeight = _mainCam.transform.localPosition.y;
			if(currHeight != 0f)
			{
				//on android builds this is currently not returning a correct value...
				//so let's set to a default temporary reasonable value until we figure out how..
				currHeight = 1.5f;
				Debug.Log("Starting height: " + currHeight);
				Vector3 lp = transform.localPosition;
				lp.y = _defaultHeight - currHeight;
				transform.localPosition = lp;
				//float scale = _defaultHeight / currHeight;
				//transform.localScale = Vector3.one * scale;
				_scaledHeight = true;
			}
		}
    }
}
