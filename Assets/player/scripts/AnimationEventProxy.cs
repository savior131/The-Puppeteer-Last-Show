using UnityEngine;

public class AnimationEventProxy : MonoBehaviour
{
    [SerializeField] private Attack attack;

    public void EnableNextCombo()
    {
        attack.EnableNextCombo();
    }

    public void ResetCombo()
    {
        attack.ResetCombo();
    }
}
