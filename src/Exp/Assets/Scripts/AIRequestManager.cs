using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System;

public class AIRequestManager : MonoBehaviour
{
    [SerializeField]
    private AIConfiguration config;

    private ExperimentData experimentData;

    public event Action<bool, string> TaskRequestCompleted;

    public event Action<bool, string> TestRequestCompleted;

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

        var jsonBody = new
        {
            model = config.AIModel,
            messages = new[] {
                new {
                    role = "user",
                    content = requestText
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

        StartCoroutine(CheckTaskRoutine(request));
    }

    private IEnumerator CheckTaskRoutine(UnityWebRequest request)
    {
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            HandleAIResponse(request.downloadHandler.text);
        }
        else
        {
            TaskRequestCompleted?.Invoke(false, "Ошибка связи с Mistral API. Попробуйте снова.");
        }
    }

    void HandleAIResponse(string response)
    {
        try
        {
            var parsedResponse = JObject.Parse(response);
            string aiText = parsedResponse["choices"]?[0]?["message"]?["content"]?.ToString().Trim();
            TaskRequestCompleted?.Invoke(true, aiText);
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка обработки: {e.Message}");
            TaskRequestCompleted?.Invoke(false, "Ошибка обработки ответа от Mistral.");
        }
    }
}
