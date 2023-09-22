using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockInNest : MonoBehaviour
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
		if(otherCollider.gameObject.name.StartsWith("Beak"))
		{
			Debug.Log("Nest collided with " + otherCollider.gameObject.name);
			//try navigation technique here, or drop rocks if we're not in the water...
			if(otherCollider.gameObject.transform.childCount == 1)
			{
				Rigidbody rb = otherCollider.gameObject.transform.GetChild(0).GetComponent<Rigidbody>();
				if(rb != null)
				{
					rb.isKinematic = false;
					
					rb.AddForce(Camera.main.transform.forward, UnityEngine.ForceMode.Impulse);
					otherCollider.gameObject.transform.GetChild(0).parent = null;
					rb.detectCollisions = true;
				}	
			}
		}
	}
}
