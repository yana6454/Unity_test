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

    [Space]
    [SerializeField]
    private ExperimentsConfig config;

    [SerializeField]
    private Experiment1 experiment1;

    private void Awake()
    {
        screenController.ExperimentStarted += OnExperimentStarted;
    }

    private void Start()
    {
        experiment1.Initialize(config.exp1);
        experiment1.Reset();
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
                experiment1.Start();
                screenController.UpdateExperimentData(experiment1.Title, experiment1.Description);
                break;
        }
    }
}
