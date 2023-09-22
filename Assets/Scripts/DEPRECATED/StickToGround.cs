using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToGround : MonoBehaviour
{
	[SerializeField]
	GameObject _rotationTransform;
	GameObject RotationTransform => _rotationTransform;
	
	[SerializeField]
	GameObject _positionTransform;
	GameObject PositionTransform => _positionTransform;

	[SerializeField]
	Vector3 _offsetToApply;
	Vector3 OffsetToApply => _offsetToApply;
	
	[SerializeField]
	Vector3 _rotationToApply;
	Vector3 RotationToApply => _rotationToApply;
	
	void LateUpdate()
	{
		Quaternion q = Quaternion.identity;
		q.eulerAngles = _rotationToApply;
		transform.rotation = _rotationTransform.transform.rotation;
		transform.rotation *= q;
		transform.position = _positionTransform.transform.position;
		transform.position += _offsetToApply;
	}
}
