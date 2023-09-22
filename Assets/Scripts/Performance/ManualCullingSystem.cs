using System;
using System.Collections.Generic;
using System.ComponentModel;
using BeauUtil;
using FieldDay;
using FieldDay.Components;
using FieldDay.SharedState;
using FieldDay.Systems;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Waddle {
    [SysUpdate(GameLoopPhase.ApplicationPreRender)]
    public class ManualCullingSystem : ComponentSystemBehaviour<ManualCullingGroup> {
        static private readonly Collider[] s_RegionColliderWorkList = new Collider[32];
        static private readonly HashSet<ManualCullingGroup> s_CullingAddWorkSet = new HashSet<ManualCullingGroup>(32);
        static private readonly HashSet<ManualCullingGroup> s_CullingRemoveWorkSet = new HashSet<ManualCullingGroup>(32);

        public override void ProcessWork(float deltaTime) {
            ManualCullingReference refRoot = Game.SharedState.Get<ManualCullingReference>();
            
            if (!refRoot.RegionsDirty && !Frame.Interval(6)) {
                return;
            }

            refRoot.RegionsDirty = false;

            s_CullingAddWorkSet.Clear();
            s_CullingRemoveWorkSet.Clear();
            foreach (var item in refRoot.CurrentRegions) {
                s_CullingRemoveWorkSet.Add(item);
            }

            int collideCount = Physics.OverlapSphereNonAlloc(refRoot.CachedTransform.position, refRoot.Radius, s_RegionColliderWorkList, LayerMasks.CullingRegion_Mask, QueryTriggerInteraction.Collide);
            if (collideCount > 0) {
                for(int i = 0; i < collideCount; i++) {
                    ManualCullingGroup group = s_RegionColliderWorkList[i].GetComponent<ManualCullingGroup>();
                    if (group) {
                        s_CullingAddWorkSet.Add(group);
                    }
                }
            }

            s_CullingRemoveWorkSet.ExceptWith(s_CullingAddWorkSet);
            s_CullingAddWorkSet.ExceptWith(refRoot.CurrentRegions);

            foreach(var region in s_CullingRemoveWorkSet) {
                SetEnabled(region, false);
                refRoot.CurrentRegions.Remove(region);
            }

            foreach (var region in s_CullingAddWorkSet) {
                SetEnabled(region, true);
                refRoot.CurrentRegions.Add(region);
            }

            s_CullingAddWorkSet.Clear();
            s_CullingRemoveWorkSet.Clear();
        }

        static private void SetEnabled(ManualCullingGroup group, bool enabled) {
            foreach (var renderer in group.MeshRenderers) {
                renderer.enabled = enabled;
            }
        }

        protected override void OnComponentAdded(ManualCullingGroup component) {
            ManualCullingReference refRoot = Game.SharedState.Get<ManualCullingReference>();
            refRoot.RegionsDirty = true;

            SetEnabled(component, false);
        }

        protected override void OnComponentRemoved(ManualCullingGroup component) {
            ManualCullingReference refRoot = Game.SharedState.Get<ManualCullingReference>();
            refRoot.RegionsDirty = true;
        }
    }
}