using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeakTrigger : MonoBehaviour
{
	public GameObject navigationTrigger;
	
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
			if(gameObject.transform.childCount == 1)
			{
				Rigidbody rb = gameObject.transform.GetChild(0).GetComponent<Rigidbody>();
				if(rb != null)
				{
					rb.isKinematic = false;
					rb.detectCollisions = false;
					//otherCollider.detectCollisions = false;
					//gameObject.GetComponent<Collider>().enabled = false;
					gameObject.transform.GetChild(0).parent = null;
					
				}	
			}
		}
		else if(otherCollider.gameObject.name.StartsWith("rock"))
		{
			//attach sphere to beak...
			if(gameObject.transform.childCount == 0)
			{
				otherCollider.gameObject.transform.parent = gameObject.transform;
				Rigidbody rb = otherCollider.gameObject.GetComponent<Rigidbody>();
				if(rb != null)
				{
					rb.isKinematic = true;
				}
				//enable the navigationtrigger collider...
				if(navigationTrigger != null)
				{
					navigationTrigger.GetComponent<Collider>().enabled = true;
				}
			}
			Debug.Log(otherCollider.gameObject.name);
		}
	}
}
