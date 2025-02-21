using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controlls stand for Test Tubes.
/// </summary>
public class TestTubeStand : MonoBehaviour
{
    [SerializeField]
    private List<Transform> tubePositions;

    private Dictionary<int, TestTube> tubes = new();
}
