using System.Collections.Generic;
using System.Text;
using FieldDay;
using Firebase;
using Firebase.Analytics;
using UnityEngine;

public class PenguinAnalytics : Singleton<PenguinAnalytics>
{
    private static int logVersion = 0;
    private static FirebaseApp firebaseApp = null;
    private static bool loggingEnabled = false;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
	        var dependencyStatus = task.Result;
		    if (dependencyStatus == DependencyStatus.Available) {
			    firebaseApp = FirebaseApp.DefaultInstance;
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                loggingEnabled = true;
                FirebaseAnalytics.LogEvent("start_game", 
                    new Parameter("log_version", logVersion));
	        } else {
		        Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
	        }
		});
    }

    public static void LogLoadMiniGame(PenguinGameManager.MiniGame minigame)
    {
        if (loggingEnabled)
        {
            FirebaseAnalytics.LogEvent("load_minigame",
            new Firebase.Analytics.Parameter("log_version", logVersion),
            new Firebase.Analytics.Parameter("minigame", minigame.ToString()));
        }
    }
}

public class PenguinSurveyHandler : ISurveyHandler
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
    }
}
