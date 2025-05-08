using System;
using UnityEngine;

/// <summary>
/// Интерфейс для скриптов контроля экспериментов.
/// </summary>
public interface IExperiment
{
    string Title { get; }

    string Description { get; }

    string[] TODOList { get; }

    /// <summary>
    /// Сигнализирует об успешном выполнении шага эксперимента.
    /// </summary>
    event Action<int> StepCompleted;

    /// <summary>
    /// Сигнализирует об успешном выполнении эксперимента.
    /// Transform здесь передаётся позиция куда нужно будет перелететь камерею.
    /// </summary>
    event Action<Transform> Completed;

    /// <summary>
    /// Начальная настройка эксперимента.
    /// </summary>
    void Initialize(ExperimentData expData);

    /// <summary>
    /// Отключение всех объетков, перевод их в начальное состояние.
    /// </summary>
    void Reset();

    /// <summary>
    /// Начало эксперимента.
    /// </summary>
    void Start();
}
