using Unity.VisualScripting;
using UnityEngine;

public class SwordParry : MonoBehaviour,IParriable
{
    BoxCollider collider;
    Animator animator;
    CodeSwordAttack swordAttack;
    public void OnParried()
    {
        Debug.Log("Parried");
        swordAttack.isDead();
        animator.SetTrigger("death");
        collider.enabled = false;
    }

    void Start()
    {
        swordAttack = GetComponentInParent<CodeSwordAttack>();
        collider = GetComponentInChildren<BoxCollider>();
        animator = GetComponentInParent<Animator>();

    }
}
