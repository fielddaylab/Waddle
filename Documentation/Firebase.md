# Firebase

## Logging

Logging to Firebase is handled in the `PenguinAnalytics` script, which inherits `Singleton` and should be present in every scene. On `Start()`, the script initializes Firebase by checking and resolving dependencies. In the Unity Editor this will always resolve to false and events will not be logged.

If dependencies are resolved properly, the `FirebaseEnabled` flag is set to true and analytics will be collected.

## Debugging

For Firebase to recognize the Oculus Quest as a debug device, run the following in the command line:

`$ adb shell setprop debug.firebase.analytics.app com.DefaultCompany.PenguinsVR`

Events logged with `FirebaseAnalytics.LogEvent` will then appear in DebugView in the Firebase console.

Console messages from Firebase can be tracked using ADB. First, run the following the command line:

`$ adb shell setprop log.tag.FA VERBOSE` \
`$ adb shell setprop log.tag.FA-SVC VERBOSE`

Then, run the following command to start tracking the log output:

`$ adb logcat -s FA FA-SVC Unity`
