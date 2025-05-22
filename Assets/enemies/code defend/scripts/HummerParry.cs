using UnityEngine;

public class HummerParry : MonoBehaviour,IParriable
{
    Animator animator;
    Collider collider;
    CodeDefendAttack codeDefendAttack;
    
    public void OnParried()
    {
        Debug.LogWarning("hammer parried");
       
        // Force stop the "attack" animation by playing the "hit" state directly
        animator.Play("hit", 0, 0f);
        collider.enabled = false;
        animator.SetTrigger("death");
        codeDefendAttack.isDead = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider = GetComponentInChildren<Collider>();
        animator = GetComponentInParent<Animator>();
        codeDefendAttack=GetComponentInParent<CodeDefendAttack>();
    }

    
}
