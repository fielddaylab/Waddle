using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurveyProgressNode : MonoBehaviour
{
    [SerializeField] private Image active;
    [SerializeField] private Image inactive;
    public void ToggleActive(bool val) {
        if (val) {
            active.enabled = true;
            inactive.enabled = false;
        } else {
            active.enabled = false;
            inactive.enabled = true;
        }
    }
}
