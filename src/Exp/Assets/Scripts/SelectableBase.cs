using UnityEngine;

[RequireComponent(typeof(Outline), typeof(Collider))]
public abstract class SelectableBase : MonoBehaviour, ISelectable
{
    [Header("Selectable")]
    [SerializeField]
    private SelectableType type;

    private Outline outline;

    /// <inheritdoc/>
    public SelectableType Type => type;

    protected virtual void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    /// <inheritdoc/>
    public void Desilect()
    {
        outline.enabled = false;
    }

    /// <inheritdoc/>
    public void Select()
    {
        outline.enabled = true;
    }

    private void OnMouseDown()
    {
        SelectManager.TrySelectObject(this);
    }
}
