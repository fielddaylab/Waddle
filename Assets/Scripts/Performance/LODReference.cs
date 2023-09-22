using System;
using System.Runtime.InteropServices;
using BeauUtil;
using FieldDay.SharedState;
using UnityEngine;

namespace Waddle {
    public class LODReference : SharedStateComponent {
        [NonSerialized] public Transform CachedTransform;
        [NonSerialized] public LODStats Stats;

        private void Awake() {
            this.CacheComponent(ref CachedTransform);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct LODStats {
        public int Close;
        public int Mid;
        public int Far;

        public ref int this[LODLevel level] {
            get {
                unsafe {
                    fixed(int* pStart = &Close) {
                        return ref *(pStart + (int) level);
                    }
                }
            }
        }
    }
}