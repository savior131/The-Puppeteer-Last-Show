using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private LayerMask damageLayer;
    [SerializeField] private string[] damageTags;
    private float currentHealth;
    public float health => currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void ApplyDamage(GameObject gameObject)
    {
        if (((1 << gameObject.layer) & damageLayer) != 0 && IsValidTag(gameObject.tag))
        {
            if (gameObject.TryGetComponent<Projectile>(out Projectile projectile))
            {
                TakeDamage(projectile.damage);
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
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        // idk what to do here
    }

    private void OnTriggerEnter(Collider collision)
    {
        ApplyDamage(collision.gameObject);
    }
}
