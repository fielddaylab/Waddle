////NSF Penguins VR Experience
////Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SkuaFlyState : MonoBehaviour, ISkuaState
//{
//	private SkuaController _sc;
	
//	public enum WalkDirection {
//		eFORWARD=0,
//		eBACK,
//		eLEFT,
//		eRIGHT,
//		eSTAY
//	};
	
//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }

	
//	IEnumerator StartMove(Vector3 newSpot, Quaternion newRot, float duration)
//	{
//		float t = 0f;
//		Vector3 startPosition = transform.position;
//		Quaternion startRotation = transform.rotation;
		
//		while(t < duration)
//		{
//			transform.position = Vector3.Lerp(startPosition, newSpot, (t/duration));
//			transform.rotation = Quaternion.Lerp(startRotation, newRot, (t/duration));
			
//			t += (Time.deltaTime);	
//			yield return null;
//		}

//		transform.rotation = newRot;
//		//_sc.GoIdle();
//		_sc.GetAnimController().SetBool("flyegg", false);
//	}
	
//	public void Handle(SkuaController sc)
//	{
//		if(_sc == null)
//		{
//			_sc = sc;
//		}
		
//		//we can assume that the new spot we want to walk to has already been set before reaching this spot...
		
//		//orient skua in the direction of the new spot
//		Vector3 p =  _sc.CurrentSpot.gameObject.transform.position;
//		//p.y += 0.05f;
//		//gameObject.transform.position = p;
		
//		Vector3 e = _sc.CurrentSpot.gameObject.transform.rotation.eulerAngles;
//		Quaternion q = Quaternion.LookRotation(Vector3.Normalize(p - transform.position), Vector3.up);
//		//q.SetFromToRotation(gameObject.transform.forward, );
//		//Vector3 e = q.eulerAngles;
//		e.y -= 90.0f;//due to skua model's local rotation.
//		//gameObject.transform.rotation = q;
		
		
//		Animator a = sc.GetAnimController();
//		if(a != null)
//		{
//			a.SetBool("grab", false);
//			a.SetBool("flyegg", true);
//		}
		
//		StartCoroutine(StartMove(_sc.CurrentSpot.transform.position, Quaternion.Euler(e), 5f));
//	}
//}
