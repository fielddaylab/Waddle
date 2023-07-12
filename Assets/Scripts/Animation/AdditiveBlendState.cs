using System;
using BeauUtil;
using FieldDay;
using FieldDay.Components;
using UnityEngine;

namespace Waddle {
    [RequireComponent(typeof(Animator))]
    public sealed class AdditiveBlendState : BatchedComponent, IRegistrationCallbacks {
        #region Types

        public unsafe struct Float8 {
            public fixed float Value[8];

            public Float8(int count, float initialValue) {
                for (int i = 0; i < count; i++) {
                    Value[i] = initialValue;
                }
            }

            public ref float this[int index] {
                get { return ref Value[index]; }
            }
        }

        public struct LerpPart {
            public Float8 Current;
            public Float8 Target;
            public Float8 Lerp;
        }

        public struct TransitionLayer {
            public Float8 Start;
            public Float8 Target;
            public Float8 Lerp;
            public Float8 InvDuration;
        }

        #endregion // Types

        #region Inspector

        [Required] public Animator Animator;
        public float DefaultLerpSpeed = 8;
        public float StateMachineMaskLerpSpeed = 20;

        #endregion // Inspector

        [NonSerialized] public int LayerCount;
        [NonSerialized] public Float8 LastApplied;
        [NonSerialized] public LerpPart ScriptLerpWeights;
        [NonSerialized] public LerpPart StateMachineMaskingWeights;

        void IRegistrationCallbacks.OnRegister() {
            LayerCount = Animator.layerCount;
            for (int i = 0; i < LayerCount; i++) {
                ScriptLerpWeights.Current[i] = Animator.GetLayerWeight(i);
            }
            ScriptLerpWeights.Target = ScriptLerpWeights.Current;
            ScriptLerpWeights.Lerp = new Float8(LayerCount, DefaultLerpSpeed);

            StateMachineMaskingWeights.Current = new Float8(LayerCount, 1);
            StateMachineMaskingWeights.Target = StateMachineMaskingWeights.Current;
            StateMachineMaskingWeights.Lerp = new Float8(LayerCount, StateMachineMaskLerpSpeed);

            LastApplied = ScriptLerpWeights.Current;
        }

        void IRegistrationCallbacks.OnDeregister() { }

#if UNITY_EDITOR

        private void Reset() {
            Animator = GetComponent<Animator>();
        }

#endif // UNITY_EDITOR
    }
}