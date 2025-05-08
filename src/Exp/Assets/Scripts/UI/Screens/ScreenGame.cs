using System;
using UnityEngine;
using UnityEngine.UI;

public class ScreenGame : ScreenBase
{
    [SerializeField]
    private Button btn_Task;

    [SerializeField]
    private Button btn_Next;

    public event Action TaskCkicked;
    
    public event Action NextCkicked;

    private void Awake()
    {
        btn_Task.onClick.AddListener(OnBtnTaskClicked);
        btn_Next.onClick.AddListener(OnBtnNextClicked);
        btn_Next.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        btn_Task.onClick.RemoveListener(OnBtnTaskClicked);
        btn_Next.onClick.RemoveListener(OnBtnNextClicked);
    }

    public void SetBtnNextActive(bool isActive)
    {
        btn_Next.gameObject.SetActive(isActive);
    }

    private void OnBtnTaskClicked()
    {
        TaskCkicked?.Invoke();
    }

    private void OnBtnNextClicked()
    {
        NextCkicked?.Invoke();
    }
}
