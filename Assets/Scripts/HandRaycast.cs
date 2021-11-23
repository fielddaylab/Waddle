using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;

public class HandRaycast : MonoBehaviour
{
	[SerializeField]
	GameObject _rightHand;
	
	[SerializeField]
	GameObject _leftHand;
	
	[SerializeField]
	LayerMask _mask;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if(_rightHand != null)
		{
			RaycastHit hitInfo;

			if(Physics.Raycast(_rightHand.transform.position, _rightHand.transform.forward, out hitInfo, Mathf.Infinity, _mask, QueryTriggerInteraction.Ignore))
			{
				if(OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.Hands))
				{
					//raycast the UI...
					
				}
				
				GameObject pObject = hitInfo.collider.transform.parent.gameObject;
				if(pObject.transform.GetChild(0).gameObject == hitInfo.collider.transform.gameObject)
				{
					pObject.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
					pObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
				}
				else if(pObject.transform.GetChild(1).gameObject == hitInfo.collider.transform.gameObject)
				{
					pObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
					pObject.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);		
				}
				else
				{
		
				}
				
				//Debug.Log("Hit " + hitInfo.collider.transform.gameObject.name);
			}
		}
    }
}
