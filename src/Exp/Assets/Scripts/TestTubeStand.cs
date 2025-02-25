using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// Controlls stand for Test Tubes.
/// </summary>
public class TestTubeStand : MonoBehaviour
{
    private CancellationTokenSource cts = new();

    private const float MovingDuration = 6f;

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

    public void AddTube(TestTube tube)
    {
        tube.gameObject.GetComponent<Collider>().isTrigger = true;
        tube.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        for (int i = 0; i < tubePositions.Count; i++)
        {
            if (tubes[i] == null)
            {
                tubes[i] = tube;
                MoveTubeAsync(tube.transform, tubePositions[i], cts.Token).Forget();
                break;
            }
        }
    }

    private async UniTask MoveTubeAsync(Transform tube, Transform target, CancellationToken token)
    {
        float timer = 0f;
        var startPosition = tube.position;
        var startRotation = tube.rotation;

        while (timer < MovingDuration && !token.IsCancellationRequested)
        {
            float step = timer / MovingDuration;

            tube.position = Vector3.Lerp(startPosition, target.position, step);
            tube.rotation = Quaternion.Lerp(startRotation, target.rotation, step);

            await UniTask.NextFrame(cancellationToken: token);
            timer += Time.unscaledDeltaTime;
        }

        tube.position = target.position;
        tube.rotation = target.rotation;
    }

    private void OnDestroy()
    {
        cts?.Cancel();
        cts = null;
    }
}
