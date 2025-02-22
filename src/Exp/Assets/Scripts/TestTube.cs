using UnityEngine;

/// <summary>
/// Class for control Test Tube.
/// </summary>
public class TestTube : MonoBehaviour
{
    private const float MaxBubbleBorderHeight = 0.2f;
    private const float MinBubbleBorderHeight = 0f;

    [SerializeField]
    private Transform liquid;

    [SerializeField]
    private Transform bubbleBorder;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float liquidAmount = 0.0f;

    private void FixedUpdate()
    {
        if (liquid.localScale.y != liquidAmount)
        {
            Vector3 scale = liquid.localScale;
            scale.y = liquidAmount;
            liquid.localScale = scale;
            liquid.gameObject.SetActive(liquidAmount != 0.0f);

            float bubbleBorderHeight = Mathf.Lerp(MinBubbleBorderHeight, MaxBubbleBorderHeight, liquidAmount);
            Vector3 borderPosition = bubbleBorder.localPosition;
            borderPosition.z = bubbleBorderHeight;
            bubbleBorder.localPosition = borderPosition;
        }
    }
}
