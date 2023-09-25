//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SkuaEatState : MonoBehaviour, ISkuaState
//{
//	private SkuaController _sc;
	
//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
		
//    }
	
//	public void Handle(SkuaController sc)
//	{
//		if(_sc == null)
//		//{
//			_sc = sc;
//		}
		
//		//adjust position of egg...
		
//		Animator a = sc.GetAnimController();
//		if(a != null && !a.GetCurrentAnimatorStateInfo(0).IsName("eat"))
//		{
//			//Debug.Log("Setting fly");
//			//a.SetBool("takeoff", false);
//			//a.SetBool("fly", false);
//			//a.SetBool("walkleft", false);
//			//a.SetBool("walkright", false);
//			//a.SetBool("walk", false);
//			//a.SetBool("idle", false);
//			//a.SetBool("eat", true);
//			transform.rotation = _sc.CurrentSpot.transform.rotation;
			
//			_sc.GetEgg.gameObject.transform.localPosition = Vector3.zero;
			
//			_sc.GetEgg.gameObject.transform.SetParent(gameObject.transform.GetChild(1).transform);
//		}
//	}
//}
