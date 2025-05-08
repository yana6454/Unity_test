using System;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    [SerializeField]
    private InteractionManager interactionManager;

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

    private readonly ScreenBase[] screens = new ScreenBase[3];

    public event Action<bool> InteractionEnable;

    public event Action<int> BtnExperimentStartClicked;

    private void Awake()
    {
        menuCamera.SetCameraActive(true);
        gameCamera.SetCameraActive(false);

        screens[0] = menu;
        screens[1] = game;
        screens[2] = task;

        menu.StartClicked += OnStartClicked;
        game.TaskCkicked += OnTaskClicked;
        game.NextCkicked += OnBtnNextCkicked;
        task.BackCLicked += OnTaskBackClicked;
    }

    private void OnDestroy()
    {
        menu.StartClicked -= OnStartClicked;
        game.TaskCkicked -= OnTaskClicked;
        game.NextCkicked -= OnBtnNextCkicked;
        task.BackCLicked -= OnTaskBackClicked;
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

    public void UpdateExperimentData(string title, string description, string[] todoList)
    {
        task.SetExperimentData(title, description, todoList);
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

    private void OnBtnNextCkicked()
    {
        Debug.LogWarning("There will be AI block.");
    }

    private void ReturnToMenu()
    {
        game.Hide();
        task.Hide();
        menu.Show();
        interactionManager.EnableInteractions(false);
        gameCamera.SetCameraActive(false);
        menuCamera.SetCameraActive(true);
    }

    private void OnTaskBackClicked()
    {
        // Делаю фейковое сокрытие, чтобы оставить список этапов всегда видимым.
        task.FakeHide();
        game.Show();
        interactionManager.EnableInteractions(true);
    }
}
