using Firebase.RemoteConfig;
using UnityEngine;
using UnityEngine.UI;

public class SurveyFetcher : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string m_SurveyName = string.Empty;

    [Header("Dependencies")]
    [SerializeField] private SurveyVR m_Survey;
    [SerializeField] private TextAsset m_DefaultSurvey;
    [SerializeField] private Button m_StartButton;

    private string surveyString = string.Empty;

    private void Start()
    {
        m_StartButton.onClick.AddListener(InitSurvey);

        // Only try to fetch survey from remote config if Firebase is enabled
        if (PenguinAnalytics.FirebaseEnabled) FetchSurvey();
    }

    /// <summary>
    /// Fetch the survey with name `m_SurveyName` defined in the Firebase console and assign
    /// to `surveyString` field.
    /// </summary>
    private async void FetchSurvey()
    {
        await FirebaseRemoteConfig.DefaultInstance.FetchAndActivateAsync();
        surveyString = FirebaseRemoteConfig.DefaultInstance.GetValue(m_SurveyName).StringValue;
    }

    /// <summary>
    /// Activate the `SurveyVR` prefab and initialize with the fetched survey string.
    /// </summary>
    private void InitSurvey()
    {
        m_Survey.gameObject.SetActive(true);
        m_Survey.Initialize(surveyString, m_DefaultSurvey, new SurveyHandler());

        Destroy(m_StartButton.gameObject);
    }
}
