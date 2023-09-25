using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : MonoBehaviour
{
	[SerializeField]
	GameObject _leftButton;
	
	[SerializeField]
	GameObject _rightButton;
	
	[SerializeField]
	GameObject _middleButton;
	
	[SerializeField]
	GameObject _surveyButton;
	
	[SerializeField]
	GameObject _creditsBack;

	[SerializeField]
	GameObject _englishButton;

	[SerializeField]
	GameObject _spanishButton;
	
	[SerializeField]
	HandRaycast _handRay;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }
	
	public void HandleButtonDown(RaycastHit hitInfo)
	{
		if(hitInfo.collider.transform.gameObject == _rightButton)
		{
			SelectButton(_leftButton, false);
			SelectButton(_rightButton, true);
			SelectButton(_middleButton, false);
			SelectButton(_surveyButton, false);	
			SelectButton(_creditsBack, false);
		}
		else if(hitInfo.collider.transform.gameObject == _leftButton)
		{
			SelectButton(_leftButton, true);
			SelectButton(_rightButton, false);
			SelectButton(_middleButton, false);	
			SelectButton(_surveyButton, false);	
			SelectButton(_creditsBack, false);							
		}
		else if(hitInfo.collider.transform.gameObject == _middleButton)
		{
			SelectButton(_leftButton, false);
			SelectButton(_rightButton, false);
			SelectButton(_middleButton, true);
			SelectButton(_surveyButton, false);	
			SelectButton(_creditsBack, false);	
		}
		else if(hitInfo.collider.transform.gameObject == _surveyButton)
		{
			SelectButton(_leftButton, false);
			SelectButton(_rightButton, false);
			SelectButton(_middleButton, false);
			SelectButton(_surveyButton, true);	
			SelectButton(_creditsBack, false);	
		}
		else if(hitInfo.collider.transform.gameObject == _creditsBack)
		{
			SelectButton(_leftButton, false);
			SelectButton(_rightButton, false);
			SelectButton(_middleButton, false);
			SelectButton(_surveyButton, false);	
			SelectButton(_creditsBack, true);		
		}
		else if(hitInfo.collider.transform.gameObject == _spanishButton)
		{
			SelectButton(_englishButton, false);
			SelectButton(_spanishButton, true);
		}
		else if(hitInfo.collider.transform.gameObject == _englishButton)
		{
			SelectButton(_englishButton, true);
			SelectButton(_spanishButton, false);	
		}
		else
		{
			SelectButton(_leftButton, false);
			SelectButton(_rightButton, false);
			SelectButton(_middleButton, false);
			SelectButton(_surveyButton, false);	
			SelectButton(_creditsBack, false);	
		}
	}
	
	public void HandleButtonUp(RaycastHit hitInfo)
	{
		/*
		if(PenguinMenuSystem.Instance.GetCurrentMenu() == PenguinMenuSystem.MenuType.MainMenu)
		{
			if(hitInfo.collider.transform.gameObject == _leftButton)
			{
				PenguinAnalytics.Instance.LogSelectMenu("show_mode");
				PenguinGameManager.Instance.BeginTheGame(PenguinGameManager.GameMode.ShowMode);
			}
			else if(hitInfo.collider.transform.gameObject == _rightButton)
			{
				PenguinAnalytics.Instance.LogSelectMenu("home_mode");
				PenguinGameManager.Instance.BeginTheGame(PenguinGameManager.GameMode.HomeMode);
			}
			else if(hitInfo.collider.transform.gameObject == _middleButton)
			{
				//credits
				PenguinAnalytics.Instance.LogSelectMenu("credits");
				PenguinGameManager.Instance.ShowCredits(true);
			}
			else if(hitInfo.collider.transform.gameObject == _creditsBack)
			{
				PenguinAnalytics.Instance.LogSelectMenu("credits_back");
				PenguinGameManager.Instance.ShowCredits(false);
			}
			else if(hitInfo.collider.transform.gameObject == _surveyButton)
			{
				PenguinAnalytics.Instance.LogSelectMenu("survey_code");
				_middleButton.transform.parent.gameObject.SetActive(false);

				if(_handRay != null)
				{
					_handRay.SwitchPanel(HandRaycast.MenuPanel.eSURVEY_CODE);
				}
			}
			else if(hitInfo.collider.transform.gameObject == _spanishButton)
			{
				PenguinMenuSystem.Instance.SwitchLanguage(PenguinMenuSystem.WhichLanguage.SPANISH);
				PenguinAnalytics.Instance.LogLanguageSelected("Spanish");
			}
			else if(hitInfo.collider.transform.gameObject == _englishButton)
			{
				PenguinMenuSystem.Instance.SwitchLanguage(PenguinMenuSystem.WhichLanguage.ENGLISH);
				PenguinAnalytics.Instance.LogLanguageSelected("English");
			}
		}
		else */
				if(PenguinMenuSystem.Instance.GetCurrentMenu() == PenguinMenuSystem.MenuType.PauseMenu)
		{
			if(hitInfo.collider.transform.gameObject == _leftButton)
			{
				//resume (just close the menu)
				//PenguinPlayer.Instance.StopShowingUI();
				//restart
				PenguinAnalytics.Instance.LogSelectMenu("restart");
				PenguinGameManager.Instance.RestartGame();
			}
			else if(hitInfo.collider.transform.gameObject == _rightButton)
			{
				//Resume
				PenguinAnalytics.Instance.LogSelectMenu("resume");
				PenguinPlayer.Instance.StopShowingUI();
				//PenguinGameManager.Instance.HandleHMDUnmounted();
				//PenguinMenuSystem.Instance.ChangeMenuTo(PenguinMenuSystem.MenuType.MainMenu);
			}
			else if(hitInfo.collider.transform.gameObject == _middleButton)
			{
				//credits
				PenguinAnalytics.Instance.LogSelectMenu("credits");
				PenguinGameManager.Instance.ShowCredits(true);
			}
			else if(hitInfo.collider.transform.gameObject == _creditsBack)
			{
				PenguinAnalytics.Instance.LogSelectMenu("credits_back");
				PenguinGameManager.Instance.ShowCredits(false);
			}
			else if(hitInfo.collider.transform.gameObject == _surveyButton)
			{
				PenguinAnalytics.Instance.LogSelectMenu("survey_code");
				_middleButton.transform.parent.gameObject.SetActive(false);

				if(_handRay != null)
				{
					_handRay.SwitchPanel(HandRaycast.MenuPanel.eSURVEY_CODE);
				}
			}
			else if(hitInfo.collider.transform.gameObject == _spanishButton)
			{
				PenguinMenuSystem.Instance.SwitchLanguage(PenguinMenuSystem.WhichLanguage.SPANISH);
				PenguinAnalytics.Instance.LogLanguageSelected("Spanish");
			}
			else if(hitInfo.collider.transform.gameObject == _englishButton)
			{
				PenguinMenuSystem.Instance.SwitchLanguage(PenguinMenuSystem.WhichLanguage.ENGLISH);
				PenguinAnalytics.Instance.LogLanguageSelected("English");
			}
		}
		else if(PenguinMenuSystem.Instance.GetCurrentMenu() == PenguinMenuSystem.MenuType.EndMenu)
		{
			if(hitInfo.collider.transform.gameObject == _leftButton)
			{
				//resume (just close the menu)
				//PenguinPlayer.Instance.StopShowingUI();
				//restart
				PenguinAnalytics.Instance.LogSelectMenu("restart");
				PenguinGameManager.Instance.RestartGame();
			}
			else if(hitInfo.collider.transform.gameObject == _rightButton)
			{
	
			}
			else if(hitInfo.collider.transform.gameObject == _middleButton)
			{
				//resume...
				PenguinAnalytics.Instance.LogSelectMenu("resume");
				PenguinPlayer.Instance.StopShowingUI();
				//PenguinAnalytics.Instance.LogSelectMenu("credits");
				//PenguinGameManager.Instance.ShowCredits(true);
			}
			else if(hitInfo.collider.transform.gameObject == _creditsBack)
			{
				PenguinAnalytics.Instance.LogSelectMenu("credits_back");
				PenguinGameManager.Instance.ShowCredits(false);
			}
			else if(hitInfo.collider.transform.gameObject == _surveyButton)
			{
				//restart...
				PenguinAnalytics.Instance.LogSelectMenu("restart");
				PenguinGameManager.Instance.RestartGame();
			}
		}
	}
	
	public void HandleHover(RaycastHit hitInfo)
	{
		//Debug.Log(hitInfo.collider.transform.gameObject.name);
		if(hitInfo.collider.transform.gameObject == _rightButton)
		{
			SelectButton(_leftButton, false);
			SelectButton(_rightButton, true);
			SelectButton(_middleButton, false);
			SelectButton(_creditsBack, false);
			SelectButton(_surveyButton, false);	
		}
		else if(hitInfo.collider.transform.gameObject == _leftButton)
		{
			SelectButton(_leftButton, true);
			SelectButton(_rightButton, false);
			SelectButton(_middleButton, false);	
			SelectButton(_creditsBack, false);	
			SelectButton(_surveyButton, false);				
		}
		else if(hitInfo.collider.transform.gameObject == _middleButton)
		{
			SelectButton(_leftButton, false);
			SelectButton(_rightButton, false);
			SelectButton(_middleButton, true);
			SelectButton(_creditsBack, false);	
			SelectButton(_surveyButton, false);	
		}
		else if(hitInfo.collider.transform.gameObject == _surveyButton)
		{
			SelectButton(_leftButton, false);
			SelectButton(_rightButton, false);
			SelectButton(_middleButton, false);
			SelectButton(_creditsBack, false);
			SelectButton(_surveyButton, true);	
		}
		else if(hitInfo.collider.transform.gameObject == _creditsBack)
		{
			SelectButton(_leftButton, false);
			SelectButton(_rightButton, false);
			SelectButton(_middleButton, false);
			SelectButton(_creditsBack, true);
			SelectButton(_surveyButton, false);				
		}
		else if(hitInfo.collider.transform.gameObject == _spanishButton)
		{
			SelectButton(_spanishButton, true);	
			SelectButton(_englishButton, false);	
		}
		else if(hitInfo.collider.transform.gameObject == _englishButton)
		{
			SelectButton(_spanishButton, false);	
			SelectButton(_englishButton, true);	
		}
		else
		{
			SelectButton(_leftButton, false);
			SelectButton(_rightButton, false);
			SelectButton(_middleButton, false);
			SelectButton(_creditsBack, false);
			SelectButton(_surveyButton, false);	
		}
	}
	
	public void HandleNoHit(RaycastHit hitInfo)
	{
		//Debug.Log("Didn't hit anything");
		SelectButton(_leftButton, false);
		SelectButton(_rightButton, false);
		SelectButton(_middleButton, false);
		SelectButton(_creditsBack, false);
		SelectButton(_spanishButton, false);
		SelectButton(_englishButton, false);
	}
	
	void SelectButton(GameObject button, bool bOn)
	{
		//assumes highlight is in child slot 1
		button.transform.GetChild(0).gameObject.SetActive(!bOn);
		button.transform.GetChild(1).gameObject.SetActive(bOn);
	}
	
}
