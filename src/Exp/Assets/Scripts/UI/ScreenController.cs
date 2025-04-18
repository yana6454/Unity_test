using System;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField]
    private Camera menuCamera;

    [SerializeField]
    private Camera gameCamera;

    [Header("Screens")]
    [SerializeField]
    private ScreenMenu menu;

    [SerializeField]
    private ScreenGame game;

    [SerializeField]
    private ScreenTask task;

    private readonly ScreenBase[] screens = new ScreenBase[3];

    public event Action<bool> InteractionEnable;

    private void Awake()
    {
        menuCamera.enabled = true;
        gameCamera.enabled = false;

        screens[0] = menu;
        screens[1] = game;
        screens[2] = task;

        menu.StartClicked += OnStartClicked;
        game.TaskCkicked += OnTaskClicked;
        game.CheckCkicked += OnCheckCkicked;
        task.BackCLicked += OnTaskBackClicked;
    }

    private void OnDestroy()
    {
        menu.StartClicked -= OnStartClicked;
        game.TaskCkicked -= OnTaskClicked;
        game.CheckCkicked -= OnCheckCkicked;
        task.BackCLicked -= OnTaskBackClicked;
    }

    private void Start()
    {
        foreach (var screen in screens)
        {
            screen.Hide();
        }

        menu.Show();
        InteractionEnable?.Invoke(false);
    }

    private void OnStartClicked()
    {
        menu.Hide();
        game.Show();
        menuCamera.enabled = false;
        gameCamera.enabled = true;
        InteractionEnable?.Invoke(true);
    }

    private void OnTaskClicked()
    {
        game.Hide();
        task.Show();
        InteractionEnable?.Invoke(false);
    }

    private void OnCheckCkicked()
    {
        Debug.LogWarning("Thwre will be check of task completion");
    }

    private void OnTaskBackClicked()
    {
        task.Hide();
        game.Show();
        InteractionEnable?.Invoke(false);
    }
}
