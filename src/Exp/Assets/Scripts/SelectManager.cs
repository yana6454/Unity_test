using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages objects selection.
/// </summary>
public static class SelectManager
{
    public static readonly List<(SelectableType, SelectableType)> Interactions = new()
    {
        (SelectableType.TestTube, SelectableType.TestTubeStand),
        (SelectableType.SolidReactive, SelectableType.TestTube),
    };

    private static ISelectable selectedObject;

    public static bool TrySelectObject(ISelectable hitObject)
    {
        if (selectedObject != null)
        {
            if (!Equals(selectedObject, hitObject))
            {
                if (!CheckInteraction(selectedObject, hitObject))
                {
                    return false;
                }

                hitObject.TryCombine(selectedObject);
                Debug.Log($"Combine {selectedObject.gameObject.name} {hitObject.gameObject.name}");
            }

            selectedObject.Desilect();
            selectedObject = null;
            return true;
        }

        selectedObject = hitObject;
        hitObject.Select();
        return true;
    }

    private static bool CheckInteraction(ISelectable object1, ISelectable object2)
    {
        foreach (var interaction in Interactions)
        {
            if (Equals(interaction, (object1.Type, object2.Type)))
            {
                return true;
            }
        }

        return false;
    }
}
