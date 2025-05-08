using System.Collections.Generic;
using UnityEngine;

public class ExperimentStuffBase : MonoBehaviour
{
    [SerializeField]
    private Transform endCameraPosition;

    [SerializeField]
    private List<SelectableBase> stuffList;

    public Transform EndCameraPosition => endCameraPosition;

    public List<SelectableBase> StuffList => stuffList;
}
