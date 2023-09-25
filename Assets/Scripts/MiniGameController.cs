//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameController : MonoBehaviour
{
    protected float _startTime = 0f;
    protected float _totalGameTime = 0f;
    protected float _currentTime = 0f;
    protected bool _isGameActive = false;

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
