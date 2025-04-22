using UnityEngine;

/// <summary>
/// Сожержит необходимые данные для эксперимента1 и контролирует его выполнение.
/// </summary>
public class Experiment1 : IExperiment
{
    [SerializeField]
    private ExperimentConfig config;

    public string Title => config.Title;

    public string Description => config.Description;

    public ExperimentStuff Stuff => config.Stuff;

    public void Start()
    {
    }
}
