using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour, IMovable
{
    protected CancellationTokenSource CTS = new();

    private const float MovingDuration = 2f;

    private Camera camera;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        startPosition = camera.transform.position;
        startRotation = camera.transform.rotation;
    }

    public void SetCameraActive(bool isActive)
    {
        camera.enabled = isActive;
    }

    public void ResetTransform()
    {
        camera.transform.position = startPosition;
        camera.transform.rotation = startRotation;
    }

    /// <inheritdoc/>
    public virtual void Move(Transform target, bool rotate = true, bool arch = false, float duration = MovingDuration)
    {
        MoveAsync(target, rotate, arch, duration, CTS.Token).Forget();
    }

    private async UniTask MoveAsync(Transform target, bool rotate, bool arch, float duration, CancellationToken token)
    {
        var direction = target.position - transform.position;

        float timer = 0f;
        var startRotation = transform.rotation;

        while (timer < duration && !token.IsCancellationRequested)
        {
            float step = timer / duration;
            float offsetY = -1f * (step - 0.5f);

            if (arch)
            {
                if (step <= 0.667f)
                {
                    transform.position += direction * (Time.unscaledDeltaTime / duration) * 1.5f;
                }
                transform.position += Vector3.up * offsetY * Time.unscaledDeltaTime;

                if (rotate)
                {
                    transform.rotation = Quaternion.Lerp(startRotation, target.rotation, step * 1.5f);
                }
            }
            else
            {
                transform.position += direction * (Time.unscaledDeltaTime / duration);

                if (rotate)
                {
                    transform.rotation = Quaternion.Lerp(startRotation, target.rotation, step);
                }
            }

            await UniTask.NextFrame(cancellationToken: token);
            timer += Time.unscaledDeltaTime;
        }

        transform.position = target.position;

        if (rotate)
        {
            transform.rotation = target.rotation;
        }
    }

    private void OnDestroy()
    {
        CTS?.Cancel();
        CTS = null;
    }
}
