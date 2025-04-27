using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Outline), typeof(Collider))]
public abstract class SelectableBase : MonoBehaviour, ISelectable, IMovable
{
    protected CancellationTokenSource CTS = new();

    private const float MovingDuration = 2f;

    [Header("Selectable")]
    [SerializeField]
    private SelectableType type;

    private Outline outline;

    protected bool interactable;

    private bool isSelected;

    /// <inheritdoc/>
    public SelectableType Type => type;

    /// <inheritdoc/>
    public bool Interactable => interactable;

    /// <inheritdoc/>
    public event Action Combined;

    protected virtual void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    private void OnMouseDown()
    {
        if (interactable)
        {
            SelectManager.TrySelectObject(this);
        }
    }

    private void OnMouseEnter()
    {
        if (interactable && !isSelected)
        {
            outline.OutlineColor = SelectManager.AbleToSelectColor;
            outline.enabled = true;
        }
    }

    private void OnMouseExit()
    {
        if (interactable && !isSelected)
        {
            outline.enabled = false;
        }
    }

    /// <inheritdoc/>
    public void Select()
    {
        outline.OutlineColor = SelectManager.SelectColor;
        outline.enabled = true;
        isSelected = true;
    }

    /// <inheritdoc/>
    public void Desilect()
    {
        outline.enabled = false;
        isSelected = false;
    }

    /// <inheritdoc/>
    public abstract void TryCombine(ISelectable combinedObject);

    protected void NotifyCombined()
    {
        Combined?.Invoke();
    }

    /// <inheritdoc/>
    public virtual void Move(Transform target, bool rotate, bool arch)
    {
        MoveAsync(target, rotate, arch, MovingDuration, CTS.Token).Forget();
    }

    /// <inheritdoc/>
    public virtual void Move(Transform target, bool rotate, bool arch, float duration)
    {
        MoveAsync(target, rotate, arch, duration, CTS.Token).Forget();
    }

    public void EnableInteraction(bool interactable)
    {
        this.interactable = interactable;
    }

    private async UniTask MoveAsync(Transform target, bool rotate, bool arch, float duration, CancellationToken token)
    {
        var direction = target.position - transform.position;

        float timer = 0f;
        var startRotation = transform.rotation;

        while (timer < duration && !token.IsCancellationRequested)
        {
            float step = timer / duration;
            float offsetY = -1f * (step - 0.5f);

            if (arch)
            {
                if (step <= 0.667f)
                {
                    transform.position += direction * (Time.unscaledDeltaTime / duration) * 1.5f;
                }
                transform.position += Vector3.up * offsetY * Time.unscaledDeltaTime;

                if (rotate)
                {
                    transform.rotation = Quaternion.Lerp(startRotation, target.rotation, step * 1.5f);
                }
            }
            else
            {
                transform.position += direction * (Time.unscaledDeltaTime / duration);

                if (rotate)
                {
                    transform.rotation = Quaternion.Lerp(startRotation, target.rotation, step);
                }
            }

            await UniTask.NextFrame(cancellationToken: token);
            timer += Time.unscaledDeltaTime;
        }

        transform.position = target.position;

        if (rotate)
        {
            transform.rotation = target.rotation;
        }
    }

    private void OnDestroy()
    {
        CTS?.Cancel();
        CTS = null;
    }
}
