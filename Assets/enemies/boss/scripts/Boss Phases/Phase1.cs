using UnityEngine;
using System.Collections;

public class Phase1 : BossPhase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private ParticleSystem fireEffect;
    [SerializeField, Range(0, 180)] private float maxRotationAngle = 45f;

    private float waitTime;
    private Coroutine fireRoutine;

    public override void StartPhase()
    {
        waitTime = fireRate / 2f;
        fireRoutine = StartCoroutine(FireLoop());
    }

    public override void EndPhase()
    {
        if (fireRoutine != null)
        {
            StopCoroutine(fireRoutine);
        }
    }
    public override void UpdatePhase()
    {

    }

    private IEnumerator FireLoop()
    {
        while (true)
        {
            angleRotation.SetRotation(Random.Range(-maxRotationAngle, maxRotationAngle));
            yield return new WaitForSeconds(waitTime);

            fireEffect.Play();
            fire(projectilePrefab, firePoint);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
