using System.Threading;
using UnityEngine;

/// <summary>
/// Interface vor movable objects.
/// </summary>
public interface IMovable
{
    public void Move(Transform target, bool rotate, bool arch, float duration);

    public void Move(Transform target, bool rotate, bool arch);
}
