using UnityEngine;
using System;

/// <summary>
/// Сожержит необходимые данные для эксперимента1 и контролирует его выполнение.
/// </summary>
public class Experiment1 : MonoBehaviour, IExperiment
{
    [SerializeField]
    private ExperimentStuffBase stuff;

    [Space]
    [SerializeField]
    private TestTubeStand stand;

    [SerializeField]
    private TestTube testTube1;

    [SerializeField]
    private TestTube testTube2;

    [SerializeField]
    private TestTube testTube3;

    [SerializeField]
    private TestTube testTube4;

    [Space]
    [SerializeField]
    private SolidReactiveGroup Mg;

    [SerializeField]
    private SolidReactiveGroup Fe;

    [SerializeField]
    private SolidReactiveGroup Zn;

    [SerializeField]
    private SolidReactiveGroup Cu;

    [SerializeField]
    private Beaker HCl;

    private ExperimentData expData;

    public string Title => expData.Title;

    public string Description => expData.Description;

    private bool isInitialized;

    private int stepIndex;

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
        stepIndex = 0;
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
    }

    private void SetupStep(int stepIndex)
    {
        switch (stepIndex)
        {
            case 0:

                break;
        }
    }
}
