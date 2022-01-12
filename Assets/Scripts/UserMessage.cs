using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMessage : MonoBehaviour
{
	bool _showingMessage = false;
	
	public bool ShowingMessage => _showingMessage;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void StartShowMessage(string message, float duration)
	{
		if(!_showingMessage)
		{
			_showingMessage = true;
			StartCoroutine(ShowMessage(message, duration));
		}
	}
	
	public void ForceMessageOff()
	{
		_showingMessage = false;
		
		transform.GetChild(0).gameObject.SetActive(false);
		transform.GetChild(1).gameObject.SetActive(false);
		transform.GetChild(2).gameObject.SetActive(false);
	}
	
	IEnumerator ShowMessage(string message, float duration)
	{
		transform.GetChild(0).gameObject.SetActive(true);
		transform.GetChild(1).gameObject.SetActive(true);
		transform.GetChild(2).gameObject.SetActive(true);
		
		//transform.GetChild(0).GetComponent<TextMesh>().text = message;
		
		yield return new WaitForSeconds(duration);
		
		//clear text 
		//transform.GetChild(0).GetComponent<TextMesh>().text = " ";
		
		_showingMessage = false;
		
		transform.GetChild(0).gameObject.SetActive(false);
		transform.GetChild(1).gameObject.SetActive(false);
		transform.GetChild(2).gameObject.SetActive(false);
	}
}
