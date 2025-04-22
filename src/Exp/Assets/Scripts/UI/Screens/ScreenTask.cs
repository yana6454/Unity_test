using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTask : ScreenBase
{
    [SerializeField]
    private Button btn_Back;

    [SerializeField]
    private TMP_Text txt_Title;

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

    /// <summary>
    /// Обновляет текст названия эксперимента и его описание.
    /// </summary>
    public void SetExperimentData(string title, string description)
    {
        txt_Title.text = title;
        txt_TaskText.text = description;
    }

    private void OnBtnBackClicked()
    {
        BackCLicked?.Invoke();
    }
}
