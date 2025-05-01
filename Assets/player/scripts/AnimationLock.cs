using UnityEngine;

public class AnimationLock : MonoBehaviour
{
    public bool canMove { get; private set; } = true;
    public bool canRotate { get; private set; } = true;

    public void DisableMovement() => canMove = false;
    public void EnableMovement() => canMove = true;
    public void DisableRotation() => canRotate = false;
    public void EnableRotation() => canRotate = true;
}
