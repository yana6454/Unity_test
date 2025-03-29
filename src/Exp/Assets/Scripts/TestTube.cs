using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// Class for control Test Tube.
/// </summary>
public class TestTube : SelectableBase
{
    private CancellationTokenSource cts = new();

    private const float MaxBubbleBorderHeight = 0.2f;
    private const float MinBubbleBorderHeight = 0f;
    private const float LiquidSpeed = 0.5f;
    private const float MovingDuration = 2.0f;

    [Header("Liquid")]
    [SerializeField]
    private Transform liquid;

    [Header("Bubbles")]
    [SerializeField]    
    private Transform bubbleBorder;

    [SerializeField]
    private ParticleSystem ps_Bubbles;

    [SerializeField]
    private MeshFilter bubblesShape;

    [SerializeField]
    private Transform bubblesShapeTransform;

    [Header("Animations")]
    [SerializeField]
    private Transform beakerPosition;

    protected override void Start()
    {
        base.Start();
        SetLiquid(0.0f, false);
        ps_Bubbles.emissionRate = 0.0f;
    }

    /// <inheritdoc/>
    public override void TryCombine(ISelectable combinedObject)
    {
        switch (combinedObject.Type)
        {
            case SelectableType.SolidReactive:
                var reactive = combinedObject.gameObject.GetComponent<SolidReactive>().Generate();

                var reactionPower = Mathf.Clamp01(reactive.Item2);
                ps_Bubbles.emissionRate = reactionPower * 100;

                AddSolidReactiveAsync(reactive.Item1.transform, cts.Token).Forget();
                break;
            case SelectableType.Beaker:
                var beaker = combinedObject.gameObject.GetComponent<Beaker>();
                beaker.GetLiquid(this, beakerPosition);
                break;
        }
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
            ps_Bubbles.gameObject.SetActive(liquidAmount != 0.0f);
            float bubbleBorderHeight = Mathf.Lerp(MinBubbleBorderHeight, MaxBubbleBorderHeight, liquidAmount);
            Vector3 borderPosition = bubbleBorder.localPosition;
            borderPosition.z = bubbleBorderHeight;
            bubbleBorder.localPosition = borderPosition;
        }
    }

    private async UniTask AddSolidReactiveAsync(Transform reactive, CancellationToken token)
    {
        ps_Bubbles.gameObject.SetActive(false);
        reactive.parent = transform;

        await MoveAsync(reactive, bubblesShapeTransform, false, token);

        bubblesShape.mesh = reactive.GetComponent<MeshFilter>().mesh;
        bubblesShapeTransform.localScale = reactive.localScale;
        
        ps_Bubbles.gameObject.SetActive(true);
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

        ps_Bubbles.gameObject.SetActive(newLiquidAmount != 0.0f);
        float bubbleBorderHeight = Mathf.Lerp(MinBubbleBorderHeight, MaxBubbleBorderHeight, newLiquidAmount);
        Vector3 borderPosition = bubbleBorder.localPosition;
        borderPosition.z = bubbleBorderHeight;
        bubbleBorder.localPosition = borderPosition;
    }

    private void OnDestroy()
    {
        cts?.Cancel();
        cts = null;
    }
}
