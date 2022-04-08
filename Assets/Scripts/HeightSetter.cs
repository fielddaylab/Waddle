using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightSetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		ResetHeight();
    }

    // Update is called once per frame
    void Update()
    {

    }
	
	public void ResetHeight()
	{
		StartCoroutine(SetHeight(0.25f));
	}
	
	IEnumerator SetHeight(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		Vector3 vPos =  transform.localPosition;
        vPos.y = 0.6036f - transform.GetChild(0).GetChild(1).localPosition.y;
		transform.localPosition = vPos;
	}
}
