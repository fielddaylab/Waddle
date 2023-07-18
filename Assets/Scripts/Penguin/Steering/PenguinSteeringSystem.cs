using System;
using UnityEngine;
using FieldDay.Components;
using FieldDay.Systems;
using FieldDay;
using BeauUtil;
using System.Globalization;
using FieldDay.Debugging;

namespace Waddle {
    [SysUpdate(GameLoopPhase.FixedUpdate, 10)]
    public sealed class PenguinSteeringSystem : ComponentSystemBehaviour<PenguinSteeringComponent, PenguinFeetSnapping> {
        public override void ProcessWorkForComponent(PenguinSteeringComponent component, PenguinFeetSnapping snapping, float deltaTime) {
            if (!component.HasTarget) {
                // TODO: Smoothly stop
                return;
            }

            Vector3 currentPos = component.PositionRoot.position;
            Vector3 targetPos = component.TargetPos;
            if (component.TargetObject != null) {
                targetPos += component.TargetObject.position;
            }
            targetPos.y = currentPos.y;

            DebugDraw.AddSphere(targetPos, 0.2f + component.TargetPosTolerance, Color.green.WithAlpha(0.2f));
            DebugDraw.AddLine(currentPos, targetPos, Color.green, 0.2f);

            Vector3 targetVector = targetPos - currentPos;
            Vector3 currentVector = component.RotationRoot.forward;
            float originalVecY = currentVector.y;
            currentVector.y = 0;

            Vector3 normalizedTargetVec = targetVector.normalized;

            Quaternion currentFacing = Quaternion.LookRotation(currentVector, Vector3.up);
            Quaternion targetFacing = Quaternion.LookRotation(normalizedTargetVec, Vector3.up);
            currentFacing = Quaternion.RotateTowards(currentFacing, targetFacing, component.TurnSpeed * deltaTime);
            currentVector = currentFacing * Vector3.forward;

            Vector3 newForward = currentVector;
            newForward.y = originalVecY;
            component.RotationRoot.forward = newForward;

            PenguinFeetUtility.IsSolidGround(snapping, currentPos, out Vector3 groundNormal);

            if (Quaternion.Angle(currentFacing, targetFacing) < component.MaxAngleDiffToMove) {
                Vector3 newPos = currentPos + currentVector * (component.MovementSpeed * deltaTime * groundNormal.y * groundNormal.y);
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