using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FieldDay;
using Firebase;
using Firebase.Analytics;
using Firebase.RemoteConfig;
using UnityEngine;
using UnityEngine.UI;

public class TestSurvey : MonoBehaviour
{
    [SerializeField] private SurveyVR m_Survey;
    [SerializeField] private TextAsset m_DefaultSurvey;
    [SerializeField] private Button startButton;

    private string survey = string.Empty;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
	        var dependencyStatus = task.Result;
		    if (dependencyStatus == DependencyStatus.Available) {
			    var firebaseApp = FirebaseApp.DefaultInstance;
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                FetchSurvey();
	        } else {
		        Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
	        }
		});

        startButton.onClick.AddListener(InitSurvey);
    }

    private void FetchSurvey()
    {
        Debug.Log("FETCHING...");
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAndActivateAsync();
        fetchTask.ContinueWith(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCompleted)
        {
            survey = FirebaseRemoteConfig.DefaultInstance.GetValue("survey_string").StringValue;
        }
    }

    private void InitSurvey()
    {
        m_Survey.gameObject.SetActive(true);
        m_Survey.Initialize(survey, m_DefaultSurvey, new TestHandler());
        Destroy(startButton.gameObject);
    }
}

public class TestHandler : ISurveyHandler
{
    private static readonly StringBuilder stringBuilder = new StringBuilder();

    public void HandleSurveyResponse(Dictionary<string, string> surveyResponses, float timeDelta = -1)
    {
        foreach (var pair in surveyResponses) {
            stringBuilder.AppendFormat("{0},", pair.Value);
        }

        stringBuilder.Length--;

        string responseString = stringBuilder.ToString();
        stringBuilder.Length = 0;

        FirebaseAnalytics.LogEvent("submit_survey", 
            new Parameter("responses", responseString),
            new Parameter("time", timeDelta));

        Debug.Log($"Survey submitted in {timeDelta} seconds. data is as follows: {responseString}");
    }
}
