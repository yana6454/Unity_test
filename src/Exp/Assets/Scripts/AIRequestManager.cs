using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System;

public class AIRequestManager : MonoBehaviour
{
    private enum RequestType
    {
        Task,
        TestGeneration,
        TestCheck,
        Result
    }

    [SerializeField]
    private AIConfiguration config;

    private ExperimentData experimentData;

    public event Action<bool, string> TaskCheckCompleted;

    public event Action<bool, string[]> TestGernerated;

    public event Action<bool, string> TestCheckCompleted;

    public void SetExperimentData(ExperimentData data)
    {
        experimentData = data;
    }

    public void CheckTask(string studentAnswer)
    {
        var requestText = 
            config.TaskStartPrompt + "\n" +
            $"Текст задания: \n{experimentData.Description}\n" +
            $"Дополнительная информация по эксперементу от учителя: " +
            experimentData.AITaskAdditionalInfo + "\n\n" +
            $"Ответ ученика: \n{studentAnswer}\n\n" +
            config.TaskEndPrompt;

        var request = CreateRequest(requestText);
        StartCoroutine(RequestRoutine(request, RequestType.Task));
    }

    public void GenerateTest()
    {
        var requestText =
            config.TestGenerationStartPrompt + "\n" +
            experimentData.Description + "\n\n" +
            config.TestGenerationEndPrompt;

        var request = CreateRequest(requestText);
        StartCoroutine(RequestRoutine(request, RequestType.TestGeneration));
    }

    public void CheckTest((string, string)[] questions)
    {
        string combinedAnswers = "";
        foreach (var q in questions)
        {
            combinedAnswers += $"Вопрос: {q.Item1}\nОтвет: {q.Item2}\n";
        }

        var requestText =
            config.TestCheckStartPrompt + "\n\n" +
            combinedAnswers + "\n" +
            config.TestCheckEndPrompt;

        var request = CreateRequest(requestText);
        StartCoroutine(RequestRoutine(request, RequestType.TestCheck));
    }

    private UnityWebRequest CreateRequest(string content)
    {
        var jsonBody = new
        {
            model = config.AIModel,
            messages = new[] {
                new {
                    role = "user",
                    content = content
                }
            }
        };

        string jsonString = JsonConvert.SerializeObject(jsonBody);

        UnityWebRequest request = new UnityWebRequest(config.APIUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {config.APIKey}");
        request.timeout = 30;

        return request;
    }

    private IEnumerator RequestRoutine(UnityWebRequest request, RequestType type)
    {
        yield return request.SendWebRequest();

        (bool, string) routineResult;

        if (request.result == UnityWebRequest.Result.Success)
        {
            routineResult = HandleResponse(request.downloadHandler.text);
        }
        else
        {
            routineResult = (false, "Ошибка связи с Mistral API. Попробуйте снова.");
            // TaskRequestCompleted?.Invoke(false, "Ошибка связи с Mistral API. Попробуйте снова.");
        }

        switch (type)
        {
            case RequestType.Task:
                TaskCheckCompleted?.Invoke(routineResult.Item1, routineResult.Item2);
                break;
            case RequestType.TestGeneration:
                string[] questions = routineResult.Item2.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                TestGernerated?.Invoke(routineResult.Item1, questions);
                break;
            case RequestType.TestCheck:
                TestCheckCompleted?.Invoke(routineResult.Item1, routineResult.Item2);
                break;
            case RequestType.Result:
                break;
        }
    }

    private (bool, string) HandleResponse(string response)
    {
        try
        {
            var parsedResponse = JObject.Parse(response);
            string aiText = parsedResponse["choices"]?[0]?["message"]?["content"]?.ToString().Trim();
            return (true, aiText);
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка обработки: {e.Message}");
            return (false, "Ошибка обработки ответа от Mistral.");
        }
    }
}
