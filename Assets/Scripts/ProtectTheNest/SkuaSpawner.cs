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
	
	float _eggTime;
	
	[SerializeField]
	GameObject _eggTimer;
	
	[SerializeField]
	GameObject _theEgg;
	
	public GameObject TheEgg => _theEgg;
	
	List<SkuaSpot> _takenSpotList = new List<SkuaSpot>();
	
    // Start is called before the first frame update
    void Start()
    {
        _startTime = Time.time;
		_updateTime = _startTime;
		_eggTime = _startTime;
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

		
		if(currTime - _updateTime > _moveFrequency)
		{
			MoveSkuas();
			_updateTime = currTime;
		}
		
		if(_eggTime - _startTime > _totalGameTime)
		{
			//game is over...
			//do a fade out...
			Camera.main.gameObject.GetComponent<OVRScreenFade>().FadeOut();
		}
		
		if(_eggTimer != null)
		{
			if(!EggIsTaken())
			{
				_eggTime += Time.deltaTime;
				float t = _eggTime - _startTime;
				float timeLeft = _totalGameTime - t;
				if(timeLeft > 0f)
				{
					System.TimeSpan ts = System.TimeSpan.FromSeconds(timeLeft);
					_eggTimer.GetComponent<TMPro.TextMeshPro>().text = string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
				}
				else
				{
					System.TimeSpan ts = System.TimeSpan.FromSeconds(0);
					_eggTimer.GetComponent<TMPro.TextMeshPro>().text = string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
				}
			}
		}
    }
	
	public bool EggIsTaken()
	{
		return _theEgg.GetComponent<Egg>().IsTaken;
	}
	
	void MoveSkuas()
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
	
	void SpawnSkua(int spawnLocation)
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
