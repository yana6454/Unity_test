using System.Collections.Generic;
using UnityEngine;

public class ExperimentStuffBase : MonoBehaviour
{
    [SerializeField]
    private List<SelectableBase> stuffList;

    public List<SelectableBase> GetStuffList()
    {
        return stuffList;
    }
}
