//Copyright WID Virtual Environments Group 2018-Present
//Authors:Simon Smith, Ross Tredinnick
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WIDVE.Utilities
{
    public class OnSelect : MonoBehaviour, ISelectable
    {
        [SerializeField]
        UnityEvent _onSelectEvent;
        UnityEvent OnSelectEvent => _onSelectEvent;

        public void Select(Selector selector)
        {
            OnSelectEvent?.Invoke();
        }
    }
}