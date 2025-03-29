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

    private void Awake()
    {
        menuCamera.enabled = true;
        gameCamera.enabled = false;

        screens[0] = menu;
        screens[1] = game;
        screens[2] = task;

        menu.StartClicked += OnStartClicked;
    }

    private void OnDestroy()
    {
        menu.StartClicked -= OnStartClicked;
    }

    private void Start()
    {
        foreach (var screen in screens)
        {
            screen.Hide();
        }

        menu.Show();
    }

    private void OnStartClicked()
    {
        menu.Hide();
        game.Show();
        menuCamera.enabled = false;
        gameCamera.enabled = true;
    }
}
