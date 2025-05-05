using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;

public class TestEvaluator : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI testHeaderText;
    public Button generateTestButton;
    public Button checkTestButton;
    public Button finalScoreButton;
    public GameObject questionContainerPrefab;
    public Transform questionParent;
    public TextMeshProUGUI feedbackText;

    [Header("From Task")]
    public ReactionCheckerWithMistral taskEvaluator;

    private List<QuestionUI> questionUIs = new List<QuestionUI>();
    private string testPrompt;

    private string apiKey = "VYw2oOShZHXFRyWH3GjS6a0A5ojRI6Ya"; // (Рекомендую потом хранить отдельно, например в ScriptableObject)
    private string apiUrl = "https://api.mistral.ai/v1/chat/completions";

    void Start()
    {
        generateTestButton.onClick.AddListener(() => StartCoroutine(GenerateTest()));
        checkTestButton.onClick.AddListener(() => StartCoroutine(CheckAnswers()));
        finalScoreButton.onClick.AddListener(() => StartCoroutine(GetFinalScore()));
    }

    IEnumerator GenerateTest()
    {
        string instruction = $"Составь небольшой тест из 2–3 вопросов по следующему эксперименту: {taskEvaluator.taskText2}. " +
                             $"Вопросы должны быть понятными и направленными на проверку понимания опыта и его вывода. Не пиши правильные ответы, только вопросы.";

        var jsonBody = new
        {
            model = "mistral-small-latest",
            messages = new[] {
                new { role = "user", content = instruction }
            }
        };

        UnityWebRequest request = CreateRequest(jsonBody);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            var parsed = JObject.Parse(response);
            string aiText = parsed["choices"]?[0]?["message"]?["content"]?.ToString().Trim();

            feedbackText.text = "Тест сгенерирован!";
            DisplayQuestions(aiText);
        }
        else
        {
            feedbackText.text = "Ошибка генерации теста.";
        }
    }

    void DisplayQuestions(string aiText)
    {
        // Очистка старых вопросов
        foreach (Transform child in questionParent)
            Destroy(child.gameObject);
        questionUIs.Clear();

        // Разделение вопросов
        string[] questions = aiText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var q in questions)
        {
            var obj = Instantiate(questionContainerPrefab, questionParent);

            var qUI = new QuestionUI
            {
                questionText = obj.GetComponentInChildren<TextMeshProUGUI>(),
                answerInput = obj.GetComponentInChildren<TMP_InputField>()
            };

            if (qUI.questionText != null)
            {
                qUI.questionText.text = q.Trim();
                Debug.Log("Добавлен вопрос: " + q);
            }
            else
            {
                Debug.LogWarning("Не найден компонент TextMeshProUGUI для вопроса в префабе.");
            }

            questionUIs.Add(qUI);
        }
    }

    IEnumerator CheckAnswers()
    {
        string combinedAnswers = "";
        foreach (var q in questionUIs)
        {
            combinedAnswers += $"{q.questionText.text}\nОтвет: {q.answerInput.text}\n\n";
        }

        string prompt = $"Вот вопросы по эксперименту и ответы ученика:\n\n{combinedAnswers}" +
                        $"Проверь, насколько они верны. Оцени общую точность ответов по шкале от 0 до 3 баллов.";

        var jsonBody = new
        {
            model = "mistral-small-latest",
            messages = new[] {
                new { role = "user", content = prompt }
            }
        };

        UnityWebRequest request = CreateRequest(jsonBody);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var parsed = JObject.Parse(request.downloadHandler.text);
            string aiText = parsed["choices"]?[0]?["message"]?["content"]?.ToString().Trim();
            feedbackText.text = aiText;
        }
        else
        {
            feedbackText.text = "Ошибка проверки ответов.";
        }
    }

    IEnumerator GetFinalScore()
    {
        string finalPrompt = $"Ученик выполнил задание:\n{taskEvaluator.taskText2}\n" +
                             $"Его ответ: {taskEvaluator.userInput.text}\n\n" +
                             $"И прошёл тест по этому заданию. На тесте он получил {feedbackText.text}.\n\n" +
                             $"Оцени его итоговую работу в целом по шкале от 0 до 5. Кратко прокомментируй результат.(1 предложение)";

        var jsonBody = new
        {
            model = "mistral-small-latest",
            messages = new[] {
                new { role = "user", content = finalPrompt }
            }
        };

        UnityWebRequest request = CreateRequest(jsonBody);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var parsed = JObject.Parse(request.downloadHandler.text);
            string aiText = parsed["choices"]?[0]?["message"]?["content"]?.ToString().Trim();
            feedbackText.text = aiText;
        }
        else
        {
            feedbackText.text = "Ошибка получения финальной оценки.";
        }
    }

    UnityWebRequest CreateRequest(object jsonBody)
    {
        string jsonString = JsonConvert.SerializeObject(jsonBody);
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
        return request;
    }

    [System.Serializable]
    public class QuestionUI
    {
        public TextMeshProUGUI questionText;
        public TMP_InputField answerInput;
    }
}
