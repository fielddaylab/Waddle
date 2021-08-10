using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skua : MonoBehaviour
{
	Animator _skuaController;
	
	bool _isHit;
	
	public bool IsHit => _isHit;
	
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
		//Debug.Log("Impulse: " + otherCollision.impulse);
		//Debug.Log("Relative Velocity: " + otherCollision.relativeVelocity);
		if(otherCollision.gameObject.name.StartsWith("Flipper"))
		{
			_isHit = true;
			
			SkuaState skuaState = GetComponent<SkuaState>();
			if(skuaState != null && skuaState.HasEgg)
			{
				//reset egg to middle...
				skuaState.ResetEgg();
			}
			
			GetComponent<Rigidbody>().useGravity = true;
			GetComponent<Rigidbody>().isKinematic = false;
			//GoIdle();
			_skuaController.enabled = false;
			GetComponent<AudioSource>().Play();
			GetComponent<Rigidbody>().AddForce((-transform.forward + transform.up)*5.0f);
		}
	}
	
	public void GoIdle()
	{
		if(_skuaController == null)
		{
			_skuaController = GetComponent<Animator>();
		}
		
		if(_skuaController != null)
		{
			//Debug.Log("Setting fly");
			_skuaController.SetBool("takeoff", false);
			_skuaController.SetBool("fly", false);
			_skuaController.SetBool("walkleft", false);
			_skuaController.SetBool("walkright", false);
			_skuaController.SetBool("walk", false);
			_skuaController.SetBool("idle", true);
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
		/*else
		{
			Debug.Log("Couldn't set fly");
		}*/
	}
	
	public void Eat()
	{
		if(_skuaController == null)
		{
			_skuaController = GetComponent<Animator>();
		}
		
		if(_skuaController != null && !_skuaController.GetCurrentAnimatorStateInfo(0).IsName("eat"))
		{
			_skuaController.SetBool("idle", false);
			_skuaController.SetBool("walkleft", false);
			_skuaController.SetBool("walkright", false);
			_skuaController.SetBool("walk", false);
			_skuaController.SetBool("eat", true);
		}
	}
	
	public void WalkForward()
	{
		if(_skuaController == null)
		{
			_skuaController = GetComponent<Animator>();
		}
		
		if(_skuaController != null && !_skuaController.GetCurrentAnimatorStateInfo(0).IsName("walk"))
		{
			_skuaController.SetBool("idle", false);
			_skuaController.SetBool("walkleft", false);
			_skuaController.SetBool("walkright", false);
			_skuaController.SetBool("walk", true);
		}
		/*else
		{
			Debug.Log("Couldn't set walk");
		}*/
	}
	
	public void WalkLeft()
	{
		if(_skuaController == null)
		{
			_skuaController = GetComponent<Animator>();
		}
		
		if(_skuaController != null)
		{
			_skuaController.SetBool("idle", false);
			_skuaController.SetBool("walk", false);
			_skuaController.SetBool("walkleft", true);
		}
	}
	
	public void WalkRight()
	{
		if(_skuaController == null)
		{
			_skuaController = GetComponent<Animator>();
		}
		
		if(_skuaController != null)
		{
			_skuaController.SetBool("idle", false);
			_skuaController.SetBool("walk", false);
			_skuaController.SetBool("walkright", true);
		}
	}
}
