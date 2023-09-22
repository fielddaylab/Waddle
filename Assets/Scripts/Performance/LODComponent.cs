using System;
using BeauUtil;
using BeauUtil.Debugger;
using FieldDay.Components;
using FieldDay.SharedState;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Waddle {
    public class LODComponent : BatchedComponent {
        [Serializable]
        public struct Level {
            public float Distance;
            public SkinQuality SkinQuality;
            public Mesh Mesh;
            public bool Cull;
        }

        #region Inspector

        public SkinnedMeshRenderer SkinnedMesh;
        public MeshRenderer MeshRenderer;
        public MeshFilter MeshFilter;
        public Animator Animator;

        [Header("LODs")]
        [FormerlySerializedAs("Level0")] public Level Close = new Level() { Distance = 0, SkinQuality = SkinQuality.Bone4 };
        [FormerlySerializedAs("Level1")] public Level Mid = new Level() { Distance = 40, SkinQuality = SkinQuality.Bone2 };
        [FormerlySerializedAs("Level2")] public Level Far = new Level() { Distance = 80, SkinQuality = SkinQuality.Bone1 };

        #endregion // Inspector

        [NonSerialized] public Transform CachedTransform;
        [NonSerialized] public LODLevel LastAppliedLevel = 0;
        [NonSerialized] public AnimatorStateSnapshot CachedAnimatorState;

        public readonly CastableEvent<LODLevel> OnLevelChanged = new CastableEvent<LODLevel>();

        public Renderer Renderer {
            get { return SkinnedMesh ? SkinnedMesh : MeshRenderer; }
        }

        private void Awake() {
            this.CacheComponent(ref CachedTransform);
            if (SkinnedMesh) {
                Close.Mesh = SkinnedMesh.sharedMesh;
            } else if (MeshFilter) {
                Close.Mesh = MeshFilter.sharedMesh;
            }

            if (Animator != null) {
                CachedAnimatorState = new AnimatorStateSnapshot(Animator);
            }
        }

        public Level GetLevel(LODLevel level) {
            switch (level) {
                case LODLevel.Close: {
                    return Close;
                }
                case LODLevel.Mid: {
                    return Mid;
                }
                case LODLevel.Far: {
                    return Far;
                }
                default: {
                    Log.Error("[LODComponent] No lod of level {0}", level);
                    return default;
                }
            }
        }

#if UNITY_EDITOR

        private void Reset() {
            SkinnedMesh = GetComponent<SkinnedMeshRenderer>();
            MeshFilter = GetComponent<MeshFilter>();
            MeshRenderer = GetComponent<MeshRenderer>();
            Animator = GetComponentInParent<Animator>();
        }

#endif // UNITY_EDITOR
    }

    public enum LODLevel {
        Close,
        Mid,
        Far
    }
}