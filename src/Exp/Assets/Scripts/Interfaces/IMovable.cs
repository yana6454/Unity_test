using System.Threading;
using UnityEngine;

/// <summary>
/// Interface vor movable objects.
/// </summary>
public interface IMovable
{
    public void Move(Transform movedObject, Transform target, bool rotate, CancellationToken token);
}
