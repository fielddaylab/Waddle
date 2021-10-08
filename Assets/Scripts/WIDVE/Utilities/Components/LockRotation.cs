//Copyright WID Virtual Environments Group 2018-Present
//Authors:Simon Smith, Ross Tredinnick
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WIDVE.Utilities
{
    [ExecuteAlways]
    public class LockRotation : MonoBehaviour
    {
        [SerializeField]
        Vector3 _lockedRotation;
        Vector3 LockedRotation => _lockedRotation;

        public void LateUpdate()
        {
            transform.rotation = Quaternion.Euler(LockedRotation);
        }
    }
}