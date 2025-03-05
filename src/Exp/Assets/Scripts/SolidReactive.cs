using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidReactive : MonoBehaviour
{
    [SerializeField]
    private GameObject reactivePrefab;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float reactionPower = 0.0f;

    public (GameObject, float) Generate()
    {
        return (Instantiate(reactivePrefab), reactionPower);
    }
}
