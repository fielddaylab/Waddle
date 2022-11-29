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
	
    int _numPebblesCollected = 0;

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

	public void LogApplicationStart()
	{
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("application_start");
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
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
	
    public void LogBeginMode(string mode)
	{
		if(_loggingEnabled)
		{
			_ogdLog.BeginEvent("begin");
            _ogdLog.EventParam("mode", mode);
			_ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
			_ogdLog.SubmitEvent();
		}
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
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogPickupRock(Vector3 pos, Quaternion rot)
    {
        if(_loggingEnabled)
		{
            _numPebblesCollected++;

            _ogdLog.BeginEvent("pickup_rock");
            _ogdLog.EventParam("posX", pos.x);
            _ogdLog.EventParam("posY", pos.y);
            _ogdLog.EventParam("posZ", pos.z);
            _ogdLog.EventParam("rotX", rot.x);
            _ogdLog.EventParam("rotY", rot.y);
            _ogdLog.EventParam("rotZ", rot.z);
            _ogdLog.EventParam("rotW", rot.w);
            _ogdLog.EventParam("total_picked_up", _numPebblesCollected);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
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
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogNestComplete()
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("nest_complete");
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogMenuAppeared()
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("menu_appeared");
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogChimes(string whichChime)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("ring_chime");
            _ogdLog.EventParam("note_played", whichChime);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }
	
	public void LogBubblePop(string bubbleID, float timing)
	{
		if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("bubble_pop");
            _ogdLog.EventParam("object_id", bubbleID);
			_ogdLog.EventParam("timing", timing);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
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
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogBubbleDisappeared(int bubbleID)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("bubble_expired");
            _ogdLog.EventParam("object_id", bubbleID);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogPinFell()
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("penguin_pin_fell");
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogFlipperBash(int skuaID, bool right)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("flipper_bash_skua");
            _ogdLog.EventParam("object_id", skuaID);
			_ogdLog.EventParam("rightFlipper", right);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogSkuaSpawned(int skuaID, Vector3 pos)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("skua_spawn");
            _ogdLog.EventParam("object_id", skuaID);
            _ogdLog.EventParam("posX", pos.x);
            _ogdLog.EventParam("posY", pos.y);
            _ogdLog.EventParam("posZ", pos.z);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogSkuaMove(string skuaID, Vector3 pos, Vector3 toPos)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("skua_move");
            _ogdLog.EventParam("object_id", skuaID);
            _ogdLog.EventParam("from_position_x", pos.x);
            _ogdLog.EventParam("from_position_y", pos.y);
            _ogdLog.EventParam("from_position_z", pos.z);
            _ogdLog.EventParam("to_position_x", toPos.x);
            _ogdLog.EventParam("to_position_y", toPos.y);
            _ogdLog.EventParam("to_position_z", toPos.z);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogMatingDanceIndicator(float percent)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("mating_dance_complete_indicator_updated");
            _ogdLog.EventParam("percent_full", percent);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogEggTimer(float timeRemaining)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("egg_hatch_indicator_updated");
            _ogdLog.EventParam("time_remaining", timeRemaining);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogEggHatched()
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("egg_hatched");
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }
    
    public void LogEggReturn()
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("recover_egg");
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }


    public void LogEggLost(string whichSkua)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("lost_egg");
            _ogdLog.EventParam("object_id", whichSkua);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogBeginMode(int mode)
    {
        if(_loggingEnabled)
		{
            //0 = Show Mode, 1 = Home mode
            _ogdLog.BeginEvent("begin_mode");
            _ogdLog.EventParam("mode", mode);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogEnterRegion(string region_name, Vector3 pos, Quaternion gaze, string gaze_object)
    {
        if(_loggingEnabled)
		{
            //0 = Show Mode, 1 = Home mode
            _ogdLog.BeginEvent("enter_region");
            _ogdLog.EventParam("region_name", region_name);
            _ogdLog.EventParam("gaze_object", gaze_object);
            _ogdLog.EventParam("gaze_pos_x", pos.x);
            _ogdLog.EventParam("gaze_pos_y", pos.y);
            _ogdLog.EventParam("gaze_pos_z", pos.z);
            _ogdLog.EventParam("gaze_rot_x", gaze.x);
            _ogdLog.EventParam("gaze_rot_y", gaze.y);
            _ogdLog.EventParam("gaze_rot_z", gaze.z);
            _ogdLog.EventParam("gaze_rot_w", gaze.w);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogExitRegion(string region_name, Vector3 pos, Quaternion gaze, string gaze_object)
    {
        if(_loggingEnabled)
		{
            //0 = Show Mode, 1 = Home mode
            _ogdLog.BeginEvent("exit_region");
            _ogdLog.EventParam("region_name", region_name);
            _ogdLog.EventParam("gaze_object", gaze_object);
            _ogdLog.EventParam("gaze_pos_x", pos.x);
            _ogdLog.EventParam("gaze_pos_y", pos.y);
            _ogdLog.EventParam("gaze_pos_z", pos.z);
            _ogdLog.EventParam("gaze_rot_x", gaze.x);
            _ogdLog.EventParam("gaze_rot_y", gaze.y);
            _ogdLog.EventParam("gaze_rot_z", gaze.z);
            _ogdLog.EventParam("gaze_rot_w", gaze.w);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogActivityBegin(string activity)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("activity_begin");
            _ogdLog.EventParam("activity_name", activity);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogActivityEnd(string activity)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("activity_end");
            _ogdLog.EventParam("activity_name", activity);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogTimerBegin(float timeLength)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("global_timer_begin");
            _ogdLog.EventParam("time_remaining", timeLength);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogTimerPause(float timeLength)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("global_timer_pause");
            _ogdLog.EventParam("time_remaining", timeLength);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogTimerUnpause(float timeLength)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("global_timer_unpause");
            _ogdLog.EventParam("time_remaining", timeLength);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogTimerExpired()
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("global_timer_expired");
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogMove(Vector3 oldPos, Vector3 pos, Quaternion gaze, int object_id)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("move");
            _ogdLog.EventParam("object_id", object_id);     //left or right waddle (0 or 1)
            _ogdLog.EventParam("old_pos_x", oldPos.x);
            _ogdLog.EventParam("old_pos_y", oldPos.y);
            _ogdLog.EventParam("old_pos_z", oldPos.z);
            _ogdLog.EventParam("gaze_pos_x", pos.x);
            _ogdLog.EventParam("gaze_pos_y", pos.y);
            _ogdLog.EventParam("gaze_pos_z", pos.z);
            _ogdLog.EventParam("gaze_rot_x", gaze.x);
            _ogdLog.EventParam("gaze_rot_y", gaze.y);
            _ogdLog.EventParam("gaze_rot_z", gaze.z);
            _ogdLog.EventParam("gaze_rot_w", gaze.w);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

	public void LogGaze(Vector3 p, Quaternion g, string scene)
	{
		if(_loggingEnabled)
		{
			_ogdLog.BeginEvent("viewport_data");
            _ogdLog.EventParam("px", p.x);
			_ogdLog.EventParam("py", p.y);
			_ogdLog.EventParam("pz", p.z);
			_ogdLog.EventParam("qx", g.x);
			_ogdLog.EventParam("qy", g.y);
			_ogdLog.EventParam("qz", g.z);
			_ogdLog.EventParam("qw", g.w);
			_ogdLog.EventParam("scene_name", scene);
			_ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
			_ogdLog.SubmitEvent();
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
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogGazeEnd(string object_id)
    {
        if(_loggingEnabled)
		{
            _ogdLog.BeginEvent("gaze_object_end");
            _ogdLog.EventParam("object_id", object_id);
            _ogdLog.EventParam("seconds_from_launch", UnityEngine.Time.time-seconds_from_start);
            _ogdLog.SubmitEvent();
        }
    }

    #endregion // Logging
}
