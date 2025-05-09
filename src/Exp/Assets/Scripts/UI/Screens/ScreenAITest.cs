using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenAITest : ScreenBase
{
    [SerializeField]
    private Button btn_Check;

    [SerializeField]
    private TMP_Text txt_BtnCheck;

    [SerializeField]
    private Button btn_Result;

    [Header("Questions")]
    [SerializeField]
    private TMP_Text txt_Question1;

    [SerializeField]
    private TMP_InputField if_Question1;

    [Space]
    [SerializeField]
    private TMP_Text txt_Question2;

    [SerializeField]
    private TMP_InputField if_Question2;

    [Space]
    [SerializeField]
    private TMP_Text txt_Question3;

    [SerializeField]
    private TMP_InputField if_Question3;

    [Space]
    [SerializeField]
    private TMP_Text txt_Answer;

    public Button Btn_Result => btn_Result;

    public event Action<(string, string)[]> CheckCLicked;

    public event Action ResultCLicked;

    private void Awake()
    {
        btn_Check.onClick.AddListener(OnBtnCheckClicked);
        btn_Result.onClick.AddListener(OnBtnResultClicked);
    }

    private void OnDestroy()
    {
        btn_Check.onClick.RemoveListener(OnBtnCheckClicked);
        btn_Result.onClick.RemoveListener(OnBtnResultClicked);
    }

    public void StartQuestionGenerating()
    {
        btn_Check.interactable = false;
        txt_Question1.text = "Тест генерируется...";
        txt_Question2.text = "Тест генерируется...";
        txt_Question3.text = "Тест генерируется...";
    }

    public void SetQuestions(string[] questions)
    {
        btn_Check.interactable = true;
        txt_Question1.text = questions[0];
        txt_Question2.text = questions[1];
        txt_Question3.text = questions[2];
    }

    public void SetQuestionsAnswer(string answer)
    {
        txt_Answer.text = answer;
        LockBtnCheck(false);
    }

    private void OnBtnCheckClicked()
    {
        LockBtnCheck(true);

        var results = new (string, string)[3]
        {
            (txt_Question1.text, if_Question1.text),
            (txt_Question2.text, if_Question2.text),
            (txt_Question3.text, if_Question3.text),
        };

        CheckCLicked?.Invoke(results);
    }

    private void OnBtnResultClicked()
    {
        ResultCLicked?.Invoke();
    }

    private void LockBtnCheck(bool isLocked)
    {
        btn_Check.interactable = !isLocked;
        txt_BtnCheck.text = isLocked ? "Ждём ответ..." : "Проверить";
    }
}
