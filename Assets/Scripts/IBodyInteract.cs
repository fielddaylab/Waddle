using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Waddle {
    public interface IBodyInteract {
        void OnBodyEnter(BodyTrigger trigger, Collider collider);
		void OnBodyExit(BodyTrigger trigger, Collider collider);
		void OnBodyStay(BodyTrigger trigger, Collider collider);
    }
}