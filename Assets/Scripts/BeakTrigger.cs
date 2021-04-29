using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeakTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter(Collider otherCollider)
	{
		if(otherCollider.gameObject.name == "NavigationTrigger")
		{
			Debug.Log("Beak collided with " + otherCollider.gameObject.name);
			//try navigation technique here, or drop rocks if we're not in the water...
			
		}
		else if(otherCollider.gameObject.name.StartsWith("Sphere"))
		{
			//attach sphere to beak...
			otherCollider.gameObject.transform.parent = gameObject.transform;
		}
	}
}
