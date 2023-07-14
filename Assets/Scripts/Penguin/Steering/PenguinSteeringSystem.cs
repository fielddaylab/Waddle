using System;
using UnityEngine;
using FieldDay.Components;
using FieldDay.Systems;
using FieldDay;
using BeauUtil;
using System.Globalization;

namespace Waddle {
    [SysUpdate(GameLoopPhase.FixedUpdate, 10)]
    public sealed class PenguinSteeringSystem : ComponentSystemBehaviour<PenguinSteeringComponent> {
        public override void ProcessWorkForComponent(PenguinSteeringComponent component, float deltaTime) {
            if (!component.HasTarget) {
                // TODO: Smoothly stop
                return;
            }

            Vector3 currentPos = component.PositionRoot.position;
            Vector3 targetPos = component.TargetPos;
            if (component.TargetObject != null) {
                targetPos += component.TargetObject.position;
            }
            Vector3 targetVector = targetPos - currentPos;
            Vector3 currentVector = component.RotationRoot.forward;
            float originalVecY = currentVector.y;
            currentVector.y = 0;
            targetVector.y = 0;

            Vector3 normalizedTargetVec = targetVector.normalized;

            Quaternion currentFacing = Quaternion.LookRotation(currentVector, Vector3.up);
            Quaternion targetFacing = Quaternion.LookRotation(normalizedTargetVec, Vector3.up);
            currentFacing = Quaternion.RotateTowards(currentFacing, targetFacing, component.TurnSpeed * deltaTime);
            currentVector = currentFacing * Vector3.forward;

            Vector3 newForward = currentVector;
            newForward.y = originalVecY;
            component.RotationRoot.forward = newForward;

            if (Quaternion.Angle(currentFacing, targetFacing) < component.MaxAngleDiffToMove) {
                Vector3 newPos = currentPos + currentVector * (component.MovementSpeed * deltaTime);
                newPos.y = currentPos.y;

                component.PositionRoot.position = newPos;

                if (Vector2.Distance(Geom.SwizzleYZ(newPos), Geom.SwizzleYZ(targetPos)) <= component.TargetPosTolerance) {
                    component.HasTarget = false;
                    component.Brain.Signal("steering-completed");
                }
            }
        }
    }
}