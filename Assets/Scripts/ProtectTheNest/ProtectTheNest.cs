//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectTheNest : MiniGameController
{
    [SerializeField]
    SkuaSpawner _skuaSpawner = null;

	[SerializeField]
	float _moveFrequency = 2f;
	public float MoveFrequency => _moveFrequency;

    [SerializeField]
    float _gameTimeLimit = 60f;
    public float GameTimeLimit => _gameTimeLimit;

	[SerializeField]
	GameObject _eggTimer = null;
	
	[SerializeField]
	GameObject _theEgg;
	public GameObject TheEgg => _theEgg;

    float _skuaMoveTime = 0f;
    float _timeWithoutEgg = 0f;
    
	bool _playingEggSequence = false;
	bool _finishingEggSequence = false;
	
	GameObject _mainCam = null;
	GameObject _ptnUnlocker = null;
    // Start is called before the first frame update
    void Start()
    {
        _mainCam = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_isGameActive)
        {
            return;
        }
		
		if(_playingEggSequence)
		{
			if(!_finishingEggSequence)
			{
				Vector3 toEgg = Vector3.Normalize(_theEgg.transform.position - _mainCam.transform.position);
				Vector3 lookDir = _mainCam.transform.forward;
				if(Vector3.Dot(toEgg, lookDir) > 0.5f)
				{
					_finishingEggSequence = true;
					
					AudioSource aSource = _theEgg.GetComponent<AudioSource>();
					if(aSource != null)
					{
						GameObject ptnUnlocker = GameObject.Find("ProtectTheNestUnlocker");
						if(ptnUnlocker != null)
						{
							AudioSource aSource2 = ptnUnlocker.GetComponent<AudioSource>();
							if(aSource2 != null)
							{
								aSource2.Stop();
								aSource.Play();
							}
						}
					}
					
					StartCoroutine(FinishChickSequence(30f));
				}
			}
			
			return;
		}
		
        UpdateTime();
		
        if(_timeWithoutEgg - _startTime > _gameTimeLimit)
		{
			_timeWithoutEgg = 0f;
			_skuaMoveTime = 0f;

			if(_skuaSpawner != null)
			{
				_skuaSpawner.ClearGame();
			}
			
			_playingEggSequence = true;
			_eggTimer.SetActive(false);
			
			StartCoroutine(StartChickSequence(2.0f));
            //EndGame();
            return;
		}

        if(_skuaSpawner != null)
        {
            _skuaSpawner.CheckForSpawn(_currentTime - _startTime);

            if(_currentTime - _skuaMoveTime > _moveFrequency)
            {
                _skuaSpawner.MoveSkuas();
                _skuaMoveTime = _currentTime;
            }

            if(_eggTimer != null)
            {
                if(!EggIsTaken())
                {
                    _timeWithoutEgg += Time.deltaTime;
                    float t = _timeWithoutEgg - _startTime;
                    float timeLeft = _gameTimeLimit - t;
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
    }

	bool EggIsTaken()
	{
        if(_theEgg != null)
        {
		    return _theEgg.GetComponent<Egg>().IsTaken;
        }

        return false;
	}

    public override void StartGame()
    {
        base.StartGame();
		
		if(_theEgg != null)
		{
			_theEgg.transform.GetChild(0).gameObject.SetActive(true);
		}
		
        _skuaMoveTime = _startTime;
        _timeWithoutEgg = _startTime;
    }

    public override void RestartGame()
    {
		//double check this after addition of chick sequence..
        if(_theEgg != null)
        {
		     _theEgg.GetComponent<Egg>().IsTaken = false;
             _theEgg.GetComponent<Egg>().Reset();
        }
		
        if(_eggTimer != null)
        {
			_eggTimer.SetActive(true);
            System.TimeSpan ts = System.TimeSpan.FromSeconds(_gameTimeLimit);
            _eggTimer.GetComponent<TMPro.TextMeshPro>().text = string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
        }

    }

    IEnumerator StartNextFrame()
    {
        yield return null;
        
        //Camera.main.gameObject.GetComponent<OVRScreenFade>().FadeIn();

        if(_skuaSpawner != null)
        {
            _skuaSpawner.StartGame();
        }

        //StartGame();
    }

    public override void EndGame()
    {
        //Camera.main.gameObject.GetComponent<OVRScreenFade>().FadeOut();
        _playingEggSequence = false;
		_finishingEggSequence = false;
		
        base.EndGame();
    }
	
	IEnumerator StartChickSequence(float delay)
	{
		yield return new WaitForSeconds(delay);
		
		if(_theEgg != null)
		{
			_theEgg.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("shake");
		}
	}
	
	IEnumerator FinishChickSequence(float waitTime)
	{
		if(_theEgg != null)
		{
			_theEgg.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("break");
			
			_theEgg.transform.GetChild(2).gameObject.GetComponent<Animator>().SetTrigger("break");
		}
		
		yield return new WaitForSeconds(8f);
		
		_theEgg.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("stop");
		_theEgg.transform.GetChild(0).gameObject.SetActive(false);
		
		yield return new WaitForSeconds(waitTime-8f);
		
		//_theEgg.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("stop");
		
		//loop the chick here instead?
		//_theEgg.transform.GetChild(2).gameObject.GetComponent<Animator>().SetTrigger("stop");
		
		AudioSource mainTrack = PenguinPlayer.Instance.GetComponent<AudioSource>();
		if(mainTrack != null)
		{
			mainTrack.Play();
		}
		
		EndGame();
	}
}
