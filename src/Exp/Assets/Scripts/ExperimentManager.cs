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

    private IExperiment currenExperiment;

    private void Awake()
    {
        screenController.BtnExperimentStartClicked += OnExperimentStarted;
    }

    private void Start()
    {
        experiment1.Initialize(config.exp1);
        experiment1.Reset();
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
        }

        currenExperiment.Start();
        currenExperiment.StepCompleted += OnStepCompleted;
        currenExperiment.Completed += OnCompleted;
        screenController.UpdateExperimentData(currenExperiment.Title, currenExperiment.Description, currenExperiment.TODOList);
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
