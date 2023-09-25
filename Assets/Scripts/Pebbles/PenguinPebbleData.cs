using System;
using UnityEngine;

namespace Waddle {
    public class PenguinPebbleData : MonoBehaviour {
        public PebbleSource[] PebbleSources;
        public Transform PebbleDropOff;
        
        public int PebblesToGather;
        [NonSerialized] public Pebble CurrentPebble;

        public Renderer HeldPebbleRenderer;

    }
}