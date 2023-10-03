using System;
using System.Collections.Generic;
using BeauUtil;
using BeauUtil.Debugger;
using FieldDay;
using FieldDay.SharedState;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Waddle
{
    [RequireComponent(typeof(Collider))]
    public class PlayerTriggerVolume : MonoBehaviour
    {
        public SerializedHash32 SetFlag;

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer != LayerMasks.Beak_Index) {
                return;
            }

            if (!SetFlag.IsEmpty) {
                Game.SharedState.Get<PlayerProgressState>().SetFlags.Add(SetFlag);
            }
        }
    }
}