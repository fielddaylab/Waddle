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
	GameObject _tripletBubbleSpot = null;
	
	[SerializeField]
	GameObject _measureBubbleSpot = null;
	
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
			Debug.Log(dataArray.Length);
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
					if(d[2].StartsWith("Clap"))
					{
						_bubbleTypes[i] = 0;
					}
					else if(d[2].StartsWith("Beat"))
					{
						_bubbleTypes[i] = 1;
					}
					else if(d[2].StartsWith("Drum"))
					{
						_bubbleTypes[i] = 2;
					}
					else if(d[2].StartsWith("Triplet"))
					{
						_bubbleTypes[i] = 3;
					}
					else if(d[2].StartsWith("Measure"))
					{
						_bubbleTypes[i] = 4;
					}
				}
			}
		}
    }

	public override void StartGame()
    {
        base.StartGame();

        AudioSource audio = GetComponent<AudioSource>();
		if(audio != null)
		{
			audio.Play();
		}
    }
	
	public override void EndGame()
	{
		AudioSource audio = GetComponent<AudioSource>();
		if(audio != null)
		{
			audio.Stop();
		}
		
		base.EndGame();
	}
	
    // Update is called once per frame
    void Update()
    {
		if(_isGameActive)
		{
			UpdateTime();
			
			if(_currentSample < _timeSamples.Length-1 && (_totalGameTime > _timeSamples[_currentSample] && _totalGameTime < _timeSamples[_currentSample+1]))
			{
				if(_bubblePrefab != null)
				{
					if(_bubbleTypes[_currentSample] == 0)
					{
						if(_currentSample % 2 == 0)
						{
							GameObject.Instantiate(_bubblePrefab, _clapBubbleSpot.transform);
						}
						else
						{
							GameObject.Instantiate(_bubblePrefab, _clapBubbleSpot2.transform);
						}
					}
					else if(_bubbleTypes[_currentSample] == 1)
					{
						GameObject.Instantiate(_bubblePrefab, _beatBubbleSpot.transform);
					}
					else if(_bubbleTypes[_currentSample] == 2)
					{
						GameObject.Instantiate(_bubblePrefab, _drumBubbleSpot.transform);
					}
					else if(_bubbleTypes[_currentSample] == 3)
					{
						GameObject.Instantiate(_bubblePrefab, _tripletBubbleSpot.transform);
					}
					else if(_bubbleTypes[_currentSample] == 4)
					{
						GameObject.Instantiate(_bubblePrefab, _measureBubbleSpot.transform);
					}
				}
				_currentSample++;
			}
		}
		
        //about 44,100 samples per second...
        //Debug.Log(_sampleData[GetComponent<AudioSource>().timeSamples].ToString("F4"));
        //pre-define a set of samples for when we need to spawn a bubble, based on the music...
        /*if(_sampleData[GetComponent<AudioSource>().timeSamples] > 0.5f)
        {
            if(_bubblePrefab != null)
            {
                GameObject.Instantiate(_bubblePrefab);
            }
        }*/
    }
}
