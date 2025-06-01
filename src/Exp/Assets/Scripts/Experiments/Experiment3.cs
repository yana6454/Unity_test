using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Сожержит необходимые данные для эксперимента3 и контролирует его выполнение.
/// </summary>
public class Experiment3: MonoBehaviour, IExperiment
{
    [SerializeField]
    private ExperimentStuffBase stuff;

    [Space]
    [SerializeField]
    private TestTubeStand stand;

    [SerializeField]
    private TestTube[] testTubes;

    [SerializeField]
    private Beaker KMnO4;

    [SerializeField]
    private Beaker H2SO4;

    [SerializeField]
    private Beaker H2O2;

    [SerializeField]
    private Beaker H2O;

    [SerializeField]
    private Beaker NaOH;

    [Header("Result liquid colors")]
    [SerializeField]
    private Material liquidTransparent;

    [SerializeField]
    private Material liquidBrown;

    [SerializeField]
    private Material liquidGreen;

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
            case 0: // Поместить пробирки в стэнд.
                foreach (var tube in testTubes)
                {
                    tube.EnableInteraction(true);
                }

                stand.EnableInteraction(true);
                break;
            case 1: // Налить перманганат калия в пробирки.
                foreach (var tube in testTubes)
                {
                    tube.EnableInteraction(true);
                }
                KMnO4.EnableInteraction(true);
                break;
            case 2: // Налить серную кислоту.
                testTubes[0].EnableInteraction(true);
                H2SO4.EnableInteraction(true);
                break;
            case 3: // Добавить перекись.
                testTubes[0].EnableInteraction(true);
                H2O2.EnableInteraction(true);
                break;
            case 4: // Добавить воду в пробирку.
                testTubes[0].SetLiquidMaterial(liquidTransparent);
                testTubes[1].EnableInteraction(true);
                H2O.EnableInteraction(true);
                break;
            case 5: // Добавить перекись.
                testTubes[1].EnableInteraction(true);
                H2O2.EnableInteraction(true);
                break;
            case 6: // Добавить раствор щелочи.
                testTubes[1].SetLiquidMaterial(liquidBrown);
                testTubes[2].EnableInteraction(true);
                NaOH.EnableInteraction(true);
                break;
            case 7: // Добавить перекись.
                testTubes[2].EnableInteraction(true);
                H2O2.EnableInteraction(true);
                break;
            case 8: // Заверпшение эксперимента
                testTubes[2].SetLiquidMaterial(liquidGreen);
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
            case 0: // Проверяем чтобы количество пробирок в стэнде было 3.
                if (stand.TestTubeCount == 3)
                {
                    return true;
                }

                break;
            case 1: // Чтобы в каждой пробирке был перманганат калия.
                bool allHaveKMnO4 = true;
                foreach (var tube in testTubes)
                {
                    allHaveKMnO4 &= tube.LiquidAmount > 0;
                }

                if (allHaveKMnO4)
                {
                    return true;
                }

                break;
            case 2:
                if (testTubes[0].LiquidAmount > KMnO4.LiquidPortionSize)
                {
                    return true;
                }
                break;
            case 3:
                if (testTubes[0].LiquidAmount > KMnO4.LiquidPortionSize)
                {
                    return true;
                }
                break;
            case 4:
                if (testTubes[1].LiquidAmount > KMnO4.LiquidPortionSize)
                {
                    return true;
                }
                break;
            case 5:
                if (testTubes[1].LiquidAmount > KMnO4.LiquidPortionSize)
                {
                    return true;
                }
                break;
            case 6:
                if (testTubes[2].LiquidAmount > KMnO4.LiquidPortionSize)
                {
                    return true;
                }
                break;
            case 7:
                if (testTubes[2].LiquidAmount > KMnO4.LiquidPortionSize)
                {
                    return true;
                }
                break;
        }

        return false;
    }
}
