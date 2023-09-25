using System;
using BeauUtil;
using FieldDay.Components;
using FieldDay.Data;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

namespace Waddle {
    [RequireComponent(typeof(Collider))]
    public class ManualCullingGroup : BatchedComponent, IEditorOnlyData {
        public Transform[] Roots;
        public MeshRenderer[] MeshRenderers;
        public GameObject[] GameObjects;

#if UNITY_EDITOR

        [ContextMenu("Find All In Roots")]
        private void FindAllInRoots() {
            Undo.RecordObject(this, "Finding renderers");
            List<MeshRenderer> renderList = new List<MeshRenderer>(128);
            foreach(var root in Roots) {
                foreach(var meshRenderer in root.GetComponentsInChildren<MeshRenderer>(false)) {
                    if (meshRenderer.enabled) {
                        renderList.Add(meshRenderer);
                    }
                }
            }
            MeshRenderers = renderList.ToArray();
            EditorUtility.SetDirty(this);
        }

        void IEditorOnlyData.ClearEditorData(bool isDevelopmentBuild) {
            Roots = null;
        }

#endif // UNITY_EDITOR
    }
}