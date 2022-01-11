using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SurveyAnswer : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private TextMeshProUGUI answerText;
    public Toggle Toggle {
        get {return toggle;}
    }

    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent.TryGetComponent<ToggleGroup>(out ToggleGroup group)) {
            toggle.group = group;
        }
    }

    public void SetText(string text) {
        answerText.text = text;
    }
}
