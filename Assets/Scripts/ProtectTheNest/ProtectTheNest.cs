//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using BeauRoutine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectTheNest : MiniGameController
{
    [SerializeField]
    SkuaCoordinator m_Coordinator = null;

    [SerializeField]
    float _gameTimeLimit = 60f;
    public float GameTimeLimit => _gameTimeLimit;

	[SerializeField]
	GameObject _eggTimer = null;
	
	[SerializeField]
	GameObject _theEgg;
	public GameObject TheEgg => _theEgg;

    [SerializeField]
    GameObject _theNest;

    [SerializeField]
    ParticleSystem[] _preHatchParticles;

    [SerializeField]
    ParticleSystem _hatchHeartParticles;

	[SerializeField]
	Cheeper _newbornCheeper;

    [SerializeField]
	GameObject _isolationVoid;

	[SerializeField]
	Transform _isolationPos;

	[SerializeField]
	CanvasGroup _endTextGroup;

    float _skuaMoveTime = 0f;
    float _timeWithoutEgg = 0f;
    
	bool _playingEggSequence = false;
	bool _finishingEggSequence = false;
	bool _chickStarting = false;
	
	GameObject _mainCam = null;
	
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
			if(!_finishingEggSequence && !_chickStarting)
			{
				Vector3 toEgg = Vector3.Normalize(_theEgg.transform.position - _mainCam.transform.position);
				Vector3 lookDir = _mainCam.transform.forward;
				// Wait until player is looking at egg
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
					
					StartCoroutine(FinishChickSequence(20f));
				}
			}
			
			return;
		}
		
        UpdateTime();
		
        if(_timeWithoutEgg - _startTime > _gameTimeLimit)
		{
			_timeWithoutEgg = 0f;
			_skuaMoveTime = 0f;

			if(m_Coordinator != null)
			{
				m_Coordinator.End();
			}
			
			_playingEggSequence = true;
			_eggTimer.SetActive(false);
			
			StartCoroutine(StartChickSequence(2.0f));
            //EndGame();
            return;
		}

        if(m_Coordinator != null)
        {
            //m_Coordinator.CheckForSpawn(_currentTime - _startTime);

            //if(_currentTime - _skuaMoveTime > _moveFrequency)
            //{
            //    m_Coordinator.MoveSkuas();
            //    _skuaMoveTime = _currentTime;
            //}

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
						PenguinAnalytics.Instance.LogEggTimer(timeLeft);
                    }
                    else
                    {
                        System.TimeSpan ts = System.TimeSpan.FromSeconds(0);
                        _eggTimer.GetComponent<TMPro.TextMeshPro>().text = string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
						PenguinAnalytics.Instance.LogEggTimer(0f);
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

		foreach(var particles in _preHatchParticles) {
            particles.Pause();
            particles.Clear();
        }
        _hatchHeartParticles.Pause();
        _hatchHeartParticles.Clear();

        _isolationVoid.SetActive(false);
        _skuaMoveTime = _startTime;
        _timeWithoutEgg = _startTime;
        _theEgg.SetActive(true);
        _theNest.SetActive(true);
        _newbornCheeper.SetState(Cheeper.CheepState.None);


        PenguinAnalytics.Instance.LogActivityBegin("skuas");
    }

    public override void RestartGame()
    {
		PenguinAnalytics.Instance.LogActivityEnd("skuas");
		
		if(m_Coordinator != null)
		{
            m_Coordinator.End();
		}
		
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
		
		_playingEggSequence = false;
		_finishingEggSequence = false;
        _isolationVoid.SetActive(false);

        if (_theEgg != null)
        {
			AudioSource aSource = _theEgg.transform.GetChild(2).GetComponent<AudioSource>();
			if(aSource != null)
			{
				aSource.Stop();
				
				AudioSource aSource2 = aSource.gameObject.transform.GetChild(0).GetComponent<AudioSource>();
				if(aSource2 != null)
				{
					aSource2.Stop();
				}
			}
		}
    }

    public override void EndGame()
    {
        //Camera.main.gameObject.GetComponent<OVRScreenFade>().FadeOut();
        _playingEggSequence = false;
		_finishingEggSequence = false;
        _isolationVoid.SetActive(false);
        foreach (var particles in _preHatchParticles) {
            particles.Pause();
            particles.Clear();
        }
        _hatchHeartParticles.Pause();
        _hatchHeartParticles.Clear();
        _newbornCheeper.SetState(Cheeper.CheepState.None);

        if (_theEgg != null)
        {
			AudioSource aSource = _theEgg.transform.GetChild(2).GetComponent<AudioSource>();
			if(aSource != null)
			{
				aSource.Stop();
				
				AudioSource aSource2 = aSource.gameObject.transform.GetChild(0).GetComponent<AudioSource>();
				if(aSource2 != null)
				{
					aSource2.Stop();
				}
			}
		}
		
		PenguinAnalytics.Instance.LogActivityEnd("skuas");
		
        base.EndGame();

		// Return to starting position

		PenguinGameManager.Instance.RestartGame();

		/* Show menu
		if(PenguinGameManager.Instance.GetGameMode == PenguinGameManager.GameMode.ResearchMode)
		{
			PenguinPlayer.Instance.gameObject.GetComponent<HandRaycast>().SwitchPanel(HandRaycast.MenuPanel.eMAIN);
		}
		
		PenguinMenuSystem.Instance.ChangeMenuTo(PenguinMenuSystem.MenuType.EndMenu);
		
		PenguinGameManager.Instance.ShowEndGameMenu();
		*/
    }
	
	IEnumerator StartChickSequence(float delay)
	{
        _chickStarting = false;

        float fadeDuration = 2f;
        OVRScreenFade.instance.FadeOut(fadeDuration);
        // yield return new WaitForSeconds(fadeDuration); // Curently breaks the animation

        _hatchHeartParticles.Pause();
        _hatchHeartParticles.Clear();
        _endTextGroup.alpha = 0;
        PenguinPlayer.Instance.transform.position = _isolationPos.transform.position;
		PenguinPlayer.Instance.transform.LookAt(_theEgg.transform);

        // activate isolation void
        _isolationVoid.SetActive(true);

        OVRScreenFade.instance.FadeIn(fadeDuration);

        yield return new WaitForSeconds(delay);


        // Activate particle fountain
        foreach (var particles in _preHatchParticles) {
            particles.Play();
        }

		_newbornCheeper.SetRate(30);
		_newbornCheeper.SetState(Cheeper.CheepState.Muffled);
		_newbornCheeper.SetFade(0.4f, 0.8f, 3f);

        if (_theEgg != null)
		{
			_theEgg.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("shake");
		}

		_chickStarting = false;
	}
	
	IEnumerator FinishChickSequence(float waitTime)
	{
		// Breaking egg
		if(_theEgg != null)
		{
			_theEgg.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("break");
			
			_theEgg.transform.GetChild(2).gameObject.GetComponent<Animator>().SetTrigger("break");
		}
		
		yield return new WaitForSeconds(8f);
		waitTime -= 8f;

		// Emerging Chick

        foreach (var particles in _preHatchParticles) {
            particles.Pause();
			particles.Clear();
        }
        _newbornCheeper.SetRate(30);
        _newbornCheeper.SetState(Cheeper.CheepState.Open);
        _newbornCheeper.SetFade(0.8f, 1, 3f);

        _theEgg.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("stop");
		_theEgg.transform.GetChild(0).gameObject.SetActive(false);

        yield return new WaitForSeconds(3f);
		waitTime -= 3f;

		// Hearts above emerging chick

        _hatchHeartParticles.Play();

        yield return new WaitForSeconds(waitTime);

		// Animation completed



        // _theEgg.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("stop");

        //loop the chick here instead?
        //_theEgg.transform.GetChild(2).gameObject.GetComponent<Animator>().SetTrigger("stop");

        //show the ending menu...!

        float fadeDuration = 2f;
        OVRScreenFade.instance.FadeOut(fadeDuration);

        yield return new WaitForSeconds(fadeDuration + 1);

		_theEgg.SetActive(false);
		_theNest.SetActive(false);

        PenguinMenuSystem.Instance.ChangeMenuTo(PenguinMenuSystem.MenuType.EndText);
        PenguinGameManager.Instance.ShowEndGameText();

        OVRScreenFade.instance.FadeIn(fadeDuration);

        PenguinPlayer.Instance.StartBackgroundMusic();

        yield return new WaitForSeconds(5f);

        OVRScreenFade.instance.FadeOut(fadeDuration);

        yield return new WaitForSeconds(fadeDuration);

        EndGame();
	}
}
