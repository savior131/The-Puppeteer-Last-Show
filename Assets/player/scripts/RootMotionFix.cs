using UnityEngine;

public class RootMotionFix : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform player;
    [SerializeField] string[] affectedAnimations;

    void LateUpdate()
    {
        if (player == null || animator == null) return;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool shouldFixRootMotion = false;

        foreach (string animName in affectedAnimations)
        {
            if (stateInfo.IsName(animName))
            {
                shouldFixRootMotion = true;
                break;
            }
        }

        if (!shouldFixRootMotion) return;

        Transform hips = animator.GetBoneTransform(HumanBodyBones.Hips);
        if (hips != null)
        {
            Vector3 fixedPosition = hips.position;
            fixedPosition.x = player.position.x;
            fixedPosition.z = player.position.z;
            hips.position = fixedPosition;
        }
    }
}
