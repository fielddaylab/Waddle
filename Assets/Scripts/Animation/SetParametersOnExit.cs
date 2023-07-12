using UnityEngine;
using BeauUtil;

namespace Waddle {
    [SharedBetweenAnimators]
    public sealed class SetParametersOnExit : StateMachineBehaviour {
        public AnimatorParamChange[] Changes;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            for (int i = 0; i < Changes.Length; i++) {
                Changes[i].Apply(animator);
            }
        }
    }
}