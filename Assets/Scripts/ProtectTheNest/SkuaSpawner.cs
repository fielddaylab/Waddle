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
	const float UPDATE_FREQ = 0.70588235f;
	
	float _updateTime;
	
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
						SpawnSkua(spawnLocation);
						_waveTimes.RemoveAt(i);
						break;
					}
				}
			}
		}

		
		if(currTime - _updateTime > UPDATE_FREQ)
		{
			MoveSkuas();
			_updateTime = currTime;
		}
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
		
		Vector3 spawnSpot = _spawnLocations[spawnLocation].gameObject.transform.position;
		spawnSpot.y += 0.5f;
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
		
		_currentSkuas.Add(newSkua);
	}
}
