//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SharedBetweenAnimators]
public class PickedUpEgg : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Picked up egg");
		SkuaController sc = animator.gameObject.transform.parent.GetComponent<SkuaController>();
			
		if(sc.HoldingEgg != null)
		{
			sc.HoldingEgg.gameObject.transform.localPosition = Vector3.zero;
			sc.HoldingEgg.gameObject.transform.SetParent(sc.gameObject.transform.GetChild(1).transform, false);
			
            PenguinAnalytics.Instance.LogEggLost(sc.gameObject.name);
            /*for(int i = 0; i < sc.GetEgg.gameObject.transform.childCount; ++i)
			{
				sc.GetEgg.gameObject.transform.GetChild(i).transform.localPosition = Vector3.zero;
			}*/

            SkuaSpot potentialSpot = sc.Spawner.FindOuterSpot();

            //start flying animation here...
            sc.FlyToSpot(potentialSpot);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
   /* override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }*/

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
