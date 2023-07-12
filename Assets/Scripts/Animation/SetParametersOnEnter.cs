using UnityEngine;
using BeauUtil;

namespace Waddle {
    [SharedBetweenAnimators]
    public sealed class SetParametersOnEnter : StateMachineBehaviour {
        public AnimatorParamChange[] Changes;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            for(int i = 0; i < Changes.Length; i++) {
                Changes[i].Apply(animator);
            }
        }
    }
}