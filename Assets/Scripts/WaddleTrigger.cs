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
	
	[SerializeField]
	AudioClip _collideClip;
	
	//int updateCount = 0;
	bool _needsUpdate = false;
	
	GameObject[] _worldColliders = null;
	float _triggerTimer = 0f;
	
	int _wasRight = 0;

    // Start is called before the first frame update
    void Start()
    {
		_worldColliders = GameObject.FindGameObjectsWithTag("WorldCollision");
		//Debug.Log(_worldColliders.Length);
    }

    // Update is called once per frame
    /*void Update()
    {
        Debug.Log(_centerEye.transform.right.y);
    }*/
	
	void OnTriggerEnter(Collider otherCollider)
	{
		//happens in physics thread...
		
		if(otherCollider.gameObject.name == "CenterEyeAnchor")
		{
			//transform.position = otherCollider.gameObject.transform.position;
			//check that their head is relatively level when entering the collider
			float currTime = UnityEngine.Time.time;
			
			if(_centerEye.transform.forward.y > -0.6f && _centerEye.transform.forward.y < 0.5f && currTime - _triggerTimer > 0.5f)
			{
				_needsUpdate = true;
				int lr = -1;
				if(gameObject.name.EndsWith("Right"))
				{
					lr = 0;
					_wasRight = 1;
					//Debug.Log("Right nav ring");
				}
				else if(gameObject.name.EndsWith("Left"))
				{
					lr = 1;
					//Debug.Log("Left nav ring");
					_wasRight = 0;
				}
				
				//Debug.Log(gameObject.name);
				//_rotationTransform.transform.position -= _rotationTransform.transform.forward * _speed * Time.deltaTime;
				transform.parent.GetComponent<NavRing>().ForceUpdate(lr);
				_triggerTimer = currTime;
			}
		}
	}
	
	void LateUpdate()
	{
		if(_needsUpdate)
		{
			//Debug.Log("Updating");
			//this moves the entire player
			
			Vector3 potentialPos = _positionTransform.transform.position + _rotationTransform.transform.forward * _speed * Time.deltaTime; 
			Vector3 startFromOffset = _rotationTransform.transform.forward * 0.3f;
			Vector3 checkPos = _centerEye.transform.position + startFromOffset + _rotationTransform.transform.forward * _speed * Time.deltaTime; 
			Vector3 heightPos = _centerEye.transform.position + startFromOffset;
			float d = 0f;
			bool validMove = true;
			
			for(int i = 0; i < _worldColliders.Length; ++i)
			{
				//check from height of person...
				
				Collider c = _worldColliders[i].GetComponent<Collider>();
				if(c != null)
				{
					if(c.bounds.Contains(checkPos))
					{
						Ray r = new Ray();
						r.origin = heightPos;
						r.direction = Vector3.Normalize(checkPos - heightPos);
						
						if(c.bounds.IntersectRay(r, out d))
						{
							if(d > 0.25f)
							{
								//Debug.Log("Intersects ray!");
								potentialPos = _positionTransform.transform.position + (d) * r.direction;// r.origin + (d) * r.direction;
								//potentialPos = potentialPos - r.direction * 0.3f;
								//potentialPos.y -= 0.28f;
							}
							else
							{
								validMove = false;
							}
							break;
						}
						//potentialPos = c.ClosestPointOnBounds(potentialPos);
						//Debug.Log(potentialPos.ToString("F4"));
						
					}
				}
			}
			
			if(validMove)
			{
				PenguinAnalytics.Instance.LogMove(_positionTransform.transform.position, potentialPos, _rotationTransform.transform.rotation, _wasRight);
				
				_positionTransform.transform.position = potentialPos;
				AudioSource audioClip = GetComponent<AudioSource>();
				if(audioClip != null)
				{
					audioClip.Play();
				}
			}
			else
			{
				AudioSource audioClip = GetComponent<AudioSource>();
				if(audioClip != null)
				{
					if(_collideClip != null)
					{
						audioClip.PlayOneShot(_collideClip);
					}
				}
			}
			//_positionTransform.transform.position += _rotationTransform.transform.forward * _speed * Time.deltaTime; 
			_needsUpdate = false;
			//updateCount++;
		}
	}
}
