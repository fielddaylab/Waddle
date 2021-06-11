using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waddle : MonoBehaviour
{
	public GameObject _centerEye;
	public GameObject _ovrPlayer;
	public GameObject _debugPlane = null;
	
	public float _speed = 100f;
	public float _threshold = 0.002f;
	public float _timeThreshold = 1f;
	public float _waddleTime = 0f;
	
	Vector3 _playerLastFrame = Vector3.zero;
	
	Plane _waddlePlane = new Plane();
	
	bool _changedSide = false;
	
	//[SerializeField]
	int _lastSidePositive = -1;
	
	bool _movementDone = true;
	float _playerDist = 0f;
	
    // Start is called before the first frame update
    void Start()
    {
        SetLastPositions();
    }

	/*IEnumerator MoveForward(Vector3 targetPosition, float duration)
	{
		float t = 0f;
		
		Vector3 startPosition = transform.position;
		
		while(t < duration)
		{
			transform.position = Vector3.Lerp(startPosition, targetPosition, (t/duration));
			
			t += (Time.deltaTime);	
			
			yield return null;
		}
		
		_movementDone = true;
		
		transform.position = targetPosition;
	}*/
	
    // Update is called once per frame
    void Update()
    {
		if(Time.timeSinceLevelLoad <= 1f)
		{
			Vector3 lrVec = Vector3.Cross(_centerEye.transform.forward, Vector3.up);
			_waddlePlane.SetNormalAndPosition(lrVec, _centerEye.transform.position);
		
			if(_debugPlane != null)
			{
				_debugPlane.transform.position = _centerEye.transform.position;
				_debugPlane.transform.rotation = Quaternion.LookRotation(_centerEye.transform.forward, lrVec);
			}
			return;
		}
		
        _playerDist = Vector3.Distance(_playerLastFrame, _centerEye.transform.position);
		
		if(_playerDist > _threshold)
		{
			/*float headSpeed = _playerDist;
			
			if(headSpeed > 1f)
			{
				headSpeed = 1f;
			}*/
			
			float headMoveDistance = _waddlePlane.GetDistanceToPoint(_centerEye.transform.position);
			
			if(_waddlePlane.GetSide(_centerEye.transform.position))
			{
				if(_lastSidePositive == 0)
				{
					_changedSide = true;
					_waddleTime = 0f;
				}
				else if(_lastSidePositive == -1)
				{
					_changedSide = false;
					_waddleTime = 0f;
				}
				_lastSidePositive = 1;
			}
			else
			{
				if(_lastSidePositive == 1)
				{
					_changedSide = true;
					_waddleTime = 0f;
				}
				else if(_lastSidePositive == -1)
				{
					_changedSide = false;
					_waddleTime = 0f;
				}

				_lastSidePositive = 0;
			}
			
			//could also add a distance check side-to-side for the plane here
			if(_changedSide)// && _movementDone)
			{
				//Vector3 newPos = transform.position - _ovrPlayer.transform.forward * headSpeed * _speed * Time.deltaTime;
				//_movementDone = false;
				//StartCoroutine(MoveForward(newPos, 2f));
				transform.position -= _ovrPlayer.transform.forward * _speed * Time.deltaTime; 
				_changedSide = false;
				_waddleTime = 0f;
				//when to update the waddle plane so the user can turn, etc. while moving...
				/*Vector3 lrVec = Vector3.Cross(_centerEye.transform.forward, Vector3.up);
				Vector3 newPos = _centerEye.transform.position;
				
				if(_lastSidePositive == 0)
				{
				newPos += (lrVec * _playerDist);
				}
				else if(_lastSidePositive == 1)
				{
					newPos -= (lrVec * _playerDist);
				}
				
				_waddlePlane.SetNormalAndPosition(lrVec, newPos);
			
				if(_debugPlane != null)
				{
					_debugPlane.transform.position = newPos;
					_debugPlane.transform.rotation = Quaternion.LookRotation(_ovrPlayer.transform.forward, lrVec);
				}*/
				
			}
			
		}
		else
		{
			_waddleTime += UnityEngine.Time.deltaTime;
			//allow  a time interval for valid movement...
			if(_waddleTime > _timeThreshold)
			{
				Vector3 lrVec = Vector3.Cross(_centerEye.transform.forward, Vector3.up);
				_waddlePlane.SetNormalAndPosition(lrVec, _centerEye.transform.position);
			
				if(_debugPlane != null)
				{
					_debugPlane.transform.position = _centerEye.transform.position;
					_debugPlane.transform.rotation = Quaternion.LookRotation(_centerEye.transform.forward, lrVec);
				}
				
				_lastSidePositive = -1;
				_waddleTime = 0f;
				_changedSide = false;
			}
		}
		
		SetLastPositions();
    }
	
	void SetLastPositions()
	{
		_playerLastFrame = _centerEye.transform.position;
	}
}
