using System;
using UnityEngine;

/// <summary>
/// Manages input signals.
/// </summary>
public class InputManager : MonoBehaviour
{
    /// <summary>
    /// Notiify about Primary (left) button click.
    /// </summary>
    public event Action PrimaryButtonClicked;

    private bool isInputActive = true;

    private void Update()
    {
        if (!isInputActive)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PrimaryButtonClicked?.Invoke();
        }
    }
}
