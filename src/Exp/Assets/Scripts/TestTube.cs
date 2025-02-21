using UnityEngine;

/// <summary>
/// Class for control Test Tube.
/// </summary>
public class TestTube : MonoBehaviour
{
    [SerializeField]
    private Transform liquid;

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
        }
    }
}
