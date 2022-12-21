using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyCodePanel : MonoBehaviour
{
	[SerializeField]
	GameObject _mainPanel;
	
	[SerializeField]
	GameObject _doneButton;
	
	[SerializeField]
	GameObject _eraseButton;
	
	[SerializeField]
	GameObject _buttonHundreds;
	
	[SerializeField]
	GameObject _buttonTens;
	
	[SerializeField]
	GameObject _buttonOnes;
	
	[SerializeField]
	HandRaycast _handRay;
	
	int _currDigit = 0;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void Reset()
	{
		_buttonHundreds.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "";
		_buttonTens.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "";
		_buttonOnes.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "";
		_currDigit = 0;
	}
	
	public void HandleButtonDown(RaycastHit hitInfo)
	{
		if(hitInfo.collider.transform.gameObject == _doneButton)
		{
			SelectButton(_doneButton, true);
			SelectButton(_eraseButton, false);
			for(int i = 0; i < 10; ++i)
			{
				SelectButton(transform.GetChild(i).gameObject, false);
			}
		}
		else if(hitInfo.collider.transform.gameObject == _eraseButton)
		{
			SelectButton(_doneButton, false);
			SelectButton(_eraseButton, true);
			for(int i = 0; i < 10; ++i)
			{
				SelectButton(transform.GetChild(i).gameObject, false);
			}
		}
		else
		{
			SelectButton(_doneButton, false);
			SelectButton(_eraseButton, false);
			
			for(int i = 0; i < 10; ++i)
			{
				if(hitInfo.collider.transform.gameObject == transform.GetChild(i).gameObject)
				{
					SelectButton(transform.GetChild(i).gameObject, true);
				}
				else
				{
					SelectButton(transform.GetChild(i).gameObject, false);
				}
			}
		}
	}
	
	public void HandleButtonUp(RaycastHit hitInfo)
	{
		if(hitInfo.collider.transform.gameObject == _doneButton)
		{
			string hundreds = _buttonHundreds.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text;
			string tens = _buttonTens.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text;
			string ones = _buttonOnes.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text;
			
			if(hundreds.Length > 0 && tens.Length > 0 && ones.Length > 0)
			{
				int h = int.Parse(hundreds);
				int t = int.Parse(tens);
				int o = int.Parse(ones);
				
				PenguinAnalytics.Instance.LogSurveyCode(h * 100 + t * 10 + o);
				_mainPanel.SetActive(true);
				gameObject.SetActive(false);
				
				if(_handRay != null)
				{
					_handRay.SwitchPanel(HandRaycast.MenuPanel.eMAIN);
				}
				
				//or restart game?
				if(PenguinGameManager.Instance.GameWasStarted)
				{
					PenguinGameManager.Instance.RestartGame();
				}
				else
				{
					PenguinGameManager.Instance.BeginTheGame(PenguinGameManager.GameMode.ResearchMode);
				}
			}
		}
		else if(hitInfo.collider.transform.gameObject == _eraseButton)
		{
			if(_currDigit == 0)
			{
				_buttonHundreds.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "";
			}
			else if(_currDigit == 1)
			{
				if(_buttonTens.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text.Length > 0)
				{
					_buttonTens.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "";
				}
				else
				{
					_currDigit--;
				}
			}
			else if(_currDigit == 2)
			{
				if(_buttonOnes.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text.Length > 0)
				{
					_buttonOnes.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "";
				}
				else
				{
					_currDigit--;
				}
			}
		}
		else
		{
			for(int i = 0; i < 10; ++i)
			{
				if(hitInfo.collider.transform.gameObject == transform.GetChild(i).gameObject)
				{
					//SelectButton(transform.GetChild(i).gameObject, true);
					if(_currDigit == 0)
					{
						_buttonHundreds.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = i.ToString();
						_currDigit++;
					}
					else if(_currDigit == 1)
					{
						_buttonTens.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = i.ToString();
						_currDigit++;
					}
					else if(_currDigit == 2)
					{
						_buttonOnes.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = i.ToString();
					}
				}
			}
		}
	}
	
	public void HandleHover(RaycastHit hitInfo)
	{
		if(hitInfo.collider.transform.gameObject == _doneButton)
		{
			SelectButton(_doneButton, true);
			SelectButton(_eraseButton, false);
			for(int i = 0; i < 10; ++i)
			{
				SelectButton(transform.GetChild(i).gameObject, false);
			}
		}
		else if(hitInfo.collider.transform.gameObject == _eraseButton)
		{
			SelectButton(_doneButton, false);
			SelectButton(_eraseButton, true);
			for(int i = 0; i < 10; ++i)
			{
				SelectButton(transform.GetChild(i).gameObject, false);
			}
		}
		else
		{
			SelectButton(_doneButton, false);
			SelectButton(_eraseButton, false);
			
			for(int i = 0; i < 10; ++i)
			{
				if(hitInfo.collider.transform.gameObject == transform.GetChild(i).gameObject)
				{
					SelectButton(transform.GetChild(i).gameObject, true);
				}
				else
				{
					SelectButton(transform.GetChild(i).gameObject, false);
				}
			}
		}
	}
	
	public void HandleNoHit(RaycastHit hitInfo)
	{
		SelectButton(_doneButton, false);
		SelectButton(_eraseButton, false);
		for(int i = 0; i < 10; ++i)
		{
			SelectButton(transform.GetChild(i).gameObject, false);
		}
	}
	
	void SelectButton(GameObject button, bool bOn)
	{
		//assumes highlight is in child slot 1
		button.transform.GetChild(0).gameObject.SetActive(!bOn);
		button.transform.GetChild(1).gameObject.SetActive(bOn);
	}
}
