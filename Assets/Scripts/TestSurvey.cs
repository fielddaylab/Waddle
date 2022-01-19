using System.Collections.Generic;
using FieldDay;
using Firebase.Analytics;
using UnityEngine;

public class TestSurvey : MonoBehaviour
{
    [SerializeField] private Survey m_Survey;
    [SerializeField] private TextAsset m_DefaultSurvey;

    private void Start()
    {
        m_Survey.Initialize(null, m_DefaultSurvey, new TestHandler());

        // Test Firebase logging
        FirebaseAnalytics.LogEvent("start");
    }
}

public class TestHandler : ISurveyHandler
{
    public void HandleSurveyResponse(Dictionary<string, string> surveyResponses)
    {
        Debug.Log("Survey submitted.");
    }
}
