using System.Collections;
using System.Collections.Generic;
using BeauUtil.Debugger;
using FieldDay.Data;
using UnityEditor.Build.Reporting;
using UnityEditor.Build;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using WIDVE.Utilities;
using BeauUtil;
using ScriptableBake;

/// <summary>
/// Loads scene from scene objects.
/// </summary>
public class LoadScenesSceneProcessor : IProcessSceneWithReport {
    public int callbackOrder { get { return -1000; } }

    public void OnProcessScene(Scene scene, BuildReport report) {
        if (EditorApplication.isPlayingOrWillChangePlaymode) {
            return;
        }

        List<SceneLoader> toBake = new List<SceneLoader>(256);
        scene.GetAllComponents(true, toBake);

        if (toBake.Count > 0) {
            Debug.LogFormat("[LoadScenesSceneProcessor] Found {0} objects wth subscenes to load", toBake.Count);
            using (Profiling.Time("loading subscenes")) {
                foreach (var obj in toBake) {
                    obj.EditorMerge(scene);
                    Baking.Destroy(obj);
                }
            }
        }
    }
}