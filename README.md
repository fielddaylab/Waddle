# Waddle

![Waddle Hero Image](https://user-images.githubusercontent.com/102836974/230182694-ac6dd98b-ac03-42ef-9dcf-8e575b68c3e1.png)

Waddle (previously Penguins VR) replicates the Antarctic environment of the Adelie penguins into a fun, task filled virtual reality space. The game’s intention is to promote curiosity and understanding about the Antarctic by getting users to feel like they are truly within the body of a penguin.

The user can access this environment through a Head Mounted Display (HMD) device, and will be given instructions to be able to move and perform tasks as a penguin like waddling, eating, move its flippers or use its beak to move or pick things, protect its eggs from predators etc.

User input via trials and surveys were recorded to gain insight into the actual versus the intended user experience of the game, in order to further improve the game’s nature. Some of these trials were part of excursion trips for primary and middle school students from the Madison Metropolitan school district as part of Expedition VRctica’s broader goal to advance STEM education in Wisconsin’s communities.

This game was developed by the Virtual Environments Group and Field Day Lab from the University of Wisconsin-Madison campus. It is planned to be released on the Meta Quest Store and the App Lab as a Virtual Reality application.

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
Full [event schema](https://github.com/opengamedata/opengamedata-core/blob/master/games/PENGUINS/schemas/PENGUINS.json.template)
