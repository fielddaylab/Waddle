using System;
using BeauUtil;
using FieldDay;
using FieldDay.SharedState;
using UnityEngine;

namespace Waddle {
    public class MusicState : SharedStateComponent {
        public AudioSource Playback;
        public MusicAsset Current;
        [Range(0, 1)] public float GlobalVolumeMultiplier = 0.85f;

        [NonSerialized] public float VolumeMultiplier = 1;
        [NonSerialized] public float PlaybackPosition = -1;

        [NonSerialized] public MusicOperation QueuedOperation;
        [NonSerialized] public MusicOperation CurrentOperation;

        [NonSerialized] public bool OnBeat;
        [NonSerialized] public bool OnMajorBeat;
        [NonSerialized] public int BeatIndex;
    }

    public struct MusicOperation {
        public MusicOperationType Type;
        public MusicAsset Asset;
        public float Duration;
    }

    public enum MusicOperationType {
        None,
        Stop,
        Play,
        Transition
    }

    static public class MusicUtility {
        static public readonly StringHash32 Event_Beat = "music-beat";
        static public readonly StringHash32 Event_MajorBeat = "music-beat-major";

        static public void Stop(float duration = 0.5f) {
            MusicState m = Game.SharedState.Get<MusicState>();
            m.QueuedOperation = new MusicOperation() {
                Type = MusicOperationType.Stop,
                Duration = duration
            };
        }

        static public void Play(MusicAsset asset, float duration = 0.5f) {
            MusicState m = Game.SharedState.Get<MusicState>();
            m.QueuedOperation = new MusicOperation() {
                Type = MusicOperationType.Play,
                Asset = asset,
                Duration = duration
            };
        }
    }
}