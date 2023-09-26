using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkuaRemoveState : MonoBehaviour, ISkuaState
{
    private SkuaController _sc;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Handle(SkuaController sc)
	{
		if(_sc == null)
		{
			_sc = sc;
		}

        Object.Destroy(gameObject);
    }
}
