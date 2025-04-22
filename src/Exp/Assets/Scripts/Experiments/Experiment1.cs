using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Сожержит необходимые данные для эксперимента1 и контролирует его выполнение.
/// </summary>
public class Experiment1 : IExperiment
{
    private ExperimentConfig config;

    public string Title => config.Title;

    public string Description => config.Description;

    private ExperimentStuffBase stuff;

    public ExperimentStuffBase Stuff => stuff;

    private List<SelectableBase> stuffList;

    public void Initialize(ExperimentConfig config, Transform stuffParent)
    {
        this.config = config;
        stuff = Object.Instantiate(config.Stuff, stuffParent);
        stuffList = stuff.GetStuffList();

        foreach (var stuff in stuffList)
        {
            stuff.Combined += OnStuffCombined;
        }
    }

    public void Start()
    {
    }

    private void Destroy()
    {
        foreach (var stuff in stuffList)
        {
            stuff.Combined -= OnStuffCombined;
        }
    }

    private void OnStuffCombined()
    {
    }
}
