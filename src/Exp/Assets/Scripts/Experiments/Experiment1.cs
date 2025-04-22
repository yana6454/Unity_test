using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Сожержит необходимые данные для эксперимента1 и контролирует его выполнение.
/// </summary>
public class Experiment1 : IExperiment
{
    [SerializeField]
    private ExperimentConfig config;

    public string Title => config.Title;

    public string Description => config.Description;

    public ExperimentStuffBase Stuff => config.Stuff;

    private List<SelectableBase> stuffList;

    public void Start()
    {
        stuffList = config.Stuff.GetStuffList();

        foreach (var stuff in stuffList)
        {
            stuff.Combined += OnStuffCombined;
        }
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
