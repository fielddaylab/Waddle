using System;
using System.Runtime.InteropServices;
using BeauUtil;
using UnityEngine;
using UnityEngine.Rendering;

namespace Waddle {
    public class VRScreenFade : MonoBehaviour {
        public DynamicMeshFilter DynamicMesh;
        public MeshRenderer Renderer;
        public Color DefaultColor;

        [NonSerialized] public Material CachedMaterial;
        [NonSerialized] private Color m_LastColor;

        private void Awake() {
            CachedMaterial = Renderer.material;
            SetColor(DefaultColor.WithAlpha(0));

            MeshData16<FaderVertex> data = new MeshData16<FaderVertex>(8);
            FaderVertex v0 = new FaderVertex() {
                Position = new Vector3(-2, -2, 0),
                UV = new Vector2(0, 0),
                Normal = -Vector3.forward
            };
            FaderVertex v1 = new FaderVertex() {
                Position = new Vector3(-2, 2, 0),
                UV = new Vector2(0, 1),
                Normal = -Vector3.forward
            };
            FaderVertex v2 = new FaderVertex() {
                Position = new Vector3(2, -2, 0),
                UV = new Vector2(1, 0),
                Normal = -Vector3.forward
            };
            FaderVertex v3 = new FaderVertex() {
                Position = new Vector3(2, 2, 0),
                UV = new Vector2(1, 1),
                Normal = -Vector3.forward
            };
            data.AddQuad(v0, v1, v2, v3);
            DynamicMesh.Upload(data);
            data.Dispose();
        }

        private void OnDestroy() {
            Destroy(CachedMaterial);
        }

        public void SetColor(Color color) {
            m_LastColor = color;
            CachedMaterial.color = color;
            Renderer.enabled = color.a > 0;
        }

        public void SetAlpha(float alpha) {
            Color newColor = m_LastColor;
            newColor.a = alpha;
            SetColor(newColor);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FaderVertex {
        [VertexAttr(VertexAttribute.Position)]
        public Vector3 Position;

        [VertexAttr(VertexAttribute.Normal)]
        public Vector3 Normal;

        [VertexAttr(VertexAttribute.TexCoord0)]
        public Vector2 UV;
    }
}