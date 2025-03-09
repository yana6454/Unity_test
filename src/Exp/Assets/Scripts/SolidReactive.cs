using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidReactive : SelectableBase
{
    [SerializeField]
    private GameObject reactivePrefab;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float reactionPower = 0.0f;

    public (GameObject, float) Generate()
    {
        var reactive = Instantiate(reactivePrefab, transform);
        reactive.transform.position += Vector3.up * 0.03f;
        return (reactive, reactionPower);
    }
}
