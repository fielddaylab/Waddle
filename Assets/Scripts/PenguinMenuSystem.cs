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
	
	MenuType _currentType = MenuType.MainMenu;
	
	Vector3 _menuOffset = new Vector3(0.0f, 0.5f, 0.0f);
	
    // Start is called before the first frame update
    void Start()
    {
		if(_versionText != null)
		{
			_versionText.GetComponent<TMPro.TextMeshPro>().text = "Version: " + UnityEngine.Application.version.ToString() + "-" + PenguinAnalytics.logVersion.ToString();
		}
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = PenguinPlayer.Instance.transform.position + PenguinPlayer.Instance.transform.forward * 0.01f + _menuOffset;
    }

	WhichLanguage GetCurrentLanguage() { return _lastLanguage; }
	
    public void SwitchLanguage(WhichLanguage w)
    {
        if(w == WhichLanguage.ENGLISH)
        {
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
        }
        else if(w == WhichLanguage.SPANISH)
        {
            if(_currentType == MenuType.MainMenu)
			{
                _leftButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Modo de Presentación";
                _middleButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Créditos";
                _rightButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Modo de Casa";
                _surveyButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Modo de Investigación";
            }
			else if(_currentType == MenuType.PauseMenu)
			{
                _leftButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Reanuda";
                _middleButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Créditos";
                _rightButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Reasume";
                _surveyButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Modo de Investigación";
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
				_leftButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Show Mode";
				_rightButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Home Mode";
				_middleButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Credits";
				_surveyButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Research Mode";
			}
			else if(_lastLanguage == WhichLanguage.SPANISH)
			{
                _leftButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Modo de Presentación";
                _middleButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Créditos";
                _rightButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Modo de Casa";
                _surveyButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Modo de Investigación";
			}
			_closeButton.SetActive(false);
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
                _surveyButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Modo de Investigación";
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
