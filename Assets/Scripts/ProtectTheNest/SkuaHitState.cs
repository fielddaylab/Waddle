//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkuaHitState : MonoBehaviour, ISkuaState
{
	private SkuaController _sc;
	
	Camera _mainCam = null;
    // Start is called before the first frame update
    void Start()
    {
        _mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
		
    }
	
	public void Handle(SkuaController sc)
	{
		if(_sc == null)
		{
			_sc = sc;
		}
		
		//adjust position of egg...
		
		Animator a = sc.GetAnimController();
		if(a != null)
		{

			//a.SetBool("walk", false);
			//a.SetBool("eat", false);
			a.SetBool("idle", false);
			a.SetBool("slapped", true);
			
			gameObject.GetComponent<Rigidbody>().useGravity = true;
			gameObject.GetComponent<Rigidbody>().isKinematic = false;

			//a.enabled = false;
			
			gameObject.GetComponent<AudioSource>().Play();
			GetComponent<Rigidbody>().AddForce((_mainCam.transform.forward*3f + transform.up*1.5f));
		}
	}
}
