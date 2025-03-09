using UnityEngine;

/// <summary>
/// Manages objects selection.
/// </summary>
public static class SelectManager
{
    private static ISelectable selectedObject;

    public static bool TrySelectObject(ISelectable hitObject)
    {
        if (selectedObject != null)
        {
            if (!Equals(selectedObject, hitObject))
            {
                return false;
            }

            selectedObject = null;
            hitObject.Desilect();
            return true;
        }

        selectedObject = hitObject;
        hitObject.Select();
        return true;
    }
}
