//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkuaWalkState : MonoBehaviour, ISkuaState
{
	private SkuaController _sc;
	
	public enum WalkDirection {
		eFORWARD=0,
		eBACK,
		eLEFT,
		eRIGHT,
		eSTAY
	};
	
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	
	IEnumerator StartMove(Vector3 newSpot, Quaternion newRot, float duration, bool isUp)
	{
		float t = 0f;
		Vector3 startPosition = transform.position;
		Quaternion startRotation = transform.rotation;
		Vector3 toNewSpot = newSpot - transform.position;
		toNewSpot = Vector3.Normalize(toNewSpot);
		Quaternion rotNewSpot = Quaternion.LookRotation(toNewSpot, Vector3.up);
		if(!isUp)
		{
			transform.rotation = rotNewSpot;
		}
		
		while(t < duration)
		{
			transform.position = Vector3.Lerp(startPosition, newSpot, (t/duration));
			if(isUp)
			{
				transform.rotation = Quaternion.Lerp(startRotation, newRot, (t/duration));
			}
			
			t += (Time.deltaTime);	
			yield return null;
		}

		/*if(_sc.CurrentSpot.IsCenter)
		{
			//take egg, then retreat..
			if(_sc != null && _sc.GetEgg != null)
			{
				_sc.GetEgg.gameObject.transform.localPosition = Vector3.zero;
				_sc.GetEgg.gameObject.transform.SetParent(gameObject.transform.GetChild(0).transform);
				_sc.GrabEgg();
			}
		}
		else
		{*/
		
			PenguinAnalytics.Instance.LogSkuaMove(_sc.gameObject.name, startPosition, transform.position);
			
			transform.rotation = newRot;
			_sc.GoIdle();
		//}
	}
	
	public void Handle(SkuaController sc)
	{
		if(_sc == null)
		{
			_sc = sc;
		}
		
		//we can assume that the new spot we want to walk to has already been set before reaching this spot...
		bool isUp = _sc.CurrentSpot.IsUp;
		
		//_wasUp = isUp;
		
		//orient skua in the direction of the new spot
		Vector3 p =  _sc.CurrentSpot.gameObject.transform.position;
		//p.y += 0.05f;
		//gameObject.transform.position = p;
		
		Vector3 e = _sc.CurrentSpot.gameObject.transform.rotation.eulerAngles;
		//Quaternion q = Quaternion.LookRotation(Vector3.Normalize(p - _sc.CenterSpot.transform.position), Vector3.up);
		//q.SetFromToRotation(gameObject.transform.forward, );
		//Vector3 e = q.eulerAngles;
		e.y -= 90.0f;//due to skua model's local rotation.
		//gameObject.transform.rotation = q;
		
		
		Animator a = sc.GetAnimController();
		if(a != null)
		{
			a.SetBool("right", false);
			a.SetBool("left", false);
			a.SetBool("back", false);
			a.SetBool("forward", false);
			a.SetBool("idle", false);
			a.SetBool("slapped", false);
			a.SetBool("walk", false);
			
			if(isUp || sc.WasUp)
			{
				if(_sc.WalkDir == SkuaWalkState.WalkDirection.eFORWARD)
				{
					a.SetBool("forward", true);
				}
				else if(_sc.WalkDir == SkuaWalkState.WalkDirection.eBACK)
				{
					a.SetBool("back", true);
				}
				else if(_sc.WalkDir == SkuaWalkState.WalkDirection.eLEFT)
				{
					a.SetBool("left", true);
				}
				else if(_sc.WalkDir == SkuaWalkState.WalkDirection.eRIGHT)
				{
					a.SetBool("right", true);
				}
			}
			else
			{
				//Debug.Log("Setting walk");
				a.SetBool("walk", true);
			}
		}
		
		StartCoroutine(StartMove(_sc.CurrentSpot.transform.position, Quaternion.Euler(e), _sc.MoveFrequency, isUp || sc.WasUp));
	}
}
