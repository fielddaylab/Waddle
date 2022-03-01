# Survey

To implement a survey, add the `SurveyVR` prefab to a scene. In the root gameobject of the prefab, enter the name of the survey to fetch in the `m_SurveyName` field. This should match the name of the variable as entered in the Firebase console.

On `Start()`, the script will asynchronously fetch the given survey from Firebase. If the given survey name isn't found or `PenguinAnalytics.FirebaseEnabled` is set to false, `string.Empty` will be returned  and the survey will be constructed from the default text asset `default.json` defined locally.

The UI requires `SurveyVR.Initialize()` to be called immediately after prefab instantiation. Because of this, the `SurveyVR` gameobject is disabled by default, since the survey needs to be fetched from Firebase before the UI can be constructed.

`SurveyFetcher` attached to the root of the `SurveyVR` prefab will first fetch the remote config variable for a survey. It attaches an `InitSurvey()` method to the survey start button, which allows for the script to fetch the survey.
