using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHitPenguin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnCollisionEnter(Collision otherCollider)
	{
		if(otherCollider.gameObject.name.StartsWith("BowlingBall"))
		{
			//Debug.Log("Hit penguin!");
			gameObject.GetComponent<Rigidbody>().useGravity = true;
			gameObject.GetComponent<Rigidbody>().isKinematic = false;
			
			GameObject animModel = transform.GetChild(0).gameObject;
			
			if(animModel)
			{
				Animator a = animModel.GetComponent<Animator>();
				if(a != null)
				{
					a.enabled = false;
				}
			}
		}
	}
}
