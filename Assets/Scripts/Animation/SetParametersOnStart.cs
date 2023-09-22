using UnityEngine;
using BeauUtil;

namespace Waddle {
    public sealed class SetParametersOnStart : MonoBehaviour {
        public Animator Animator;
        public AnimatorParamChange[] Changes;

        private void Start() {
            for(int i = 0; i < Changes.Length; i++) {
                Changes[i].Apply(Animator);
            }
        }
    }
}