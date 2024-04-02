//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using BeauRoutine;
using BeauUtil;
using FieldDay.Processes;
using UnityEngine;
using Waddle;

public class SkuaHitState : SkuaStateBase, IProcessStateSequence {
    public struct Args {
        public Vector3 HitDirection;
        public Quaternion OriginalRotation;
    }

	public override void Handle(Process process, SkuaController sc)
	{
		Vector3 pos = Vector3.zero;
        Quaternion view = Quaternion.identity;
        PenguinPlayer.Instance.GetGaze(out pos, out view);    
			
		PenguinAnalytics.Instance.LogFlipperBashSkua(sc.gameObject.name, false, sc.gameObject.transform.position, pos);
		
        Animator a = sc.AnimController;
		//a.SetBool("walk", false);
		//a.SetBool("eat", false);
		a.SetBool("idle", false);
		a.SetBool("slapped", true);
		a.SetBool("break", false);

        Rigidbody rb = sc.Rigidbody;
        sc.SetPhysics(true);

        rb.velocity = default;
        rb.angularVelocity = default;
			
        SFXUtility.Play(sc.Sounds, sc.HitSound);

        ref Args args = ref process.Data<Args>();
        args.OriginalRotation = rb.rotation;
		rb.AddForce((args.HitDirection + Vector3.up * 3).normalized * 4, ForceMode.Impulse);
        sc.AssignToSpot(null);
	}

    public IEnumerator Sequence(Process process) {
        SkuaController sc = Brain(process);

        yield return FlashRoutine(sc.Renderer, sc.FlashMaterial, 8);

        sc.SetPhysics(false);

        Vector2 randomLook2d = Geom.Rotate(Vector2.zero, RNG.Instance.NextFloat(Mathf.PI * 2));
        Vector3 randomLook3d = new Vector3(randomLook2d.x, 0, randomLook2d.y);
        Quaternion safeyRot = Quaternion.Euler(randomLook3d) * SkuaController.RotationAdjust;

        yield return Routine.Combine(
            sc.CachedTransform.MoveTo(sc.CachedTransform.position.y + 0.15f, 0.2f, Axis.Y, Space.World).Yoyo(true).Ease(Curve.SineIn),
            sc.CachedTransform.RotateQuaternionTo(safeyRot, 0.4f, Space.World).Ease(Curve.Smooth)
        );

        yield return 0.5f;

        SkuaSpot safetySpot = sc.Spawner.FindOuterSpot();

        Vector3 targetPos = safetySpot.CachedPosition;
        Quaternion targetRot = GetRotationToLook(sc, safetySpot);

        sc.TargetSpot = safetySpot;
        sc.Spawner.SetPendingOccupancy(sc.TargetSpot, true);
        sc.AssignToSpot(null);

        Animator a = sc.AnimController;
        a.SetBool("slapped", false);
        a.SetBool("forward", true);

        yield return HopToPosition(sc, targetPos, new Vector3(0, 1, 0), targetRot, 2, Curve.Smooth);

        sc.AssignToSpot(safetySpot);

        process.TransitionTo(SkuaStates.Idle);
    }

    private IEnumerator FlashRoutine(SkinnedMeshRenderer meshRenderer, Material flashMaterial, int count) {
        Material defaultMaterial = meshRenderer.sharedMaterial;
        try {
            while (count-- > 0) {
                meshRenderer.sharedMaterial = flashMaterial;
                yield return 0.1f;
                meshRenderer.sharedMaterial = defaultMaterial;
                yield return 0.1f;
            }
        } finally {
            if (meshRenderer) {
                meshRenderer.sharedMaterial = defaultMaterial;
            }
        }
    }
}
