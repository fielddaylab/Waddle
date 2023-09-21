using System;
using BeauUtil;
using BeauUtil.Debugger;
using FieldDay.Components;
using FieldDay.SharedState;
using UnityEngine;
using UnityEngine.Rendering;

namespace Waddle {
    public class LODComponent : BatchedComponent {
        [Serializable]
        public struct Level {
            public float Distance;
            public SkinQuality SkinQuality;
            public Mesh Mesh;
        }

        #region Inspector

        public SkinnedMeshRenderer SkinnedMesh;
        public MeshRenderer MeshRenderer;
        public MeshFilter MeshFilter;

        [Header("LODs")]
        public Level Level0 = new Level() { Distance = 0, SkinQuality = SkinQuality.Bone4 };
        public Level Level1 = new Level() { Distance = 40, SkinQuality = SkinQuality.Bone2 };
        public Level Level2 = new Level() { Distance = 80, SkinQuality = SkinQuality.Bone1 };

        #endregion // Inspector

        [NonSerialized] public Transform CachedTransform;
        [NonSerialized] public int LastAppliedLevel = 0;

        public Renderer Renderer {
            get { return SkinnedMesh ? SkinnedMesh : MeshRenderer; }
        }

        private void Awake() {
            this.CacheComponent(ref CachedTransform);
            if (SkinnedMesh) {
                Level0.Mesh = SkinnedMesh.sharedMesh;
            } else if (MeshFilter) {
                Level0.Mesh = MeshFilter.sharedMesh;
            }
        }

        public Level GetLevel(int idx) {
            switch (idx) {
                case 0: {
                    return Level0;
                }
                case 1: {
                    return Level1;
                }
                case 2: {
                    return Level2;
                }
                default: {
                    Log.Error("[LODComponent] No lod of level {0}", idx);
                    return default;
                }
            }
        }
    }
}