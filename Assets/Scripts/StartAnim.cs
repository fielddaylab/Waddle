using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnim : MonoBehaviour
{
	[SerializeField]
	float _startTimeDelay = 0f;
	
	[SerializeField]
	string _startTransition = "";
	
    // Start is called before the first frame update
    void Start()
    {
		GameObject animModel = transform.GetChild(0).gameObject;
		if(animModel != null)
		{
			Animator a = animModel.GetComponent<Animator>();
			if(a != null)
			{
				a.enabled = false;
			}
		}
		
		StartCoroutine(StartAnimation());
    }

	IEnumerator StartAnimation()
	{
		yield return new WaitForSeconds(_startTimeDelay);

		GameObject animModel = transform.GetChild(0).gameObject;
		if(animModel)
		{
			Animator a = animModel.GetComponent<Animator>();
			if(a != null)
			{
				a.enabled = true;
				if(_startTransition.Length == 0)
				{
					a.SetBool(_startTransition, true);
				}
			}
		}
	
	}
	
    // Update is called once per frame
    void Update()
    {
        
    }
}
