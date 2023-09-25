//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System;
using System.Collections;
using System.Collections.Generic;
using BeauPools;
using FieldDay;
using UnityEngine;
using Waddle;

public class SkuaCoordinator : MonoBehaviour
{
    #region Types

    [Serializable]
    public struct WaveInfo {
        public int MajorBeatLength;
        public int SkuaCount;
        public int ShiftFrequency;
        public int AttackFrequency;
    }

    [Serializable] public class SkuaPool : SerializablePool<SkuaBrain> { }

    [SerializeField] private SkuaPool m_SkuaPool;
    [SerializeField] private Transform[] m_SpawnPoints;
    [SerializeField] private SkuaSpot[] m_Spots;
    [SerializeField] private WaveInfo[] m_Waves;

    #endregion // Types

    [NonSerialized] private int m_BeatCount;
    [NonSerialized] private int m_MajorBeatCount;
    [NonSerialized] private int m_WaveIndex;

    public void Begin() {
        Game.Events.Register(MusicUtility.Event_Beat, OnBeat)
            .Register(MusicUtility.Event_MajorBeat, OnMajorBeat);
    }

    public void End() {
        Game.Events.DeregisterAllForContext(this);
        m_SkuaPool.Reset();
    }

    private void OnMajorBeat() {
        m_MajorBeatCount++;
        if (m_WaveIndex < m_Waves.Length && m_MajorBeatCount >= m_Waves[m_WaveIndex].MajorBeatLength) {
            m_WaveIndex++;
            m_BeatCount = 0;
            AdvanceWave();
        }

        OnBeat();
        SpawnSkuaIfNeeded();
    }

    private void OnBeat() {
        m_BeatCount++;
    }

    private void AdvanceWave() {

    }

    private void SpawnSkuaIfNeeded() {
        if (m_SkuaPool.ActiveObjects.Count >= m_Waves[m_WaveIndex].SkuaCount) {
            return;
        }
    }
}
