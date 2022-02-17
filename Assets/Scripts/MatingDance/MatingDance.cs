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
	
	GameObject _lastSpawn = null;
	
	const float BUBBLE_SHRINK_LENGTH = 2f;	//2 seconds...
	
	GameObject _matingDancePenguin = null;
	
	public static uint _popCount = 0;
	
    void Start()
    {
        Debug.Log(GetComponent<AudioSource>().clip.samples);
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
					else if(d[2].StartsWith("Drum"))
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
				_matingDancePenguin.transform.GetChild(0).GetComponent<Animator>().SetBool("bop", true);
			}
		}
		else
		{
			_matingDancePenguin.transform.GetChild(0).GetComponent<Animator>().SetBool("bop", true);
		}
    }
	
	public override void EndGame()
	{
		AudioSource audio = GetComponent<AudioSource>();
		if(audio != null)
		{
			audio.Stop();
		}
		
		AudioSource mainTrack = PenguinPlayer.Instance.GetComponent<AudioSource>();
		if(mainTrack != null)
		{
			mainTrack.Play();
		}
		
		_currentSample = 0;
		if(_matingDancePenguin != null)
		{
			_matingDancePenguin.transform.GetChild(0).GetComponent<Animator>().SetBool("bop", false);
		}
		
		base.EndGame();
	}
	
	IEnumerator DestroyCo(float duration, GameObject toDestroy)
	{
		yield return new WaitForSeconds(duration);
		
		Object.Destroy(toDestroy);
	}
	
    // Update is called once per frame
    void Update()
    {
		if(_isGameActive)
		{
			UpdateTime();
			
			if(_popCount >= 3)
			{
				GameObject heart = GameObject.Instantiate(_heartPrefab, _heartSpawn.transform);
				StartCoroutine(DestroyCo(5f, heart));
				_popCount = 0;
			}
		
			if(_currentSample < _timeSamples.Length-1 && (_totalGameTime > _timeSamples[_currentSample] && _totalGameTime < _timeSamples[_currentSample+1]))
			{
				if(_bubblePrefab != null)
				{
					if(_bubbleTypes[_currentSample] == 0)
					{
						if(_currentSample % 2 == 0)
						{
							_lastSpawn = GameObject.Instantiate(_bubblePrefab, _clapBubbleSpot.transform);
						}
						else
						{
							_lastSpawn = GameObject.Instantiate(_bubblePrefab, _clapBubbleSpot2.transform);
						}
					}
					else if(_bubbleTypes[_currentSample] == 1)
					{
						_lastSpawn = GameObject.Instantiate(_bubblePrefab, _beatBubbleSpot.transform);
					}
					else if(_bubbleTypes[_currentSample] == 2)
					{
						if(_currentSample % 2 == 0)
						{
							_lastSpawn = GameObject.Instantiate(_bubblePrefab, _drumBubbleSpot.transform);
						}
						else
						{
							_lastSpawn = GameObject.Instantiate(_bubblePrefab, _drumBubbleSpot2.transform);
						}
					}
					else if(_bubbleTypes[_currentSample] == 3)
					{
						GameObject o = null;
						
						if(_currentSample % 3 == 0)
						{
							o = GameObject.Instantiate(_bubblePrefab, _tripletBubbleSpot.transform);
						}
						else if(_currentSample % 3 == 1)
						{
							o = GameObject.Instantiate(_bubblePrefab, _tripletBubbleSpot2.transform);
						}
						else
						{
							o = GameObject.Instantiate(_bubblePrefab, _tripletBubbleSpot3.transform);
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
					}
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
