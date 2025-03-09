using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// Controlls stand for Test Tubes.
/// </summary>
public class TestTubeStand : SelectableBase
{
    private CancellationTokenSource cts = new();

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
            AddTubeAsync(combinedObject.gameObject.GetComponent<TestTube>(), cts.Token).Forget();
        }
    }

    private async UniTask AddTubeAsync(TestTube tube, CancellationToken token)
    {
        tube.gameObject.GetComponent<Collider>().isTrigger = true;
        tube.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        for (int i = 0; i < tubePositions.Count; i++)
        {
            if (tubes[i] == null)
            {
                tubes[i] = tube;
                await MoveAsync(tube.transform, tubePositions[i], true, token);
                tube.SetLiquid(0.3f, true);
                break;
            }
        }
    }

    private void OnDestroy()
    {
        cts?.Cancel();
        cts = null;
    }
}
