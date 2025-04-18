using System;
using UnityEngine;
using UnityEngine.UI;

public class ScreenGame : ScreenBase
{
    [SerializeField]
    private Button btn_Task;

    [SerializeField]
    private Button btn_Check;

    public event Action TaskCkicked;
    
    public event Action CheckCkicked;

    private void Awake()
    {
        btn_Task.onClick.AddListener(OnBtnTaskClicked);
        btn_Check.onClick.AddListener(OnBtnCheckClicked);
    }

    private void OnDestroy()
    {
        btn_Task.onClick.RemoveListener(OnBtnTaskClicked);
        btn_Check.onClick.RemoveListener(OnBtnCheckClicked);
    }

    private void OnBtnTaskClicked()
    {
        TaskCkicked?.Invoke();
    }

    private void OnBtnCheckClicked()
    {
        CheckCkicked?.Invoke();
    }
}
