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

    [Header("TODO List")]
    [SerializeField]
    private Transform grp_TODOList;

    [SerializeField]
    private Transform grp_List;

    [SerializeField]
    private TMP_Text[] todoList;

    [Header("fake Show/Hide")]
    [SerializeField]
    private GameObject[] hiddenObjects;

    public Transform TODOList => grp_TODOList;

    public event Action BackCLicked;

    private void Awake()
    {
        btn_Back.onClick.AddListener(OnBtnBackClicked);
    }

    private void OnDestroy()
    {
        btn_Back.onClick.RemoveListener(OnBtnBackClicked);
    }

    public void FakeShow()
    {
        Show();

        foreach (var item in hiddenObjects)
        {
            item.SetActive(true);
        }
    }

    public void FakeHide()
    {
        foreach (var item in hiddenObjects)
        {
            item.SetActive(false);
        }
    }

    /// <summary>
    /// Обновляет текст названия эксперимента и его описание.
    /// </summary>
    public void SetExperimentData(string title, string description, string[] todoTexts)
    {
        txt_Title.text = title;
        txt_TaskText.text = description;

        for (int i = 0; i < todoList.Length; i++)
        {
            if (i < todoTexts.Length)
            {
                todoList[i].gameObject.SetActive(true);
                todoList[i].text = $"{i}. {todoTexts[i]}"; ;
            }
            else
            {
                todoList[i].gameObject.SetActive(false);
            }
        }
    }

    public void StrikeCompletedStep(int stepIndex)
    {
        if (stepIndex < todoList.Length)
        {
            todoList[stepIndex].text = $"<s>{todoList[stepIndex].text}</s>";
        }
    }

    private void OnBtnBackClicked()
    {
        BackCLicked?.Invoke();
    }
}
