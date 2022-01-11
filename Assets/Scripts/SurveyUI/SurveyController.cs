using BeauRoutine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FieldDay;

public class SurveyController : MonoBehaviour
{
    [SerializeField] private GameObject questionPrefab;
    [SerializeField] private GameObject questionHolder;
    [SerializeField] private GameObject progressPrefab;
    [SerializeField] private GameObject progressHolder;
    [SerializeField] private Button submitButton;
    private RectTransform questionHolderTransform;
    [SerializeField] private int questionCount;
    [SerializeField] private float offset;
    private List<SurveyQuestion> questions = new List<SurveyQuestion>();
    private List<SurveyProgressNode> progressPoints = new List<SurveyProgressNode>();
    private int currentQuestion = 0;
    private int answerCount = 0;
    void Start()
    {
        submitButton.gameObject.SetActive(false);

        for (int i = 0; i < questionCount; i++) {
            SurveyQuestion question = Instantiate(questionPrefab, questionHolder.transform).GetComponent<SurveyQuestion>();
            question.SetController(this);
            questions.Add(question);
            progressPoints.Add(Instantiate(progressPrefab, progressHolder.transform).GetComponent<SurveyProgressNode>());
        }

        foreach (var q in questions) {
            q.SetUnfocused();
        }
        if (questions.Count > 0) {
            questions[0].SetPrimaryFocus();
            progressPoints[0].ToggleActive(true);
            if (questions.Count > 1) {
                questions[1].SetSecondaryFocus();
                questions[1].NextButton.onClick.AddListener(OnNext);
            }
        }
    }

    public void Initialize(string inSurveyName, TextAsset inDefaultJSON, ISurveyHandler inSurveyHandler) {
    
    }

    public void OnNext() {
        if (currentQuestion + 1 < questions.Count) {

            questions[currentQuestion].NextButton.onClick.AddListener(OnPrev);
            questions[currentQuestion].SetSecondaryFocus();

            questions[currentQuestion + 1].NextButton.onClick.RemoveAllListeners();
            questions[currentQuestion + 1].SetPrimaryFocus();

            if (currentQuestion + 2 < questions.Count) {
                questions[currentQuestion + 2].NextButton.onClick.AddListener(OnNext);
                questions[currentQuestion + 2].SetSecondaryFocus();
            }
            if (currentQuestion - 1 >= 0) {
                questions[currentQuestion - 1].NextButton.onClick.RemoveAllListeners();
                questions[currentQuestion - 1].SetUnfocused();
            }

            Vector2 start = ((RectTransform)questionHolder.transform).anchoredPosition;
            Vector2 end = start + new Vector2(0, offset);
            TweenQuestionPos(start, end);

            progressPoints[currentQuestion].ToggleActive(false);
            currentQuestion++;
            progressPoints[currentQuestion].ToggleActive(true);

            if (currentQuestion == questions.Count - 1 && answerCount >= questions.Count)
                submitButton.gameObject.SetActive(true);
        } else if (answerCount >= questions.Count)
            submitButton.gameObject.SetActive(true);
    }

    public void OnPrev() {
        if (currentQuestion > 0) {
            questions[currentQuestion].NextButton.onClick.AddListener(OnNext);
            questions[currentQuestion].SetSecondaryFocus();

            questions[currentQuestion - 1].NextButton.onClick.RemoveAllListeners();
            questions[currentQuestion - 1].SetPrimaryFocus();

            if (currentQuestion - 2 >= 0) {
                questions[currentQuestion - 2].NextButton.onClick.AddListener(OnPrev);
                questions[currentQuestion - 2].SetSecondaryFocus();
            }
            if (currentQuestion + 1 < questions.Count) {
                questions[currentQuestion + 1].NextButton.onClick.RemoveAllListeners();
                questions[currentQuestion + 1].SetUnfocused();
            }

            Vector2 start = ((RectTransform)questionHolder.transform).anchoredPosition;
            Vector2 end = start - new Vector2(0, offset);
            TweenQuestionPos(start, end);

            progressPoints[currentQuestion].ToggleActive(false);
            currentQuestion--;
            progressPoints[currentQuestion].ToggleActive(true);
        }
        submitButton.gameObject.SetActive(false);
    }

    private void TweenQuestionPos(Vector2 start, Vector2 end) {
        Tween.Vector(start, end, (pos) => {((RectTransform)questionHolder.transform).anchoredPosition = pos; }, 0.7f).Ease(Curve.QuadOut).Play(this);
    }

    public void OnToggleSelected(bool val) {
        if (val) {
            answerCount++;
            OnNext();
        } else 
            answerCount--;
    }
}
