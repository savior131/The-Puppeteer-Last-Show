using UnityEngine;
using System.Collections;

public class Phase4 : BossPhase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private ParticleSystem fireEffect;
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private float burstDelay = 0.1f;
    [SerializeField] private float xrotation = -30f;
    [SerializeField, Range(0, 180)] private float maxRotationAngle = 45f;

    private Coroutine fireRoutine;

    public override void StartPhase()
    {
        fireRoutine = StartCoroutine(FireLoop());
    }

    public override void EndPhase()
    {
        if (fireRoutine != null)
            StopCoroutine(fireRoutine);
    }

    public override void UpdatePhase() { }

    private IEnumerator FireLoop()
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {
            angleRotation.SetXRotation(xrotation);
            angleRotation.SetYRotation(Random.Range(-maxRotationAngle, maxRotationAngle));

            yield return new WaitForSeconds(fireRate / 2f);

            for (int i = 0; i < 3; i++)
            {
                fireEffect.Play();
                fire(projectilePrefab, firePoint);
                yield return new WaitForSeconds(burstDelay);
            }

            yield return new WaitForSeconds(fireRate / 2f);
        }
    }
}
