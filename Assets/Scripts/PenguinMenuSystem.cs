//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinMenuSystem : Singleton<PenguinMenuSystem>
{
	public enum MenuType
	{
		MainMenu,
		PauseMenu,
		EndMenu
	}

	public enum WhichLanguage
    {
        ENGLISH,
        SPANISH
    };

	WhichLanguage _lastLanguage = WhichLanguage.ENGLISH;

	[SerializeField]
	GameObject _closeButton;
	
	[SerializeField]
	GameObject _leftButton;
	
	[SerializeField]
	GameObject _rightButton;
	
	[SerializeField]
	GameObject _middleButton;

	[SerializeField]
	GameObject _surveyButton;
	
	[SerializeField]
	GameObject _titleText;
	
	[SerializeField]
	GameObject _versionText;
	
	[SerializeField]
	GameObject _backButton;
	
	[SerializeField]
	GameObject _doneButton;
	
	[SerializeField]
	GameObject _creditsBack;
	
	MenuType _currentType = MenuType.MainMenu;
	
	Vector3 _menuOffset = new Vector3(0.0f, -0.1f, 0.0f);
	
	bool _wasGamePaused = false;
	bool _wasNowUnpaused = false;
    // Start is called before the first frame update
    void Start()
    {
		if(_versionText != null)
		{
			_versionText.GetComponent<TMPro.TextMeshPro>().text = "Version: " + UnityEngine.Application.version.ToString() + "-" + PenguinAnalytics.logVersion.ToString();
		}
    }

    // Update is called once per frame
    void LateUpdate()
    {
		if(PenguinGameManager._isGamePaused)
		{
			//transform.position = PenguinPlayer.Instance.transform.position + PenguinPlayer.Instance.transform.forward * 0.01f + _menuOffset;
			_wasGamePaused = true;
		}
		else
		{
			if(_wasGamePaused)
			{
				_wasNowUnpaused = true;
			}
		}
		
		if(_wasGamePaused && _wasNowUnpaused)
		{
			ForceUpdate();
			_wasNowUnpaused = false;
			_wasGamePaused = false;
		}
    }
	
	public void ForceUpdate()
	{
		Vector3 p = Vector3.zero;
		Vector3 f = Vector3.zero;
		PenguinPlayer.Instance.GetPosForward(out p, out f);
		f.y = 0.0f;
		f = Vector3.Normalize(f);
		transform.position = p + f * 0.01f + _menuOffset;
		Quaternion q = transform.rotation;
		q.SetLookRotation(f, Vector3.up);
		transform.rotation = q;
	}

	WhichLanguage GetCurrentLanguage() { return _lastLanguage; }
	
    public void SwitchLanguage(WhichLanguage w)
    {
        if(w == WhichLanguage.ENGLISH)
        {
			_leftButton.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(20, 1);
			_surveyButton.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(20, 1);
			_surveyButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().fontSize = 10;
            if(_currentType == MenuType.MainMenu)
			{
                _leftButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Show Mode";
                _middleButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Credits";
                _rightButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Home Mode";
                _surveyButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Research Mode";
            }
			else if(_currentType == MenuType.PauseMenu)
			{
                _leftButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Restart";
                _middleButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Credits";
                _rightButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Resume";
                _surveyButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Research Mode";
			}
			else if(_currentType == MenuType.EndMenu)
			{
				_middleButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Keep Playing";
				_surveyButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Restart";
			}
			
			if(_versionText != null)
			{
				_versionText.GetComponent<TMPro.TextMeshPro>().text = "Version: " + UnityEngine.Application.version.ToString() + "-" + PenguinAnalytics.logVersion.ToString();
			}
			
			if(_doneButton != null)
			{
				_doneButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Done";
			}
			
			if(_backButton != null)
			{
				_backButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Back";
			}
			
			if(_creditsBack != null)
			{
				_creditsBack.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Back";
			}
        }
        else if(w == WhichLanguage.SPANISH)
        {
			_leftButton.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(20, 2);
			_surveyButton.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(20, 2);
            if(_currentType == MenuType.MainMenu)
			{
				_surveyButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().fontSize = 8;
                _leftButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Modo de\nPresentación";
                _middleButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Créditos";
                _rightButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Modo de Casa";
                _surveyButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Modo de\nInvestigación";
            }
			else if(_currentType == MenuType.PauseMenu)
			{
                _leftButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Reanuda";
                _middleButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Créditos";
                _rightButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Reasume";
                _surveyButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Modo de\nInvestigación";
			}
			else if(_currentType == MenuType.EndMenu)
			{
				_middleButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Siga Jugando";
				_surveyButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Reanuda";
			}

			if(_versionText != null)
			{
				_versionText.GetComponent<TMPro.TextMeshPro>().text = "Versión: " + UnityEngine.Application.version.ToString() + "-" + PenguinAnalytics.logVersion.ToString();
			}
			
			if(_doneButton != null)
			{
				_doneButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Hecho";
			}
			
			if(_backButton != null)
			{
				_backButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Regresa";
			}
			
			if(_creditsBack != null)
			{
				_creditsBack.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Regresa";
			}
        }

		_lastLanguage = w;
    }

	public MenuType GetCurrentMenu()
	{
		return _currentType;
	}
	
	public void ChangeMenuTo(MenuType menu)
	{
		_currentType = menu;
		if(menu == MenuType.MainMenu)
		{
			_leftButton.SetActive(true);
			_rightButton.SetActive(true);
			_titleText.GetComponent<MeshRenderer>().enabled = false;
			//_titleText.GetComponent<TMPro.TextMeshPro>().text = "Penguins VR";
			if(_lastLanguage == WhichLanguage.ENGLISH)
			{
				_leftButton.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(20, 1);
				_surveyButton.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(20, 1);
				_surveyButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().fontSize = 10;
				_leftButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Show Mode";
				_rightButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Home Mode";
				_middleButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Credits";
				_surveyButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Research Mode";
			}
			else if(_lastLanguage == WhichLanguage.SPANISH)
			{
				_leftButton.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(20, 2);
				_surveyButton.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(20, 2);
			
                _leftButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Modo de\nPresentación";
                _middleButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Créditos";
                _rightButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Modo de Casa";
                _surveyButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Modo de\nInvestigación";
			}
			//_closeButton.SetActive(false);
		}
		else if(menu == MenuType.PauseMenu)
		{
			_leftButton.SetActive(true);
			_rightButton.SetActive(true);
			//_titleText.GetComponent<TMPro.TextMeshPro>().text = "Options";
			_titleText.GetComponent<MeshRenderer>().enabled = false;
			
			if(_lastLanguage == WhichLanguage.ENGLISH)
			{
				_leftButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Restart";
				_rightButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Resume";
				_middleButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Credits";
				_surveyButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Research Mode";
			}
			else if(_lastLanguage == WhichLanguage.SPANISH)
			{
                _leftButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Reanuda";
                _middleButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Créditos";
                _rightButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Reasume";
                _surveyButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Modo de\nInvestigación";
			}
			//_closeButton.SetActive(true);
		}
		else if(menu == MenuType.EndMenu)
		{
			_leftButton.SetActive(false);
			_rightButton.SetActive(false);
			_titleText.GetComponent<MeshRenderer>().enabled = true;
			
			if(_lastLanguage == WhichLanguage.ENGLISH)
			{
				_titleText.GetComponent<TMPro.TextMeshPro>().text = "You did it!\nYou hatched a chick!";
				_middleButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Keep Playing";
				_surveyButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Restart";
			}
			else if(_lastLanguage == WhichLanguage.SPANISH)
			{
				_titleText.GetComponent<TMPro.TextMeshPro>().text = "¡Lo Hiciste! ¡Eclosionaste un bebe pinguino!";
				_middleButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Siga Jugando";
				_surveyButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Reanuda";
			}
			
			//_closeButton.SetActive(false);
		}
	}
	
}
