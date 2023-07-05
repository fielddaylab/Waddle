using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SurveyResponseSave : MonoBehaviour
{
    // Accesses the response that was chosen and adds it to the current user response
    // which will later be saved to the array of user responses 
    void OnTriggerEnter(Collider snowball)
    {
        string responseToSave = transform.parent.GetComponent<TMP_Text>().text;
        SurveyInitializer surveyScript = transform.parent.parent.GetComponent<SurveyInitializer>();
        surveyScript.currUserResponse += "responseToSave ";
    }
}
