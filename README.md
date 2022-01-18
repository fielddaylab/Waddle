# penguinsVR

## Firebase Debugging

For Firebase to recognize the Oculus Quest as a debug device, run the following in the command line:

`$ adb shell setprop debug.firebase.analytics.app com.DefaultCompany.PenguinsVR`

Events logged with `FirebaseAnalytics.LogEvent` will then appear in DebugView in the Firebase console.
