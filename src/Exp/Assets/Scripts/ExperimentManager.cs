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

    [SerializeField]
    private Experiment2 experiment2;

    [SerializeField]
    private Experiment3 experiment3;

    private IExperiment currenExperiment;

    private void Awake()
    {
        screenController.BtnExperimentStartClicked += OnExperimentStarted;
    }

    private void Start()
    {
        experiment1.Initialize(config.exp1);
        experiment1.Reset();
        experiment2.Initialize(config.exp2);
        experiment2.Reset();
        experiment3.Initialize(config.exp3);
        experiment3.Reset();
    }

    private void OnDestroy()
    {
        screenController.BtnExperimentStartClicked -= OnExperimentStarted;
    }

    private void OnExperimentStarted(int index)
    {
        switch (index)
        {
            case 0:
                currenExperiment = experiment1;
                break;
            case 1:
                currenExperiment = experiment2;
                break;
            case 2:
                currenExperiment = experiment3;
                break;
        }

        currenExperiment.Start();
        currenExperiment.StepCompleted += OnStepCompleted;
        currenExperiment.Completed += OnCompleted;
        screenController.UpdateExperimentData(currenExperiment.ExperimentData);
    }

    private void OnStepCompleted(int index)
    {
        screenController.UpdateStep(index);
    }

    private void OnCompleted(Transform endCameraPosition)
    {
        screenController.ShowExperimentEnding(endCameraPosition);
        currenExperiment.StepCompleted -= OnStepCompleted;
        currenExperiment.Completed -= OnCompleted;
        currenExperiment = null;
    }
}
