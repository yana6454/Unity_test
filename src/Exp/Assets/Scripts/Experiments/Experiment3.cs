using UnityEngine;
using System;

/// <summary>
/// Сожержит необходимые данные для эксперимента1 и контролирует его выполнение.
/// </summary>
public class Experiment3 : MonoBehaviour, IExperiment
{
    private ExperimentData expData;

    public ExperimentData ExperimentData => expData;

    private int stepIndex;

    /// <inheritdoc/>
    public event Action<int> StepCompleted;

    /// <inheritdoc/>
    public event Action<Transform> Completed;

    /// <inheritdoc/>
    public void Initialize(ExperimentData expData)
    {
        this.expData = expData;
    }

    /// <inheritdoc/>
    public void Reset()
    {
    }

    /// <inheritdoc/>
    public void Start()
    {
    }
}
