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

    private const float MovingDuration = 2f;

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
                MoveTubeAsync(tube, tubePositions[i], cts.Token).Forget();
                break;
            }
        }
    }

    private async UniTask MoveTubeAsync(TestTube tube, Transform target, CancellationToken token)
    {
        var tubeTransform = tube.transform;

        float timer = 0f;
        var startPosition = tubeTransform.position;
        var startRotation = tubeTransform.rotation;

        while (timer < MovingDuration && !token.IsCancellationRequested)
        {
            float step = timer / MovingDuration;

            tubeTransform.position = Vector3.Lerp(startPosition, target.position, step);
            tubeTransform.rotation = Quaternion.Lerp(startRotation, target.rotation, step);

            await UniTask.NextFrame(cancellationToken: token);
            timer += Time.unscaledDeltaTime;
        }

        tubeTransform.position = target.position;
        tubeTransform.rotation = target.rotation;
        tube.SetLiquid(0.3f, true);
    }

    private void OnDestroy()
    {
        cts?.Cancel();
        cts = null;
    }
}
