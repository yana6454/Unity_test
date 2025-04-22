using UnityEngine;

/// <summary>
/// Описывает конфигурацию эксперимента, здесь можно задать необходимые для эксперимента данные.
/// </summary>
[CreateAssetMenu(fileName = "ExperimentConfig", menuName = "ScriptableObjects/ExperimentConfig", order = 0)]
public class ExperimentConfig : ScriptableObject
{
    [SerializeField]
    [TextArea(1,2)]
    private string title;

    [SerializeField]
    [TextArea(3,10)]
    private string description;

    [Space]
    [SerializeField]
    private ExperimentStuff stuffPrefab;

    public string Title => title;

    public string Description => description;

    public ExperimentStuff Stuff => stuffPrefab;
}
