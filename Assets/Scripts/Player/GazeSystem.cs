using UnityEngine;
using FieldDay.SharedState;
using FieldDay.Systems;
using FieldDay;

namespace Waddle
{
    [SysUpdate(GameLoopPhase.Update, 1000)]
    public class GazeSystem : ComponentSystemBehaviour<GazeState>
    {
        public override void ProcessWorkForComponent(GazeState component, float deltaTime) {
            Ray ray = new Ray(component.Root.position, component.Root.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, component.Mask, QueryTriggerInteraction.Ignore)) {
                GameObject go;
                
            } else {
                component.RaycastObject = null;
                component.RaycastTimer = 0;
            }
        }
    }
}