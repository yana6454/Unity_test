using UnityEngine;
using System;

/// <summary>
/// Сожержит необходимые данные для эксперимента1 и контролирует его выполнение.
/// </summary>
public class Experiment2 : MonoBehaviour, IExperiment
{
    [SerializeField]
    private ExperimentStuffBase stuff;

    [Space]
    [SerializeField]
    private TestTubeStand stand;

    [SerializeField]
    private TestTube testTube;

    [SerializeField]
    private Beaker FeCl3;

    [SerializeField]
    private Beaker NH4SCN;

    [SerializeField]
    private Material bloodMaterial;

    private ExperimentData expData;

    public ExperimentData ExperimentData => expData;

    private bool isInitialized;

    private int stepIndex;

    /// <inheritdoc/>
    public event Action<int> StepCompleted;

    /// <inheritdoc/>
    public event Action<Transform> Completed;

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

        SetupStep(stepIndex);
    }

    private void OnStuffCombined()
    {
        if (CheckStepCompletion(stepIndex))
        {
            StepCompleted?.Invoke(stepIndex);
            stepIndex++;
            Debug.Log($"Этап {stepIndex} выполнен");
            SetupStep(stepIndex);
        }
    }

    private void SetupStep(int stepIndex)
    {
        // Выключаем интеракцию у всех объектов.
        foreach (var obj in stuff.StuffList)
        {
            obj.EnableInteraction(false);
        }

        // А здесь включаем только у тех, какие будут активные на этом этапе.
        switch (stepIndex)
        {
            case 0: // Поместить пробирку в стэнд.
                testTube.EnableInteraction(true);
                stand.EnableInteraction(true);
                break;
            case 1: // Налить хлорид железа.
                testTube.EnableInteraction(true);
                FeCl3.EnableInteraction(true);
                break;
            case 2: // Капнуть роданид аммония.
                testTube.EnableInteraction(true);
                NH4SCN.EnableInteraction(true);
                break;
            case 3: // Завершение эксперимента.
                testTube.SetLiquidMaterial(bloodMaterial);
                Completed?.Invoke(stuff.EndCameraPosition);
                break;
        }
    }

    private bool CheckStepCompletion(int stepIndex)
    {
        // Проверяем выполнилось ли условтие текущего этапа,
        // если да, увеличиваем индекс этапа.
        switch (stepIndex)
        {
            case 0: // Проверяем чтобы количество пробирок в стэнде было 1.
                if (stand.TestTubeCount == 1)
                {
                    return true;
                }
                break;
            case 1: // В пробирке есть хлорид железа.
                if (testTube.LiquidAmount > 0.0)
                {
                    return true;
                }
                break;
            case 2: // В пробирку капнули роданид аммония.
                if (testTube.LiquidAmount > 0.3)
                {
                    return true;
                }
                break;
        }

        return false;
    }
}
