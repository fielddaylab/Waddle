using System;
using BeauRoutine;
using FieldDay;
using FieldDay.Processes;
using UnityEngine;

namespace Waddle {
    public class PlayerPebble : MonoBehaviour, IBeakInteract, ISlapInteract {
        public Collider Collider;
        public MeshRenderer Renderer;
        public SelectablePebble Shine;
        public SFXAsset PickUpSFX;
        public ParticleSystem Particles;

        [NonSerialized] public bool PickedUp;

        [NonSerialized] private TransformState m_OriginalTransform;

        private void Start() {
            m_OriginalTransform = TransformState.WorldState(transform);
            transform.SetParent(null);

            PenguinGameManager.OnReset += () => {
                m_OriginalTransform.Apply(transform);
                transform.SetParent(null);
                PickedUp = false;
                Collider.enabled = true;
                Renderer.enabled = true;
                Shine.SetEnabled(true);
                Particles.Play();
            };
        }

        public void OnBeakInteract(PlayerBeakState state, BeakTrigger trigger, Collider triggeredCollider) {
			
			Vector3 pos = Vector3.zero;
            Quaternion view = Quaternion.identity;
            PenguinPlayer.Instance.GetGaze(out pos, out view);
            PenguinAnalytics.Instance.LogPeckRock(triggeredCollider.gameObject.name, pos);
			
			
            if (state.HoldingPebble != null) {
                return;
            }

            Collider.enabled = false;
            PickedUp = true;
            transform.SetParent(state.HoldRoot, true);
            transform.localPosition = default;
            Shine.SetEnabled(false);
            SFXUtility.Play(GetComponent<AudioSource>(), PickUpSFX);
            Particles.Stop();

            state.HoldingPebble = this;
			PenguinAnalytics.Instance.LogPickupRock(pos, view);
			
            Game.Events.Queue(PlayerBeakUtility.Event_PickedUp);
        }
		
		public void OnSlapInteract(PlayerHeadState state, SlapTrigger trigger, Collider slappedCollider, Vector3 slapDirection, Collision collisionInfo) {
			
			if(slappedCollider != null) {
				Vector3 pos = Vector3.zero;
				Quaternion view = Quaternion.identity;
				PenguinPlayer.Instance.GetGaze(out pos, out view);    
				
				PenguinAnalytics.Instance.LogFlipperBashRock(slappedCollider.gameObject.name, false, slappedCollider.gameObject.transform.position, pos);
			}
			
			//if (slapDirection.magnitude > 0.5f) {
			//	SFXUtility.Play(_sounds, _hitSound);
			//	trigger.PlayHaptics();
			//	trigger.SetCooldown(slappedCollider, 1);
			//}
		}

        public void Hide() {
            transform.SetParent(null);
            Renderer.enabled = false;
            PickedUp = false;
        }
    }
}