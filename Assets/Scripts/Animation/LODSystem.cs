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

                Vector3 vec = component.CachedTransform.position - refPos;
                float dist = vec.magnitude;

                vec.Normalize();
                float look = Vector3.Dot(refLook, vec);

                int level;
                if (dist >= component.Level2.Distance) {
                    level = 2;
                } else if (dist >= component.Level1.Distance) {
                    level = 1;
                } else {
                    level = 0;
                }

                Renderer r = component.Renderer;

                r.enabled = level == 0 || look > -0.2f;

                if (level != component.LastAppliedLevel) {
                    component.LastAppliedLevel = level;
                    var levelData = component.GetLevel(level);

                    if (component.SkinnedMesh) {
                        component.SkinnedMesh.quality = levelData.SkinQuality;
                        component.SkinnedMesh.sharedMesh = levelData.Mesh ? levelData.Mesh : component.Level0.Mesh;
                        component.SkinnedMesh.receiveShadows = level < 1;
                    } else if (component.MeshFilter) {
                        component.MeshFilter.sharedMesh = levelData.Mesh ? levelData.Mesh : component.Level0.Mesh;
                        component.MeshRenderer.receiveShadows = level < 1;
                    }

                    r.shadowCastingMode = level < 2 ? ShadowCastingMode.On : ShadowCastingMode.Off;

                    Log.Msg("[LODSystem] Object '{0}' transitioned to lod{1}", component.gameObject.name, level);
                    switch (level) {
                        case 0: {
                            DebugDraw.AddSphere(component.CachedTransform.position, 1, Color.green.WithAlpha(0.2f), 2);
                            break;
                        }
                        case 1: {
                            DebugDraw.AddSphere(component.CachedTransform.position, 1, Color.yellow.WithAlpha(0.2f), 2);
                            break;
                        }
                        case 2: {
                            DebugDraw.AddSphere(component.CachedTransform.position, 1, Color.red.WithAlpha(0.2f), 2);
                            break;
                        }
                    }
                }
            }
        }
    }
}