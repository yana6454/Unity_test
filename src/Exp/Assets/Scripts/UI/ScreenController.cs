using System;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    [SerializeField]
    private InteractionManager interactionManager;

    [SerializeField]
    private AIRequestManager aiRequestManager;

    [Header("Cameras")]
    [SerializeField]
    private CameraController menuCamera;

    [SerializeField]
    private CameraController gameCamera;

    [Header("Screens")]
    [SerializeField]
    private ScreenMenu menu;

    [SerializeField]
    private ScreenGame game;

    [SerializeField]
    private ScreenTask task;

    [SerializeField]
    private ScreenAITask aiTask;

    [SerializeField]
    private ScreenAITest aiTest;

    [SerializeField]
    private ScreenResult resultScreen;

    public event Action<bool> InteractionEnable;

    public event Action<int> BtnExperimentStartClicked;

    private ScreenBase[] screens;

    private void Awake()
    {
        menuCamera.SetCameraActive(true);
        gameCamera.SetCameraActive(false);

        screens = new ScreenBase[6]
        {
            menu, game, task, aiTask, aiTest, resultScreen
        };

        menu.StartClicked += OnStartClicked;
        game.TaskCkicked += OnTaskClicked;
        game.NextCkicked += OnGameNextCkicked;
        task.BackCLicked += OnTaskBackClicked;
        aiTask.CheckCLicked += OnAITaskCheckClicked;
        aiTask.NextCLicked += OnAITaskNextClicked;
        aiTest.CheckCLicked += OnAITestCheckClicked;
        aiTest.ResultCLicked += OnAITestResultClicked;
        resultScreen.ResultClicked += OnResultResultClicked;
        resultScreen.QuitClicked += OnQuitClicked;
    }

    private void OnDestroy()
    {
        menu.StartClicked -= OnStartClicked;
        game.TaskCkicked -= OnTaskClicked;
        game.NextCkicked -= OnGameNextCkicked;
        task.BackCLicked -= OnTaskBackClicked;
        aiTask.CheckCLicked -= OnAITaskCheckClicked;
        aiTask.NextCLicked -= OnAITaskNextClicked;
        aiTest.CheckCLicked -= OnAITestCheckClicked;
        aiTest.ResultCLicked -= OnAITestResultClicked;
        resultScreen.ResultClicked -= OnResultResultClicked;
        resultScreen.QuitClicked -= OnQuitClicked;
    }

    private void Start()
    {
        foreach (var screen in screens)
        {
            screen.Hide();
        }

        menu.Show();
        interactionManager.EnableInteractions(false);
    }

    public void UpdateExperimentData(ExperimentData data)
    {
        task.SetExperimentData(data.Title, data.Description, data.TODOList);
        aiRequestManager.SetExperimentData(data);
        aiTask.SetAITask(data.AITaskText);
    }

    public void UpdateStep(int stepIndex)
    {
        task.StrikeCompletedStep(stepIndex);
    }

    public void ShowExperimentEnding(Transform endCameraPosition)
    {
        gameCamera.Move(endCameraPosition);
        game.SetBtnNextActive(true);
    }

    private void OnStartClicked(int experimentIndex)
    {
        menu.Hide();
        game.Show();
        menuCamera.SetCameraActive(false);
        gameCamera.SetCameraActive(true);
        interactionManager.EnableInteractions(true);

        BtnExperimentStartClicked?.Invoke(experimentIndex);

        OnTaskClicked();
    }

    private void OnTaskClicked()
    {
        game.Hide();
        task.FakeShow();
        interactionManager.EnableInteractions(false);
    }

    private void OnGameNextCkicked()
    {
        game.Hide();
        task.Hide();
        aiTask.Show();
        interactionManager.EnableInteractions(false);

        aiTask.Btn_Next.gameObject.SetActive(false);
        aiRequestManager.TaskCheckCompleted += OnTaskCheckCompleted;
    }

    private void OnTaskBackClicked()
    {
        // Делаю фейковое сокрытие, чтобы оставить список этапов всегда видимым.
        task.FakeHide();
        game.Show();
        interactionManager.EnableInteractions(true);
    }

    private void OnAITaskCheckClicked(string studentAnswer)
    {
        aiRequestManager.CheckTask(studentAnswer);
    }

    private void OnAITaskNextClicked()
    {
        aiTask.Hide();
        aiRequestManager.TaskCheckCompleted -= OnTaskCheckCompleted;

        aiTest.Show();
        aiTest.Btn_Result.gameObject.SetActive(false);
        aiRequestManager.TestGernerated += OnTestGenerated;
        aiRequestManager.TestCheckCompleted += OnTestCheckCompleted;

        aiRequestManager.GenerateTest();
        aiTest.StartQuestionGenerating();
    }

    private void OnTaskCheckCompleted(bool success, string response)
    {
        aiTask.SetAIAnswer(response);
        aiTask.Btn_Next.gameObject.SetActive(success);
    }

    private void OnAITestCheckClicked((string, string)[] results)
    {
        aiRequestManager.CheckTest(results);
    }

    private void OnAITestResultClicked()
    {
        aiTest.Hide();
        aiRequestManager.TestGernerated -= OnTestGenerated;
        aiRequestManager.TestCheckCompleted -= OnTestCheckCompleted;

        aiRequestManager.ResultGot += OnResultGot;
        aiRequestManager.GetResult(aiTask.AITaskResult, aiTest.AITestResult);

        resultScreen.Show();
        resultScreen.Btn_GetResult.gameObject.SetActive(false);
    }

    private void OnTestGenerated(bool success, string[] results)
    {
        aiTest.SetQuestions(results);
    }

    private void OnTestCheckCompleted(bool success, string result)
    {
        aiTest.SetQuestionsAnswer(result);
        aiTest.Btn_Result.gameObject.SetActive(success);
    }

    private void OnResultResultClicked()
    {
        aiRequestManager.GetResult(aiTask.AITaskResult, aiTest.AITestResult);
    }

    private void OnQuitClicked()
    {
        aiRequestManager.ResultGot -= OnResultGot;
        Application.Quit();
    }

    private void OnResultGot(bool success, string result)
    {
        resultScreen.SetResult(result);
        resultScreen.Btn_GetResult.gameObject.SetActive(!success);
    }
}
