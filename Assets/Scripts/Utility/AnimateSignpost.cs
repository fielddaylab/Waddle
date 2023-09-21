using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateSignpost : MonoBehaviour
{
	[SerializeField]
	Texture2D[] _texturesToAnimate;
	
	[SerializeField]
	float _timeToSwitch = 1f;
	
	float _lastSwitchTime = 0f;
	
	int _currTextureIndex = 0;
	
    // Start is called before the first frame update
    void Start()
    {
		_lastSwitchTime = UnityEngine.Time.time;
		if(_texturesToAnimate.Length > 0)
		{
			transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.mainTexture = _texturesToAnimate[0];
		}
    }

    // Update is called once per frame
    void Update()
    {
        if(UnityEngine.Time.time - _lastSwitchTime > _timeToSwitch)
		{
			_currTextureIndex++;
			if(_currTextureIndex == _texturesToAnimate.Length)
			{
				_currTextureIndex = 0;
			}
			
			if(_texturesToAnimate.Length > 0)
			{
				transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.mainTexture = _texturesToAnimate[_currTextureIndex];
			}
			_lastSwitchTime = UnityEngine.Time.time;
		}
    }
}
