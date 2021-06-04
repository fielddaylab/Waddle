using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waddle : MonoBehaviour
{
	public GameObject _centerEye;
	public GameObject _ovrPlayer;
	
	public float _speed = 100f;
	public float _threshold = 0.002f;
	
	Vector3 _playerLastFrame;
	Vector3 _newLocation = Vector3.zero;
	Vector3 _startLocation = Vector3.zero;
	Vector3 _lookDir = Vector3.zero;
	
	bool _changedSide = false;
	
	[SerializeField]
	bool _lastSidePositive = false;
	
	bool _movementDone = true;
	
	float _transitionDuration = 1.5f;
	float _playerDist = 0f;
	
    // Start is called before the first frame update
    void Start()
    {
        SetLastPositions();
    }

	IEnumerator MoveForward()
	{
		float t = 0f;
		
		while(t < 1f)
		{
			t += (Time.deltaTime * (Time.timeScale/_transitionDuration));	
			
			transform.position = Vector3.Lerp(_startLocation, _newLocation, t);
			
			if(t >= 1f)
			{
				_movementDone = true;
			}
			
			yield return null;
		}
	}
	
    // Update is called once per frame
    void Update()
    {
        _playerDist = Vector3.Distance(_playerLastFrame, _centerEye.transform.position);
		
		if(_playerDist > _threshold)
		{
			float headSpeed = _playerDist;
			
			if(headSpeed > 1f)
			{
				headSpeed = 1f;
			}
			
			Vector3 lrVec = Vector3.Cross(_ovrPlayer.transform.forward, Vector3.up);
			
			Plane p = new Plane(lrVec, _ovrPlayer.transform.position);
			
			if(Time.timeSinceLevelLoad > 1f)
			{
				if(p.GetSide(_centerEye.transform.position))
				{
					if(!_lastSidePositive)
					{
						_changedSide = true;
					}
					
					_lastSidePositive = true;
				}
				else
				{
					if(_lastSidePositive)
					{
						_changedSide = true;
					}
					
					_lastSidePositive = false;
				}
				
				if(_changedSide)// && _movementDone)
				{
					/*_startLocation = transform.position;
					_newLocation = transform.position - _ovrPlayer.transform.forward * headSpeed * _speed * Time.deltaTime;
					_movementDone = false;

					StartCoroutine(MoveForward());*/
					transform.position -= _ovrPlayer.transform.forward * headSpeed * _speed * Time.deltaTime; 
					_changedSide = false;
				}
			}
		}
		
		SetLastPositions();
    }
	
	void SetLastPositions()
	{
		_playerLastFrame = _centerEye.transform.position;
	}
}
