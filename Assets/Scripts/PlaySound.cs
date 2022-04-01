using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnEnable()
	{
		AudioSource aSource = GetComponent<AudioSource>();
		
		if(aSource != null)
		{
			aSource.Play();
		}
	}
	
	void OnDisable()
	{
		AudioSource aSource = GetComponent<AudioSource>();
		
		if(aSource != null)
		{
			aSource.Stop();
		}
	}
}
