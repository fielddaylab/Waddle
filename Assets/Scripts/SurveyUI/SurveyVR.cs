using BeauRoutine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FieldDay;
using BeauData;

public class SurveyVR : MonoBehaviour
{
    [Header("Prefabs and Components")]
    [SerializeField] private GameObject questionPrefab;
    [SerializeField] private GameObject questionHolder;
    [SerializeField] private GameObject progressPrefab;
    [SerializeField] private GameObject progressHolder;
    [SerializeField] private Button submitButton;
    private RectTransform questionHolderTransform;
    private int questionCount;
    [Header("Transition parameters")]
    [SerializeField] private float offset;
    private List<SurveyQuestion> questions = new List<SurveyQuestion>();
    private List<SurveyProgressNode> progressPoints = new List<SurveyProgressNode>();
    private int currentQuestion = 0;
    private int answerCount = 0;
// OpenGameData variables
    private SurveyData surveyData;
    private TextAsset defaultJSON;
    private ISurveyHandler surveyHandler;
    private Dictionary<string, string> answers = new Dictionary<string, string>();
    private float startTime;


    void Start()
    {
        foreach (var q in questions) {
            q.SetUnfocused();
        }
        
        if (questions.Count > 0) {
            questions[0].SetPrimaryFocus();
            progressPoints[0].ToggleActive(true);
            if (questions.Count > 1) {
                questions[1].SetSecondaryFocus();
                questions[1].NextButton.onClick.AddListener(OnNext);
                questions[1].SetInteraction(false, false);
            }
        }
        startTime = Time.time;
    }

    /// <summary>
    /// Should be called immediately after prefab instantiation,
    ///performs setup operations propogating the input survey into the question prefabs generated
    /// </summary>
    /// <param name="inSurveyName"> Name of the desired survey to fetch.</param>
    /// <param name="inDefaultJSON"> JSON represeting the survey to show in case survey fetch fails.</param>
    /// <param name="inSurveyHandler"> Survey Handler defining submit functionality of the survey.</param>
    public void Initialize(string inSurveyName, TextAsset inDefaultJSON, ISurveyHandler inSurveyHandler) {
        defaultJSON = inDefaultJSON;
        surveyHandler = inSurveyHandler;

        #if UNITY_EDITOR
        surveyData = ReadSurveyData(string.Empty);
        #else
        //Survey.FetchSurvey(inSurveyName);
        surveyData = ReadSurveyData(string.Empty);
        #endif

        questionCount = surveyData.Questions.Count;

        submitButton.onClick.AddListener(OnSubmit);
        submitButton.gameObject.SetActive(false);

        for (int i = 0; i < questionCount; i++) {
            SurveyQuestion question = Instantiate(questionPrefab, questionHolder.transform).GetComponent<SurveyQuestion>();
            question.Initialize(surveyData.Questions[i], this);
            questions.Add(question);
            progressPoints.Add(Instantiate(progressPrefab, progressHolder.transform).GetComponent<SurveyProgressNode>());
        }
    }

    private SurveyData ReadSurveyData(string inSurveyString)
    {
        SurveyData surveyData = null;

        if (inSurveyString.Equals(string.Empty))
        {
            surveyData = Serializer.Read<SurveyData>(defaultJSON);
        }
        else
        {
            surveyData = Serializer.Read<SurveyData>(inSurveyString);
        }

        return surveyData;
    }

    /// <summary>
    /// Called by secondary focus question to move one question forward in the list.
    /// </summary>
    public void OnNext() {
        if (currentQuestion + 1 < questions.Count) {

            questions[currentQuestion].NextButton.onClick.AddListener(OnPrev);
            questions[currentQuestion].SetSecondaryFocus();

            questions[currentQuestion + 1].NextButton.onClick.RemoveAllListeners();
            questions[currentQuestion + 1].SetPrimaryFocus();

            if (currentQuestion + 2 < questions.Count) {
                questions[currentQuestion + 2].NextButton.onClick.AddListener(OnNext);
                questions[currentQuestion + 2].SetSecondaryFocus();
                if (!questions[currentQuestion + 1].IsAnswered())
                    questions[currentQuestion + 2].SetInteraction(false, false);
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

    /// <summary>
    /// Called by secondary focus question to move one question back in the list.
    /// </summary>
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

    /// <summary>
    /// Performs tweening on the question holder between two positions
    /// </summary>
    /// <param name="start">Start position for the tween.</param>
    /// <param name="end">End position for the tween.</param>
    private void TweenQuestionPos(Vector2 start, Vector2 end) {
        Tween.Vector(start, end, (pos) => {((RectTransform)questionHolder.transform).anchoredPosition = pos; }, 0.7f).Ease(Curve.QuadOut).Play(this);
    }

    /// <summary>
    /// Called by toggles when value changes.
    /// </summary>
    /// <param name="val">Current value of the toggle.</param>
    public void OnToggleSelected(bool val) {
        if (val) {
            questions[currentQuestion].LogAnswer(ref answers);
            answerCount++;
            OnNext();
        } else 
            answerCount--;
    }

    /// <summary>
    /// Called by submit button on click, performs submission operations and calls HandleSurveyResponse.
    /// </summary>
    private void OnSubmit() {
        surveyHandler.HandleSurveyResponse(answers, Time.time - startTime);
        Destroy(gameObject);
    }
}
