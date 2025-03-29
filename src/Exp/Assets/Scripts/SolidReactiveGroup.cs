using UnityEngine;

public class SolidReactiveGroup : SelectableBase
{
    [SerializeField]
    private SolidReactive reactivePrefab;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float reactionPower = 0.0f;

    public SolidReactive Generate()
    {
        var reactive = Instantiate(reactivePrefab, transform);
        reactive.transform.position += Vector3.up * 0.03f;
        reactive.Initialize(reactionPower);
        return reactive;
    }

    public override void TryCombine(ISelectable combinedObject)
    {
    }
}
