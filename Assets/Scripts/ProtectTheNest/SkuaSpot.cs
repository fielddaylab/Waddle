//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkuaSpot : MonoBehaviour
{
	[SerializeField]
	SkuaSpot _spotIn;
	
	[SerializeField]
	SkuaSpot _spotOut;
	
	[SerializeField]
	SkuaSpot _spotLeft;
	
	[SerializeField]
	SkuaSpot _spotRight;
    
	public SkuaSpot SpotRight => _spotRight;
	public SkuaSpot SpotLeft => _spotLeft;
	public SkuaSpot SpotIn => _spotIn;
	public SkuaSpot SpotOut => _spotOut;
	
	[SerializeField]
	bool _isOuter;
	
	public bool IsOuter => _isOuter;
	
	[SerializeField]
	bool _isCenter;
	
	public bool IsCenter => _isCenter;
	
	[SerializeField]
	bool _isBlocked;
	
	public bool IsBlocked => _isBlocked;
	
	[SerializeField]
	bool _isInner;
	
	public bool IsInner => _isInner;
	
	[SerializeField]
	bool _isUp = false;
	public bool IsUp => _isUp;

    [NonSerialized] public SkuaBrain Occupant;
}
