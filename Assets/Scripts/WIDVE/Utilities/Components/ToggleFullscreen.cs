//Copyright WID Virtual Environments Group 2018-Present
//Authors:Simon Smith, Ross Tredinnick
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WIDVE.Utilities
{
    public class ToggleFullscreen : MonoBehaviour
    {
        [SerializeField]
        WindowSettings WindowSettings;

        [SerializeField]
        ButtonFloat Button;

		void Update()
		{
			if(Button.GetUp())
			{
				WindowSettings.ToggleWindowedMode();
			}
		}
	}
}