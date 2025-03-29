using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

/// <summary>
/// Class for control Beaker.
/// </summary>
public class Beaker : SelectableBase
{
    private CancellationTokenSource cts = new();

    private const float LiquidSpeed = 0.5f;
    private const float MovingDuration = 2.0f;

    [Header("Liquid")]
    [SerializeField]
    private Transform liquid;

    [SerializeField]
    private Transform startPosition;

    protected override void Start()
    {
        base.Start();
        SetLiquid(1.0f, false);
    }

    /// <inheritdoc/>
    public override void TryCombine(ISelectable combinedObject)
    {
    }

    public void SetLiquid(float liquidAmount, bool smooth)
    {
        if (smooth)
        {
            SetLiquidAsync(Mathf.Clamp01(liquidAmount), cts.Token).Forget();
        }
        else
        {
            Vector3 scale = liquid.localScale;
            scale.y = liquidAmount;
            liquid.localScale = scale;
            liquid.gameObject.SetActive(liquidAmount != 0.0f);
        }
    }

    public void GetLiquid(TestTube tube, Transform targetTransform)
    {
        GetLiquidAsync(tube, targetTransform, cts.Token).Forget();
    }

    private async UniTask SetLiquidAsync(float newLiquidAmount, CancellationToken token)
    {
        liquid.gameObject.SetActive(true);
        Vector3 scale = liquid.localScale;

        while (scale.y < newLiquidAmount && !token.IsCancellationRequested)
        {
            scale.y += Time.unscaledDeltaTime * LiquidSpeed;
            liquid.localScale = scale;

            await UniTask.NextFrame(cancellationToken: token);
        }

        liquid.gameObject.SetActive(newLiquidAmount != 0.0f);
    }

    private async UniTask GetLiquidAsync(TestTube tube, Transform targetTransform, CancellationToken token)
    {
        await MoveAsync(transform, targetTransform, true, token);
        tube.SetLiquid(0.3f, true);
        await UniTask.WaitForSeconds(0.6f, cancellationToken: token);
        await MoveAsync(transform, startPosition, true, token);
    }

    private void OnDestroy()
    {
        cts?.Cancel();
        cts = null;
    }
}
