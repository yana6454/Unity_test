using UnityEngine;

/// <summary>
/// Интерфейс для скриптов контроля экспериментов.
/// </summary>
public interface IExperiment
{
    string Title { get; }

    string Description { get; }

    ExperimentStuff Stuff { get; }

    void Start();
}
