using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenResult : ScreenBase
{
    [SerializeField]
    private Button btn_GetResult;

    [SerializeField]
    private TMP_Text txt_BtnResult;

    [SerializeField]
    private Button btn_Quit;

    [Header("AI Result")]
    [SerializeField]
    private TMP_Text txt_Result;

    public Button Btn_GetResult => btn_GetResult;

    public event Action ResultClicked;

    public event Action QuitClicked;

    private void Awake()
    {
        btn_GetResult.onClick.AddListener(OnBtnGetRsultClicked);
        btn_Quit.onClick.AddListener(OnBtnQuitClicked);
    }

    private void OnDestroy()
    {
        btn_GetResult.onClick.RemoveListener(OnBtnGetRsultClicked);
        btn_Quit.onClick.RemoveListener(OnBtnQuitClicked);
    }

    public void SetResult(string result)
    {
        LockButtons(false);
        txt_Result.text = result;
    }

    private void OnBtnGetRsultClicked()
    {
        LockButtons(true);
        ResultClicked?.Invoke();
    }

    private void OnBtnQuitClicked()
    {
        QuitClicked?.Invoke();
    }

    private void LockButtons(bool isLocked)
    {
        btn_GetResult.interactable = !isLocked;
        btn_Quit.interactable = !isLocked;
        txt_BtnResult.text = isLocked ? "Ждём ответ..." : "Проверить";
    }
}
