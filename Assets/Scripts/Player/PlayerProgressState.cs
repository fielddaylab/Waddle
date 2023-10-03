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
    public class PlayerProgressState : SharedStateComponent, IRegistrationCallbacks
    {
        public HashSet<StringHash32> SetFlags = new HashSet<StringHash32>(16);
        public HashSet<PenguinGameManager.MiniGame> CompletedGames = new HashSet<PenguinGameManager.MiniGame>(2);
        
        public bool HasFlag(StringHash32 id) {
            return SetFlags.Contains(id);
        }

        public bool HasCompleted(PenguinGameManager.MiniGame game) {
            return CompletedGames.Contains(game);
        }

        private void OnReset() {
            SetFlags.Clear();
            CompletedGames.Clear();
        }

        void IRegistrationCallbacks.OnDeregister() {
            PenguinGameManager.OnReset -= OnReset;
        }

        void IRegistrationCallbacks.OnRegister() {
            PenguinGameManager.OnReset += OnReset;
        }
    }
}