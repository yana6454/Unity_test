using UnityEngine;

public class SolidReactiveGroup : SelectableBase
{
    [SerializeField]
    private SolidReactive reactivePrefab;

    public SolidReactive Generate()
    {
        var reactive = Instantiate(reactivePrefab, transform);
        reactive.transform.position += Vector3.up * 0.03f;
        reactive.Initialize(this);
        return reactive;
    }

    public override void TryCombine(ISelectable combinedObject)
    {
    }
}
