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
    [SerializeField] private GameObject greatSword;
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private CheckpointManager checkpointManager;
    private GameObject droppedSwordInstance;
    [SerializeField] private ImpactFlash impactFlash;
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

        if (((1 << gameObject.layer) & damageLayer) != 0 )
        {
            if (!IsValidTag(gameObject.tag)) return;

            if (gameObject.TryGetComponent<IDamagingObject>(out IDamagingObject damagingObject))
            {
                TakeDamage(damagingObject.Damage);
                Debug.Log($"player health is {health}");
            }
            if (gameObject.TryGetComponent<Projectile>(out Projectile projectile))
            {
                TakeDamage(projectile.damage);
            }
            else if (gameObject.TryGetComponent<ExplosionExpander>(out ExplosionExpander explosionExpander))
            {
                TakeDamage(explosionExpander.damage);
            }
            if (gameObject.CompareTag("Trap"))
            {
                TakeDamage(10f,false);
            }
            if (gameObject.CompareTag("Swing Blade"))
            {
                TakeDamage(50f);
            }
            if (gameObject.CompareTag("Water"))
            {
                TakeDamage(20,false);
                if (!isDead)
                {
                    checkpointManager.Respawn();
                }
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

    public void TakeDamage(float amount,bool animation=true)
    {
        impactFlash.TriggerFlash();
        if (animation)
        {
        animator.SetTrigger("hit");    
        }
        currentHealth -= amount;
        if (currentHealth <= 0) {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetBool("death", true);
        damageCollider.enabled = false;

        greatSword.gameObject.SetActive(false);

        droppedSwordInstance = Instantiate(swordPrefab, greatSword.transform);
        droppedSwordInstance.SetActive(true);
        droppedSwordInstance.transform.SetParent(null);

        var rb = droppedSwordInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
         ApplyDamage(collision.gameObject);
       
    }
    private void OnTriggerEnter(Collider collision)
    {
        ApplyDamage(collision.gameObject);
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Trap"))
        {
            TakeDamage(40f * Time.deltaTime,false);
        }
    }
    public void ResetPlayer(Transform spawnPoint)
    {

        if (!isDead)
        {
            transform.position = spawnPoint.position;
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            animator.Play("idle", 0, 0f);
            return;
        }

        isDead = false;
        currentHealth = maxHealth;
        damageCollider.enabled = true;

        transform.position = spawnPoint.position;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

        animator.SetBool("death", false);
        animator.Play("idle", 0, 0f);
        greatSword.SetActive(true);

    }

}
