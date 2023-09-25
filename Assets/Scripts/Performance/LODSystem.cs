using System;
using System.Globalization;
using BeauUtil;
using BeauUtil.Debugger;
using FieldDay;
using FieldDay.Debugging;
using FieldDay.SharedState;
using FieldDay.Systems;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.UI;

namespace Waddle {
    [SysUpdate(GameLoopPhase.LateUpdate)]
    public class LODSystem : ComponentSystemBehaviour<LODComponent> {
        public override void ProcessWork(float deltaTime) {
            LODReference refRoot = Game.SharedState.Get<LODReference>();

            Vector3 refPos = refRoot.CachedTransform.position;
            Vector3 refLook = refRoot.CachedTransform.forward;

            int idx = 0;
            foreach(var component in m_Components) {
                if (!Frame.Interval(4, idx++)) {
                    continue;
                }

                if (component.Animator && !component.Animator.isInitialized) {
                    continue;
                }

                Vector3 vec = component.CachedTransform.position - refPos;
                float dist = vec.magnitude;

                vec.Normalize();
                float look = Vector3.Dot(refLook, vec);

                LODLevel level;
                if (dist >= component.Far.Distance) {
                    level = LODLevel.Far;
                } else if (dist >= component.Mid.Distance) {
                    level = LODLevel.Mid;
                } else {
                    level = LODLevel.Close;
                }

                Renderer r = component.Renderer;

                var levelData = component.GetLevel(level);
                r.enabled = level == 0 || (look > -0.2f && !levelData.Cull);

                if (level != component.LastAppliedLevel) {
                    refRoot.Stats[component.LastAppliedLevel]--;
                    component.LastAppliedLevel = level;
                    refRoot.Stats[level]++;

                    if (component.SkinnedMesh) {
                        component.SkinnedMesh.quality = levelData.SkinQuality;
                        component.SkinnedMesh.sharedMesh = levelData.Mesh ? levelData.Mesh : component.Close.Mesh;
                        component.SkinnedMesh.receiveShadows = level == LODLevel.Close;
                    } else if (component.MeshFilter) {
                        component.MeshFilter.sharedMesh = levelData.Mesh ? levelData.Mesh : component.Close.Mesh;
                        component.MeshRenderer.receiveShadows = level == LODLevel.Close;
                    }

                    if (component.Animator) {
                        bool animating = component.Animator.enabled;
                        if (animating != !levelData.Cull) {
                            if (animating) {
                                component.CachedAnimatorState.Read(component.Animator);
                                component.Animator.enabled = false;
                            } else {
                                component.Animator.enabled = true;
                                component.CachedAnimatorState.Write(component.Animator);
                            }
                        }
                    }

                    r.shadowCastingMode = level < LODLevel.Far ? ShadowCastingMode.On : ShadowCastingMode.Off;

                    component.OnLevelChanged.Invoke(level);

                    Log.Msg("[LODSystem] Object '{0}' transitioned to lod{1}", component.gameObject.name, level);
                    switch (level) {
                        case LODLevel.Close: {
                            DebugDraw.AddSphere(component.CachedTransform.position, .5f, Color.green.WithAlpha(0.5f), 2);
                            break;
                        }
                        case LODLevel.Mid: {
                            DebugDraw.AddSphere(component.CachedTransform.position, .5f, Color.yellow.WithAlpha(0.5f), 2);
                            break;
                        }
                        case LODLevel.Far: {
                            DebugDraw.AddSphere(component.CachedTransform.position, .5f, Color.red.WithAlpha(0.5f), 2);
                            break;
                        }
                    }
                }
            }
        
            if (Frame.Interval(16)) {
                //DebugDraw.AddViewportText(new Vector2(0, 1), new Vector2(8, -8), string.Format("LODs {0} | {1} | {2}", refRoot.Stats.Close, refRoot.Stats.Mid, refRoot.Stats.Far), Color.yellow, 17f / 90, TextAnchor.UpperLeft, DebugTextStyle.BackgroundDark);
            }
        }

        protected override void OnComponentAdded(LODComponent component) {
            LODReference refRoot = Game.SharedState.Get<LODReference>();
            refRoot.Stats[component.LastAppliedLevel]++;
        }

        protected override void OnComponentRemoved(LODComponent component) {
            LODReference refRoot = Game.SharedState.Get<LODReference>();
            refRoot.Stats[component.LastAppliedLevel]--;
        }
    }
}