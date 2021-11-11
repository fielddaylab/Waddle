using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkuaIdleState : MonoBehaviour, ISkuaState
{
	private SkuaController _sc;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		
    }
	
	/*IEnumerator CheckForMove(float moveFreq)
	{
		yield return new WaitForSeconds(moveFreq);
		
		SkuaSpot newSpot = _sc.SearchForNewSpot();
		if(newSpot != _sc.CurrentSpot)
		{
			_sc.SetNewSpot(newSpot);
			_sc.WalkToSpot();
		}
		else
		{
			//stay here idle
			//todo - could play a different animation or something here for some variation..
			_sc.GoIdle();
		}
	}*/
	
	public void Handle(SkuaController sc)
	{
		if(_sc == null)
		{
			_sc = sc;
		}
		
		Animator a = sc.GetAnimController();
		if(a != null)
		{
			//Debug.Log("Setting fly");
			//a.SetBool("takeoff", false);
			//a.SetBool("fly", false);
			//a.SetBool("walkleft", false);
			//a.SetBool("walkright", false);
			a.SetBool("forward", false);
			a.SetBool("back", false);
			a.SetBool("left", false);
			a.SetBool("right", false);
			a.SetBool("idle", true);
		}

		//StartCoroutine(CheckForMove(_sc.MoveFrequency));
	}
}
