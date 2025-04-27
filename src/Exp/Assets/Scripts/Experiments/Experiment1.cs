using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Сожержит необходимые данные для эксперимента1 и контролирует его выполнение.
/// </summary>
public class Experiment1 : MonoBehaviour, IExperiment
{
    [SerializeField]
    private ExperimentStuffBase stuff;

    [SerializeField]
    private TestTubeStand stand;

    private ExperimentData expData;

    public string Title => expData.Title;

    public string Description => expData.Description;

    private bool isInitialized;

    /// <inheritdoc/>
    public event Action Completed;

    /// <inheritdoc/>
    public void Initialize(ExperimentData expData)
    {
        this.expData = expData;

        foreach (var stuff in stuff.StuffList)
        {
            stuff.Combined += OnStuffCombined;
        }

        isInitialized = true;
    }

    private void OnDestroy()
    {
        if (!isInitialized)
        {
            return;
        }

        foreach (var stuff in stuff.StuffList)
        {
            stuff.Combined -= OnStuffCombined;
        }
    }

    /// <inheritdoc/>
    public void Reset()
    {
        stuff.gameObject.SetActive(false);
    }

    /// <inheritdoc/>
    public void Start()
    {
        stuff.gameObject.SetActive(true);

        foreach (var stuff in stuff.StuffList)
        {
            stuff.EnableInteraction(true);
        }
    }

    private void OnStuffCombined()
    {
        Debug.LogWarning($"stand count {stand.TestTubes.Count}");
    }
}
