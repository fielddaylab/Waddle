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

    [SerializeField]
    GameObject _startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public virtual void StartGame()
    {
        _isGameActive = true;
        _startTime = UnityEngine.Time.time;
    }

    public virtual void EndGame()
    {
        //eventually have a blizzard arrive here.
        Camera.main.gameObject.GetComponent<OVRScreenFade>().FadeOut();
        _isGameActive = false;
        _startTime = 0f;
    }

    public void RestartGame()
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

    // Update is called once per frame
    void Update()
    {
       
    }
}
