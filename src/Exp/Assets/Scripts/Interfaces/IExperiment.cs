using System;

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
    /// </summary>
    event Action Completed;

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
