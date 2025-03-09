using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Outline), typeof(Collider))]
public abstract class SelectableBase : MonoBehaviour, ISelectable, IMovable
{
    private const float MovingDuration = 2f;

    [Header("Selectable")]
    [SerializeField]
    private SelectableType type;

    private Outline outline;

    protected bool interactable = true;

    /// <inheritdoc/>
    public SelectableType Type => type;

    /// <inheritdoc/>
    public bool Interactable => interactable;

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

    /// <inheritdoc/>
    public abstract void TryCombine(ISelectable combinedObject);

    /// <inheritdoc/>
    public void Move(Transform movedObject, Transform target, bool rotate, CancellationToken token)
    {
        MoveAsync(movedObject, target, rotate, token).Forget();
    }

    protected async UniTask MoveAsync(Transform movedObject, Transform target, bool rotate, CancellationToken token)
    {
        var direction = target.position - movedObject.transform.position;

        float timer = 0f;
        var startRotation = movedObject.rotation;

        while (timer < MovingDuration && !token.IsCancellationRequested)
        {
            float step = timer / MovingDuration;
            float offsetY = -1f * (step - 0.5f);

            if (step <= 0.667f)
            {
                movedObject.position += direction * (Time.unscaledDeltaTime / MovingDuration) * 1.5f;
            }

            movedObject.position += Vector3.up * offsetY * Time.unscaledDeltaTime;

            if (rotate)
            {
                movedObject.rotation = Quaternion.Lerp(startRotation, target.rotation, step * 1.5f);
            }

            await UniTask.NextFrame(cancellationToken: token);
            timer += Time.unscaledDeltaTime;
        }

        movedObject.position = target.position;

        if (rotate)
        {
            movedObject.rotation = target.rotation;
        }
    }
}
