using System.Collections.Generic;
using System.Text;
using FieldDay;
using Firebase.Analytics;

public class SurveyHandler : ISurveyHandler
{
    private readonly StringBuilder stringBuilder = new StringBuilder();

    public void HandleSurveyResponse(Dictionary<string, string> surveyResponses, float timeDelta = -1)
    {
        if (!PenguinAnalytics.FirebaseEnabled) return;

        foreach (var pair in surveyResponses) {
            stringBuilder.AppendFormat("{0}:{1},", pair.Key, pair.Value);
        }

        stringBuilder.Length--;

        string responseString = stringBuilder.ToString();
        stringBuilder.Length = 0;

        FirebaseAnalytics.LogEvent("submit_survey", 
            new Parameter("log_version", PenguinAnalytics.logVersion),
            new Parameter("responses", responseString),
            new Parameter("time", timeDelta));
    }
}
