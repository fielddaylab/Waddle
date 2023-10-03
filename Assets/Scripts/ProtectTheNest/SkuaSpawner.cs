//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections.Generic;
using BeauUtil;
using UnityEngine;
using UnityEngine.Serialization;
using System;
using Waddle;
using FieldDay;
using FieldDay.Debugging;
using BeauUtil.Debugger;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

public class SkuaSpawner : MonoBehaviour
{
	[SerializeField] private SkuaController m_SkuaPrefab;
	[SerializeField, FormerlySerializedAs("_spawnLocations")] private SkuaSpot[] m_AllSpots;
    [SerializeField] private SkuaSpot[] m_SpawnSpots;
    [SerializeField] private SkuaSpot[] m_OuterSpots;
    [SerializeField] private Egg m_TheEgg;

    private RingBuffer<SkuaController> m_CurrentSkuas = new RingBuffer<SkuaController>(8, RingBufferMode.Expand);
    private BitSet32 m_OccupiedSpotMask;
    private BitSet32 m_PendingOccupantMask;
    private WeightedSet<SkuaSpot> m_SpotChooser = new WeightedSet<SkuaSpot>(16);
    private RingBuffer<SkuaController> m_SkuaWorkList = new RingBuffer<SkuaController>(8, RingBufferMode.Expand);
    [NonSerialized] private int m_SpotLookUpdateIndex;
	
	public Egg TheEgg { get { return m_TheEgg; } }

    #region Unity Events

    private void Awake() {
        for(int i = 0; i < m_AllSpots.Length; i++) {
            m_AllSpots[i].Index = i;
        }
    }

    // Update is called once per frame
    void Update() {
        if (m_TheEgg != null) {
            if (m_TheEgg.GetComponent<Egg>().WasReset) {
                m_TheEgg.GetComponent<Egg>().WasReset = false;
            }
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.S)) {
            foreach(var skua in m_CurrentSkuas) {
                skua.SkuaHit(Vector3.up * 4);
            }
        }

        foreach(var skua in m_CurrentSkuas) {
            DebugDraw.AddWorldText(skua.CachedTransform.position, skua.MainProcess.CurrentStateId.ToDebugString(), Color.white, Frame.DeltaTime, TextAnchor.MiddleCenter, DebugTextStyle.BackgroundDarkOpaque);
        }
        foreach(var spot in m_AllSpots) {
            if (m_OccupiedSpotMask.IsSet(spot.Index)) {
                DebugDraw.AddWorldText(spot.CachedPosition, "occupied", Color.red, Frame.DeltaTime);
            } else if (m_PendingOccupantMask.IsSet(spot.Index)) {
                DebugDraw.AddWorldText(spot.CachedPosition, "pending", ColorBank.Orange, Frame.DeltaTime);
            } else {
                DebugDraw.AddWorldText(spot.CachedPosition, "free", Color.blue, Frame.DeltaTime);
            }
        }
#endif // UNITY_EDITOR
    }

    #endregion // Unity Events

    #region Operations

    public void UpdateLookScores() {
        LODReference playerHead = Game.SharedState.Get<LODReference>();
        UpdateLookScoresPartial(playerHead.CachedTransform.position, playerHead.CachedTransform.forward);
    }

    [ContextMenu("Spawn Skua")]
    public void SpawnSkua() {
        SkuaSpot bestSpawn = FindSpawnSpot(m_OccupiedSpotMask | m_PendingOccupantMask);
        if (bestSpawn != null) {
            InstantiateSkua(bestSpawn);
        }
    }

    private SkuaController InstantiateSkua(SkuaSpot spot) {
        Vector3 spawnSpot = spot.CachedTransform.position;
        SkuaController newSkua = Instantiate(m_SkuaPrefab, spot.CachedTransform.position, Quaternion.identity);
        newSkua.name = "Skua" + m_CurrentSkuas.Count.ToStringLookup();
        newSkua.Spawner = this;
        PenguinAnalytics.Instance.LogSkuaSpawned(newSkua.name, spawnSpot);

        Vector3 spawnRot = spot.CachedRotation.eulerAngles;
        spawnRot.y += newSkua.CachedTransform.eulerAngles.y; //due to skua model's local rotation.

        newSkua.CachedTransform.SetPositionAndRotation(spawnSpot, Quaternion.Euler(spawnRot));
        AssignToSpot(newSkua, spot);
        newSkua.StartMainProcess(SkuaStates.Idle);

        m_CurrentSkuas.PushBack(newSkua);
        return newSkua;
    }

    private void UpdateLookScoresPartial(Vector3 pos, Vector3 forward) {
        forward.y = 0;
        forward.Normalize();
        for(int i = 0; i < 8; i++) {
            SkuaSpot spot = m_AllSpots[m_SpotLookUpdateIndex];
            Vector3 normal = spot.CachedPosition - pos;
            pos.y = 0;
            normal.Normalize();
            spot.LookScore = (1 + Vector3.Dot(normal, forward)) / 2;
            m_SpotLookUpdateIndex = (m_SpotLookUpdateIndex + 1) % m_AllSpots.Length;

            //DebugDraw.AddWorldText(spot.CachedPosition, string.Format("lookscore {0}", spot.LookScore), Color.black, 0.2f, TextAnchor.MiddleCenter, DebugTextStyle.BackgroundLightOpaque);
        }
    }

    public void MoveSkuas(int attackingCount, bool normalMove) {
        m_SkuaWorkList.Clear();
        foreach (SkuaController skua in m_CurrentSkuas) {
            if (skua.InHitState() || !skua.CurrentSpot || skua.HoldingEgg) {
                continue;
            }

            m_SkuaWorkList.PushBack(skua);
        }
        RNG.Instance.Shuffle(m_SkuaWorkList);
        if (attackingCount > 0) {
            m_SkuaWorkList.Sort((a, b) => {
                if (a.CurrentSpot.IsInner) {
                    if (b.CurrentSpot.IsInner) {
                        return 0;
                    } else {
                        return -1;
                    }
                } else if (b.CurrentSpot.IsInner) {
                    return 1;
                } else {
                    return 0;
                }
            });
        }

        Log.Msg("[SkuaSpawner] Attacking {0} Normal {1}", attackingCount, normalMove);
        BitSet32 newLocations = m_OccupiedSpotMask | m_PendingOccupantMask;

        for(int i = 0; i < m_SkuaWorkList.Count; i++) {
            SkuaController sc = m_SkuaWorkList[i];
            if (attackingCount > 0) {
                SkuaSpot attackSpot = sc.CurrentSpot.SpotIn;
                if (attackSpot && !newLocations.IsSet(attackSpot.Index)) {
                    newLocations.Unset(sc.CurrentSpot.Index);
                    newLocations.Set(attackSpot.Index);
                    if (attackSpot.IsCenter) {
                        sc.GrabEgg(attackSpot, m_TheEgg);
                    } else {
                        sc.WalkToSpot(attackSpot, SkuaMovementDirection.FORWARD, RNG.Instance.NextFloat(0.1f));
                    }
                    attackingCount--;
                    continue;
                }
            }

            if (normalMove) {
                SkuaSpot nextSpot = FindNextSpot(sc.CurrentSpot, 1, 0.5f, 0, 0.2f, 0.2f, newLocations);
                if (nextSpot != null && nextSpot != sc.CurrentSpot) {
                    newLocations.Unset(sc.CurrentSpot.Index);
                    newLocations.Set(nextSpot.Index);
                    if (nextSpot.IsCenter) {
                        sc.GrabEgg(nextSpot, m_TheEgg);
                    } else {
                        sc.WalkToSpot(nextSpot, sc.CurrentSpot.GetDirection(nextSpot), RNG.Instance.NextFloat(0.2f));
                    }
                }
            }
        }
    }

    public void RetreatSkuas() {
        m_SkuaWorkList.Clear();
        foreach (SkuaController skua in m_CurrentSkuas) {
            if (skua.InHitState() || !skua.CurrentSpot || skua.HoldingEgg) {
                continue;
            }

            m_SkuaWorkList.PushBack(skua);
        }
        RNG.Instance.Shuffle(m_SkuaWorkList);

        BitSet32 newLocations = m_OccupiedSpotMask | m_PendingOccupantMask;
        
        for (int i = 0; i < m_SkuaWorkList.Count; i++) {
            SkuaController sc = m_SkuaWorkList[i];
            SkuaSpot retreatSpot = sc.CurrentSpot.SpotOut;
            if (retreatSpot && !newLocations.IsSet(retreatSpot.Index)) {
                newLocations.Unset(sc.CurrentSpot.Index);
                newLocations.Set(retreatSpot.Index);
                sc.WalkToSpot(retreatSpot, SkuaMovementDirection.BACK, RNG.Instance.NextFloat(0.2f));
            }
        }
    }

    public void AssignToSpot(SkuaController skua, SkuaSpot spot) {
        if (skua.CurrentSpot == spot) {
            return;
        }

        if (skua.CurrentSpot != null) {
            if (skua.CurrentSpot.CurrentSkua == skua) {
                m_OccupiedSpotMask.Unset(skua.CurrentSpot.Index);
                skua.CurrentSpot.CurrentSkua = null;
            }
        }

        skua.CurrentSpot = spot;
        if (spot) {
            spot.CurrentSkua = skua;
            m_OccupiedSpotMask.Set(spot.Index);
            m_PendingOccupantMask.Unset(spot.Index);
        }
    }

    public void SetPendingOccupancy(SkuaSpot spot, bool set) {
        m_PendingOccupantMask.Set(spot.Index, set);
    }

    public void ClearGame() {
        m_OccupiedSpotMask.Clear();
        m_PendingOccupantMask.Clear();

        for (int i = 0; i < m_CurrentSkuas.Count; ++i) {
            m_CurrentSkuas[i].SkuaRemove();
        }

        for (int i = 0; i < m_AllSpots.Length; ++i) {
            m_AllSpots[i].CurrentSkua = null;
        }

        m_CurrentSkuas.Clear();
    }

    #endregion // Operations

    #region Checks

    /// <summary>
    /// Finds an open spot near another spot.
    /// </summary>
    private SkuaSpot FindNextSpot(SkuaSpot from, float horizontalWeight, float forwardWeight, float forwardGrabWeight, float backWeight, float stayWeight, BitSet32 occupiedMask) {
        m_SpotChooser.Clear();

        if (horizontalWeight > 0) {
            if (from.SpotLeft && !occupiedMask.IsSet(from.SpotLeft.Index)) {
                m_SpotChooser.Add(from.SpotLeft, (1 + from.SpotLeft.LookScore * 3) * horizontalWeight);
            }
            if (from.SpotRight && !occupiedMask.IsSet(from.SpotRight.Index)) {
                m_SpotChooser.Add(from.SpotRight, (1 + from.SpotRight.LookScore * 3) * horizontalWeight);
            }
        }
        if (forwardWeight > 0 || forwardGrabWeight > 0) {
            if (from.SpotIn && !occupiedMask.IsSet(from.SpotIn.Index)) {
                if (from.SpotIn.IsCenter) {
                    m_SpotChooser.Add(from.SpotIn, (1 + from.SpotIn.LookScore * 3) * forwardGrabWeight);
                } else {
                    m_SpotChooser.Add(from.SpotIn, (1 + from.SpotIn.LookScore * 3) * forwardWeight);
                }
            }
        }
        if (backWeight > 0) {
            if (from.SpotOut && !occupiedMask.IsSet(from.SpotOut.Index)) {
                m_SpotChooser.Add(from.SpotOut, (1 + from.SpotOut.LookScore * 3) * backWeight);
            }
        }
        if (stayWeight > 0) {
            m_SpotChooser.Add(from, (1 + from.LookScore) * forwardWeight);
        }

        return m_SpotChooser.GetItem();
    }

    /// <summary>
    /// Finds an open outer location.
    /// </summary>
    private SkuaSpot FindOuterSpot(BitSet32 occupiedMask) {
        m_SpotChooser.Clear();

        foreach(var spot in m_OuterSpots) {
            if (spot.IsBlocked || occupiedMask.IsSet(spot.Index)) {
                continue;
            }

            m_SpotChooser.Add(spot, 1 + spot.LookScore * 3);
        }

        return m_SpotChooser.GetItem();
    }

    /// <summary>
    /// Finds an open outer location.
    /// </summary>
    public SkuaSpot FindOuterSpot() {
        return FindOuterSpot(m_OccupiedSpotMask | m_PendingOccupantMask);
    }

    /// <summary>
    /// Finds an open spawn location.
    /// </summary>
    private SkuaSpot FindSpawnSpot(BitSet32 occupiedMask) {
        m_SpotChooser.Clear();

        foreach (var spot in m_SpawnSpots) {
            if (spot.IsBlocked || occupiedMask.IsSet(spot.Index)) {
                continue;
            }

            m_SpotChooser.Add(spot, 1 + spot.LookScore * 3);
        }

        return m_SpotChooser.GetItem();
    }

    private bool EggIsTaken() {
        if (m_TheEgg != null) {
            return m_TheEgg.IsTaken;
        }

        return false;
    }

    #endregion // Checks

#if UNITY_EDITOR
    [ContextMenu("Find Spawn Points")]
    private void GatherSpawnPoints() {
        Undo.RecordObject(this, "gathering spawn points");
        EditorUtility.SetDirty(this);

        List<SkuaSpot> allSpawnSpots = new List<SkuaSpot>();
        foreach(var spot in m_AllSpots) {
            if (spot.IsSpawn) {
                allSpawnSpots.Add(spot);
            }
        }
        m_SpawnSpots = allSpawnSpots.ToArray();

        List<SkuaSpot> allOuterSpots = new List<SkuaSpot>();
        foreach(var spot in m_AllSpots) {
            if (spot.IsOuter) {
                allOuterSpots.Add(spot);
            }
        }
        m_OuterSpots = allOuterSpots.ToArray();
    }
#endif // UNITY_EDITOR
}
