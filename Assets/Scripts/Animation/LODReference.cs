using System;
using BeauUtil;
using FieldDay.SharedState;
using UnityEngine;

namespace Waddle {
    public class LODReference : SharedStateComponent {
        [NonSerialized] public Transform CachedTransform;

        private void Awake() {
            this.CacheComponent(ref CachedTransform);
        }
    }
}