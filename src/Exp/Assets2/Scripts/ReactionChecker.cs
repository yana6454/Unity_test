using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System;
using Newtonsoft.Json;

public class ReactionCheckerWithMistral : MonoBehaviour
{
    public GameObject task;
    public GameObject test;

    [Header("Task")]
    public TextMeshProUGUI taskText;
    public TMP_InputField userInput;
    public TextMeshProUGUI feedbackText;
    public Button submitButton;

    [Header("Test")]

    private string apiKey = "VYw2oOShZHXFRyWH3GjS6a0A5ojRI6Ya"; // Ваш API ключ Mistral
    private string apiUrl = "https://api.mistral.ai/v1/chat/completions"; // URL Mistral API

    public string taskText2;

    void Start()
    {
        // task.SetActive(true);
        // test.SetActive(false);

        submitButton.onClick.AddListener(() => StartCoroutine(CheckAnswerAsync()));
        taskText.text = "Задание:\n" +
            "Напишите вывод по пройденному эксперименту. В каких пробирках происходит выделение газа? " +
            "Расположите используемые реактивы так, чтобы скорость выделения газа уменьшалась слева направо. ";
        taskText2 = "В четыре пробирки поместите кусочек магниевой стружки, гранулу цинка, порошок железа, медную фольгу. " +
            "В каждую пробирку прилейте по 1 мл соляной кислоты. В каких пробирках происходит выделение газа? " +
            "Расположите пробирки в штативе так, чтобы скорость выделения газа уменьшалась слева направо. " +
            "Газ – водород. Чем сильнее выделяется газ, тем активнее металл. Активный металл может вытеснить менее активный из раствора его соли. " +
            "Водород вытесняется только металлами, расположенными левее него в ряду активности.";
        feedbackText.text = ""; // Начально поле пустое

        // Включение переноса строк для поля ввода (текста ученика)
        if (userInput != null && userInput.textComponent != null)
        {
            userInput.textComponent.enableWordWrapping = true;
            userInput.textComponent.overflowMode = TextOverflowModes.Overflow; // или .Truncate, если нужно обрезать
            userInput.lineType = TMP_InputField.LineType.MultiLineNewline; // разрешаем многострочный ввод
        }

        // Включение переноса строк для placeholder (если есть)
        if (userInput.placeholder is TextMeshProUGUI placeholder)
        {
            placeholder.enableWordWrapping = true;
            placeholder.overflowMode = TextOverflowModes.Overflow;
        }

    }

    IEnumerator CheckAnswerAsync()
    {
        string userMessage = userInput.text.Trim();
        string taskDescription = taskText2;

        var jsonBody = new
        {
            model = "mistral-small-latest",
            messages = new[] {
                new {
                    role = "user",
                    content = $"Задание:\n{taskDescription}\n\n" +
                              $"Ответ пользователя:\n\"{userMessage}\"\n\n" +
                              "Оцени этот ответ: верно или не верно. Если не верно, объясни кратко, что не так, " +
                              "но не говори правильный ответ. Если верно то похвали его и выстави балл от 1 до 2. " +
                              "если ответ верен но есть большая и серьезная ошибка снижай на 1 балл. Будь лоялен. Не придирайся к формулировке "
                }
            }
        };

        string jsonString = JsonConvert.SerializeObject(jsonBody);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            HandleAIResponse(response);
        }
        else
        {
            feedbackText.text = "Ошибка связи с Mistral API. Попробуйте снова.";
            Debug.LogError($"Ошибка: {request.error}, Ответ: {request.downloadHandler.text}");
        }
    }

    void HandleAIResponse(string response)
    {
        try
        {
            var parsedResponse = JObject.Parse(response);
            string aiText = parsedResponse["choices"]?[0]?["message"]?["content"]?.ToString().Trim();

            Debug.Log($"Ответ от ИИ: {aiText}");

            feedbackText.text = aiText; // Убираем окрашивание

        }
        catch (Exception e)
        {
            feedbackText.text = "Ошибка обработки ответа от Mistral.";
            Debug.LogError($"Ошибка обработки: {e.Message}");
        }
    }
}
