using Firebase;
using Firebase.Analytics;
using UnityEngine;
//using FieldDay;

public class PenguinAnalytics : Singleton<PenguinAnalytics>
{
	public static bool FirebaseEnabled { get; set; }
    public static int logVersion = 0;
    
	static string _DB_NAME = "PENGUINS";
	
    float seconds_from_start = 0f;
	
	FieldDay.OGDLog _ogdLog;
	
	FieldDay.FirebaseConsts _firebase;

	[SerializeField]
	bool _loggingEnabled = true;
	
    private void Start()
    {
        // Try to initialize Firebase and fix dependencies (will always be false in editor)
        // If successful, set FirebaseEnabled flag to true allowing analytics to be sent
		/*Debug.Log("Initializing Firebase");
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
	        var dependencyStatus = task.Result;
		    if (dependencyStatus == DependencyStatus.Available) {
				Debug.Log("Firebase initialized...");
                FirebaseEnabled = true;
	        } else {
		        Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
	        }
		});*/
		
		_ogdLog = new FieldDay.OGDLog(_DB_NAME, UnityEngine.Application.version);
		_ogdLog.UseFirebase(_firebase);
        //_ogdLog.SetDebug(true);
    }

    #region Logging

	public void LogStartGame()
	{
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("start");
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
		/*if (FirebaseEnabled)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, new Parameter(FirebaseAnalytics.ParameterLevelName, "antarctica"), new Parameter("start", UnityEngine.Time.time-seconds_from_start));
                //new Parameter("app_version", logVersion));
        }	*/
	}
	
	public void LogEndGame()
	{
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("end");
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
		/*if (FirebaseEnabled)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, new Parameter(FirebaseAnalytics.ParameterLevelName, "antarctica"), new Parameter("start", UnityEngine.Time.time-seconds_from_start));
                //new Parameter("app_version", logVersion));
        }	*/
	}

    public void LogHeadsetOn()
    {
		if(_loggingEnabled)
		{
			_ogdLog.BeginEvent("headset_on");
			_ogdLog.SubmitEvent();
		}
        /*if(FirebaseEnabled)
        {
            seconds_from_start = UnityEngine.Time.time;
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventScreenView, FirebaseAnalytics.ParameterValue, "headset_on");
        }*/
    }
	
	public void LogHeadsetOff()
    {
		if(_loggingEnabled)
		{
			_ogdLog.BeginEvent("headset_off");
			_ogdLog.SubmitEvent();
		}
        /*if(FirebaseEnabled)
        {
            seconds_from_start = UnityEngine.Time.time;
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventScreenView, FirebaseAnalytics.ParameterValue, "headset_on");
        }*/
    }


    public void LogLanguageSelected(string language)
    {
		if(_loggingEnabled)
		{
			_ogdLog.BeginEvent("language_selected");
			_ogdLog.EventParam("language", language);
			_ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
			_ogdLog.SubmitEvent();
		}
        /*if(FirebaseEnabled)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventSelectItem, new Parameter("language_selected", language), new Parameter("seconds_from_start", UnityEngine.Time.time-seconds_from_start));
        }*/
    }

    public void LogObjectAssigned(string obj)
    {
		if(_loggingEnabled)
		{
			_ogdLog.BeginEvent("object_assigned");
			_ogdLog.EventParam("object", obj);
			_ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
			_ogdLog.SubmitEvent();
        }
        /*if(FirebaseEnabled)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventSelectItem, new Parameter("object_assigned", obj), new Parameter("seconds_from_start", UnityEngine.Time.time-seconds_from_start));
        }*/
    }
	
	public void LogOpenMenu()
	{
		if(_loggingEnabled)
		{
			_ogdLog.BeginEvent("open_menu");
			_ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
			_ogdLog.SubmitEvent();
		}
	}
	
	public void LogCloseMenu()
	{
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("close_menu");
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
	}
	
	public void LogSelectMenu(string item)
	{
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("select_menu_item");
            _ogdLog.EventParam("item", item);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
	}
	
    public void LogObjectSelected(string obj)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("object_selected");
            _ogdLog.EventParam("gaze_point_name", obj);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
       /* if(FirebaseEnabled)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventSelectItem, new Parameter("gaze_point_name", obj), new Parameter("seconds_from_start", UnityEngine.Time.time-seconds_from_start));
        }*/
    }

    public void LogSceneChanged(string scene)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("scene_change");
            _ogdLog.EventParam("scene_name", scene);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }

        /*if (FirebaseEnabled)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, new Parameter(FirebaseAnalytics.ParameterLevelName, scene), new Parameter("seconds_from_start", UnityEngine.Time.time-seconds_from_start));
                //new Parameter("app_version", logVersion));
        }*/	
    }

    public void LogAudioStarted(string clip)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("script_audio_started");
            //_ogdLog.EventParam("clip_identifier", clip);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogAudioComplete(string clip)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("script_audio_complete");
            //_ogdLog.EventParam("clip_identifier", clip);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogCaption(string caption)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("script_audio_started");
            _ogdLog.EventParam("caption", caption);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogObjectDisplayed(bool hasIndicator, string obj, Vector3 pos, Quaternion rot)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("new_object_displayed");
            _ogdLog.EventParam("has_the_indicator", hasIndicator);
            _ogdLog.EventParam("object", obj);
            _ogdLog.EventParam("posX", pos.x);
            _ogdLog.EventParam("posY", pos.y);
            _ogdLog.EventParam("posZ", pos.z);
            _ogdLog.EventParam("rotX", rot.x);
            _ogdLog.EventParam("rotY", rot.y);
            _ogdLog.EventParam("rotZ", rot.z);
            _ogdLog.EventParam("rotW", rot.w);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogEatFish(Vector3 pos, Quaternion rot)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("eat_fish");
            _ogdLog.EventParam("posX", pos.x);
            _ogdLog.EventParam("posY", pos.y);
            _ogdLog.EventParam("posZ", pos.z);
            _ogdLog.EventParam("rotX", rot.x);
            _ogdLog.EventParam("rotY", rot.y);
            _ogdLog.EventParam("rotZ", rot.z);
            _ogdLog.EventParam("rotW", rot.w);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogPickupRock(Vector3 pos, Quaternion rot, int howMany)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("pickup_rock");
            _ogdLog.EventParam("posX", pos.x);
            _ogdLog.EventParam("posY", pos.y);
            _ogdLog.EventParam("posZ", pos.z);
            _ogdLog.EventParam("rotX", rot.x);
            _ogdLog.EventParam("rotY", rot.y);
            _ogdLog.EventParam("rotZ", rot.z);
            _ogdLog.EventParam("rotW", rot.w);
            _ogdLog.EventParam("total_picked_up", howMany);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogPushSnowball(Vector3 pos, Quaternion rot)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("push_snowball");
            _ogdLog.EventParam("posX", pos.x);
            _ogdLog.EventParam("posY", pos.y);
            _ogdLog.EventParam("posZ", pos.z);
            _ogdLog.EventParam("rotX", rot.x);
            _ogdLog.EventParam("rotY", rot.y);
            _ogdLog.EventParam("rotZ", rot.z);
            _ogdLog.EventParam("rotW", rot.w);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogChimes(int whichChime)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("ring_chime");
            _ogdLog.EventParam("note_played", whichChime);
            _ogdLog.SubmitEvent();
        }
    }
	
	public void LogBubblePop(int bubbleID, float timing)
	{
		if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("bubble_pop");
            _ogdLog.EventParam("object_id", bubbleID);
			_ogdLog.EventParam("timing", timing);
            _ogdLog.SubmitEvent();
        }
	}

    #endregion // Logging
}
