//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkuaSpawner : MonoBehaviour
{
	[SerializeField]
	GameObject _skuaPrefab;
	
	[SerializeField]
	float _totalGameTime;
	
	[SerializeField]
	List<SkuaSpot> _spawnLocations;
	
	[SerializeField]
	List<float> _waveTimes;
	
	float _startTime;
	
	List<GameObject> _currentSkuas = new List<GameObject>();
	
	//should move 85 times / minute...
	[SerializeField]
	float _moveFrequency = 0.70588235f;
	
	public float MoveFrequency => _moveFrequency;
	
	float _updateTime;
	
	[SerializeField]
	GameObject _eggTimer;
	
	[SerializeField]
	GameObject _theEgg;
	
	public GameObject TheEgg => _theEgg;
	
    // Start is called before the first frame update
    void Start()
    {
        _startTime = Time.time;
		_updateTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
		float currTime = Time.time;
		
		if(_waveTimes.Count > 0)
		{
			float spawnTime = currTime - _startTime;
			for(int i = 0; i < _waveTimes.Count; ++i)
			{
				if(spawnTime > _waveTimes[i])
				{
					int spawnLocation = -1;

					while(true)
					{
						spawnLocation = Random.Range(0, _spawnLocations.Count-1);
						if(_spawnLocations[spawnLocation].CurrentSkua == null)
						{
							break;
						}
					}
					
					if(spawnLocation != -1)
					{
						if(_waveTimes.Count == 6)
						{
							SpawnSkua(spawnLocation);
						}
						_waveTimes.RemoveAt(i);
						break;
					}
				}
			}
		}

		
		if(currTime - _updateTime > _moveFrequency)
		{
			MoveSkuas();
			_updateTime = currTime;
		}
		
		if(currTime - _startTime > _totalGameTime)
		{
			//game is over...
			//do a fade out...
			Camera.main.gameObject.GetComponent<OVRScreenFade>().FadeOut();
		}
		
		if(_eggTimer != null)
		{
			float t = currTime - _startTime;
			float timeLeft = _totalGameTime - t;
			System.TimeSpan ts = System.TimeSpan.FromSeconds(timeLeft);
			_eggTimer.GetComponent<TMPro.TextMeshPro>().text = string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
		}
    }
	
	public bool EggIsTaken()
	{
		return _theEgg.GetComponent<Egg>().IsTaken;
	}
	
	void MoveSkuas()
	{
		foreach(GameObject g in _currentSkuas)
		{
			g.GetComponent<SkuaState>().MoveSkua();
		}
	}
	
	void SpawnSkua(int spawnLocation)
	{
		GameObject newSkua = Instantiate(_skuaPrefab);
		
		//newSkua.GetComponent<Skua>().FlyIn();
		//newSkua.GetComponent<Skua>().WalkForward();
		
		Vector3 spawnSpot = _spawnLocations[spawnLocation].gameObject.transform.position;
		//spawnSpot.y += 0.05f;
		Vector3 spawnRot = _spawnLocations[spawnLocation].gameObject.transform.rotation.eulerAngles;
		spawnRot.y += newSkua.transform.rotation.eulerAngles.y;//due to skua model's local rotation.
		
		newSkua.transform.position = spawnSpot;
		newSkua.transform.rotation = Quaternion.Euler(spawnRot);
		
		_spawnLocations[spawnLocation].CurrentSkua = newSkua;
		
		newSkua.GetComponent<SkuaState>().CurrentSpot = _spawnLocations[spawnLocation];
		
		if(_spawnLocations[spawnLocation].Penguin != null)
		{
			_spawnLocations[spawnLocation].Penguin.GetComponent<AudioSource>().Play();
		}
		
		if(_spawnLocations[spawnLocation].OtherPenguin != null)
		{
			_spawnLocations[spawnLocation].OtherPenguin.GetComponent<AudioSource>().Play();
		}
		
		newSkua.GetComponent<SkuaState>().Spawner = this;
		
		_currentSkuas.Add(newSkua);
	}
}
