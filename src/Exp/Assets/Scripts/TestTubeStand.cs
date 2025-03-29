using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controlls stand for Test Tubes.
/// </summary>
public class TestTubeStand : SelectableBase
{
    [Header("Stand")]
    [SerializeField]
    private List<Transform> tubePositions;

    private readonly Dictionary<int, TestTube> tubes = new()
    {
        { 0, null },
        { 1, null },
        { 2, null },
        { 3, null },
        { 4, null },
    };

    public override void TryCombine(ISelectable combinedObject)
    {
        if (combinedObject.Type == SelectableType.TestTube)
        {
            AddTube(combinedObject.gameObject.GetComponent<TestTube>());
        }
    }

    private void AddTube(TestTube tube)
    {
        tube.gameObject.GetComponent<Collider>().isTrigger = true;
        tube.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        for (int i = 0; i < tubePositions.Count; i++)
        {
            if (tube.Equals(tubes[i]))
            {
                tubes[i] = null;
                continue;
            }

            if (tubes[i] == null)
            {
                tubes[i] = tube;
                tube.Move(tubePositions[i], true, true);
                break;
            }
        }
    }
}
