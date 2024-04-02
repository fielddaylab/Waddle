using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Waddle;


public class BodyStand : MonoBehaviour, IBodyInteract
{
	
	public enum StandType
	{
		ROCK,
		NEST
	}
	
	public enum StandState
	{
		IN,
		OUT
	}
	
	public StandType _type;
	
	private StandState _state;
	
	private const float STAND_TIME_AMOUNT = 0.25f;
	
	private bool _sentLog = false;
	private float _stayTime = 0f;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void OnBodyEnter(BodyTrigger trigger, Collider collider)
	{
		_state = StandState.IN;
		_stayTime = UnityEngine.Time.unscaledTime;
		_sentLog = false;
	}
	
	public void OnBodyExit(BodyTrigger trigger, Collider collider)
	{
		_state = StandState.OUT;
		_sentLog = false;
		_stayTime = 0f;
	}
	
	public void OnBodyStay(BodyTrigger trigger, Collider collider)
	{
		if(_state == StandState.IN && (UnityEngine.Time.unscaledTime - _stayTime) > STAND_TIME_AMOUNT && !_sentLog)
		{
			if(_type == StandType.ROCK)
			{
				PenguinAnalytics.Instance.LogStandOnRock(collider.gameObject.name, collider.gameObject.transform.position);
				_sentLog = true;
			}
			else if(_type == StandType.NEST)
			{
				PenguinAnalytics.Instance.LogStandOnNest(collider.gameObject.name, collider.gameObject.transform.position);
				_sentLog = true;
			}
		}
	}
}
