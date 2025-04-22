using System;
using UnityEngine;

/// <summary>
/// Interface for selectable objects.
/// </summary>
public interface ISelectable
{
    public bool Interactable { get; }

    public SelectableType Type { get; }

    public GameObject gameObject { get; }

    public event Action Combined;

    public void Select();

    public void Desilect();

    public void TryCombine(ISelectable combinedObject);
}

/// <summary>
/// Types of selectable objects.
/// </summary>
public enum SelectableType
{
    None,
    TestTube,
    TestTubeStand,
    Beaker,
    SolidReactiveGroup,
    Liquid,
}
