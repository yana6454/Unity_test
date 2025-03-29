using UnityEngine;

public class SolidReactive : SelectableBase
{
    [Range(0.0f, 1.0f)]
    private float reactionPower = 0.0f;

    private bool isInitialized = false;

    public float ReactionPower => reactionPower;

    public void Initialize(float reactionPower)
    {
        if (isInitialized)
        {
            return;
        }

        this. reactionPower = reactionPower;
        isInitialized = true;
    }

    public override void TryCombine(ISelectable combinedObject)
    {
    }
}
