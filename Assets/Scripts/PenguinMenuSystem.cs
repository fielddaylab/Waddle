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
	
	MenuType _currentType = MenuType.MainMenu;
	
	Vector3 _menuOffset = new Vector3(0.0f, 0.5f, 0.0f);
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = PenguinPlayer.Instance.transform.position + PenguinPlayer.Instance.transform.forward * 0.01f + _menuOffset;
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
			_titleText.GetComponent<TMPro.TextMeshPro>().text = "Penguins VR";
			_leftButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Show Mode";
			_rightButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Home Mode";
			_closeButton.SetActive(false);
		}
		else if(menu == MenuType.PauseMenu)
		{
			_titleText.GetComponent<TMPro.TextMeshPro>().text = "Options";
			_leftButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Restart";
			_rightButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Resume";
			_closeButton.SetActive(true);
		}
		else if(menu == MenuType.EndMenu)
		{
			_titleText.GetComponent<TMPro.TextMeshPro>().text = "You did it!\nYou hatched a chick!";
			_leftButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Keep Playing";
			_rightButton.transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = "Restart";
			_closeButton.SetActive(false);
		}
	}
	
}
