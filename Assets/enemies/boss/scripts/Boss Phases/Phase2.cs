using UnityEngine;

public class Phase2 : BossPhase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private ParticleSystem fireEffect;
    private float fireCooldown;


    public override void StartPhase()
    {
        fireCooldown = fireRate;
    }

    public override void UpdatePhase()
    {
        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0)
        {
            angleRotation.SetRotation(Random.Range(-45, 45));
            fireEffect.Play();
            fire(projectilePrefab, firePoint);
            fireCooldown = fireRate;
        }
    }

    public override void EndPhase()
    {

    }

}

