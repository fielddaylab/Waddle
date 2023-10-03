using System.Collections;
using BeauRoutine;
using BeauRoutine.Splines;
using FieldDay;
using FieldDay.Processes;
using UnityEngine;

public abstract class SkuaStateBase : IProcessStateEnterExit
{
    public virtual void OnEnter(Process process) {
        Handle(process, Brain(process));
    }
    public virtual void OnExit(Process process) { }
    public abstract void Handle(Process process, SkuaController sc);

    static protected SkuaController Brain(Process process) {
        return process.Context<SkuaController>();
    }

    static protected Quaternion GetRotationToLook(SkuaController sc, SkuaSpot spot) {
        return Quaternion.LookRotation(spot.CachedPosition - sc.CachedTransform.position, Vector3.up) * SkuaController.RotationAdjust;
    }

    static protected IEnumerator HopToPosition(SkuaController sc, Vector3 position, Vector3 controlAdjust, Quaternion rotation, float duration, Curve curve) {
        sc.CachedTransform.GetPositionAndRotation(out Vector3 startPos, out Quaternion startRot);
        SimpleSpline moveSpline = Spline.Simple(startPos, position, 0.5f, controlAdjust);

        float t = 0;
        while (t < duration) {
            t += Frame.DeltaTime;
            float lerp = Curve.Smooth.Evaluate(Mathf.Clamp01(t / duration));

            sc.CachedTransform.SetPositionAndRotation(moveSpline.GetPoint(lerp), Quaternion.Slerp(startRot, rotation, lerp));
            yield return null;
        }
    }

    static protected IEnumerator MoveRotateToPosition(SkuaController sc, Vector3 position, Quaternion rotation, float duration, Curve curve) {
        sc.CachedTransform.GetPositionAndRotation(out Vector3 startPos, out Quaternion startRot);

        float t = 0;
        while(t < duration) {
            t += Frame.DeltaTime;
            float lerp = curve.Evaluate(Mathf.Clamp01(t / duration));
            sc.CachedTransform.SetPositionAndRotation(
                Vector3.Lerp(startPos, position, lerp),
                Quaternion.Slerp(startRot, rotation, lerp)
            );
            yield return null;
        }
    }

    static protected IEnumerator MoveToPosition(SkuaController sc, Vector3 position, float duration, Curve curve) {
        Vector3 startPos = sc.CachedTransform.position;

        float t = 0;
        while (t < duration) {
            t += Frame.DeltaTime;
            float lerp = curve.Evaluate(Mathf.Clamp01(t / duration));
            sc.CachedTransform.position = Vector3.Lerp(startPos, position, lerp);
            yield return null;
        }
    }
}

static public class SkuaStates {
    static public readonly ProcessStateDefinition Idle = ProcessStateDefinition.FromCallbacks("idle", new SkuaIdleState());
    static public readonly ProcessStateDefinition Walk = ProcessStateDefinition.FromCallbacks("walk", new SkuaWalkState());
    static public readonly ProcessStateDefinition Remove = ProcessStateDefinition.FromCallbacks("remove", new SkuaRemoveState());
    static public readonly ProcessStateDefinition Hit = ProcessStateDefinition.FromCallbacks("hit", new SkuaHitState());
    static public readonly ProcessStateDefinition Fly = ProcessStateDefinition.FromCallbacks("fly", new SkuaFlyState());
    static public readonly ProcessStateDefinition Grab = ProcessStateDefinition.FromCallbacks("grab", new SkuaGrabState());
    static public readonly ProcessStateDefinition Eat = ProcessStateDefinition.FromCallbacks("eat", new SkuaEatState());
}
