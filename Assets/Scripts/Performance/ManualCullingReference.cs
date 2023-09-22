using System;
using System.Collections.Generic;
using BeauUtil;
using FieldDay.Components;
using FieldDay.SharedState;
using UnityEngine;

namespace Waddle {
    public class ManualCullingReference : SharedStateComponent {
        public float Radius = 1;

        [NonSerialized] public HashSet<ManualCullingGroup> CurrentRegions = new HashSet<ManualCullingGroup>(4);

        [NonSerialized] public bool RegionsDirty;

        [NonSerialized] public Transform CachedTransform;

        private void Awake() {
            this.CacheComponent(ref CachedTransform);
        }
    }
}