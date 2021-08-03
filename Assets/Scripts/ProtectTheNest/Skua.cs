using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skua : MonoBehaviour
{
	Animator _skuaController;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnCollisionEnter(Collision otherCollision)
	{
		//Debug.Log("Skua collided");
		if(otherCollision.gameObject.name.StartsWith("Flipper"))
		{
			GetComponent<Rigidbody>().useGravity = true;
			GetComponent<Rigidbody>().isKinematic = false;
			GetComponent<Rigidbody>().AddForce(Vector3.up*500.0f);
		}
	}
	
	public void FlyIn()
	{	
		if(_skuaController == null)
		{
			_skuaController = GetComponent<Animator>();
		}
		
		if(_skuaController != null)
		{
			//Debug.Log("Setting fly");
			_skuaController.SetBool("idle", false);
			_skuaController.SetBool("takeoff", true);
			_skuaController.SetBool("fly", true);
		}
		else
		{
			Debug.Log("Couldn't set fly");
		}
	}
	
	public void WalkForward()
	{
		if(_skuaController == null)
		{
			_skuaController = GetComponent<Animator>();
		}
		
		if(_skuaController != null)
		{
			_skuaController.SetBool("idle", false);
			_skuaController.SetBool("walk", true);
		}
		else
		{
			Debug.Log("Couldn't set walk");
		}
	}
}
