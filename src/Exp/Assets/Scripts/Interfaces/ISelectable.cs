/// <summary>
/// Interface for selectable objects.
/// </summary>
public interface ISelectable
{
    public SelectableType Type { get; }

    public void Select();

    public void Desilect();
}

/// <summary>
/// Types of selectable objects.
/// </summary>
public enum SelectableType
{
    None,
    TestTube,
    TestTubeStand,
    SolidReactive,
    Liquid,
}
