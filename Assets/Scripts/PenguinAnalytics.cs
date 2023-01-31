using Firebase;
using Firebase.Analytics;
using UnityEngine;
//using FieldDay;

[System.Serializable]
public class LogPosition
{
    public float xPos;
    public float yPos;
    public float zPos;
}

[System.Serializable]
public class LogRotation
{
    public float xRot;
    public float yRot;
    public float zRot;
    public float wRot;
}

[System.Serializable]
public class LogGazeData
{
    public uint frame;
    public LogPosition pos;
    public LogRotation rot;
}

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
	
    int _numPebblesCollected = 0;

    int _viewportDataCount = 0;
    const int MAX_VIEWPORT_DATA = 72;
    LogGazeData[] _viewportData = new LogGazeData[MAX_VIEWPORT_DATA];

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
		
        Debug.Log("Starting analytics");
		_ogdLog = new FieldDay.OGDLog(_DB_NAME, UnityEngine.Application.version);
		_ogdLog.UseFirebase(_firebase);
        //_ogdLog.SetDebug(true);
    }

    private void LogGazeGameState()
    {
        Vector3 pos = Vector3.zero;
        Quaternion quat = Quaternion.identity;
        PenguinPlayer.Instance.GetGaze(out pos, out quat);
        _ogdLog.GameStateParam("posX", pos.x);
        _ogdLog.GameStateParam("posY", pos.y);
        _ogdLog.GameStateParam("posZ", pos.z);
        _ogdLog.GameStateParam("rotX", quat.x);
        _ogdLog.GameStateParam("rotY", quat.y);
        _ogdLog.GameStateParam("rotZ", quat.z);
        _ogdLog.GameStateParam("rotW", quat.w);
    }

    #region Logging

	public void LogSurveyCode(int code)
    {
		if(_loggingEnabled)
		{
			_ogdLog.BeginEvent("survey_code");
			_ogdLog.EventParam("survey_code_number", code);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitGameState();
		}
	}
	
	public void LogApplicationStart()
	{
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("application_start");
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitGameState();
        }
		/*if (FirebaseEnabled)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, new Parameter(FirebaseAnalytics.ParameterLevelName, "antarctica"), new Parameter("start", UnityEngine.Time.time-seconds_from_start));
                //new Parameter("app_version", logVersion));
        }	*/
	}
	

	public void LogStartGame()
	{
        if(_loggingEnabled)
		{
            _numPebblesCollected = 0;

            _ogdLog.BeginEvent("start");
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitGameState();
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
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitGameState();
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

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitGameState();
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

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitGameState();
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
			_ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitGameState();
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
			_ogdLog.SubmitEvent();


            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitGameState();
        }
        /*if(FirebaseEnabled)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventSelectItem, new Parameter("object_assigned", obj), new Parameter("seconds_from_start", UnityEngine.Time.time-seconds_from_start));
        }*/
    }
	
    public void LogBeginMode(string mode)
	{
		if(_loggingEnabled)
		{
			_ogdLog.BeginEvent("begin");
            _ogdLog.EventParam("mode", mode);
			_ogdLog.SubmitEvent();


            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitGameState();
		}
	}

	public void LogOpenMenu()
	{
		if(_loggingEnabled)
		{
			_ogdLog.BeginEvent("open_menu");
			_ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
		}
	}
	
	public void LogCloseMenu()
	{
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("close_menu");
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
	}
	
	public void LogSelectMenu(string item)
	{
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("select_menu_item");
            _ogdLog.EventParam("item", item);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
	}
	
    public void LogObjectSelected(string obj)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("object_selected");
            _ogdLog.EventParam("gaze_point_name", obj);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
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
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.GameStateParam("scene_name", scene);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
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
            _ogdLog.EventParam("clip_identifier", clip);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogAudioComplete(string clip)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("script_audio_complete");
            _ogdLog.EventParam("clip_identifier", clip);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogCaption(string caption)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("script_audio_started");
            _ogdLog.EventParam("caption", caption);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
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
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogEatFish(Vector3 pos, Quaternion rot)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("eat_fish");
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogPickupRock(Vector3 pos, Quaternion rot)
    {
        if(_loggingEnabled)
		{
            _numPebblesCollected++;

            _ogdLog.BeginEvent("pickup_rock");
            _ogdLog.EventParam("total_picked_up", _numPebblesCollected);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }


    public void LogPushSnowball(Vector3 pos, Quaternion rot)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("push_snowball");
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogNestComplete()
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("nest_complete");
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogMenuAppeared()
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("menu_appeared");
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogChimes(string whichChime)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("ring_chime");
            _ogdLog.EventParam("note_played", whichChime);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }
	
	public void LogBubblePop(int bubbleID, float timing)
	{
		if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("bubble_pop");
            _ogdLog.EventParam("object_id", bubbleID);
			_ogdLog.EventParam("timing_error", timing);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
	}
    
    public void LogBubbleAppeared(int bubbleID, Vector3 pos)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("bubble_appeared");
            _ogdLog.EventParam("object_id", bubbleID);
            _ogdLog.EventParam("posX", pos.x);
            _ogdLog.EventParam("posY", pos.y);
            _ogdLog.EventParam("posZ", pos.z);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogBubbleDisappeared(int bubbleID)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("bubble_expired");
            _ogdLog.EventParam("object_id", bubbleID);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogPinFell()
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("penguin_pin_fell");
            _ogdLog.SubmitEvent();
            
            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogFlipperBash(string skuaID, bool right)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("flipper_bash_skua");
            _ogdLog.EventParam("object_id", skuaID);
			//_ogdLog.EventParam("rightFlipper", right);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogSkuaSpawned(string skuaID, Vector3 pos)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("skua_spawn");
            _ogdLog.EventParam("object_id", skuaID);
            _ogdLog.EventParam("posX", pos.x);
            _ogdLog.EventParam("posY", pos.y);
            _ogdLog.EventParam("posZ", pos.z);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogSkuaMove(string skuaID, Vector3 pos, Vector3 toPos)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("skua_move");
            _ogdLog.EventParam("object_id", skuaID);
            _ogdLog.EventParam("from_posX", pos.x);
            _ogdLog.EventParam("from_posY", pos.y);
            _ogdLog.EventParam("from_posZ", pos.z);
            _ogdLog.EventParam("to_posX", toPos.x);
            _ogdLog.EventParam("to_posY", toPos.y);
            _ogdLog.EventParam("to_posZ", toPos.z);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogMatingDanceIndicator(float percent)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("mating_dance_indicator_updated");
            _ogdLog.EventParam("percent_full", percent);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogEggTimer(float timeRemaining)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("egg_hatch_indicator_updated");
            _ogdLog.EventParam("time_remaining", timeRemaining);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogEggHatched()
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("egg_hatched");
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }
    
    public void LogEggReturn()
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("egg_recovered");
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }


    public void LogEggLost(string whichSkua)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("egg_lost");
            _ogdLog.EventParam("object_id", whichSkua);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogBeginMode(int mode)
    {
        if(_loggingEnabled)
		{
            //0 = Show Mode, 1 = Home mode, 2 = research mode
            _ogdLog.BeginEvent("begin_mode");
            _ogdLog.EventParam("mode", mode);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogEnterRegion(string region_name)
    {
        if(_loggingEnabled)
		{
            //0 = Show Mode, 1 = Home mode
            _ogdLog.BeginEvent("enter_region");
            _ogdLog.EventParam("region_name", region_name);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogExitRegion(string region_name)
    {
        if(_loggingEnabled)
		{
            //0 = Show Mode, 1 = Home mode
            _ogdLog.BeginEvent("exit_region");
            _ogdLog.EventParam("region_name", region_name);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogActivityBegin(string activity)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("activity_begin");
            _ogdLog.EventParam("activity_name", activity);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogActivityEnd(string activity)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("activity_end");
            _ogdLog.EventParam("activity_name", activity);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogTimerBegin(float timeLength)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("global_timer_begin");
            _ogdLog.EventParam("time_remaining", timeLength);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogTimerPause(float timeLength)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("global_timer_pause");
            _ogdLog.EventParam("time_remaining", timeLength);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogTimerUnpause(float timeLength)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("global_timer_unpause");
            _ogdLog.EventParam("time_remaining", timeLength);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogTimerExpired()
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("global_timer_expired");
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogMove(Vector3 oldPos, Vector3 pos, Quaternion gaze, int object_id)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("player_waddle");
            _ogdLog.EventParam("object_id", object_id);     //left or right waddle (0 or 1)
            _ogdLog.EventParam("old_posX", oldPos.x);
            _ogdLog.EventParam("old_posY", oldPos.y);
            _ogdLog.EventParam("old_posZ", oldPos.z);
            _ogdLog.EventParam("posX", pos.x);
            _ogdLog.EventParam("posY", pos.y);
            _ogdLog.EventParam("posZ", pos.z);
            _ogdLog.EventParam("rotX", gaze.x);
            _ogdLog.EventParam("rotY", gaze.y);
            _ogdLog.EventParam("rotZ", gaze.z);
            _ogdLog.EventParam("rotW", gaze.w);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitGameState();
        }
    }

	public void LogGaze(Vector3 p, Quaternion q, uint gazeLogFrameCount, bool sendToServer=false)//, string scene)
	{
		if(_loggingEnabled)
		{
			if(_viewportDataCount < MAX_VIEWPORT_DATA)
			{
				if(_viewportData[_viewportDataCount] == null)
				{
					_viewportData[_viewportDataCount] = new LogGazeData();
					_viewportData[_viewportDataCount].pos = new LogPosition();
					_viewportData[_viewportDataCount].rot = new LogRotation();
				}

				_viewportData[_viewportDataCount].pos.xPos = p.x;
				_viewportData[_viewportDataCount].pos.yPos = p.y;
				_viewportData[_viewportDataCount].pos.zPos = p.z;

				_viewportData[_viewportDataCount].rot.xRot = q.x;
				_viewportData[_viewportDataCount].rot.yRot = q.y;
				_viewportData[_viewportDataCount].rot.zRot = q.z;
				_viewportData[_viewportDataCount].rot.wRot = q.w;

				_viewportData[_viewportDataCount].frame = gazeLogFrameCount;
				_viewportDataCount++;
			}

            if(sendToServer)
            {
                string gazeData = "";
                for(int i = 0; i < _viewportDataCount; ++i)
                {
                    gazeData += JsonUtility.ToJson(_viewportData[i]);
                }

                //Debug.Log(gazeData);
                _ogdLog.BeginEvent("viewport_data");
                _ogdLog.EventParam("gaze_data_package", gazeData);
                _ogdLog.SubmitEvent();

                _ogdLog.BeginGameState();
                _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
                LogGazeGameState();
                _ogdLog.SubmitGameState();

                _viewportDataCount = 0;
            }
		}

		/*if (FirebaseEnabled)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, new Parameter(FirebaseAnalytics.ParameterLevelName, "antarctica"), new Parameter("start", UnityEngine.Time.time-seconds_from_start));
                //new Parameter("app_version", logVersion));
        }	*/
	}

    public void LogGazeBegin(string object_id)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("gaze_object_begin");
            _ogdLog.EventParam("object_id", object_id);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    public void LogGazeEnd(string object_id)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("gaze_object_end");
            _ogdLog.EventParam("object_id", object_id);
            _ogdLog.SubmitEvent();

            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            LogGazeGameState();
            _ogdLog.SubmitGameState();
        }
    }

    #endregion // Logging
}
