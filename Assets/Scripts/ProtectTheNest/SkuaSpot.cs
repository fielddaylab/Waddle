//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

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
	
	[SerializeField]
	GameObject _currentSkua;
	
	public GameObject CurrentSkua
	{
		get { return _currentSkua; }
		set { _currentSkua = value; }
	}
	
	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
