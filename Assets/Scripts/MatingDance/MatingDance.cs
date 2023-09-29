//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System;
using System.Collections;
using System.Collections.Generic;
using BeauRoutine;
using BeauUtil.Debugger;
using FieldDay;
using UnityEngine;
using UnityEngine.Android;
using Waddle;

public class MatingDance : MiniGameController {
    #region Inspector

    [SerializeField]
    private float m_Duration = 10;

    [SerializeField]
    private float m_LookThreshold = 0.2f;

    [SerializeField]
    private MusicAsset m_Music;

    [SerializeField]
    private SFXAsset m_GoodFeedback;

    [SerializeField]
    private Transform m_Heart;

    [SerializeField]
    private Transform m_HeartRenderer;

    [SerializeField]
    private SFXAsset m_HeartSound;

    #endregion // Inspector

    [NonSerialized] private MatingDancePenguin m_MatingDancePenguin;
    [NonSerialized] private PenguinBrain m_MatingDancePenguinBrain;
    private Routine m_PlayRoutine;
    private float m_DanceCooldown;
    private bool m_Dancing;
    private bool m_FeedbackQueued;
    private Routine m_BigHeartRoutine;

    private void Start() {
        m_MatingDancePenguin = GameObject.FindObjectOfType<MatingDancePenguin>();
        m_MatingDancePenguinBrain = m_MatingDancePenguin.GetComponent<PenguinBrain>();

        m_HeartRenderer.gameObject.SetActive(false);
    }

    public override void StartGame()
    {
        base.StartGame();

		Debug.Log("Starting mating dance");
        base.StartGame();
        m_PlayRoutine.Replace(this, Sequence());
        m_Heart.SetScale(0);

        Game.Events.Register(PlayerMovementUtility.Event_WaddleDetected, OnWaddle)
            .Register(MusicUtility.Event_Beat, OnBeat)
            .Register(MusicUtility.Event_MajorBeat, OnMajorBeat);

        PenguinAnalytics.Instance.LogActivityBegin("mating_dance");
    }
	
	public override void RestartGame()
    {
        m_HeartRenderer.gameObject.SetActive(false);
        m_HeartRenderer.SetRotation(0, Axis.Y, Space.Self);
        m_Heart.SetScale(0);
        m_BigHeartRoutine.Stop();

        m_PlayRoutine.Stop();
        m_DanceCooldown = 0;
        m_Dancing = false;
        m_FeedbackQueued = false;
        Game.Events.DeregisterAllForContext(this);
    }

    private void OnWaddle() {
        if (!m_Dancing) {
            return;
        }

        PlayerHeadState head = Game.SharedState.Get<PlayerHeadState>();
        Vector3 towardsPartner = m_MatingDancePenguin.DesiredPlayerLook.position - head.HeadRoot.position;
        towardsPartner.Normalize();
        float looking = Vector3.Dot(head.HeadRoot.forward, towardsPartner);
        Log.Msg("[MatingDance] Player looking at partner {0}", looking);
        if (looking >= m_LookThreshold) {
            m_MatingDancePenguin.HeartParticles.Play();
            m_DanceCooldown = 0.6f;
            m_FeedbackQueued = true;
        }
    }

    private void OnBeat() {
        if (!m_Dancing) {
            return;
        }

        Log.Msg("on the beat");
        
        //if (m_FeedbackQueued) {
        //    m_MatingDancePenguinBrain.Vocalize(m_GoodFeedback);
        //    m_FeedbackQueued = false;
        //}
    }

    private void OnMajorBeat() {
        if (!m_Dancing) {
            return;
        }

        Log.Msg("on the MAJOR beat");

        if (m_FeedbackQueued) {
            m_MatingDancePenguinBrain.Vocalize(m_GoodFeedback);
            m_FeedbackQueued = false;
        }
    }

	private void CleanUpGame() {
        PenguinPlayer.Instance.StartBackgroundMusic();

        Game.Events.DeregisterAllForContext(this);
        m_Dancing = false;
        m_FeedbackQueued = false;

        // Log early exit from game?
    }

	public override void EndGame()
	{
		CleanUpGame();
		
		PenguinAnalytics.Instance.LogActivityEnd("mating_dance");

		base.EndGame();
	}

    private IEnumerator Sequence() {
        MusicUtility.Stop(8);

        m_MatingDancePenguinBrain.ForceToAnimatorState("Bow", 0.1f);
        m_MatingDancePenguinBrain.Animator.SetBool("BopDance", true);
        yield return m_MatingDancePenguinBrain.Animator.WaitForState("BopBeat_Action");

        MusicUtility.Play(m_Music);
        m_Dancing = true;

        float fillUp = 0;

        while (fillUp < m_Duration) {
            var emission = m_MatingDancePenguin.HeartParticles.emission;
            if (m_DanceCooldown > 0) {
                emission.enabled = true;
                m_DanceCooldown -= Frame.DeltaTime;
                fillUp += Frame.DeltaTime;
            } else {
                emission.enabled = false;
            }
            yield return null;
        }

        m_Dancing = false;
        MusicUtility.Stop(3);

        m_MatingDancePenguin.HeartParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);

        m_HeartRenderer.gameObject.SetActive(true);
        m_MatingDancePenguinBrain.Animator.SetBool("BopDance", false);
        m_BigHeartRoutine.Replace(this, BigHeartSequence());
        yield return m_MatingDancePenguinBrain.Animator.WaitToCompleteState("Call");

        EndGame();
    }

    private IEnumerator BigHeartSequence() {
        SFXUtility.Play(m_Heart.GetComponent<AudioSource>(), m_HeartSound);
        yield return Routine.Combine(
            m_Heart.ScaleTo(1, 0.25f).Ease(Curve.BackOut),
            m_HeartRenderer.RotateTo(360 * 3, 0.7f, Axis.Y, Space.Self, AngleMode.Absolute).Ease(Curve.CubeOut)
        );
        yield return 0.3f;
        yield return m_Heart.ScaleTo(1.03f, 0.4f).YoyoLoop(true).Ease(Curve.SineIn);
    }
}
