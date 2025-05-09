using System;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMenu : ScreenBase
{
    [SerializeField]
    private Button btn_Start;

    [SerializeField]
    private Button btn_Test2;

    [SerializeField]
    private Button btn_Test3;

    [SerializeField]
    private Button btn_Settings;

    public event Action<int> StartClicked;

    private void Awake()
    {
        btn_Start.onClick.AddListener(OnBtnStartClicked);
        btn_Settings.onClick.AddListener(OnBtnSettingsClicked);

        btn_Test2.onClick.AddListener(OnBtnTest2Clicked);
        btn_Test3.onClick.AddListener(OnBtnTest3Clicked);
    }

    private void OnDestroy()
    {
        btn_Start.onClick.RemoveListener(OnBtnStartClicked);
        btn_Settings.onClick.RemoveListener(OnBtnSettingsClicked);

        btn_Test2.onClick.RemoveListener(OnBtnTest2Clicked);
        btn_Test3.onClick.RemoveListener(OnBtnTest3Clicked);
    }

    private void OnBtnStartClicked()
    {
        StartClicked?.Invoke(0);
    }

    private void OnBtnTest2Clicked()
    {
        StartClicked?.Invoke(1);
    }

    private void OnBtnTest3Clicked()
    {
        StartClicked?.Invoke(2);
    }

    private void OnBtnSettingsClicked()
    {
    }
}
