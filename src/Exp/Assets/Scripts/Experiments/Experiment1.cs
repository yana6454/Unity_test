using UnityEngine;
using System;
using System.Collections.Generic;

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
    private TestTube[] testTubes;

    [Space]
    [SerializeField]
    private SolidReactiveGroup[] reactives;

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

        SetupStep(stepIndex);
    }

    private void OnStuffCombined()
    {
        if (CheckStepCompletion(stepIndex))
        {
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
            case 1: // Налить HCL в пробирки.
                foreach (var tube in testTubes)
                {
                    tube.EnableInteraction(true);
                }

                HCl.EnableInteraction(true);
                break;
            case 2: // Поместить металлы в пробирки.
                foreach (var tube in testTubes)
                {
                    tube.EnableInteraction(true);
                }

                foreach (var reactive in reactives)
                {
                    reactive.EnableInteraction(true);
                }

                break;
            case 3: // Завершение эксперимента.
                break;
        }
    }

    private bool CheckStepCompletion(int stepIndex)
    {
        // Проверяем выполнилось ли условтие текущего этапа,
        // если да, увеличиваем индекс этапа.
        switch (stepIndex)
        {
            case 0: // Проверяем чтобы количество пробирок в стэнде было 4.
                if (stand.TestTubes.Count == 4)
                {
                    return true;
                }

                break;
            case 1: // Чтобы во всех пробирках была кислота.
                bool allHaveLiquid = true;
                foreach (var tube in testTubes)
                {
                    allHaveLiquid &= tube.LiquidAmount > 0;
                }

                if (allHaveLiquid)
                {
                    return true;
                }

                break;
            case 2: // Чтобы в каждой пробирке был реактив.
                bool allHaveReactives = true;
                foreach (var tube in stand.TestTubes)
                {
                    allHaveReactives &= tube.Reactive != null;

                    if (tube.Interactable && tube.Reactive != null)
                    {
                        // Выключаем интеракции на реактив и пробирку,
                        // чтобы не сделать дубоирование и в эту не положить другие реактивы.
                        tube.EnableInteraction(false);
                        tube.Reactive.ParentGroup.EnableInteraction(false);
                    }
                    else
                    {
                        allHaveReactives = false;
                    }
                }

                if (allHaveReactives)
                {
                    return true;
                }
                break;
        }

        return false;
    }
}
