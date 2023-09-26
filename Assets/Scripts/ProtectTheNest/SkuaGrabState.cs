//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Waddle;

public class SkuaGrabState : MonoBehaviour, ISkuaState
{
	private SkuaController _sc;
	
	IEnumerator StartMove(Vector3 newSpot, Quaternion newRot, float duration)
	{
		float t = 0f;
		Vector3 startPosition = transform.position;
		//Quaternion startRotation = transform.rotation;
		
		while(t < duration)
		{
			transform.position = Vector3.Lerp(startPosition, newSpot, (t/duration));
			//transform.rotation = Quaternion.Lerp(startRotation, newRot, (t/duration));
			
			t += (Time.deltaTime);	
			yield return null;
		}
		
		Animator a = _sc.GetAnimController();
		if(a != null)
		{
			a.SetBool("grab", false);
			a.SetBool("flyegg", true);
		}
	}
	
	public void Handle(SkuaController sc)
	{
		if(_sc == null)
		{
			_sc = sc;
		}
		
		//adjust position of egg...
		Vector3 p =  _sc.CurrentSpot.gameObject.transform.position;
		//p.y += 0.05f;
		//gameObject.transform.position = p;
		
		Vector3 e = _sc.CurrentSpot.gameObject.transform.rotation.eulerAngles;
		//Quaternion q = Quaternion.LookRotation(Vector3.Normalize(p - _sc.CenterSpot.transform.position), Vector3.up);
		//q.SetFromToRotation(gameObject.transform.forward, );
		//Vector3 e = q.eulerAngles;
		e.y -= 90.0f;
		
		Animator a = _sc.GetAnimController();
		if(a != null)
		{
			//Debug.Log("Setting fly");
			//a.SetBool("takeoff", false);
			//a.SetBool("fly", false);
			//a.SetBool("walkleft", false);
			//a.SetBool("walkright", false);
			//a.SetBool("walk", false);
			//a.SetBool("idle", false);
			//a.SetBool("eat", true);
			a.SetBool("idle", false);
			a.SetBool("grab", true);
			
			//todo move skua towards egg while playing grab animation...
			StartCoroutine(StartMove(_sc.CurrentSpot.transform.position, Quaternion.Euler(e), _sc.MoveFrequency));
		}
	}
}
