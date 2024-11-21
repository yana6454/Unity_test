using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txt_Time;

    [SerializeField]
    private Button btn_Good;

    private void Awake()
    {
        btn_Good.onClick.AddListener(OnBtnGoodClick);
    }

    private void OnDestroy()
    {
        btn_Good.onClick.RemoveListener(OnBtnGoodClick);
    }

    private void Update()
    {
        txt_Time.text = $"Time: {(int)Time.time}";
    }

    private void OnBtnGoodClick()
    {
        Debug.Log("That lesson was really GOOD!!");
    }
}
