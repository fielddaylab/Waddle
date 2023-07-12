using System;
using FieldDay.Components;
using FieldDay.SharedState;
using UnityEngine;

namespace Waddle
{
    public class GazeState : BatchedComponent
    {
        public Transform Root;
        public LayerMask Mask;

        [NonSerialized] public GameObject RaycastObject;
        [NonSerialized] public float RaycastTimer;
    }
}