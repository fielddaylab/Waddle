using FieldDay.Processes;
using UnityEngine;

namespace Waddle {
    public class Pebble : MonoBehaviour {
        public bool PlayerOnly = false;
        public bool Placed = false;

        public ProcessId OwningProcess;
    }
}