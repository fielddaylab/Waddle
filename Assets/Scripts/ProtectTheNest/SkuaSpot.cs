//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System;
using BeauUtil;
using BeauUtil.Debugger;
using FieldDay.Components;
using UnityEngine;
using UnityEngine.Serialization;

public class SkuaSpot : BatchedComponent
{
	[FormerlySerializedAs("_spotIn")] public SkuaSpot SpotIn;
	[FormerlySerializedAs("_spotOut")] public SkuaSpot SpotOut;
	[FormerlySerializedAs("_spotLeft")] public SkuaSpot SpotLeft;
	[FormerlySerializedAs("_spotRight")] public SkuaSpot SpotRight;

    [Header("Flags")]
    public bool IsSpawn = false;
	[FormerlySerializedAs("_isOuter")] public bool IsOuter;
    [FormerlySerializedAs("_isInner")] public bool IsInner;
    [FormerlySerializedAs("_isCenter")] public bool IsCenter;
    [FormerlySerializedAs("_isBlocked")] public bool IsBlocked;
    [FormerlySerializedAs("_isUp")] public bool IsUp;

    [NonSerialized] public Transform CachedTransform;
    [NonSerialized] public Vector3 CachedPosition;
    [NonSerialized] public Quaternion CachedRotation;
    [NonSerialized] public SkuaController CurrentSkua;
    [NonSerialized] public int Index;
    [NonSerialized] public float LookScore;

    private void Awake() {
        this.CacheComponent(ref CachedTransform);
        CachedTransform.GetPositionAndRotation(out CachedPosition, out CachedRotation);
    }

    public SkuaMovementDirection GetDirection(SkuaSpot spot) {
        if (ReferenceEquals(spot, SpotIn)) {
            return SkuaMovementDirection.FORWARD;
        } else if (ReferenceEquals(spot, SpotOut)) {
            return SkuaMovementDirection.BACK;
        } else if (ReferenceEquals(spot,  SpotLeft)) {
            return SkuaMovementDirection.LEFT;
        } else if (ReferenceEquals(spot, SpotRight)) {
            return SkuaMovementDirection.RIGHT;
        } else if (ReferenceEquals(spot, this)) {
            return SkuaMovementDirection.STAY;
        } else {
            Assert.Fail("SkuaSpot is not adjacent");
            return SkuaMovementDirection.STAY;
        }
    }

    public SkuaSpot GetNeighbor(SkuaMovementDirection dir) {
        switch (dir) {
            case SkuaMovementDirection.LEFT:
                return SpotLeft;
            case SkuaMovementDirection.RIGHT:
                return SpotRight;
            case SkuaMovementDirection.FORWARD:
                return SpotIn;
            case SkuaMovementDirection.BACK:
                return SpotOut;
            case SkuaMovementDirection.STAY:
                return this;
            default:
                return null;
        }
    }
}
