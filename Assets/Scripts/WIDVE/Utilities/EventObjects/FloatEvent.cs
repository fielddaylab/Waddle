//Copyright WID Virtual Environments Group 2018-Present
//Authors:Simon Smith, Ross Tredinnick
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WIDVE.Utilities
{
	[CreateAssetMenu(fileName = nameof(FloatEvent), menuName = WIDVEEditor.MENU + "/" + nameof(FloatEvent), order = WIDVEEditor.C_ORDER)]
	public class FloatEvent : ScriptableEvent<float> { }
}