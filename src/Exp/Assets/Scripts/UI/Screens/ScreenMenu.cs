using System;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMenu : ScreenBase
{
    [SerializeField]
    private Button btn_Start;

    [SerializeField]
    private Button btn_Settings;

    public event Action StartClicked;

    private void Awake()
    {
        btn_Start.onClick.AddListener(OnBtnStartClicked);
        btn_Settings.onClick.AddListener(OnBtnSettingsClicked);
    }

    private void OnDestroy()
    {
        btn_Start.onClick.RemoveListener(OnBtnStartClicked);
        btn_Settings.onClick.RemoveListener(OnBtnSettingsClicked);
    }

    private void OnBtnStartClicked()
    {
        StartClicked?.Invoke();
    }

    private void OnBtnSettingsClicked()
    {
    }
}
