using System.Collections;
using UnityEngine;
using BeauUtil;
using FieldDay.Processes;
using System;
using FieldDay;

namespace Waddle {
    public class PenguinThoughtGuide : PenguinThoughtState {
        public override IEnumerator Sequence(Process process) {
            PenguinBrain brain = Brain(process);
            PenguinGuideParams guideParms = brain.GetComponent<PenguinGuideParams>();
            PlayerProgressState progress = Game.SharedState.Get<PlayerProgressState>();

            brain.Animator.SetBool("BopDance", true);
            brain.ForceToAnimatorState("BopBeat_Action", 0.2f);
            yield return WaitForPlayerInRange(brain);
            brain.Animator.SetBool("BopDance", false);
            brain.ForceToAnimatorState("Idle", 0.2f);
            brain.SetWalkState(guideParms.FirstWalkNode);
            yield return null;
            while(brain.Steering.HasTarget) {
                yield return null;
            }

            StringHash32 afterSign = "AfterSign";
            if (!progress.HasFlag(afterSign)) {
                brain.Animator.SetBool("AttackedTwice", true);
                brain.Animator.SetBool("FrontAttack", true);
                yield return 4;
                brain.Animator.SetBool("AttackedTwice", false);
                brain.Animator.SetBool("FrontAttack", false);
                while(!progress.HasFlag(afterSign)) {
                    yield return null;
                }
            }

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
            yield return 4;
            brain.Animator.SetBool("AttackedTwice", false);
            brain.Animator.SetBool("FrontAttack", false);
        }

        static private IEnumerator WaitForPlayerInRange(PenguinBrain brain) {
            while (!brain.LookSmoothing.IsLooking) {
                yield return null;
            }
        }
    }
}