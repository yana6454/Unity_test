using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentStuff : MonoBehaviour
{
    [SerializeField]
    private List<SelectableBase> stuffList;

    public List<SelectableBase> GetStuffList()
    {
        return stuffList;
    }
}
