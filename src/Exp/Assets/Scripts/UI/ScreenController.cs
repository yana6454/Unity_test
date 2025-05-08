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

    public event Action<bool> InteractionEnable;

    public event Action<int> BtnExperimentStartClicked;

    private readonly ScreenBase[] screens = new ScreenBase[4];

    private ExperimentData data;

    private void Awake()
    {
        menuCamera.SetCameraActive(true);
        gameCamera.SetCameraActive(false);

        screens[0] = menu;
        screens[1] = game;
        screens[2] = task;
        screens[3] = aiTask;

        menu.StartClicked += OnStartClicked;
        game.TaskCkicked += OnTaskClicked;
        game.NextCkicked += OnGameNextCkicked;
        task.BackCLicked += OnTaskBackClicked;
        aiTask.CheckCLicked += OnAITaskCheckClicked;
        aiTask.NextCLicked += OnAITaskNextClicked;
    }

    private void OnDestroy()
    {
        menu.StartClicked -= OnStartClicked;
        game.TaskCkicked -= OnTaskClicked;
        game.NextCkicked -= OnGameNextCkicked;
        task.BackCLicked -= OnTaskBackClicked;
        aiTask.CheckCLicked -= OnAITaskCheckClicked;
        aiTask.NextCLicked -= OnAITaskNextClicked;
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
        this.data = data;
        task.SetExperimentData(data.Title, data.Description, data.TODOList);
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

    private void OnStartClicked()
    {
        menu.Hide();
        game.Show();
        menuCamera.SetCameraActive(false);
        gameCamera.SetCameraActive(true);
        interactionManager.EnableInteractions(true);
        BtnExperimentStartClicked?.Invoke(0);
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
        aiRequestManager.TaskRequestCompleted += OnAITaskRequestCompleted;
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
        aiRequestManager.TaskRequestCompleted -= OnAITaskRequestCompleted;
    }

    private void OnAITaskRequestCompleted(bool success, string response)
    {
        aiTask.SetAIAnswer(response);
        aiTask.Btn_Next.gameObject.SetActive(success);
    }

    private void OnAITestRequestCompleted(bool success, string response)
    {

    }
}
