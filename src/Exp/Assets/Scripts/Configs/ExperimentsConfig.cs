using System;
using UnityEngine;

/// <summary>
/// Описывает конфигурацию экспериментов, здесь можно задать необходимые для эксперимента данные.
/// </summary>
[CreateAssetMenu(fileName = "ExperimentsConfig", menuName = "ScriptableObjects/ExperimentsConfig", order = 0)]
public class ExperimentsConfig : ScriptableObject
{
    [Header("Experiment 1")]
    [SerializeField]
    public ExperimentData exp1;
}

/// <summary>
/// Сожержит данные для одного эксперимента.
/// </summary>
[Serializable]
public struct ExperimentData
{
    [Header("Experiment Block")]
    [SerializeField]
    [TextArea(1, 2)]
    public string Title;

    [SerializeField]
    [TextArea(3, 10)]
    public string Description;

    [SerializeField]
    [TextArea(1, 2)]
    public string[] TODOList;

    [Header("AI Block")]
    [SerializeField]
    [TextArea(2, 4)]
    public string AITaskText;

    public ExperimentData(
        string title,
        string description,
        string[] todo,
        string aiTaskText)
    {
        Title = title;
        Description = description;
        TODOList = todo;
        AITaskText = aiTaskText;
    }
}
