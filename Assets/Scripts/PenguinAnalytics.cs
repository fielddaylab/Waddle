using Firebase;
using Firebase.Analytics;
using UnityEngine;
//using FieldDay;
using System.Globalization;
using System.Text;
using BeauUtil;
using FieldDay;
using System;
using System.Security.Cryptography;
using Waddle;
using System.Runtime.InteropServices;
using System.Collections;

//[System.Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct LogGazeData
{
    public unsafe fixed float pos[3];
    public unsafe fixed float rot[4];

    public void Write(Vector3 pos, Quaternion rot) {
        unsafe {
            fixed(float* pPos = this.pos) {
                *(Vector3*) pPos = pos;
            }
            fixed (float* pRot = this.rot) {
                *(Quaternion*) pRot = rot;
            }
        }
    }
	
	/*public void WritePos(Vector3 pos) {
		unsafe {
            fixed(float* pPos = this.pos) {
                *(Vector3*) pPos = pos;
            }
        }
	}*/
}

public static class RunMode {
  public enum ApplicationRunMode {
    Device,
    Editor,
    Simulator
  }
  public static ApplicationRunMode Current {
    get {
      #if UNITY_EDITOR
      return UnityEngine.Device.Application.isEditor && !UnityEngine.Device.Application.isMobilePlatform ? ApplicationRunMode.Editor : ApplicationRunMode.Simulator;
      #else
      return ApplicationRunMode.Device;
      #endif
    }
  }
}

public class PenguinAnalytics : Singleton<PenguinAnalytics>
{
	public static bool FirebaseEnabled { get; set; }
    public static int logVersion = 10;
    
	static string _DB_NAME = "PENGUINS";

    [NonSerialized] float seconds_at_start = 0f;
	
	FieldDay.OGDLog _ogdLog;
	
	FieldDay.FirebaseConsts _firebase;

	[SerializeField]
	bool _loggingEnabled = true;

    [NonSerialized] int _numPebblesCollected = 0;

    [NonSerialized] int _viewportDataCount = 0;
    const int MAX_VIEWPORT_DATA = 36;
    LogGazeData[] _viewportData = new LogGazeData[MAX_VIEWPORT_DATA];
	LogGazeData[] _leftHandData = new LogGazeData[MAX_VIEWPORT_DATA];
	LogGazeData[] _rightHandData = new LogGazeData[MAX_VIEWPORT_DATA];

    StringBuilder m_GazeBuilder = new StringBuilder(2048);
	StringBuilder m_WaddlePosBuilder = new StringBuilder(128);
    StringBuilder m_WaddlePosOldBuilder = new StringBuilder(128);
	StringBuilder m_RotBuilder = new StringBuilder(128);
	StringBuilder m_PosBuilder = new StringBuilder(128);
	
	[NonSerialized] private string m_HardwareId;

    public void StartAnalytics()
    {
        m_HardwareId = GenerateHardwareId();

        //Debug.Log("Starting analytics");
		FieldDay.OGDLogConsts c = new FieldDay.OGDLogConsts();
		c.AppId = _DB_NAME;
		c.AppVersion = UnityEngine.Application.version;
		c.ClientLogVersion = logVersion;
		_ogdLog = new FieldDay.OGDLog(c);

        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
		
		if(RunMode.Current != RunMode.ApplicationRunMode.Device) {
			_loggingEnabled = false;
		}
		
		//_ogdLog.UseFirebase(_firebase);
        //_ogdLog.SetDebug(true);
    }
	
	void OnDestroy()
	{
		if(_ogdLog != null)
		{
			_ogdLog.Dispose();
			_ogdLog = null;
		}
	}

    private void LogGazeGameState()
    {
        Vector3 pos;
        Quaternion quat;
        PenguinPlayer.Instance.GetGaze(out pos, out quat);
		m_PosBuilder.Clear().Append("[").AppendNoAlloc(pos.x, 3).Append(',').AppendNoAlloc(pos.y, 3).Append(',').AppendNoAlloc(pos.z, 3).Append(',').Append("]");
		m_RotBuilder.Clear().Append("[").AppendNoAlloc(quat.x, 3).Append(',').AppendNoAlloc(quat.y, 3).Append(',').AppendNoAlloc(quat.z, 3).Append(',').AppendNoAlloc(quat.w, 3).Append("]");
		
        _ogdLog.GameStateParam("pos", m_PosBuilder.ToString());
        _ogdLog.GameStateParam("rot", m_RotBuilder.ToString());
		
		LogCurrentRegion();
    }
	
	private void LogCurrentRegion()
	{
		//_ogdLog.GameStateParam("current_region", PenguinPlayer.Instance.CurrentRegion);
	}

    #region Logging
	
	public void SetUserID(int code)
	{
		if(_loggingEnabled)
		{
			_ogdLog.SetUserId(code.ToString());
		}
	}
	
	void SetGameState()
	{
		bool hasRock=false;
		PlayerBeakState pbs = Game.SharedState.Get<PlayerBeakState>();
		if(pbs != null)
		{
			if(pbs.HoldingPebble != null)
			{
				hasRock = true;
			}
		}
		
		_ogdLog.BeginGameState();
		_ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_at_start);
		_ogdLog.GameStateParam("has_rock", hasRock);
		LogGazeGameState();
		_ogdLog.SubmitGameState();
	}
	
	
	public void LogSurveyCode(int code)
    {
		if(_loggingEnabled)
		{
			SetGameState();
			
			_ogdLog.BeginEvent("survey_code");
			_ogdLog.EventParam("survey_code_number", code);
            _ogdLog.SubmitEvent();
		}
	}
	
	public void LogApplicationStart()
	{
        if(_loggingEnabled)
		{
            seconds_at_start = UnityEngine.Time.time;
			
            _ogdLog.BeginGameState();
            _ogdLog.GameStateParam("seconds_from_launch", UnityEngine.Time.time-seconds_at_start);
			
			Vector3 pos = Vector3.zero;
			Quaternion quat = Quaternion.identity;
			
			m_PosBuilder.Clear().Append("[").AppendNoAlloc(pos.x, 3).Append(',').AppendNoAlloc(pos.y, 3).Append(',').AppendNoAlloc(pos.z, 3).Append(',').Append("]");
			m_RotBuilder.Clear().Append("[").AppendNoAlloc(quat.x, 3).Append(',').AppendNoAlloc(quat.y, 3).Append(',').AppendNoAlloc(quat.z, 3).Append(',').AppendNoAlloc(quat.w, 3).Append("]");
			
			_ogdLog.GameStateParam("pos", m_PosBuilder.ToString());
			_ogdLog.GameStateParam("rot", m_RotBuilder.ToString());
			
			_ogdLog.SubmitGameState();
			
            _ogdLog.BeginEvent("session_start");
            _ogdLog.SubmitEvent();
        }
		/*if (FirebaseEnabled)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, new Parameter(FirebaseAnalytics.ParameterLevelName, "antarctica"), new Parameter("start", UnityEngine.Time.time-seconds_at_start));
                //new Parameter("app_version", logVersion));
        }	*/
	}
	

	public void LogStartGame()
	{
        if(_loggingEnabled)
		{
            _numPebblesCollected = 0;
            
			SetGameState();
            _ogdLog.ResetSessionId();
			
			long sessionID = _ogdLog.GetSessionId();
			UnityEngine.Random.seed = (int)sessionID;
			RNG.Instance = new System.Random((int)sessionID);
			
			//RNG.Instance.
			
            _ogdLog.BeginEvent("device_identifier");
            _ogdLog.EventParam("hardware_uuid", m_HardwareId);
            _ogdLog.SubmitEvent();
			
            _ogdLog.BeginEvent("session_start");
			_ogdLog.EventParam("random_seed", (int)sessionID);
            _ogdLog.SubmitEvent();
        }
		/*if (FirebaseEnabled)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, new Parameter(FirebaseAnalytics.ParameterLevelName, "antarctica"), new Parameter("start", UnityEngine.Time.time-seconds_at_start));
                //new Parameter("app_version", logVersion));
        }	*/
	}
	
	public void LogEndGame()
	{
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("end");
            _ogdLog.SubmitEvent();

        }
		/*if (FirebaseEnabled)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, new Parameter(FirebaseAnalytics.ParameterLevelName, "antarctica"), new Parameter("start", UnityEngine.Time.time-seconds_at_start));
                //new Parameter("app_version", logVersion));
        }	*/
	}

    public void LogHeadsetOn()
    {
		if(_loggingEnabled)
		{
            //seconds_at_start = UnityEngine.Time.time;
            SetGameState();
			
			_ogdLog.BeginEvent("headset_on");
			_ogdLog.SubmitEvent();

		}

        /*if(FirebaseEnabled)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventScreenView, FirebaseAnalytics.ParameterValue, "headset_on");
        }*/
    }
	
	public void LogHeadsetOff()
    {
		if(_loggingEnabled)
		{
			SetGameState();
			
			_ogdLog.BeginEvent("headset_off");
			_ogdLog.SubmitEvent();

		}
        /*if(FirebaseEnabled)
        {
            seconds_at_start = UnityEngine.Time.time;
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventScreenView, FirebaseAnalytics.ParameterValue, "headset_on");
        }*/
    }


    public void LogLanguageSelected(string language)
    {
		if(_loggingEnabled)
		{
            SetGameState();
			
			_ogdLog.BeginEvent("language_selected");
			_ogdLog.EventParam("language", language);
			_ogdLog.SubmitEvent();
		}
        /*if(FirebaseEnabled)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventSelectItem, new Parameter("language_selected", language), new Parameter("seconds_at_start", UnityEngine.Time.time-seconds_at_start));
        }*/
    }

    public void LogObjectAssigned(string obj)
    {
		if(_loggingEnabled)
		{
            SetGameState();
			
			_ogdLog.BeginEvent("object_assigned");
			_ogdLog.EventParam("object", obj);
			_ogdLog.SubmitEvent();

        }
        /*if(FirebaseEnabled)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventSelectItem, new Parameter("object_assigned", obj), new Parameter("seconds_at_start", UnityEngine.Time.time-seconds_at_start));
        }*/
    }
	
    public void LogBeginMode(string mode)
	{
		if(_loggingEnabled)
		{
            SetGameState();
			
			_ogdLog.BeginEvent("game_start");
            _ogdLog.EventParam("mode", mode);
			_ogdLog.SubmitEvent();
		}
	}


	public void LogOpenMenu()
	{
		if(_loggingEnabled)
		{
			SetGameState();
			
			_ogdLog.BeginEvent("open_menu");
			_ogdLog.SubmitEvent();
		}
	}
	
	public void LogCloseMenu()
	{
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("close_menu");
            _ogdLog.SubmitEvent();
        }
	}
	
	public void LogSelectMenu(string item)
	{
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("select_menu_item");
            _ogdLog.EventParam("item", item);
            _ogdLog.SubmitEvent();
        }
	}
	
    public void LogObjectSelected(string obj)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("object_selected");
            _ogdLog.EventParam("gaze_point_name", obj);
            _ogdLog.SubmitEvent();
        }
       /* if(FirebaseEnabled)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventSelectItem, new Parameter("gaze_point_name", obj), new Parameter("seconds_at_start", UnityEngine.Time.time-seconds_at_start));
        }*/
    }

    public void LogSceneChanged(string scene)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("scene_change");
            _ogdLog.EventParam("scene_name", scene);
            _ogdLog.SubmitEvent();
        }

        /*if (FirebaseEnabled)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, new Parameter(FirebaseAnalytics.ParameterLevelName, scene), new Parameter("seconds_at_start", UnityEngine.Time.time-seconds_at_start));
                //new Parameter("app_version", logVersion));
        }*/	
    }

    public void LogAudioStarted(string clip)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("script_audio_started");
            _ogdLog.EventParam("clip_identifier", clip);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogAudioComplete(string clip)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("script_audio_complete");
            _ogdLog.EventParam("clip_identifier", clip);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogCaption(string caption)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("script_audio_started");
            _ogdLog.EventParam("caption", caption);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogObjectDisplayed(bool hasIndicator, string obj, Vector3 pos, Quaternion rot)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("new_object_displayed");
            _ogdLog.EventParam("has_the_indicator", hasIndicator);
            _ogdLog.EventParam("object", obj);

			m_PosBuilder.Clear().Append("[").AppendNoAlloc(pos.x, 3).Append(',').AppendNoAlloc(pos.y, 3).Append(',').AppendNoAlloc(pos.z, 3).Append(',').Append("]");
			m_RotBuilder.Clear().Append("[").AppendNoAlloc(rot.x, 3).Append(',').AppendNoAlloc(rot.y, 3).Append(',').AppendNoAlloc(rot.z, 3).Append(',').AppendNoAlloc(rot.w, 3).Append("]");
			
			_ogdLog.GameStateParam("pos", m_PosBuilder.ToString());
			_ogdLog.GameStateParam("rot", m_RotBuilder.ToString());
            _ogdLog.SubmitEvent();
        }
    }

    public void LogEatFish(Vector3 pos, Quaternion rot)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("eat_fish");
            _ogdLog.SubmitEvent();
        }
    }
	
    public void LogPeckRock(string name, Vector3 pos)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
			StringBuilder s = new StringBuilder(128);
			s.Clear().Append("[").AppendNoAlloc(pos.x, 3).Append(',').AppendNoAlloc(pos.y, 3).Append(',').AppendNoAlloc(pos.z, 3).Append(',').Append("]");
			
            _ogdLog.BeginEvent("peck_rock");
            _ogdLog.EventParam("rock_id", name);
			_ogdLog.EventParam("rock_pos", s.ToString());
            _ogdLog.SubmitEvent();
        }
    }
	
	public void LogPeckNest(string name, Vector3 pos)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
			StringBuilder s = new StringBuilder(128);
			s.Clear().Append("[").AppendNoAlloc(pos.x, 3).Append(',').AppendNoAlloc(pos.y, 3).Append(',').AppendNoAlloc(pos.z, 3).Append(',').Append("]");
			
            _ogdLog.BeginEvent("peck_nest");
            _ogdLog.EventParam("nest_id", name);
			_ogdLog.EventParam("nest_pos", s.ToString());
            _ogdLog.SubmitEvent();
        }
    }

	public void LogPeckPenguin(string name, Vector3 pos)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
			StringBuilder s = new StringBuilder(128);
			s.Clear().Append("[").AppendNoAlloc(pos.x, 3).Append(',').AppendNoAlloc(pos.y, 3).Append(',').AppendNoAlloc(pos.z, 3).Append(',').Append("]");
			
            _ogdLog.BeginEvent("peck_penguin");
            _ogdLog.EventParam("penguin_id", name);
			_ogdLog.EventParam("penguin_pos", s.ToString());
            _ogdLog.SubmitEvent();
        }
    }

	public void LogPeckSkua(string name, Vector3 pos)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
			StringBuilder s = new StringBuilder(128);
			s.Clear().Append("[").AppendNoAlloc(pos.x, 3).Append(',').AppendNoAlloc(pos.y, 3).Append(',').AppendNoAlloc(pos.z, 3).Append(',').Append("]");
			
            _ogdLog.BeginEvent("peck_skua");
            _ogdLog.EventParam("skua_id", name);
			_ogdLog.EventParam("skua_pos", s.ToString());
            _ogdLog.SubmitEvent();
        }
    }
	
	public void LogStandOnRock(string name, Vector3 pos)
	{
		if(_loggingEnabled)
		{
			//Debug.Log("Standing on rock");
			
			SetGameState();
			
			StringBuilder s = new StringBuilder(128);
			s.Clear().Append("[").AppendNoAlloc(pos.x, 3).Append(',').AppendNoAlloc(pos.y, 3).Append(',').AppendNoAlloc(pos.z, 3).Append(',').Append("]");
			
            _ogdLog.BeginEvent("stand_on_rock");
            _ogdLog.EventParam("rock_id", name);
			_ogdLog.EventParam("rock_pos", s.ToString());
            _ogdLog.SubmitEvent();
        }
	}
	
	public void LogStandOnNest(string name, Vector3 pos)
	{
		if(_loggingEnabled)
		{
			//Debug.Log("Standing on nest");
			
			SetGameState();
			
			StringBuilder s = new StringBuilder(128);
			s.Clear().Append("[").AppendNoAlloc(pos.x, 3).Append(',').AppendNoAlloc(pos.y, 3).Append(',').AppendNoAlloc(pos.z, 3).Append(',').Append("]");
			
            _ogdLog.BeginEvent("stand_on_nest");
            _ogdLog.EventParam("nest_id", name);
			_ogdLog.EventParam("nest_pos", s.ToString());
            _ogdLog.SubmitEvent();
        }
	}
	
    public void LogPickupRock(Vector3 pos, Quaternion rot)
    {
        if(_loggingEnabled)
		{
            _numPebblesCollected++;
			
			SetGameState();
			
            _ogdLog.BeginEvent("pickup_rock");
            _ogdLog.EventParam("total_picked_up", _numPebblesCollected);
            _ogdLog.SubmitEvent();
        }
    }


    public void LogPushSnowball(Vector3 pos, Quaternion rot)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("push_snowball");
            _ogdLog.SubmitEvent();
        }
    }

    public void LogNestComplete()
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("nest_complete");
            _ogdLog.SubmitEvent();
        }
    }
	
	public void LogPlaceRock(int numRocks)
	{
		if(_loggingEnabled)
		{
			SetGameState();
			
			_ogdLog.BeginEvent("place_rock");
			_ogdLog.EventParam("rock_count", numRocks+1);
			_ogdLog.EventParam("percent_complete", (float)(numRocks+1f)/4f);
			_ogdLog.SubmitEvent();
		}
	}

    public void LogMenuAppeared()
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("menu_appeared");
            _ogdLog.SubmitEvent();
        }
    }

    public void LogChimes(string whichChime)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("ring_chime");
            _ogdLog.EventParam("note_played", whichChime);
            _ogdLog.SubmitEvent();
        }
    }
	
	public void LogBubblePop(int bubbleID, float timing)
	{
		if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("bubble_pop");
            _ogdLog.EventParam("object_id", bubbleID);
			_ogdLog.EventParam("timing_error", timing);
            _ogdLog.SubmitEvent();
        }
	}
    
    public void LogBubbleAppeared(int bubbleID, Vector3 pos)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("bubble_appeared");
            _ogdLog.EventParam("object_id", bubbleID);
            _ogdLog.EventParam("posX", pos.x);
            _ogdLog.EventParam("posY", pos.y);
            _ogdLog.EventParam("posZ", pos.z);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogBubbleDisappeared(int bubbleID)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("bubble_expired");
            _ogdLog.EventParam("object_id", bubbleID);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogPinFell()
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("penguin_pin_fell");
            _ogdLog.SubmitEvent();
        }
    }

    public void LogFlipperBashSkua(string skuaID, bool right, Vector3 skuaPos, Vector3 penguinPos)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
			StringBuilder sPos = new StringBuilder(128);
			sPos.Clear().Append("[").AppendNoAlloc(skuaPos.x, 3).Append(',').AppendNoAlloc(skuaPos.y, 3).Append(',').AppendNoAlloc(skuaPos.z, 3).Append("]");
            
			StringBuilder pPos = new StringBuilder(128);
			pPos.Clear().Append("[").AppendNoAlloc(penguinPos.x, 3).Append(',').AppendNoAlloc(penguinPos.y, 3).Append(',').AppendNoAlloc(penguinPos.z, 3).Append("]");
            
			
			_ogdLog.BeginEvent("flipper_bash_skua");
            _ogdLog.EventParam("skua_id", skuaID);
			_ogdLog.EventParam("skua_pos", sPos.ToString());
			_ogdLog.EventParam("penguin_pos", pPos.ToString());

			_ogdLog.EventParam("hand", right ? "RIGHT" : "LEFT");
			//_ogdLog.EventParam("rightFlipper", right);
            _ogdLog.SubmitEvent();
        }
    }

	public void LogFlipperBashRock(string rockID, bool right, Vector3 rockPos, Vector3 penguinPos)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
			StringBuilder sPos = new StringBuilder(128);
			sPos.Clear().Append("[").AppendNoAlloc(rockPos.x, 3).Append(',').AppendNoAlloc(rockPos.y, 3).Append(',').AppendNoAlloc(rockPos.z, 3).Append("]");
            
			//StringBuilder pPos = new StringBuilder(128);
			//pPos.Clear().Append("[").AppendNoAlloc(penguinPos.x, 3).Append(',').AppendNoAlloc(penguinPos.y, 3).Append(',').AppendNoAlloc(penguinPos.z, 3).Append("]");
            
			
			_ogdLog.BeginEvent("flipper_bash_rock");
            _ogdLog.EventParam("rock_id", rockID);
			_ogdLog.EventParam("rock_pos", sPos.ToString());
			//_ogdLog.EventParam("penguin_pos", pPos.ToString());
			
			_ogdLog.EventParam("hand", right ? "RIGHT" : "LEFT");
            _ogdLog.SubmitEvent();
        }
    }
	
	public void LogFlipperBashPenguin(string penguinID, bool right, Vector3 penguinPos, Vector3 playerPos)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
			StringBuilder sPos = new StringBuilder(128);
			sPos.Clear().Append("[").AppendNoAlloc(penguinPos.x, 3).Append(',').AppendNoAlloc(penguinPos.y, 3).Append(',').AppendNoAlloc(penguinPos.z, 3).Append("]");
            
			//StringBuilder pPos = new StringBuilder(128);
			//pPos.Clear().Append("[").AppendNoAlloc(penguinPos.x, 3).Append(',').AppendNoAlloc(penguinPos.y, 3).Append(',').AppendNoAlloc(penguinPos.z, 3).Append("]");
            
			
			_ogdLog.BeginEvent("flipper_bash_penguin");
            _ogdLog.EventParam("penguin_id", penguinID);
			_ogdLog.EventParam("penguin_pos", sPos.ToString());
			//_ogdLog.EventParam("penguin_pos", pPos.ToString());
			
			_ogdLog.EventParam("hand", right ? "RIGHT" : "LEFT");
            _ogdLog.SubmitEvent();
        }
    }
	
	public void LogFlipperBashNest(string nestID, bool right, Vector3 nestPos, Vector3 penguinPos)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
			StringBuilder sPos = new StringBuilder(128);
			sPos.Clear().Append("[").AppendNoAlloc(nestPos.x, 3).Append(',').AppendNoAlloc(nestPos.y, 3).Append(',').AppendNoAlloc(nestPos.z, 3).Append("]");
            
			//StringBuilder pPos = new StringBuilder(128);
			//pPos.Clear().Append("[").AppendNoAlloc(penguinPos.x, 3).Append(',').AppendNoAlloc(penguinPos.y, 3).Append(',').AppendNoAlloc(penguinPos.z, 3).Append("]");
            
			
			_ogdLog.BeginEvent("flipper_bash_nest");
            _ogdLog.EventParam("nest_id", nestID);
			_ogdLog.EventParam("nest_pos", sPos.ToString());
			//_ogdLog.EventParam("penguin_pos", pPos.ToString());
			
			_ogdLog.EventParam("hand", right ? "RIGHT" : "LEFT");
            _ogdLog.SubmitEvent();
        }
    }
	
    public void LogSkuaSpawned(string skuaID, Vector3 pos)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("skua_spawn");
            _ogdLog.EventParam("object_id", skuaID);
            _ogdLog.EventParam("posX", pos.x);
            _ogdLog.EventParam("posY", pos.y);
            _ogdLog.EventParam("posZ", pos.z);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogSkuaMove(string skuaID, Vector3 pos, Vector3 toPos)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("skua_move");
            _ogdLog.EventParam("object_id", skuaID);
            _ogdLog.EventParam("from_posX", pos.x);
            _ogdLog.EventParam("from_posY", pos.y);
            _ogdLog.EventParam("from_posZ", pos.z);
            _ogdLog.EventParam("to_posX", toPos.x);
            _ogdLog.EventParam("to_posY", toPos.y);
            _ogdLog.EventParam("to_posZ", toPos.z);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogMatingDanceIndicator(float percent)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("mating_dance_indicator_updated");
            _ogdLog.EventParam("percent_full", percent);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogEggTimer(float timeRemaining)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("egg_hatch_indicator_updated");
            _ogdLog.EventParam("time_remaining", timeRemaining);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogEggHatched()
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("egg_hatched");
            _ogdLog.SubmitEvent();
        }
    }
    
    public void LogEggReturn()
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("egg_recovered");
            _ogdLog.SubmitEvent();
        }
    }


    public void LogEggLost(string whichSkua)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("egg_lost");
            _ogdLog.EventParam("object_id", whichSkua);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogBeginMode(int mode)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            //0 = Show Mode, 1 = Home mode, 2 = research mode
            _ogdLog.BeginEvent("begin_mode");
            _ogdLog.EventParam("mode", mode);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogEnterRegion(string region_name)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            //0 = Show Mode, 1 = Home mode
            _ogdLog.BeginEvent("enter_region");
            _ogdLog.EventParam("region_name", region_name);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogExitRegion(string region_name)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            //0 = Show Mode, 1 = Home mode
            _ogdLog.BeginEvent("exit_region");
            _ogdLog.EventParam("region_name", region_name);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogActivityBegin(string activity)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("activity_begin");
            _ogdLog.EventParam("activity_name", activity);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogActivityEnd(string activity)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("activity_end");
            _ogdLog.EventParam("activity_name", activity);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogTimerBegin(float timeLength)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("global_timer_begin");
            _ogdLog.EventParam("time_remaining", timeLength);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogTimerPause(float timeLength)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("global_timer_pause");
            _ogdLog.EventParam("time_remaining", timeLength);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogTimerUnpause(float timeLength)
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("global_timer_unpause");
            _ogdLog.EventParam("time_remaining", timeLength);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogTimerExpired()
    {
        if(_loggingEnabled)
		{
			SetGameState();
			
            _ogdLog.BeginEvent("global_timer_expired");
            _ogdLog.SubmitEvent();
        }
    }

    public void LogMove(Vector3 oldPos, Vector3 pos, Quaternion gaze, int object_id, PlayerMovementSource source)
    {
        if(_loggingEnabled)
		{
            SetGameState();
			
			m_WaddlePosBuilder.Clear().Append("[").AppendNoAlloc(pos.x, 3).Append(',').AppendNoAlloc(pos.y, 3).Append(',').AppendNoAlloc(pos.z, 3).Append("]");
			m_WaddlePosOldBuilder.Clear().Append("[").AppendNoAlloc(oldPos.x, 3).Append(',').AppendNoAlloc(oldPos.y, 3).Append(',').AppendNoAlloc(oldPos.z, 3).Append("]");
			
            _ogdLog.BeginEvent("player_waddle");
            _ogdLog.EventParam("object_id", object_id);     //left or right waddle (0 or 1)
			
            _ogdLog.EventParam("pos_old", m_WaddlePosOldBuilder.ToString());
            _ogdLog.EventParam("pos_new", m_WaddlePosBuilder.ToString());
            _ogdLog.EventParam("source", source == PlayerMovementSource.Button ? "button" : "waddle");
            _ogdLog.SubmitEvent();
        }
    }

	public unsafe bool LogGaze(Vector3 p, Quaternion q, uint gazeLogFrameCount, bool sendToServer=false)//, string scene)
	{
		if(_loggingEnabled)
		{
			if(_viewportDataCount < MAX_VIEWPORT_DATA)
			{
                _viewportData[_viewportDataCount].Write(p, q);
                //_viewportData[_viewportDataCount].rot = (q.x.ToString("F3")+","+q.y.ToString("F3")+","+q.z.ToString("F3")+","+q.w.ToString("F3"));
				
				Vector3 leftPos = Vector3.zero;
				Quaternion leftRot;
				
				PenguinPlayer.Instance.GetHandTransform(true, out leftPos, out leftRot);
                _leftHandData[_viewportDataCount].Write(leftPos, leftRot);
				
				Vector3 rightPos;
                Quaternion rightRot;
				
				PenguinPlayer.Instance.GetHandTransform(false, out rightPos, out rightRot);
                _rightHandData[_viewportDataCount].Write(rightPos, rightRot);

				_viewportDataCount++;
			}
			else
			{
				sendToServer = true;
			}

            if(sendToServer)
            {
                WriteGazeData(m_GazeBuilder, "gaze_data_package", _viewportData, _viewportDataCount);
                //Debug.Log(gazeLogFrameCount);
                //Debug.Log(_viewportDataCount);
                //Debug.Log(m_GazeBuilder);
                SetGameState();
                _ogdLog.Log("viewport_data", m_GazeBuilder);

                WriteGazeData(m_GazeBuilder, "left_hand_data_package", _leftHandData, _viewportDataCount);
				SetGameState();
                //Debug.Log(m_GazeBuilder);
                _ogdLog.Log("left_hand_data", m_GazeBuilder);

                WriteGazeData(m_GazeBuilder, "right_hand_data_package", _rightHandData, _viewportDataCount);
				SetGameState();
                //Debug.Log(m_GazeBuilder);
                _ogdLog.Log("right_hand_data", m_GazeBuilder);

                _viewportDataCount = 0;
				return true;
            }
		}
		
		return false;
		/*if (FirebaseEnabled)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, new Parameter(FirebaseAnalytics.ParameterLevelName, "antarctica"), new Parameter("start", UnityEngine.Time.time-seconds_at_start));
                //new Parameter("app_version", logVersion));
        }	*/
	}

    static private unsafe void WriteGazeData(StringBuilder sb, string paramName, LogGazeData[] data, int count) {
        sb.Clear().Append("{\"").Append(paramName).Append("\":\"[");
        if (count > 0) {
            for (int i = 0; i < count; i++) {
                AppendGazeFrame(sb, data[i]);
                sb.Append(',');
            }
        }
        sb.Length--; // eliminate last comma
        sb.Append("]\"}");
    }

    static private unsafe void AppendGazeFrame(StringBuilder sb, LogGazeData data) {
        sb.Append("{\\\"pos\\\":[").AppendNoAlloc(data.pos[0], 3).Append(',').AppendNoAlloc(data.pos[1], 3).Append(',').AppendNoAlloc(data.pos[2], 3).Append(']')
            .Append(",\\\"rot\\\":[").AppendNoAlloc(data.rot[0], 3).Append(',').AppendNoAlloc(data.rot[1], 3).Append(',').AppendNoAlloc(data.rot[2], 3).Append(',').AppendNoAlloc(data.rot[3], 3).Append("]}");
    }

    public void LogGazeBegin(string object_id)
    {
        if(_loggingEnabled)
		{
			//Debug.Log("Gaze begin: " + object_id);
			
			SetGameState();
				
            _ogdLog.BeginEvent("gaze_object_begin");
            _ogdLog.EventParam("object_id", object_id);
            _ogdLog.SubmitEvent();
        }
    }

    public void LogGazeEnd(string object_id)
    {
        if(_loggingEnabled)
		{
			//Debug.Log("Gaze end: " + object_id);
			
			SetGameState();
			
            _ogdLog.BeginEvent("gaze_object_end");
            _ogdLog.EventParam("object_id", object_id);
            _ogdLog.SubmitEvent();

        }
    }

    static private unsafe string GenerateHardwareId() {
        using (SHA256 sha = SHA256.Create()) {
            byte[] nameData = sha.ComputeHash(Encoding.UTF8.GetBytes(_DB_NAME));
            byte[] hashData = sha.ComputeHash(Encoding.UTF8.GetBytes(SystemInfo.deviceUniqueIdentifier));
            StringBuilder sb = new StringBuilder(64);
            for(int i = 0; i < hashData.Length; i++) {
                sb.Append((nameData[i] ^ hashData[i]).ToString("x2"));
            }
            return sb.ToString();
        }
    }

    #endregion // Logging
}
