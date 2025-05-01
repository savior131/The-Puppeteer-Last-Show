using UnityEngine;

public class AnimationEventProxy : MonoBehaviour
{
    [SerializeField] private Attack attack;
    [SerializeField] private AnimationLock animationLock;

    public void EnableNextCombo()
    {
        attack?.EnableNextCombo();
    }

    public void ResetCombo()
    {
        attack?.ResetCombo();
    }

    public void EnableMovement()
    {
        animationLock?.EnableMovement();
    }

    public void DisableMovement()
    {
        animationLock?.DisableMovement();
    }

    public void EnableRotation()
    {
        animationLock?.EnableRotation();
    }

    public void DisableRotation()
    {
        animationLock?.DisableRotation();
    }
}
