using BeauRoutine;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SurveyQuestion : MonoBehaviour
{
    [Header("Prefabs and Components")]
    [SerializeField] private GameObject answerPrefab;
    [SerializeField] private GameObject answerHolder;
    private List<SurveyAnswer> answers = new List<SurveyAnswer>();
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button button;
    public Button NextButton {
        get {
            return button;
        }
    }
    [SerializeField] private CanvasGroup canvasGroup;
    [Header("Transition parameters")]
    [SerializeField] private float primaryFocusScale;
    [SerializeField] private float secondaryFocusScale;
    [SerializeField] private float unfocusedScale;
    [SerializeField] private float primaryFocusAlpha;
    [SerializeField] private float secondaryFocusAlpha;
    [SerializeField] private float unfocusedAlpha;
    [SerializeField] private float primaryFocusZ;
    [SerializeField] private float secondaryFocusZ;
    [SerializeField] private float unfocusedZ;
    
    
    // OpenGameData variables
    private string currentText;

    /// <summary>
    /// Called on creation internally, performs setup for question prefab.
    /// </summary>
    /// <param name="question"> OpenGameData survey question that this prefab will be asking.</param>
    /// <param name="controller"> SurveyVR component which owns this question.</param>
    public void Initialize(FieldDay.SurveyQuestion question, SurveyVR controller) {
        for (int i = 0; i < question.Answers.Count; i++) {
            SurveyAnswer answer = Instantiate(answerPrefab, answerHolder.transform).GetComponent<SurveyAnswer>();
            answer.Initialize(question.Answers[i]);
            answer.Toggle.onValueChanged.AddListener(controller.OnToggleSelected);
            answers.Add(answer);
        }
        SetText(question.Text);
        currentText = question.Text;
    }

    /// <summary>
    /// Sets this question as the primary focus and adjusts interactables and visuals accordingly.
    /// </summary>
    public void SetPrimaryFocus() {
        SetInteraction(false, true);
        TweenFocus(primaryFocusAlpha, primaryFocusScale, primaryFocusZ);
    }

    /// <summary>
    /// Sets this question as a secondary focus and adjusts interactables and visuals accordingly.
    /// </summary>
    public void SetSecondaryFocus() {
        SetInteraction(true, false);
        TweenFocus(secondaryFocusAlpha, secondaryFocusScale, secondaryFocusZ);
    }

    /// <summary>
    /// Sets this question as unfocused and adjusts interactables and visuals accordingly.
    /// </summary>
    public void SetUnfocused() {
        SetInteraction(false, false);
        TweenFocus(unfocusedAlpha, unfocusedScale, unfocusedZ);
    }

    /// <summary>
    /// Performs tweening operations for when the question changes its focus state.
    /// </summary>
    /// <param name="alpha"> alpha value to tween to from current alpha.</param>
    /// <param name="scale"> x/y scale to tween to from current scale.</param>
    /// <param name="zPos"> z position to tween to from current z position .</param>
    private void TweenFocus(float alpha, float scale, float zPos) {
        Tween.Float(canvasGroup.alpha, alpha, (f) => {canvasGroup.alpha = f;}, 0.7f).Play(this);
        Tween.Float(transform.localScale.x, scale, (f) => {transform.localScale = new Vector3(f, f, transform.localScale.z);}, 0.7f).Play(this);
        Tween.Float(transform.localPosition.z, zPos, (f) => {transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, f);}, 0.7f).Ease(Curve.CircleOut).Play(this);
    }

    /// <summary>
    /// Sets the text of this question
    /// </summary>
    /// <param name="text"> String value to set the answer text to.</param>
    private void SetText(string text) {
        questionText.text = text;
    }

    /// <summary>
    /// Records the currently selected answer in a given dictionary
    /// </summary>
    /// <param name="allAnswers"> Dictionary containing a space for all of the answers for this survey.</param>
    public void LogAnswer(ref Dictionary<string, string> allAnswers) {
        string text;
        foreach (var ans in answers) {
            if ((text = ans.TryGetSelected()) != null) {
                allAnswers[currentText] = text;
                break;
            }
        }
    }

    /// <summary>
    /// Checks if this question is currently answered.
    /// </summary>
    /// <returns> True if any one of the toggles for this question is true, false otherwise.</returns>
    public bool IsAnswered() {
        foreach (var ans in answers) {
            if (ans.TryGetSelected() != null) {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Adjusts which interactable components of a question are enabled/disabled.
    /// </summary>
    /// <param name="button"> Should the button component of this question be enabled?</param>
    /// <param name="toggle"> Should the toggle components of this question be enabled?</param>
    public void SetInteraction(bool button, bool toggle) {
        foreach(var ans in answers) {
            ans.Toggle.interactable = toggle;
        }
        this.button.gameObject.SetActive(button);
    }
}
