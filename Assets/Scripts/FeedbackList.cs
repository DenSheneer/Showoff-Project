using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class FeedbackList : MonoBehaviour
{
    private List<FeedbackQuestion> questions = new List<FeedbackQuestion>();
    
    void Awake()
    {
        foreach(Transform pTransform in this.transform)
        {
            FeedbackQuestion question = pTransform.GetComponent<FeedbackQuestion>();

            if (question != null)
            {
                questions.Add(question);
            }
        }
    }

    private void OnDisable()
    {
        SaveFeedBack();

        foreach (FeedbackQuestion question in questions)
            question.ResetButton();
    }

    private void SaveFeedBack()
    {
        Dictionary<string, FeedbackContainer> questionsAndAwnsers = new Dictionary<string, FeedbackContainer>();

        string loadPath = Application.persistentDataPath + "/Feedback/";
        if (!Directory.Exists(loadPath))
        {
            Directory.CreateDirectory(loadPath);
        }
        string filePath = loadPath+"feedback.json";
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Dispose();
        }

        string jsonString = File.ReadAllText(filePath);
        JSONObject jObject = JSONObject.Create(jsonString);
        if (jObject != null && jObject.type == JSONObject.Type.OBJECT)
        {
            foreach(string questionKey in jObject.keys)
            {
                int VeryUnHappyCount = (int)jObject[questionKey]["VeryUnHappyCount"].i;
                int UnHappyCount = (int)jObject[questionKey]["UnHappyCount"].i;
                int NeutralCount = (int)jObject[questionKey]["NeutralCount"].i;
                int HappyCount = (int)jObject[questionKey]["HappyCount"].i;
                int VeryHappyCount = (int)jObject[questionKey]["VeryHappyCount"].i;

                FeedbackContainer feedbackContainer = new FeedbackContainer(VeryUnHappyCount , UnHappyCount, NeutralCount, HappyCount, VeryHappyCount);
                questionsAndAwnsers[questionKey] = feedbackContainer;
            }
        }

        foreach (FeedbackQuestion feedbackQuestion in questions)
        {
            string questionKey = feedbackQuestion.GetQuestion();
            FeedbackButtons.FeedBackEmotion feedBack = feedbackQuestion.FeedbackEmotion();

            switch (feedBack)
            {
                case FeedbackButtons.FeedBackEmotion.VERYUNHAPPY:
                    if (questionsAndAwnsers.ContainsKey(questionKey))
                        questionsAndAwnsers[questionKey].veryUnHappyCount++;
                    else
                        questionsAndAwnsers[questionKey] = new FeedbackContainer(1,0,0,0,0);

                    break;
                case FeedbackButtons.FeedBackEmotion.UNHAPPY:
                    if (questionsAndAwnsers.ContainsKey(questionKey))
                        questionsAndAwnsers[questionKey].unHappyCount++;
                    else
                        questionsAndAwnsers[questionKey] = new FeedbackContainer(0, 1, 0, 0, 0);

                    break;
                case FeedbackButtons.FeedBackEmotion.NEUTRAL:
                    if (questionsAndAwnsers.ContainsKey(questionKey))
                        questionsAndAwnsers[questionKey].neutralCount++;
                    else
                        questionsAndAwnsers[questionKey] = new FeedbackContainer(0, 0, 1, 0, 0);

                    break;
                case FeedbackButtons.FeedBackEmotion.HAPPY:
                    if (questionsAndAwnsers.ContainsKey(questionKey))
                        questionsAndAwnsers[questionKey].happyCount++;
                    else
                        questionsAndAwnsers[questionKey] = new FeedbackContainer(0, 0, 0, 1, 0);

                    break;
                case FeedbackButtons.FeedBackEmotion.VERYHAPPY:
                    if (questionsAndAwnsers.ContainsKey(questionKey))
                        questionsAndAwnsers[questionKey].veryHappyCount++;
                    else
                        questionsAndAwnsers[questionKey] = new FeedbackContainer(0, 0, 0, 0, 1);

                    break;
                default:
                    if (!questionsAndAwnsers.ContainsKey(questionKey))
                        questionsAndAwnsers[questionKey] = new FeedbackContainer(0, 0, 0, 0, 0);

                    break;
            }

        }

        SaveToFile(questionsAndAwnsers, filePath);
    }

    private void RemoveDeletedQuestions(Dictionary<string, FeedbackContainer> pQuestionsAndAwnsers)
    {
        List<string> removeKeys = new List<string>();

        foreach(KeyValuePair<string, FeedbackContainer> pair in pQuestionsAndAwnsers)
        {
            if (!QuestionIsInQuestionList(pair.Key))
                removeKeys.Add(pair.Key);
        }

        foreach(string key in removeKeys)
            pQuestionsAndAwnsers.Remove(key);
    }


    private void SaveToFile(Dictionary<string, FeedbackContainer> pQuestionsAndAwnsers,string pFilePath)
    {
        //RemoveDeletedQuestions(pQuestionsAndAwnsers);

        JSONObject jFile = new JSONObject(JSONObject.Type.OBJECT);
        foreach (KeyValuePair<string, FeedbackContainer> pair in pQuestionsAndAwnsers)
        {
            JSONObject jQuestion = new JSONObject(JSONObject.Type.OBJECT);
            jQuestion.AddField("VeryUnHappyCount", pair.Value.veryUnHappyCount);
            jQuestion.AddField("UnHappyCount", pair.Value.unHappyCount);
            jQuestion.AddField("NeutralCount", pair.Value.neutralCount);
            jQuestion.AddField("HappyCount", pair.Value.happyCount);
            jQuestion.AddField("VeryHappyCount", pair.Value.veryHappyCount);

            jFile.AddField(pair.Key, jQuestion);
        }

        File.WriteAllText(pFilePath, string.Empty);
        File.WriteAllText(pFilePath, jFile.ToString());

    }
    private bool QuestionIsInQuestionList(string pQuestion)
    {
        foreach (FeedbackQuestion question in questions)
        {
            if (question.GetQuestion() == pQuestion)
                return true;
        }

        return false;
    }

    private class FeedbackContainer
    {
        public int veryUnHappyCount, unHappyCount, neutralCount, happyCount, veryHappyCount;

        public FeedbackContainer(int pVeryUnHappyCount, int pUnHappyCount, int pNeutralCount, int pHappyCount, int pVeryHappyCount)
        {
            veryUnHappyCount = pVeryUnHappyCount;
            unHappyCount = pUnHappyCount;
            neutralCount = pNeutralCount;
            happyCount = pHappyCount;
            veryHappyCount = pVeryHappyCount;
        }
    }
}