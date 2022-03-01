using Firebase;
using Firebase.Analytics;
using UnityEngine;

public class PenguinAnalytics : Singleton<PenguinAnalytics>
{
    public static bool FirebaseEnabled { get; set; }
    public static int logVersion = 0;
    
    private void Start()
    {
        // Try to initialize Firebase and fix dependencies (will always be false in editor)
        // If successful, set FirebaseEnabled flag to true allowing analytics to be sent
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
	        var dependencyStatus = task.Result;
		    if (dependencyStatus == DependencyStatus.Available) {
                FirebaseEnabled = true;
	        } else {
		        Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
	        }
		});
    }

    #region Logging

    public static void LogLoadMiniGame(PenguinGameManager.MiniGame minigame)
    {
        if (FirebaseEnabled)
        {
            FirebaseAnalytics.LogEvent("load_minigame",
                new Parameter("log_version", logVersion),
                new Parameter("minigame", minigame.ToString()));
        }
    }

    #endregion // Logging
}
