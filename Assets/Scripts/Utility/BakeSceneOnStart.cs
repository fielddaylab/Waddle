using ScriptableBake;
using UnityEngine;

namespace Waddle {
    [DefaultExecutionOrder(100000)]
    public class BakeSceneOnStart : MonoBehaviour {
        private void Start() {
#if UNITY_EDITOR
            Baking.BakeCurrentScene(BakeFlags.Verbose);
#endif // UNITY_EDITOR
        }
    }
}