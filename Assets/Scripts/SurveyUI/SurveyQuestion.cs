using BeauRoutine;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SurveyQuestion : MonoBehaviour
{
    [SerializeField] private GameObject answerPrefab;
    [SerializeField] private GameObject answerHolder;
    private List<SurveyAnswer> answers = new List<SurveyAnswer>();
    [SerializeField] private int answerCount;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button button;
    public Button NextButton {
        get {
            return button;
        }
    }
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float primaryFocusScale;
    [SerializeField] private float secondaryFocusScale;
    [SerializeField] private float unfocusedScale;
    [SerializeField] private float primaryFocusAlpha;
    [SerializeField] private float secondaryFocusAlpha;
    [SerializeField] private float unfocusedAlpha;
    private SurveyController controller;

    // Start is called before the first frame update
    void Start() {
        for (int i = 0; i < answerCount; i++) {
            SurveyAnswer answer = Instantiate(answerPrefab, answerHolder.transform).GetComponent<SurveyAnswer>();
            answer.Toggle.onValueChanged.AddListener(controller.OnToggleSelected);
            answers.Add(answer);
        }
    }
    public void SetPrimaryFocus() {
        button.gameObject.SetActive(false);
        TweenFocus(primaryFocusAlpha, primaryFocusScale);
    }

    public void SetSecondaryFocus() {
        button.gameObject.SetActive(true);
        TweenFocus(secondaryFocusAlpha, secondaryFocusScale);
    }

    public void SetUnfocused() {
        button.gameObject.SetActive(false);
        TweenFocus(unfocusedAlpha, unfocusedScale);
    }

    private void TweenFocus(float alpha, float scale) {
        Tween.Float(canvasGroup.alpha, alpha, (f) => {canvasGroup.alpha = f;}, 0.7f).Play(this);
        Tween.Float(transform.localScale.x, scale, (f) => {transform.localScale = new Vector3(f, f, transform.localScale.z);}, 0.7f).Play(this);
    }

    public void SetController(SurveyController controller) {
        this.controller = controller;
    }

    public SurveyController GetController() {
        return controller;
    }
}
