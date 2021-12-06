//Copyright WID Virtual Environments Group 2018-Present
//Authors:Simon Smith, Ross Tredinnick
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using WIDVE.Utilities;
using WIDVE.Patterns;

[ExecuteAlways]
public class OrientToUser : MonoBehaviour
{
	[SerializeField]
	bool _rotateX = false;

	[SerializeField]
	bool _rotateY = true;

	[SerializeField]
	bool _rotateZ = false;

	static Camera _mainCamera;
	static Camera MainCamera => (_mainCamera) ? _mainCamera : (_mainCamera = GetCamera());

	/// <summary>
	/// Returns the Main camera in Play mode.
	/// <para>Returns the SceneView camera in Edit mode.</para>
	/// </summary>
	/// <returns></returns>
	public static Camera GetCamera()
	{
		if(Application.isPlaying)
		{
			//get main camera
			return Camera.main;
		}
#if UNITY_EDITOR
		else
		{
			//get scene view camera
			return SceneView.lastActiveSceneView.camera;
		}
#endif
		return null;
	}

	bool ShouldRotate(int axis)
	{
		if(axis == 0) return _rotateX;
		else if(axis == 1) return _rotateY;
		else if(axis == 2) return _rotateZ;
		else return false;
	}

	public void Rotate()
	{
		if(!MainCamera) return;

		//rotate to face camera
		Transform cameraTransform = MainCamera.transform;
		//Vector3 lookPosition = transform.position - (cameraTransform.position - transform.position).normalized;
		transform.LookAt(cameraTransform);

		//lock some axes
		Vector3 rotationAngles = transform.rotation.eulerAngles;
		for(int i = 0; i < 3; i++)
		{
			if(!ShouldRotate(i))
			{
				rotationAngles[i] = 0;
			}
		}

		//apply final rotation
		//transform.localRotation = Quaternion.Euler(rotationAngles);
		//transform.RotateAround(transform.position, Vector3.up, rotationAngles.y);
		//transform.Rotate(0, rotationAngles.y, 0, Space.Self);
	}

#if UNITY_EDITOR
	void OnRenderObject()
	{
		LateUpdate();
	}
#endif

	void LateUpdate()
	{
		//if(!gameObject.ExistsInScene()) return;

		//Rotate();
	}

#if UNITY_EDITOR
	[CanEditMultipleObjects]
	[CustomEditor(typeof(OrientToUser))]
	class Editor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			if(!MainCamera)
			{
				string message = Application.isPlaying ? "Can't find main camera!" : "Can't find scene view camera!";
				EditorGUILayout.HelpBox(message, MessageType.Warning);
			}

			EditorGUILayout.LabelField("Rotation:");

			EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_rotateX)), label: new GUIContent("X"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_rotateY)), label: new GUIContent("Y"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_rotateZ)), label: new GUIContent("Z"));

			serializedObject.ApplyModifiedProperties();
		}
	}
#endif
}
