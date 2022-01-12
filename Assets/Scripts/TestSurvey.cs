using System.Collections.Generic;
using FieldDay;
using UnityEngine;

public class TestSurvey : MonoBehaviour
{
    [SerializeField] private SurveyVR m_Survey;
    [SerializeField] private TextAsset m_DefaultSurvey;

    private void Start()
    {
        m_Survey.Initialize(null, m_DefaultSurvey, new TestHandler());
    }
}

public class TestHandler : ISurveyHandler
{
    public void HandleSurveyResponse(Dictionary<string, string> surveyResponses, float timeDelta = -1)
    {
        string stringData = "";
        foreach (var pair in surveyResponses) {
            stringData += $"\n{pair.Key}, {pair.Value}";
        }
        Debug.Log($"Survey submitted in {timeDelta} seconds. data is as follows: {stringData}");
    }
}
