# penguinsVR

## Firebase Debugging

For Firebase to recognize the Oculus Quest as a debug device, run the following in the command line:

`$ adb shell setprop debug.firebase.analytics.app com.DefaultCompany.PenguinsVR`

Events logged with `FirebaseAnalytics.LogEvent` will then appear in DebugView in the Firebase console.

## Firebase Telemetry Events

Firebase automatically adds the following parameters to all events, documented [here](https://support.google.com/firebase/answer/7061705?hl=en). Event data is then dumped to BigQuery daily - the BigQuery schema can be found [here](https://support.google.com/firebase/answer/7029846?hl=en&ref_topic=7029512).
* event_timestamp
* user_id (We need to manually set this if we have it)
* device.category
* device.mobile_brand_name (i.e Apple)
* device.mobile_model_name (i.e Safari)
* device.operating_system
* device.language
* geo.country
* geo.region (i.e Wisconsin)
* geo.city (i.e Madison)
* ga_session_id

Firebase automatically logs the following meaningful events, documented [here](https://support.google.com/firebase/answer/9234069?hl=en&ref_topic=6317484).
* first_visit
* page_view
* session_start
* user_engagement

### Change Log
1. Initial version (2/4/22)

### Events

1. [start_game](#start_game)
2. [load_minigame](#load_minigame)

<a name="start_game"/>

#### start_game

Player starts the game.

| Parameter | Description |
| --- | --- |
| app_version | Current logging version |

<a name="load_minigame"/>

#### load_minigame

Player loads a given minigame scene.

| Parameter | Description |
| --- | --- |
| app_version | Current logging version |
| minigame | The minigame being loaded |

| minigame |
| --- |
| ProtectTheNest |
| MatingDance |
