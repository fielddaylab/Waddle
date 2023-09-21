using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform2 : MonoBehaviour
{
	public Transform Follow;

    // Update is called once per frame
    void LateUpdate()
    {
        if (Follow != null) {
            Follow.GetPositionAndRotation(out Vector3 pos, out Quaternion rot);
            transform.SetPositionAndRotation(pos, rot);
        }
    }
}
