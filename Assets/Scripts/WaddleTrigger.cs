//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaddleTrigger : MonoBehaviour
{
	[SerializeField]
	GameObject _rotationTransform;
	
	[SerializeField]
	GameObject _centerEye;
	
	[SerializeField]
	GameObject _positionTransform;

	[SerializeField]
	float _speed;
	
	public float Speed
	{
		get { return _speed; }
		set { _speed = value; }
	}
	
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
		if(otherCollider.gameObject.name == "CenterEyeAnchor")
		{
			//transform.position = otherCollider.gameObject.transform.position;
			//check that their head is relatively level when entering the collider
			if(_centerEye.transform.forward.y > -0.25f && _centerEye.transform.forward.y < 0.25f)
			{
				_needsUpdate = true;
				/*int lr = -1;
				if(gameObject.name.EndsWith("Right"))
				{
					lr = 0;
					Debug.Log("Right nav ring");
				}
				else if(gameObject.name.EndsWith("Left"))
				{
					lr = 1;
					Debug.Log("Left nav ring");
				}*/
				
				//Debug.Log(gameObject.name);
				//_rotationTransform.transform.position -= _rotationTransform.transform.forward * _speed * Time.deltaTime;
				transform.parent.GetComponent<NavRing>().ForceUpdate();
			}
		}
	}
	
	void LateUpdate()
	{
		if(_needsUpdate)
		{
			
			//Debug.Log("Updating: " + updateCount);
			//this moves the entire player
			_positionTransform.transform.position += _rotationTransform.transform.forward * _speed * Time.deltaTime; 
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
