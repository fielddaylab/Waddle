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
	
	LineRenderer _lineRenderer;
	
	bool _wasPinching = false;
	
    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = transform.GetChild(5).gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
		//add check for hand tracking being active...
		if(PenguinPlayer.Instance.ShowingUI && _rightHand != null)
		{
			RaycastHit hitInfo;
			
			Vector3 castOrigin = _rightHand.transform.position - _rightHand.transform.right*0.11f - _rightHand.transform.forward*0.025f - _rightHand.transform.up * 0.075f;
			
			if(OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.Hands))
			{
				if(Physics.Raycast(castOrigin, -_rightHand.transform.up, out hitInfo, Mathf.Infinity, _mask, QueryTriggerInteraction.Ignore))
				{
					GameObject pObject = hitInfo.collider.transform.parent.gameObject;
					
					
					if(pObject.transform.GetChild(0).gameObject == hitInfo.collider.transform.gameObject)
					{
						pObject.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
						pObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
					}
					else if(pObject.transform.GetChild(1).gameObject == hitInfo.collider.transform.gameObject)
					{
						pObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
						pObject.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);		
					}
					else
					{
			
					}
					
					if(_lineRenderer != null)
					{
						_lineRenderer.SetPosition(0, castOrigin);
						_lineRenderer.SetPosition(1, hitInfo.point);
					}
					//Debug.Log("Hit " + hitInfo.collider.transform.gameObject.name);
				}
				else
				{
					if(_lineRenderer != null)
					{
						_lineRenderer.SetPosition(0, castOrigin);
						_lineRenderer.SetPosition(1, _rightHand.transform.position - _rightHand.transform.right*0.11f - _rightHand.transform.forward*0.025f - _rightHand.transform.up * 10f);
					}
				}
			}
			else if(OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.Hands))
			{
				if(Physics.Raycast(castOrigin, -_rightHand.transform.up, out hitInfo, Mathf.Infinity, _mask, QueryTriggerInteraction.Ignore))
				{
					GameObject pObject = hitInfo.collider.transform.parent.gameObject;
						
					//raycast the UI...
					//Debug.Log("Pressing button with hand");
					if(pObject.transform.GetChild(0).gameObject == hitInfo.collider.transform.gameObject)
					{
						//resume (just close the menu)
						PenguinPlayer.Instance.StopShowingUI();
					}
					else if(pObject.transform.GetChild(1).gameObject == hitInfo.collider.transform.gameObject)
					{
						//restart
						PenguinGameManager.Instance.HandleHMDUnmounted();
						PenguinGameManager.Instance.HandleHMDMounted();
						PenguinPlayer.Instance.StopShowingUI();
					}
					
					if(_lineRenderer != null)
					{
						_lineRenderer.SetPosition(0, Vector3.zero);
						_lineRenderer.SetPosition(1, Vector3.zero);
					}
				}
				else
				{
					if(_lineRenderer != null)
					{
						_lineRenderer.SetPosition(0, Vector3.zero);
						_lineRenderer.SetPosition(1, Vector3.zero);
					}
				}
			}
			
			/*else
			{
				if(_lineRenderer != null)
				{
					_lineRenderer.SetPosition(0, castOrigin);
					_lineRenderer.SetPosition(1, _rightHand.transform.position - _rightHand.transform.right*0.125f - _rightHand.transform.forward*0.075f - _rightHand.transform.up * 10f);
				}
			}*/
		}
    }
}
