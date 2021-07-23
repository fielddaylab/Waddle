//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaddleTrigger : MonoBehaviour
{
	public GameObject _ovrPlayer;
	public float _speed;
	
	//int updateCount = 0;
	bool _needsUpdate = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
	
	void OnTriggerEnter(Collider otherCollider)
	{
		//happens in physics thread...
		if(otherCollider.gameObject.name == "BeakRight")
		{
			//transform.position = otherCollider.gameObject.transform.position;
			_needsUpdate = true;
			int lr = -1;
			if(gameObject.name.EndsWith("Right"))
			{
				lr = 0;
				//Debug.Log("Right nav ring");
			}
			else if(gameObject.name.EndsWith("Left"))
			{
				lr = 1;
				//Debug.Log("Left nav ring");
			}
			
			//Debug.Log(gameObject.name);
			//_ovrPlayer.transform.position -= _ovrPlayer.transform.forward * _speed * Time.deltaTime;
			transform.parent.GetComponent<NavRing>().ForceUpdate(lr);
		}
	}
	
	void LateUpdate()
	{
		if(_needsUpdate)
		{
			//Debug.Log("Updating: " + updateCount);
			//this moves the entire player
			_ovrPlayer.transform.position -= _ovrPlayer.transform.forward * _speed * Time.deltaTime; 
			_needsUpdate = false;
			//updateCount++;
			
			AudioSource audioClip = GetComponent<AudioSource>();
			if(audioClip != null)
			{
				audioClip.Play();
			}
		}
	}
}
