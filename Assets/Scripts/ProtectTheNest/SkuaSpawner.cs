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
	List<SkuaSpot> _spawnLocations;
	
	[SerializeField]
	List<float> _waveTimes;

	List<float> _originalWaveTimes;

	List<GameObject> _currentSkuas = new List<GameObject>();
	
	List<SkuaSpot> _takenSpotList = new List<SkuaSpot>();
	
	[SerializeField]
	GameObject _theEgg;
	public GameObject TheEgg => _theEgg;

    // Start is called before the first frame update
    void Start()
    {
        _originalWaveTimes = new List<float>(_waveTimes);
    }
	
	public void StartGame()
	{
		for(int i = 0; i < _originalWaveTimes.Count; ++i)
		{
			_waveTimes.Add(_originalWaveTimes[i]);
		}
	}

	public void ClearGame()
	{
		_takenSpotList.Clear();
		
		for(int i = 0; i < _currentSkuas.Count; ++i)
		{
			//DestroyObject(_currentSkuas[i]);
			SkuaController sc = _currentSkuas[i].GetComponent<SkuaController>();
			sc.SkuaRemove();
		}

		for(int i = 0; i < _spawnLocations.Count; ++i)
		{
			_spawnLocations[i].CurrentSkua = null;
		}
		
		_currentSkuas.Clear();
	}
	
	bool EggIsTaken()
	{
        if(_theEgg != null)
        {
		    return _theEgg.GetComponent<Egg>().IsTaken;
        }

        return false;
	}

	public void CheckForSpawn(float timeSinceStart)
	{
		if(_waveTimes.Count > 0)
		{
			for(int i = 0; i < _waveTimes.Count; ++i)
			{
				if(timeSinceStart > _waveTimes[i])
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
						//if(_waveTimes.Count == 6)
						{
							SpawnSkua(spawnLocation);
						}
						_waveTimes.RemoveAt(i);
						break;
					}
				}
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
		
    }
	
	public void MoveSkuas()
	{
		_takenSpotList.Clear();
		
		foreach(GameObject g in _currentSkuas)
		{
			SkuaSpot potentialSpot = null;
			int numIterations = 0;
			SkuaController sc = g.GetComponent<SkuaController>();
			
			if(sc.GetEgg != null)
			{
				//if this skua has the egg, and it hasn't retreated to an outer spot yet, do so...
				if(!sc.CurrentSpot.IsOuter)
				{
					while(potentialSpot == null && numIterations < 10)
					{
						potentialSpot = sc.SearchForOuterSpot();
						//if the egg is currently taken, don't allow spot0 to be valid...
						
						if(potentialSpot != null)
						{
							if(!_takenSpotList.Contains(potentialSpot))
							{
								_takenSpotList.Add(potentialSpot);
								break;
							}
							else
							{
								potentialSpot = null;
							}
						}
						numIterations++;
					}
					
					sc.SetNewSpot(potentialSpot);
					sc.WalkToSpot();
				}
				else
				{
					//continue eating...
					sc.Eat();
				}
			}
			else if(sc.InHitState())
			{
				while(potentialSpot == null && numIterations < 10)
				{
					potentialSpot = sc.SearchForOuterSpot();
					//if the egg is currently taken, don't allow spot0 to be valid...
					
					if(potentialSpot != null)
					{
						if(!_takenSpotList.Contains(potentialSpot))
						{
							_takenSpotList.Add(potentialSpot);
							break;
						}
						else
						{
							potentialSpot = null;
						}
					}
					numIterations++;
				}
				
				//turn physics back off, animation back on
				g.GetComponent<Rigidbody>().useGravity = false;
				g.GetComponent<Rigidbody>().isKinematic = true;
				sc.GetAnimController().enabled = true;
				
				sc.SetNewSpot(potentialSpot);
				sc.WalkToSpot();
			}
			else
			{
				while(potentialSpot == null && numIterations < 10)
				{
					potentialSpot = sc.SearchForNewSpot();
					//if the egg is currently taken, don't allow spot0 to be valid...
					
					if(potentialSpot != null)
					{
						//don't have skuas walk into the middle if the egg is gone already...
						if(EggIsTaken() && potentialSpot.IsCenter)
						{
							potentialSpot = null;
						}
						else
						{
							if(!_takenSpotList.Contains(potentialSpot))
							{
								_takenSpotList.Add(potentialSpot);
								break;
							}
							else
							{
								potentialSpot = null;
							}
						}
					}
					numIterations++;
				}
				
				/*if(numIterations == 10)
				{
					Debug.Log("Num iters");
				}*/
				
				if(potentialSpot == sc.CurrentSpot)
				{
					sc.GoIdle();
				}
				else
				{
					if(potentialSpot.IsCenter)
					{
						_theEgg.GetComponent<Egg>().IsTaken = true;
						sc.SetEggRef(_theEgg.GetComponent<Egg>());
					}
					
					sc.SetNewSpot(potentialSpot);
					sc.WalkToSpot();
				}
			}
		}
	}
	
	public void SpawnSkua(int spawnLocation)
	{
		GameObject newSkua = Instantiate(_skuaPrefab);
		
		//newSkua.GetComponent<Skua>().WalkForward();
		
		Vector3 spawnSpot = _spawnLocations[spawnLocation].gameObject.transform.position;
		//spawnSpot.y += 0.05f;
		Vector3 spawnRot = _spawnLocations[spawnLocation].gameObject.transform.rotation.eulerAngles;
		spawnRot.y += newSkua.transform.rotation.eulerAngles.y;//due to skua model's local rotation.
		
		newSkua.transform.position = spawnSpot;
		newSkua.transform.rotation = Quaternion.Euler(spawnRot);
		
		_spawnLocations[spawnLocation].CurrentSkua = newSkua;
		
		newSkua.GetComponent<SkuaController>().SetNewSpot(_spawnLocations[spawnLocation]);
		//newSkua.GetComponent<SkuaState>().CurrentSpot = _spawnLocations[spawnLocation];
		
		if(_spawnLocations[spawnLocation].Penguin != null)
		{
			_spawnLocations[spawnLocation].Penguin.GetComponent<AudioSource>().Play();
		}
		
		if(_spawnLocations[spawnLocation].OtherPenguin != null)
		{
			_spawnLocations[spawnLocation].OtherPenguin.GetComponent<AudioSource>().Play();
		}
		
		//newSkua.GetComponent<SkuaState>().Spawner = this;
		
		//newSkua.GetComponent<SkuaState>().GoIdle();
		
		_currentSkuas.Add(newSkua);
	}
}
