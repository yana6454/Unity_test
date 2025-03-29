using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class ScreenBase : MonoBehaviour, IScreen
{
    private Canvas cnv_Screen;

    private void Start()
    {
        cnv_Screen = GetComponent<Canvas>();
    }

    public void Show()
    {
        cnv_Screen.enabled = true;
    }

    public void Hide()
    {
        cnv_Screen.enabled = false;
    }
}
