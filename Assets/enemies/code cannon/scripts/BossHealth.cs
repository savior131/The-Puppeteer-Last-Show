using UnityEngine;
using System.Collections.Generic;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private List<Animator> animators;
    [SerializeField] private LayerMask damageLayer;
    [SerializeField] private List<DamageSource> damageSources;
    [SerializeField] private BossController bossController;
    [SerializeField] private List<ParticleSystem> damageParticles;
    [SerializeField] private CameraManager cameraManager;


    private int currentHealth;
    public int CurrentHealth => currentHealth;

    private bool isDead;

    [System.Serializable]
    private struct DamageSource
    {
        public string tag;
        public int damage;
    }

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & damageLayer) != 0)
        {
            int damage = GetDamageFromTag(other.gameObject.tag);
            if (damage > 0)
                TakeDamage(damage);
        }
    }

    private int GetDamageFromTag(string tag)
    {
        foreach (var source in damageSources)
        {
            if (source.tag == tag)
                return source.damage;
        }
        return 0;
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        foreach (var animator in animators)
        {
            animator.SetTrigger("hit");
        }
        int phaseIndex = bossController.getCurrentPhaseIndex();
        int totalPhases = bossController.getPhases();

        if (phaseIndex < totalPhases - 1) 
        {
            float nextPhaseThreshold = maxHealth - ((phaseIndex + 1) * (maxHealth / totalPhases));
            if (currentHealth <= nextPhaseThreshold)
            {
                DestroyAllProjectiles();
                cameraManager.OverrideCamera("BossCamera", 2f);
                foreach (var animator in animators)
                {
                    animator.SetTrigger("rage");
                }
                damageParticles[phaseIndex].Play();
                bossController.NextPhase();
            }
        }



        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void DestroyAllProjectiles()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (var bullet in bullets)
        {
            Destroy(bullet);
        }
    }

    private void Die()
    {
        if (isDead)
            return;
        cameraManager.OverrideCamera("BossCamera", 2f);
        bossController.OnDeath();
        isDead = true;
        foreach (var animator in animators)
        {
            animator.SetBool("death", true);
        }
        
    }
}
