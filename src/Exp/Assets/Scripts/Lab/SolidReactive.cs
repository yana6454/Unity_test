using UnityEngine;

public class SolidReactive : SelectableBase
{
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float reactionPower = 0.0f;

    public float ReactionPower => reactionPower;

    public override void TryCombine(ISelectable combinedObject)
    {
    }
}
