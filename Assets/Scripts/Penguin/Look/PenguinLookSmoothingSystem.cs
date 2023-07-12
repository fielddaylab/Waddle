using BeauRoutine;
using FieldDay.Components;
using FieldDay.Systems;
using UnityEngine;

namespace Waddle {
    public class PenguinLookSmoothingSystem : ComponentSystemBehaviour<PenguinLookSmoothing> {
        static private int LookXParam;
        static private int LookYParam;

        public override void Initialize() {
            base.Initialize();

            LookXParam = Animator.StringToHash("LookX");
            LookYParam = Animator.StringToHash("LookY");
        }

        public override void ProcessWorkForComponent(PenguinLookSmoothing component, float deltaTime) {
            float targetX, targetY;
            if (component.IsLooking) {
                Vector3 look = component.LookVector;
                look.Normalize();
                if (component.LookSpace == Space.World) {
                    look = component.WorldLookDirectionToLocal(look);
                }
                targetX = look.x;
                targetY = look.y;
            } else {
                targetX = targetY = 0;
            }

            float lerpAmt = TweenUtil.Lerp(component.LookLerpSpeed, 1, deltaTime);

            targetX = Mathf.Lerp(component.LastAppliedLook.x, targetX, lerpAmt);
            targetY = Mathf.Lerp(component.LastAppliedLook.y, targetY, lerpAmt);

            if (targetX != component.LastAppliedLook.x) {
                component.Animator.SetFloat(LookXParam, targetX);
                component.LastAppliedLook.x = targetX;
            }
            if (targetY != component.LastAppliedLook.y) {
                component.Animator.SetFloat(LookYParam, targetY);
                component.LastAppliedLook.y = targetY;
            }
        }
    }
}