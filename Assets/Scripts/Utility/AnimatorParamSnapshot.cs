using BeauUtil;
using BeauUtil.Variants;
using UnityEngine;

namespace Waddle {
    public sealed class AnimatorStateSnapshot {
        private struct CachedParamMeta {
            public int NameHash;
            public AnimatorControllerParameterType Type;
        }

        private struct LayerData {
            public int FullHash;
            public float NormalizedTime;
        }

        private readonly Animator m_Source;
        private readonly CachedParamMeta[] m_ParamMeta;
        private readonly Variant[] m_ParamValues;
        private readonly LayerData[] m_LayerData;

        public AnimatorStateSnapshot(Animator animator) {
            int paramCount = animator.parameterCount;
            m_ParamMeta = new CachedParamMeta[paramCount];
            m_ParamValues = new Variant[paramCount];
            m_Source = animator;

            for(int i = 0; i < paramCount; i++) {
                AnimatorControllerParameter param = animator.GetParameter(i);
                m_ParamMeta[i] = new CachedParamMeta() {
                    NameHash = param.nameHash,
                    Type = param.type
                };
            }

            int layerCount = animator.layerCount;
            m_LayerData = new LayerData[layerCount];

            Cache();
        }

        public void Cache() {
            int count = m_ParamMeta.Length;
            for(int i = 0; i < count; i++) {
                CachedParamMeta meta = m_ParamMeta[i];
                ref Variant value = ref m_ParamValues[i];
                switch (meta.Type) {
                    case AnimatorControllerParameterType.Float: {
                        value = m_Source.GetFloat(meta.NameHash);
                        break;
                    }
                    case AnimatorControllerParameterType.Int: {
                        value = m_Source.GetInteger(meta.NameHash);
                        break;
                    }
                    case AnimatorControllerParameterType.Bool: {
                        value = m_Source.GetBool(meta.NameHash);
                        break;
                    }
                }
            }

            count = m_LayerData.Length;
            for(int i = 0; i < count; i++) {
                AnimatorStateInfo state = m_Source.GetCurrentAnimatorStateInfo(i);
                m_LayerData[i] = new LayerData() {
                    FullHash = state.fullPathHash,
                    NormalizedTime = state.normalizedTime
                };
            }
        }

        public void Restore() {
            int count = m_ParamMeta.Length;
            for (int i = 0; i < count; i++) {
                CachedParamMeta meta = m_ParamMeta[i];
                Variant value = m_ParamValues[i];
                switch (meta.Type) {
                    case AnimatorControllerParameterType.Float: {
                        m_Source.SetFloat(meta.NameHash, value.AsFloat());
                        break;
                    }
                    case AnimatorControllerParameterType.Int: {
                        m_Source.SetInteger(meta.NameHash, value.AsInt());
                        break;
                    }
                    case AnimatorControllerParameterType.Bool: {
                        m_Source.SetBool(meta.NameHash, value.AsBool());
                        break;
                    }
                    case AnimatorControllerParameterType.Trigger: {
                        m_Source.ResetTrigger(meta.NameHash);
                        break;
                    }
                }
            }

            count = m_LayerData.Length;
            for (int i = 0; i < count; i++) {
                LayerData data = m_LayerData[i];
                m_Source.Play(data.FullHash, i, data.NormalizedTime);
            }

            m_Source.Rebind();
        }
    }
}