using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txt_Time;

    private float time;

    private void Update()
    {
        txt_Time.text = $"Time: {(int)Time.time}";
    }
}
