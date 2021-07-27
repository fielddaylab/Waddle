using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
	bool _isTaken;
	
	public bool IsTaken
	{
		get { return _isTaken; }
		set { _isTaken = value; }
	}
	
    // Start is called before the first frame update
    void Start()
    {
        _isTaken = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
