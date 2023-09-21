using System.Collections;
using UnityEngine;
using BeauUtil;
using FieldDay.Processes;
using System;

namespace Waddle {
    public class PenguinThoughtGuide : PenguinThoughtState {
        public override IEnumerator Sequence(Process process) {
            PenguinBrain brain = Brain(process);
            PenguinGuideParams guideParms = brain.GetComponent<PenguinGuideParams>();
            brain.Animator.SetBool("BopDance", true);
            brain.ForceToAnimatorState("BopBeat_Action", 0.2f);
            yield return process.WaitForSignal("player-cross-initial-threshold");
            brain.Animator.SetBool("BopDance", false);
            brain.ForceToAnimatorState("Idle", 0.2f);
            brain.SetWalkState(guideParms.FirstWalkNode);
            yield return null;
            while(brain.Steering.HasTarget) {
                yield return null;
            }
            brain.Animator.SetBool("AttackedTwice", true);
            brain.Animator.SetBool("FrontAttack", true);
            yield return new WaitForSeconds (4);
            brain.Animator.SetBool("AttackedTwice", false);
            brain.Animator.SetBool("FrontAttack", false);
            yield return process.WaitForSignal("player-cross-second-threshold");
            brain.SetWalkState(guideParms.SecondWalkNode);
            yield return null;
            while (brain.Steering.HasTarget)
            {
                yield return null;
            }
            brain.SetWalkState(guideParms.ThirdWalkNode);
            yield return null;
            while (brain.Steering.HasTarget)
            {
                yield return null;
            }
            brain.Animator.SetBool("AttackedTwice", true);
            brain.Animator.SetBool("FrontAttack", true);
            yield return new WaitForSeconds(4);
            brain.Animator.SetBool("AttackedTwice", false);
            brain.Animator.SetBool("FrontAttack", false);
        }
    }
}