using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class ScreenController : MonoBehaviour
{
    [SerializeField]
    private ScreenMenu menu;

    [SerializeField]
    private ScreenGame game;

    [SerializeField]
    private ScreenTask task;

    private readonly ScreenBase[] screens = new ScreenBase[3];

    private void Awake()
    {
        screens[0] = menu;
        screens[1] = game;
        screens[2] = task;
    }

    private void Start()
    {
        foreach (var screen in screens)
        {
            screen.Hide();
        }

        menu.Show();
    }
}
