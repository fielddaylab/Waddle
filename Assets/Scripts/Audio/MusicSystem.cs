using FieldDay;
using FieldDay.SharedState;
using FieldDay.Systems;
using UnityEngine;

namespace Waddle {
    [SysUpdate(GameLoopPhase.LateUpdate)]
    public class MusicSystem : SharedStateSystemBehaviour<MusicState> {
        public override void ProcessWork(float deltaTime) {
            if (AudioListener.pause) {
                return;
            }

            AudioSource playback = m_State.Playback;

            // process queued operation
            if (m_State.QueuedOperation.Type != 0) {
                m_State.CurrentOperation = m_State.QueuedOperation;
                m_State.QueuedOperation = default;

                if (m_State.CurrentOperation.Type == MusicOperationType.Play && m_State.Current != null && m_State.Current != m_State.CurrentOperation.Asset) {
                    m_State.CurrentOperation.Type = MusicOperationType.Transition;
                }
            }

            MusicOperation op = m_State.CurrentOperation;

            // process current operation
            switch (m_State.CurrentOperation.Type) {
                case MusicOperationType.Stop: {
                    if (op.Duration <= 0 || !playback.isPlaying) {
                        StopPlayback();
                        EndCurrentOperation();
                    } else {
                        m_State.VolumeMultiplier -= deltaTime / op.Duration;
                        if (m_State.VolumeMultiplier <= 0) {
                            StopPlayback();
                            EndCurrentOperation();
                        }
                    }
                    break;
                }

                case MusicOperationType.Play: {
                    if (op.Duration <= 0) {
                        m_State.Current = op.Asset;
                        StartPlayback(1);
                        EndCurrentOperation();
                    } else { 
                        if (!playback.isPlaying) {
                            m_State.Current = op.Asset;
                            StartPlayback(0);
                        }
                        m_State.VolumeMultiplier += deltaTime / op.Duration;
                        if (m_State.VolumeMultiplier >= 1) {
                            m_State.VolumeMultiplier = 1;
                            EndCurrentOperation();
                        }
                    }
                    break;
                }

                case MusicOperationType.Transition: {
                    if (op.Duration <= 0) {
                        m_State.Current = op.Asset;
                        StartPlayback(1);
                        EndCurrentOperation();
                    } else {
                        float partDuration = op.Duration * 0.66f;
                        if (m_State.Current != op.Asset) {
                            m_State.VolumeMultiplier -= deltaTime / partDuration;
                            if (m_State.VolumeMultiplier <= 0) {
                                m_State.Current = op.Asset;
                                StartPlayback(0);
                            }
                        } else {
                            m_State.VolumeMultiplier += deltaTime / partDuration;
                            if (m_State.VolumeMultiplier >= 1) {
                                m_State.VolumeMultiplier = 1;
                                EndCurrentOperation();
                            }
                        }
                    }
                    break;
                }
            }

            if (m_State.Current != null) {
                m_State.Playback.volume = m_State.Current.Volume * m_State.VolumeMultiplier * m_State.GlobalVolumeMultiplier;

                float currentTime = playback.time;
                if (m_State.Current.BPM > 0) {
                    // BPM detection
                    float timeToBeatIndex = m_State.Current.BPM / 60f;
                    int prevBeat = (int) (m_State.PlaybackPosition * timeToBeatIndex);
                    int currentBeat = (int) (currentTime * timeToBeatIndex);
                    m_State.BeatIndex = currentBeat;
                    if (currentBeat != prevBeat || m_State.PlaybackPosition < 0) {
                        m_State.OnBeat = true;
                        m_State.OnMajorBeat = currentBeat > 0 && (currentBeat % m_State.Current.Measure) == m_State.Current.MajorOn;

                        if (m_State.OnMajorBeat) {
                            Game.Events.Dispatch(MusicUtility.Event_MajorBeat);
                        } else {
                            Game.Events.Dispatch(MusicUtility.Event_Beat);
                        }
                    } else {
                        m_State.OnBeat = false;
                        m_State.OnMajorBeat = false;
                    }
                }

                m_State.PlaybackPosition = currentTime;
            }
        }

        private void StopPlayback() {
            m_State.Playback.Stop();
            m_State.Playback.clip = null;
            m_State.Current = null;
            m_State.VolumeMultiplier = 0;
        }

        private void StartPlayback(float volume) {
            m_State.Playback.Stop();

            m_State.Playback.clip = m_State.Current.Clip;
            m_State.Playback.volume = m_State.Current.Volume * volume * m_State.GlobalVolumeMultiplier;
            m_State.Playback.time = 0;
            m_State.VolumeMultiplier = volume;
            m_State.PlaybackPosition = -1;

            m_State.Playback.Play();
        }

        private void EndCurrentOperation() {
            m_State.CurrentOperation = default;
        }
    }
}