using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// Class for control Test Tube.
/// </summary>
public class TestTube : MonoBehaviour
{
    private CancellationTokenSource cts = new();

    private const float MaxBubbleBorderHeight = 0.2f;
    private const float MinBubbleBorderHeight = 0f;
    private const float LiquidSpeed = 0.5f;

    [Header("Liquid")]
    [SerializeField]
    private Transform liquid;

    [Header("SolidReactive")]
    [SerializeField]
    private Transform reactivePosition;

    [Header("Bubbles")]
    [SerializeField]
    private Transform bubbleBorder;

    [SerializeField]
    private ParticleSystem ps_Bubbles;

    [SerializeField]
    private MeshFilter bubblesShape;

    [SerializeField]
    private Transform bubblesShapeTransform;

    private void Start()
    {
        SetLiquid(0.0f, false);
        ps_Bubbles.emissionRate = 0.0f;
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
            scale.y = 0f;
            liquid.localScale = scale;
            liquid.gameObject.SetActive(liquidAmount != 0.0f);
            ps_Bubbles.gameObject.SetActive(liquidAmount != 0.0f);
            float bubbleBorderHeight = Mathf.Lerp(MinBubbleBorderHeight, MaxBubbleBorderHeight, liquidAmount);
            Vector3 borderPosition = bubbleBorder.localPosition;
            borderPosition.z = bubbleBorderHeight;
            bubbleBorder.localPosition = borderPosition;
        }
    }

    public void AddSolidReactive(GameObject reactive, float reactionPower)
    {
        reactive.transform.parent = transform;

        // [TODO] Move Reactive logic.
        reactive.transform.position = reactivePosition.position;

        bubblesShape.mesh = reactive.GetComponent<MeshFilter>().mesh;
        bubblesShapeTransform.localScale = reactive.transform.localScale;
        reactionPower = Mathf.Clamp01(reactionPower);
        ps_Bubbles.emissionRate = reactionPower * 100;
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
