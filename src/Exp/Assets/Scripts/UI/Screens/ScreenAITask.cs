using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenAITask : ScreenBase
{
    [SerializeField]
    private Button btn_Check;

    [SerializeField]
    private TMP_Text txt_BtnNext;

    [SerializeField]
    private Button btn_Next;

    [Header("AI Task")]
    [SerializeField]
    private TMP_Text txt_AITask;

    [SerializeField]
    private TMP_InputField if_AITaskInput;

    [SerializeField]
    private TMP_Text txt_AIAnswer;

    public Button Btn_Next => btn_Next;

    public string AITaskResult => txt_AIAnswer.text;

    public event Action<string> CheckCLicked;

    public event Action NextCLicked;

    private void Awake()
    {
        btn_Check.onClick.AddListener(OnBtnCheckClicked);
        btn_Next.onClick.AddListener(OnBtnNextClicked);
    }

    private void OnDestroy()
    {
        btn_Check.onClick.RemoveListener(OnBtnCheckClicked);
        btn_Next.onClick.RemoveListener(OnBtnNextClicked);
    }

    public void SetAITask(string aiTaskText)
    {
        txt_AITask.text = aiTaskText;
    }

    public void SetAIAnswer(string answer)
    {
        txt_AIAnswer.text = answer;
        LockBtnCheck(false);
    }

    private void OnBtnCheckClicked()
    {
        LockBtnCheck(true);
        CheckCLicked?.Invoke(if_AITaskInput.text);
    }

    private void OnBtnNextClicked()
    {
        NextCLicked?.Invoke();
    }

    private void LockBtnCheck(bool isLocked)
    {
        btn_Check.interactable = !isLocked;
        txt_BtnNext.text = isLocked ? "Ждём ответ..." : "Проверить";
    }
}
