using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SurveyAnswer : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private TextMeshProUGUI answerText;
    private string text;
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

    /// <summary>
    /// Called on creation internally, performs setup for answer prefab.
    /// </summary>
    /// <param name="text"> String value to set the answer text to.</param>
    public void Initialize(string text) {
        SetText(text);
    }

    /// <summary>
    /// Sets the text of this answer.
    /// </summary>
    /// <param name="text"> String value to set the answer text to.</param>
    public void SetText(string text) {
        answerText.text = text;
        this.text = text;
    }

    /// <summary>
    /// Tries to get the text of thhis answer if it is currently selected.
    /// </summary>
    /// <returns> A string representing the text of this answer, or null if not selected.</returns>
    public string TryGetSelected() {
        if (toggle.isOn)
            return text;
        return null;
    }
}
