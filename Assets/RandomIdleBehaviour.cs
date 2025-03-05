using UnityEngine;

public class RandomIdleBehaviour : StateMachineBehaviour
{
    [SerializeField] private int idleVariations = 2;
    [SerializeField] private float blendSpeed = 1f;

    private float targetBlend;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        targetBlend = Random.Range(0, idleVariations);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float currentBlend = animator.GetFloat("idleBlend");
        float newBlend = Mathf.Lerp(currentBlend, targetBlend, blendSpeed * Time.deltaTime);
        animator.SetFloat("idleBlend", newBlend);
        if (stateInfo.normalizedTime % 1 >= 0.95f)
        {
            targetBlend = Random.Range(0, idleVariations);
        }
    }

}