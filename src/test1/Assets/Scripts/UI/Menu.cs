using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txt_Time;

    [SerializeField]
    private Button btn_Good;

    [SerializeField]
    private Button btn_Bad;

    [SerializeField]
    private Button btn_Menu;

    [SerializeField]
    private Button btn_Bye;

    [SerializeField]
    private Button btn_HelloWorld; 

    [SerializeField]
    private Image img_Display;

    [SerializeField]
    private Sprite Poperechniy;

    [SerializeField]
    private TMP_Text txt_Message;

    [SerializeField]
    private TMP_Text txt_Message2;

    private bool isTimeOver = false;
    private bool isHelloWorldPressed = false; 

    private void Awake()
    {
        btn_Good.onClick.AddListener(OnBtnGoodClick);
        btn_Bad.onClick.AddListener(OnBtnBadClick);
        btn_Menu.onClick.AddListener(OnBtnMenuClick);
        btn_HelloWorld.onClick.AddListener(OnBtnHelloWorldClick); 
        btn_Bye.onClick.AddListener(OnBtnByeClick);
    }

    private void OnDestroy()
    {
        btn_Good.onClick.RemoveListener(OnBtnGoodClick);
        btn_Bad.onClick.RemoveListener(OnBtnBadClick);
        btn_Menu.onClick.RemoveListener(OnBtnMenuClick);
        btn_HelloWorld.onClick.RemoveListener(OnBtnHelloWorldClick); 
        btn_Bye.onClick.RemoveListener(OnBtnByeClick);

    }

    private void Update()
    {
        if (!isTimeOver)
        {
            txt_Time.text = $"Time: {(int)Time.time}";
        }
    }

    private void OnBtnGoodClick()
    {
        Debug.Log("That lesson was really GOOD!!");
    }

    private void OnBtnMenuClick()
    {
        Debug.Log("Добро пожаловать в меню");

        isTimeOver = true;

        txt_Time.text = " ";
    }

    private void OnBtnByeClick()
    {
        Debug.Log("Ты нажал на \"Досвидос\", пора прощаться, друг!");

        isTimeOver = true;

        txt_Time.text = "БАЙ БИТЧ";
    }

    private void OnBtnBadClick()
    {
        Debug.Log("ТЫ ОШИБСЯ!!!!!!!!!");

        isTimeOver = true;

        txt_Time.text = "Time is OVER";

        txt_Message.text = "Все кончено!";

        if (img_Display != null && Poperechniy != null)
        {
            img_Display.sprite = Poperechniy;
            img_Display.gameObject.SetActive(true);
        }
    }

    private void OnBtnHelloWorldClick()


    {
        Debug.Log("Кнопка \"Hello World\" нажата!");

        if (isHelloWorldPressed)
        {
            btn_Good.gameObject.SetActive(true);
            btn_Bad.gameObject.SetActive(true);
            btn_Menu.gameObject.SetActive(true);
            btn_Bye.gameObject.SetActive(true);
            txt_Message2.gameObject.SetActive(false); // Скрыть текст при повторном нажатии

        }
        else
        {
            btn_Good.gameObject.SetActive(false);
            btn_Bad.gameObject.SetActive(false);
            btn_Menu.gameObject.SetActive(false);
            btn_Bye.gameObject.SetActive(false);
            txt_Message2.gameObject.SetActive(true); // Показать текст при нажатии

        }

        isHelloWorldPressed = !isHelloWorldPressed;
    }
}
