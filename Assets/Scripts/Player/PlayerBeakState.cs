using System;
using BeauUtil;
using FieldDay;
using FieldDay.SharedState;
using UnityEngine;

namespace Waddle
{
    public class PlayerBeakState : SharedStateComponent, IRegistrationCallbacks
    {
        public Transform HoldRoot;

        [NonSerialized] public PlayerPebble HoldingPebble;

        private void OnReset() {
            HoldingPebble = null;
        }

        void IRegistrationCallbacks.OnDeregister() {
            PenguinGameManager.OnReset -= OnReset;
        }

        void IRegistrationCallbacks.OnRegister() {
            PenguinGameManager.OnReset += OnReset;
        }
    }

    static public class PlayerBeakUtility {
        static public readonly StringHash32 Event_PickedUp = "player-beak-picked-up";
        static public readonly StringHash32 Event_Dropped = "player-beak-dropped";
    }
}