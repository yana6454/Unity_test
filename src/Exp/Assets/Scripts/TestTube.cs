using Cysharp.Threading.Tasks;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// Class for control Test Tube.
/// </summary>
public class TestTube : SelectableBase
{
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
            case SelectableType.SolidReactiveGroup:
                AddSolidReactive(combinedObject.gameObject.GetComponent<SolidReactiveGroup>().Generate());
                break;
            case SelectableType.Beaker:
                combinedObject.gameObject.GetComponent<Beaker>().GetLiquid(this, beakerPosition);
                break;
        }
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
            ps_Bubbles.gameObject.SetActive(liquidAmount != 0.0f);
            float bubbleBorderHeight = Mathf.Lerp(MinBubbleBorderHeight, MaxBubbleBorderHeight, liquidAmount);
            Vector3 borderPosition = bubbleBorder.localPosition;
            borderPosition.z = bubbleBorderHeight;
            bubbleBorder.localPosition = borderPosition;
        }
    }

    private void AddSolidReactive(SolidReactive reactive)
    {
        ps_Bubbles.emissionRate = Mathf.Clamp01(reactive.ReactionPower) * 100;

        ps_Bubbles.gameObject.SetActive(false);
        reactive.transform.parent = transform;
        reactive.Move(bubblesShapeTransform, false, true);

        bubblesShape.mesh = reactive.GetComponent<MeshFilter>().mesh;
        bubblesShapeTransform.localScale = reactive.transform.localScale;
        EnableBubblesAsync(CTS.Token).Forget();
    }

    private async UniTask EnableBubblesAsync(CancellationToken token)
    {
        await UniTask.WaitForSeconds(2, cancellationToken: token);
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
}
