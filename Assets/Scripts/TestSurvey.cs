using System.Collections.Generic;
using FieldDay;
using UnityEngine;

public class TestSurvey : MonoBehaviour
{
    [SerializeField] private SurveyController m_Survey;
    [SerializeField] private TextAsset m_DefaultSurvey;

    private void Start()
    {
        m_Survey.Initialize(null, m_DefaultSurvey, new TestHandler());
    }
}

public class TestHandler : ISurveyHandler
{
    public void HandleSurveyResponse(Dictionary<string, string> surveyResponses)
    {
        Debug.Log("Survey submitted.");
    }
}
