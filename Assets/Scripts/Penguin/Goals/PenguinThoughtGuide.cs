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
            yield return process.WaitForSignal("player-cross-initial-threshold");
            brain.Animator.SetBool("BopDance", false);
            brain.SetWalkState(guideParms.FirstWalkNode);
            yield return null;
            while(brain.Steering.HasTarget) {
                yield return null;
            }
            brain.Animator.Play("BackDefense.Left Turn Around", 0);
            yield return 2;
        }
    }
}