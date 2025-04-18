using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Отвечает за вклюение и выключение интеракции (выделения) объектов.
/// Перед началом работы нужно задать маску интеракций и список активных объектов.
/// </summary>
public class InteractionManager : MonoBehaviour
{
    private List<SelectableType> interactionMask;

    private List<SelectableBase> interactableObjects = new();

    /// <summary>
    /// Добавления списка типов объектов, которые будут кликаться на этом этапе.
    /// Все эти типы добавляются в маску интеракции.
    /// </summary>
    public void SetInteractionMask(List<SelectableType> newMask) 
    {
        interactionMask = newMask;
    }

    public bool AddSelectableObject(SelectableBase obj)
    {
        if (!interactableObjects.Contains(obj))
        {
            interactableObjects.Add(obj);
            return true;
        }

        return false;
    }

    public bool RemoveSelectableObject(SelectableBase obj)
    {
        if (interactableObjects.Contains(obj))
        {
            interactableObjects.Remove(obj);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Включает/выключает интеракцию у всех объектов, которые содержат нужные типы.
    /// </summary>
    public void EnableInteractions(bool interactable)
    {
        if (interactable)
        {
            foreach (var obj in interactableObjects)
            {
                obj.EnableInteraction(interactionMask.Contains(obj.Type)); // Вкллючает интеракцию объекта, если его тип есть в маске.
            }
        }
        else
        {
            foreach (var obj in interactableObjects)
            {
                obj.EnableInteraction(false);
            }
        }
    }

    /// <summary>
    /// Очистка всех данных этого менеджера.
    /// </summary>
    public void Reset()
    {
        EnableInteractions(false);
        interactionMask.Clear();
        interactableObjects.Clear();
    }
}
