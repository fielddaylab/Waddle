//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using BeauRoutine;
using FieldDay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Waddle;

public class ProtectTheNest : MiniGameController
{
    [Serializable]
    public struct RoundInfo {
        public int BeatCount;
        public int SkuaSpawns;
        public int SpawnFrequency;
        public int MovementFrequency;
        public int AttackFrequency;
        public int AttackerCount;
    }

    [SerializeField]
    SkuaSpawner _skuaSpawner = null;

    [SerializeField]
    float _gameTimeLimit = 60f;
    public float GameTimeLimit => _gameTimeLimit;

	[SerializeField]
	GameObject _eggTimer = null;
	
	[SerializeField]
	Egg _theEgg;

    [SerializeField]
    MusicAsset _music;

    [SerializeField]
    private RoundInfo[] m_Rounds;

    [Header("Ending")]

    [SerializeField]
    GameObject _endingEgg;

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
    AudioSource _hatchingFanfare;

    [NonSerialized] float _skuaMoveTime = 0f;
    [NonSerialized] float _timeWithoutEgg = 0f;

    [NonSerialized] bool _playingEggSequence = false;
    [NonSerialized] bool _finishingEggSequence = false;
	[NonSerialized] bool _chickStarting = false;

    [NonSerialized] GameObject _mainCam = null;
    [NonSerialized] GameObject _ptnUnlocker;
    [NonSerialized] private RoundInfo m_CurrentRound;
    [NonSerialized] private int m_CurrentRoundIndex = 0;
    [NonSerialized] private int m_CurrentRoundBeatIndex;

    // Start is called before the first frame update
    void Start()
    {
        _mainCam = Camera.main.gameObject;

        _ptnUnlocker = GameObject.Find("ProtectTheNestUnlocker");
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
				Vector3 toEgg = Vector3.Normalize(_endingEgg.transform.position - _mainCam.transform.position);
				Vector3 lookDir = _mainCam.transform.forward;
				// Wait until player is looking at egg
				if(Vector3.Dot(toEgg, lookDir) > 0.5f)
				{
					_finishingEggSequence = true;
					
					AudioSource aSource = _endingEgg.GetComponent<AudioSource>();
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
        _skuaSpawner.UpdateLookScores();
		
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
            MusicUtility.Stop();

            StartCoroutine(StartChickSequence(2.0f));
            //EndGame();
            return;
		}

        if(_skuaSpawner != null)
        {
            MusicState music = Game.SharedState.Get<MusicState>();

            if (music.OnBeat) {
                m_CurrentRoundBeatIndex++;

                if (m_CurrentRound.BeatCount > 0 && m_CurrentRoundBeatIndex >= m_CurrentRound.BeatCount) {
                    m_CurrentRound = m_Rounds[m_CurrentRoundIndex++];
                    m_CurrentRoundBeatIndex = 0;
                }

                if (m_CurrentRound.SkuaSpawns > 0 && OnBeatInterval(m_CurrentRound.SpawnFrequency, -1)) {
                    m_CurrentRound.SkuaSpawns--;
                    _skuaSpawner.SpawnSkua();
                }

                int attackers = 0;
                bool normalMove = OnBeatInterval(m_CurrentRound.MovementFrequency, 1);
                if (!EggIsTaken() && OnBeatInterval(m_CurrentRound.AttackFrequency, 0)) {
                    attackers = m_CurrentRound.AttackerCount;
                }

                if (attackers > 0 || normalMove) {
                    _skuaSpawner.MoveSkuas(attackers, normalMove);
                    _skuaMoveTime = _currentTime;
                }
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

    private bool OnBeatInterval(int interval, int offset) {
        return interval <= 0 || ((m_CurrentRoundBeatIndex + offset) % interval) == 0;
    }

	bool EggIsTaken()
	{
        if(_theEgg != null)
        {
		    return _theEgg.IsTaken;
        }

        return false;
	}

    public override void StartGame()
    {
        base.StartGame();

		foreach(var particles in _preHatchParticles) {
            particles.Pause();
            particles.Clear();
        }
        _hatchHeartParticles.Pause();
        _hatchHeartParticles.Clear();

        _isolationVoid.SetActive(false);
        _skuaMoveTime = _startTime;
        _timeWithoutEgg = _startTime;
        _theEgg.gameObject.SetActive(true);
        _theNest.SetActive(false);
        _newbornCheeper.SetState(Cheeper.CheepState.None);

        MusicUtility.Play(_music, 1);

        PenguinAnalytics.Instance.LogActivityBegin("skuas");

        m_CurrentRoundIndex = 0;
        m_CurrentRound = m_Rounds[0];
        m_CurrentRoundBeatIndex = 0;
    }

    public override void RestartGame()
    {
        PenguinAnalytics.Instance.LogActivityEnd("skuas");
		
		if(_skuaSpawner != null)
		{
			_skuaSpawner.ClearGame();
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


        if (_theEgg != null) {
            _theEgg.IsTaken = false;
            _theEgg.ResetToStart();
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

        if (_endingEgg != null)
        {
			AudioSource aSource = _endingEgg.transform.GetChild(2).GetComponent<AudioSource>();
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
        Game.SharedState.Get<PlayerProgressState>().CompletedGames.Add(PenguinGameManager.MiniGame.ProtectTheNest);

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
        _theNest.SetActive(true);
        PenguinPlayer.Instance.transform.position = _isolationPos.transform.position;

        Vector3 lookVec = _endingEgg.transform.position - PenguinPlayer.Instance.transform.position;
        lookVec.y = 0;

        Quaternion absFacing = Quaternion.LookRotation(lookVec, Vector3.up);
        Vector3 headRot = Game.SharedState.Get<PlayerHeadState>().BodyRoot.localEulerAngles;
        headRot.x = headRot.z = 0;

        PenguinPlayer.Instance.transform.rotation = absFacing * Quaternion.Inverse(Quaternion.Euler(headRot));
        Game.SharedState.Get<PlayerVignetteState>().FadeEnabled = false;
        Game.SharedState.Get<WeatherState>().Mute = true;

        // activate isolation void
        _isolationVoid.SetActive(true);

        _hatchingFanfare.Play();

        OVRScreenFade.instance.FadeIn(fadeDuration);

        yield return new WaitForSeconds(delay);

        _ptnUnlocker.transform.GetChild((int) MiniGameUnlocker.MiniGameCommonObjects.SNOW).gameObject.SetActive(false);

        // Activate particle fountain
        foreach (var particles in _preHatchParticles) {
            particles.Play();
        }

		_newbornCheeper.SetRate(30);
		_newbornCheeper.SetState(Cheeper.CheepState.Muffled);
		_newbornCheeper.SetFade(0, 0.8f, 3f);

        if (_endingEgg != null)
		{
            _endingEgg.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("shake");
		}

		_chickStarting = false;
	}
	
	IEnumerator FinishChickSequence(float waitTime)
	{
		// Breaking egg
		if(_endingEgg != null)
		{
            _endingEgg.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("break");

            _endingEgg.transform.GetChild(2).gameObject.GetComponent<Animator>().SetTrigger("break");
		}
		
		yield return new WaitForSeconds(8f);
		waitTime -= 8f;

		// Emerging Chick

        foreach (var particles in _preHatchParticles) {
            particles.Pause();
			particles.Clear();
        }
        _newbornCheeper.SetRate(60);
        _newbornCheeper.SetState(Cheeper.CheepState.Open);
        _newbornCheeper.SetFade(0.8f, 1, 3f);

        _endingEgg.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("stop");
        _endingEgg.transform.GetChild(0).gameObject.SetActive(false);

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

		_endingEgg.gameObject.SetActive(false);
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

    public void Fail() {
        if (!this._isGameActive) {
            return;
        }

        this._isGameActive = false;
        _ptnUnlocker.GetComponent<StartGame>().FailGame();
        PenguinAnalytics.Instance.LogActivityEnd("skuas");
    }
}
