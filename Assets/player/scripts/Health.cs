using System;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private LayerMask damageLayer;
    [SerializeField] private string[] damageTags;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider damageCollider;
    [SerializeField] private Transform greatSword;
    private float currentHealth;
    public float health => currentHealth;
    [NonSerialized] public bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void ApplyDamage(GameObject gameObject)
    {
        if (isDead) return;

        if (((1 << gameObject.layer) & damageLayer) != 0 && IsValidTag(gameObject.tag))
        {
            if (gameObject.TryGetComponent<Projectile>(out Projectile projectile))
            {
                TakeDamage(projectile.damage);
            }
            else if(gameObject.TryGetComponent<ExplosionExpander>(out ExplosionExpander explosionExpander))
            {
                TakeDamage(explosionExpander.damage);
            }
        }
    }

    private bool IsValidTag(string objTag)
    {
        foreach (string tag in damageTags)
        {
            if (objTag == tag) return true;
        }
        return false;
    }

    public void TakeDamage(float amount)
    {
        animator.SetTrigger("hit");
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        isDead = true;
        animator.SetBool("death", true);
        greatSword.SetParent(null);
        damageCollider.enabled = false;

    }

    private void OnTriggerEnter(Collider collision)
    {
        ApplyDamage(collision.gameObject);
    }
}
