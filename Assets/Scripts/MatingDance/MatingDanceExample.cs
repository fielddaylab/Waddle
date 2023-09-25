using System;
using System.Collections;
using BeauRoutine;
using BeauUtil;
using UnityEngine;

namespace Waddle {
    public class MatingDanceExample : MonoBehaviour {
        public TriggerListener Range;
        public Animator[] Penguins;
        public float Duration;

        private AnimatorStateSnapshot[] m_Snapshots;
        private Routine m_DanceRoutine;

        [NonSerialized] private bool m_Triggered;

        private void Awake() {
            m_Snapshots = new AnimatorStateSnapshot[Penguins.Length];
            for(int i = 0; i < m_Snapshots.Length; i++) {
                m_Snapshots[i] = new AnimatorStateSnapshot(Penguins[i]);
            }

            Range.onTriggerEnter.AddListener(OnEnter);
        }

        private void OnEnable() {
            PenguinGameManager.OnReset += OnReset;
        }

        private void OnDisable() {
            PenguinGameManager.OnReset -= OnReset;
        }
        
        private void OnReset() {
            for(int i = 0; i < m_Snapshots.Length; i++) {
                m_Snapshots[i].Write(Penguins[i]);
            }
            m_Triggered = false;
            m_DanceRoutine.Stop();
        }

        private void OnEnter(Collider c) {
            if (m_Triggered) {
                return;
            }

            m_Triggered = true;
            m_DanceRoutine.Replace(this, DanceRoutine());
        }

        private IEnumerator DanceRoutine() {
            foreach(var penguin in Penguins) {
                penguin.SetBool("BopDance", true);
                penguin.CrossFadeInFixedTime("Bow", 0.2f);
                yield return 0.5f;
            }

            //yield return Duration;

            //foreach (var penguin in Penguins) {
            //    penguin.SetBool("BopDance", false);
            //}
        }
    }
}