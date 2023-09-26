//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MiniGameController : MonoBehaviour
{
    [NonSerialized] protected float _startTime = 0f;
    [NonSerialized] protected float _totalGameTime = 0f;
    [NonSerialized] protected float _currentTime = 0f;
    [NonSerialized] protected bool _isGameActive = false;

	public delegate void OnEndGameDelegate();
	public static event OnEndGameDelegate _endGameDelegate;

    public virtual void StartGame()
    {
        _isGameActive = true;
        _startTime = UnityEngine.Time.time;
		_totalGameTime = 0f;
    }

    public virtual void EndGame()
    {
        _isGameActive = false;
        _startTime = 0f;
		
		_endGameDelegate();
    }

    public virtual void RestartGame()
    {
        EndGame();
        StartGame();
    }

    public void UpdateTime()
    {
        if(_isGameActive)
        {
            _currentTime = UnityEngine.Time.time;
            _totalGameTime += UnityEngine.Time.deltaTime;
        }
    }
}
