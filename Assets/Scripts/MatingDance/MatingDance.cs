//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatingDance : MiniGameController
{
    // Start is called before the first frame update
    float[] _sampleData = null;
    
    [SerializeField]
    GameObject _bubblePrefab = null;

	[SerializeField]
	GameObject _bubbleLinePrefab = null;

	private List<GameObject> _activeBubbles;

    private List<Coroutine> _activeRoutines;

    [SerializeField]
	TextAsset _dataFile = null;
	
	float[] _timeSamples = null;
	int[] _bubbleTypes = null;
	
	int _currentSample = 0;
	
	[SerializeField]
	GameObject _clapBubbleSpot = null;
	
	[SerializeField]
	GameObject _clapBubbleSpot2 = null;
	
	[SerializeField]
	GameObject _beatBubbleSpot = null;
	
	[SerializeField]
	GameObject _drumBubbleSpot = null;
	
	[SerializeField]
	GameObject _drumBubbleSpot2 = null;
	
	[SerializeField]
	GameObject _tripletBubbleSpot = null;
	
	[SerializeField]
	GameObject _tripletBubbleSpot2 = null;
	
	[SerializeField]
	GameObject _tripletBubbleSpot3 = null;
	
	[SerializeField]
	GameObject _measureBubbleSpot = null;
	
	[SerializeField]
	GameObject _pivotPoint = null;
	
	[SerializeField]
	GameObject _heartSpawn = null;
	
	[SerializeField]
	GameObject _heartPrefab = null;
	
	[SerializeField]
	GameObject _demoBubble = null;
	
	GameObject _lastSpawn = null;
	
	const float BUBBLE_SHRINK_LENGTH = 2f;	//2 seconds...
	
	GameObject _matingDancePenguin = null;
	
	public static uint _popCount = 0;
	
	// bool _demoDone = false;
	
	[SerializeField]
	GameObject _walkToSpot = null;
	
    void Start()
    {
		_activeBubbles = new List<GameObject>();
		_activeRoutines = new List<Coroutine>();

        //Debug.Log(GetComponent<AudioSource>().clip.samples);
        _sampleData = new float[GetComponent<AudioSource>().clip.samples];
        GetComponent<AudioSource>().clip.GetData(_sampleData, 0);
		

		if(_dataFile != null)
		{
			string sData = _dataFile.text;
			string[] dataArray = sData.Split('\n');
			//number of time samples = length of data array.
			//Debug.Log(dataArray.Length);
			_timeSamples = new float[dataArray.Length];
			_bubbleTypes = new int[dataArray.Length];
			
			for(int i = 0; i < dataArray.Length; ++i)
			{
				string[] d = dataArray[i].Split('\t');
				if(d[0].Length > 0)
				{
					//Debug.Log(d[0]);
					//parse time stamp
					_timeSamples[i] = float.Parse(d[0]);
					if(d[2].StartsWith("Clap") || d[2].StartsWith("Croak"))
					{
						_bubbleTypes[i] = 0;
					}
					else if(d[2].StartsWith("Beat") || d[2].StartsWith("Beep"))
					{
						_bubbleTypes[i] = 1;
					}
					else if(d[2].StartsWith("Drum") || d[2].StartsWith("Saxophone"))
					{
						_bubbleTypes[i] = 2;
					}
					else if(d[2].StartsWith("Triplet") || d[2].StartsWith("Double-croak"))
					{
						_bubbleTypes[i] = 3;
					}
					else if(d[2].StartsWith("Measure") || d[2].StartsWith("Double-beep"))
					{
						_bubbleTypes[i] = 4;
					}
				}
			}
		}
    }

	IEnumerator DestroyLine(GameObject go, float duration)
	{
		yield return new WaitForSeconds(duration);
		
		Object.Destroy(go);
	}
	
	public override void StartGame()
    {
		Debug.Log("Starting mating dance");
        base.StartGame();
		
		if(_matingDancePenguin == null)
		{
			//todo - not ideal, but for cross-scene reference acquisition..
			_matingDancePenguin = GameObject.FindWithTag("MatingDancePenguin");
			if(_matingDancePenguin != null)
			{
				_matingDancePenguin.transform.GetChild(0).GetComponent<Animator>().SetBool("bop", true);
			}
		}
		else
		{
			_matingDancePenguin.transform.GetChild(0).GetComponent<Animator>().SetBool("bop", true);
		}
		
		GameObject bub = GameObject.Instantiate(_bubblePrefab, _demoBubble.transform);
		
		// Coroutine demo = StartCoroutine(DemoPop(0.7f));
		Coroutine destroy = StartCoroutine(DestroyCo(3.3f, bub, true));

        _activeBubbles.Add(bub);
        // _activeRoutines.Add(demo);
        _activeRoutines.Add(destroy);

        PenguinAnalytics.Instance.LogActivityBegin("mating_dance");
    }
	
	IEnumerator StartMove(Vector3 newSpot, float duration)
	{
		float t = 0f;
		Vector3 startPosition = _matingDancePenguin.transform.position;
		Quaternion startRotation = _matingDancePenguin.transform.rotation;
		Vector3 toNewSpot = newSpot - _matingDancePenguin.transform.position;
		toNewSpot = Vector3.Normalize(toNewSpot);
		Quaternion rotNewSpot = Quaternion.LookRotation(toNewSpot, Vector3.up);

		_matingDancePenguin.transform.rotation = rotNewSpot;
		
		while(t < duration)
		{
			_matingDancePenguin.transform.position = Vector3.Lerp(startPosition, newSpot, (t/duration));

			t += (Time.deltaTime);	
			yield return null;
		}
		
		_matingDancePenguin.transform.rotation = startRotation;
		_matingDancePenguin.transform.GetChild(0).GetComponent<Animator>().SetBool("walk", false);
	}
	
	public override void RestartGame()
    {
		if(_matingDancePenguin != null)
		{
			_matingDancePenguin.transform.GetChild(0).GetComponent<Animator>().SetBool("bop", false);
			_matingDancePenguin.transform.GetChild(0).GetComponent<Animator>().SetBool("walk", false);
		}

		Debug.Log("[Pops] restarting...");
        for (int i = 0; i < _activeBubbles.Count; i++) {
            Object.Destroy(_activeBubbles[i]);
        }
        _activeBubbles.Clear();
        for (int i = 0; i < _activeRoutines.Count; i++) {
            StopCoroutine(_activeRoutines[i]);
        }
        _activeRoutines.Clear();
    }

	public override void EndGame()
	{
		AudioSource audio = GetComponent<AudioSource>();
		if(audio != null)
		{
			audio.Stop();
		}

        Debug.Log("[Pops] ending...");
        for (int i = 0; i < _activeBubbles.Count; i++) {
			Object.Destroy(_activeBubbles[i]);
		}
		_activeBubbles.Clear();
		for (int i = 0; i < _activeRoutines.Count; i++) {
			StopCoroutine(_activeRoutines[i]);
		}
		_activeRoutines.Clear();

		AudioSource mainTrack = PenguinPlayer.Instance.GetComponent<AudioSource>();
		if(mainTrack != null)
		{
			mainTrack.Play();
		}

		_currentSample = 0;
		
		//walk this penguin towards protect the nest
		if(_matingDancePenguin != null)
		{
			_matingDancePenguin.transform.GetChild(0).GetComponent<Animator>().SetBool("walk", true);
			_matingDancePenguin.transform.GetChild(0).GetComponent<Animator>().SetBool("bop", false);
			StartCoroutine(StartMove(_walkToSpot.transform.position, 20f));
		}
		
		_currentSample = 0;
		// _demoDone = false;
		
		PenguinAnalytics.Instance.LogActivityEnd("mating_dance");

		base.EndGame();
	}
	
	IEnumerator DemoPop(float duration)
	{
		yield return new WaitForSeconds(duration);
		
		if(_matingDancePenguin != null)
		{
			_matingDancePenguin.transform.GetChild(0).GetComponent<Animator>().SetBool("peck", true);
		}
	}
	
	IEnumerator DestroyCo(float duration, GameObject toDestroy, bool playSound)
	{
		yield return new WaitForSeconds(duration);
		
		if (!_isGameActive || toDestroy == null) {
			// game terminated or reset before routine completed; return
			yield break;
		}

		if(playSound)
		{
			AudioSource audio = toDestroy.GetComponent<AudioSource>();
			if(audio != null)
			{
				//Debug.Log("Bubble Hit!");
				audio.Play();
				// _demoDone = true;
			}
		}
		_activeBubbles.Remove(toDestroy);
		Object.Destroy(toDestroy);
	}
	
    // Update is called once per frame
    void Update()
    {
		if(_isGameActive)
		{
			// if(_demoDone)
			{
				if(_currentSample == 0)
				{
					AudioSource mainTrack = PenguinPlayer.Instance.GetComponent<AudioSource>();
					if(mainTrack != null)
					{
						mainTrack.Stop();
					}
					
					AudioSource audio = GetComponent<AudioSource>();
					if(audio != null)
					{
						audio.Play();
					}
					
					if(_pivotPoint != null)
					{
						OrientToUser otu = _pivotPoint.GetComponent<OrientToUser>();
						if(otu != null)
						{
							otu.Rotate();
						}
					}
					
					if(_matingDancePenguin == null)
					{
						//todo - not ideal, but for cross-scene reference acquisition..
						_matingDancePenguin = GameObject.FindWithTag("MatingDancePenguin");
						if(_matingDancePenguin != null)
						{
							_matingDancePenguin.transform.GetChild(0).GetComponent<Animator>().SetBool("peck", false);
							_matingDancePenguin.transform.GetChild(0).GetComponent<Animator>().SetBool("bop", true);
						}
					}
					else
					{
						_matingDancePenguin.transform.GetChild(0).GetComponent<Animator>().SetBool("peck", false);
						_matingDancePenguin.transform.GetChild(0).GetComponent<Animator>().SetBool("bop", true);
					}
				}
				
				UpdateTime();
				
				if(_popCount >= 3)
				{
					GameObject heart = GameObject.Instantiate(_heartPrefab, _heartSpawn.transform);
					Coroutine destroy = StartCoroutine(DestroyCo(5f, heart, false));

					_activeBubbles.Add(heart);
					_activeRoutines.Add(destroy);

					_popCount = 0;
				}

				if(_currentSample < _timeSamples.Length-1 && (_totalGameTime > _timeSamples[_currentSample] && _totalGameTime < _timeSamples[_currentSample+1]))
				{
					if(_bubblePrefab != null)
					{
						// TODO: load corresponding audio clip
						if(_bubbleTypes[_currentSample] == 0)
						{
							if(_currentSample % 2 == 0)
							{
								_lastSpawn = GameObject.Instantiate(_bubblePrefab, _clapBubbleSpot.transform);
								PenguinAnalytics.Instance.LogBubbleAppeared(_currentSample, _clapBubbleSpot.transform.position);
							}
							else
							{
								_lastSpawn = GameObject.Instantiate(_bubblePrefab, _clapBubbleSpot2.transform);
								PenguinAnalytics.Instance.LogBubbleAppeared(_currentSample, _clapBubbleSpot2.transform.position);
							}
						}
						else if(_bubbleTypes[_currentSample] == 1)
						{
							_lastSpawn = GameObject.Instantiate(_bubblePrefab, _beatBubbleSpot.transform);
							PenguinAnalytics.Instance.LogBubbleAppeared(_currentSample, _beatBubbleSpot.transform.position);
						}
						else if(_bubbleTypes[_currentSample] == 2)
						{
							if(_currentSample % 2 == 0)
							{
								_lastSpawn = GameObject.Instantiate(_bubblePrefab, _drumBubbleSpot.transform);
								PenguinAnalytics.Instance.LogBubbleAppeared(_currentSample, _drumBubbleSpot.transform.position);
							}
							else
							{
								_lastSpawn = GameObject.Instantiate(_bubblePrefab, _drumBubbleSpot2.transform);
								PenguinAnalytics.Instance.LogBubbleAppeared(_currentSample, _drumBubbleSpot2.transform.position);
							}
						}
						else if(_bubbleTypes[_currentSample] == 3)
						{
							GameObject o = null;
							
							if(_currentSample % 3 == 0)
							{
								o = GameObject.Instantiate(_bubblePrefab, _tripletBubbleSpot.transform);
								PenguinAnalytics.Instance.LogBubbleAppeared(_currentSample, _tripletBubbleSpot.transform.position);
							}
							else if(_currentSample % 3 == 1)
							{
								o = GameObject.Instantiate(_bubblePrefab, _tripletBubbleSpot2.transform);
								PenguinAnalytics.Instance.LogBubbleAppeared(_currentSample, _tripletBubbleSpot2.transform.position);
							}
							else
							{
								o = GameObject.Instantiate(_bubblePrefab, _tripletBubbleSpot3.transform);
								PenguinAnalytics.Instance.LogBubbleAppeared(_currentSample, _tripletBubbleSpot3.transform.position);
							}
							
							if(_bubbleTypes[_currentSample-1] == 3)
							{
								if(_bubbleLinePrefab != null)
								{
									GameObject bubbleLine = GameObject.Instantiate(_bubbleLinePrefab, _tripletBubbleSpot.transform);
									//set the line vertices to this sample and last sample..
									bubbleLine.GetComponent<LineRenderer>().SetPosition(0, _lastSpawn.transform.position);
									bubbleLine.GetComponent<LineRenderer>().SetPosition(1, o.transform.position);
									StartCoroutine(DestroyLine(bubbleLine, BUBBLE_SHRINK_LENGTH));
								}
							}
							
							_lastSpawn = o;
						}
						else if(_bubbleTypes[_currentSample] == 4)
						{
							_lastSpawn = GameObject.Instantiate(_bubblePrefab, _measureBubbleSpot.transform);
							PenguinAnalytics.Instance.LogBubbleAppeared(_currentSample, _measureBubbleSpot.transform.position);
						}
					}

					if(_lastSpawn != null)
					{
						_lastSpawn.transform.GetChild(0).gameObject.GetComponent<ShrinkRing>().SetWhichBubble(_currentSample);
						_activeBubbles.Add(_lastSpawn);
					}
					
					_currentSample++;
				}
				else
				{
					if(_currentSample == _timeSamples.Length - 2)
					{
						_popCount = 0;
						EndGame();
					}
				}
			}
		}
    }
}
