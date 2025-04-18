using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTask : ScreenBase
{
    [SerializeField]
    private Button btn_Back;

    [SerializeField]
    private TMP_Text txt_TaskText;

    public event Action BackCLicked;

    private void Awake()
    {
        btn_Back.onClick.AddListener(OnBtnBackClicked);
    }

    private void OnDestroy()
    {
        btn_Back.onClick.RemoveListener(OnBtnBackClicked);
    }

    public void SetTaskText(string text)
    {
        txt_TaskText.text = text;
    }

    private void OnBtnBackClicked()
    {
        BackCLicked?.Invoke();
    }
}
