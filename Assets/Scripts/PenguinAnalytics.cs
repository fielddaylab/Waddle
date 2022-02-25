using System.Collections.Generic;
using FieldDay;
using Firebase.Analytics;

public class PenguinAnalytics : Singleton<PenguinAnalytics>
{
    private static int m_LogVersion = 0;

    public static void LogStartGame()
    {
        FirebaseAnalytics.LogEvent("start_game", 
            new Firebase.Analytics.Parameter("app_version", m_LogVersion));
    }

    public static void LogLoadMiniGame(PenguinGameManager.MiniGame minigame)
    {
        FirebaseAnalytics.LogEvent("load_minigame",
            new Firebase.Analytics.Parameter("app_version", m_LogVersion),
            new Firebase.Analytics.Parameter("minigame", minigame.ToString()));
    }
}

public class PenguinSurveyHandler : ISurveyHandler
{
    public void HandleSurveyResponse(Dictionary<string, string> surveyResponses, float timedelta = -1)
    {
        foreach (KeyValuePair<string, string> kvp in surveyResponses)
        {
            FirebaseAnalytics.LogEvent($"survey_question_{kvp.Key}",
                new Firebase.Analytics.Parameter("response", kvp.Value));
        }
    }
}
