using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Описывает настройки блока с ИИ.
/// </summary>
[CreateAssetMenu(fileName = "AIConfig", menuName = "ScriptableObjects/AIConfig", order = 1)]
public class AIConfiguration : ScriptableObject
{
    [Header("Mistral Settings")]
    [SerializeField]
    public string APIKey;

    [SerializeField]
    public string APIUrl;

    [SerializeField]
    public string AIModel;

    [Header("AI Task")]
    [SerializeField]
    [TextArea(1, 4)]
    public string TaskStartPrompt;

    [SerializeField]
    [TextArea(1, 4)]
    public string TaskEndPrompt;
}
