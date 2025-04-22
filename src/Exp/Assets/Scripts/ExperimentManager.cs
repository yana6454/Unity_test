using UnityEngine;

/// <summary>
/// Содержит логику для запуска и управления экспериментом.
/// </summary>
public class ExperimentManager : MonoBehaviour
{
    [SerializeField]
    private Transform stuffParent;

    [SerializeField]
    private ScreenController screenController;

    [Header("Experiment 1")]
    [SerializeField]
    private ExperimentConfig experiment1Config;

    private Experiment1 experiment1 = new();

    private void Awake()
    {
        screenController.ExperimentStarted += OnExperimentStarted;
    }

    private void OnDestroy()
    {
        screenController.ExperimentStarted -= OnExperimentStarted;
    }

    private void OnExperimentStarted(int index)
    {
        switch (index)
        {
            case 0:
                experiment1.Initialize(experiment1Config, stuffParent);
                screenController.UpdateExperimentData(experiment1.Title, experiment1.Description);
                experiment1.Start();
                break;
        }
    }
}
