using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

/// <summary>
/// Class for control Beaker.
/// </summary>
public class Beaker : SelectableBase
{
    private const float LiquidSpeed = 0.5f;

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
            SetLiquidAsync(Mathf.Clamp01(liquidAmount), CTS.Token).Forget();
        }
        else
        {
            Vector3 scale = liquid.localScale;
            scale.y = liquidAmount;
            liquid.localScale = scale;
            liquid.gameObject.SetActive(liquidAmount != 0.0f);

            NotifyCombined();
        }
    }

    public void GetLiquid(TestTube tube, Transform targetTransform)
    {
        GetLiquidAsync(tube, targetTransform, CTS.Token).Forget();
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

        NotifyCombined();

        liquid.gameObject.SetActive(newLiquidAmount != 0.0f);
    }

    private async UniTask GetLiquidAsync(TestTube tube, Transform targetTransform, CancellationToken token)
    {
        Move(targetTransform, true, false, 1f);
        await UniTask.WaitForSeconds(1f, cancellationToken: token);
        tube.SetLiquid(0.3f, true);
        NotifyCombined();
        await UniTask.WaitForSeconds(0.6f, cancellationToken: token);
        Move(startPosition, true, false, 1f);
    }
}
