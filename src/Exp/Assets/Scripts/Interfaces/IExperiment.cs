/// <summary>
/// Интерфейс для скриптов контроля экспериментов.
/// </summary>
public interface IExperiment
{
    string Title { get; }

    string Description { get; }

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
