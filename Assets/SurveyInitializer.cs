using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using FieldDay;


    public class SurveyInitializer : MonoBehaviour
    {
        public TextAsset JSONFile;
        private SurveyPanel panel;
        // error
        //private OGDLog m_Logger = new OGDLog();
        private SurveyPackage currPackage;

        [SerializeField] private TMP_Text m_Header;
        [SerializeField] private Button next;
        [SerializeField] private Button finish;

        private SurveyData[] surveys;
        private SurveyQuestion[] questions;
        private int numQuestions;
        [SerializeField] private TMP_Text[] responseDisplays;
        private string[] userResponses;

        // Start is called before the first frame update
        // Parse json file and set up the survey panels
        void Start()
        {
            currPackage = SurveyPackage.Parse(JSONFile.text);
            if (currPackage == null)
            {
                Debug.Log("Current Package currPackage is null. Parse did not work as expected.");
            }

            surveys = currPackage.Surveys;
            if (surveys == null)
            {
                Debug.Log("surveys is null.");
            }

            Debug.Log("Prompt is " + surveys[0].Pages[0].Questions[0].Responses[0]);

            questions = surveys[0].Pages[0].Questions;
            numQuestions = questions.Length;
            userResponses = new string[numQuestions];

            m_Header.SetText(surveys[0].Header);
            next.onClick.AddListener(NextClickHandler);
            finish.onClick.AddListener(FinishClickHandler); //Change it later to only be enabled when last question is displayed
        }


        private int currQuestion = 0;
        private int prevQuestion = -1;
        public string currUserResponse = "";
        [SerializeField] private TMP_Text currPrompt;
        private string[] questionResponses;

        //private int time = 1;

    // Update is called once per frame
        void Update()
        {
            // Checks if next has been clicked (next increments the currQuestion count)
            // to see if update to next question is required
            if (currQuestion > prevQuestion && currQuestion < numQuestions)
            {
                currPrompt.SetText(questions[currQuestion].Prompt);
                questionResponses = questions[currQuestion].Responses;
                
                //sets up the response panels
                for (int i = 0; i < questionResponses.Length; i++)
                {
                responseDisplays[i].transform.parent.gameObject.SetActive(true);
                responseDisplays[i].SetText(questionResponses[i]);
                }

                //disables the panels that aren't required
                for (int j = questionResponses.Length; j < 5; j++)
                {
                    responseDisplays[j].transform.parent.gameObject.SetActive(false);
                }
                ++prevQuestion;
            }

            // Code for testing the "next" button

            //if (time == 10)
            //{
                //next.onClick.Invoke();
                //time = 0;
            //}
            //++time;
        }

        private void NextClickHandler()
        {
            userResponses[currQuestion] = currUserResponse; //saves the user responses to the current question
            currUserResponse = "";
            ++currQuestion;
        }

        private StringBuilder surveyBuilder = new StringBuilder(256);

        private void FinishClickHandler()
        {
            userResponses[currQuestion] = currUserResponse;
            ++currQuestion;

            //Format taken from SurveyPanel.cs of opengamedata-unity package
            surveyBuilder.Clear()
                    .Append("{\"package_config_id\":\"").Append(currPackage.PackageConfigId).Append("\",")
                    .Append("\"display_event_id\":\"").Append(surveys[0].DisplayEventId).Append("\",")
                    .Append("\"responses\":[");

            for (int i = 0; i < userResponses.Length; i++)
            {
                surveyBuilder.Append("{\"prompt\":\"").Append(questions[i].Prompt).Append("\",")
                    .Append("\"response\":\"").Append(userResponses[i]).Append("\"")
                    .Append("},");
            }

            // error
            //OGDLogUtils.TrimEnd(surveyBuilder, ',');
            surveyBuilder.Append("]}");
            
            //Logging is throwing errors
            //m_Logger.Log("survey_submitted", surveyBuilder);
        }
    }