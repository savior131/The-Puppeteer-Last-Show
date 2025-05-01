using UnityEngine;
using System.Collections;

public class Phase4 : BossPhase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private ParticleSystem fireEffect;
    [SerializeField] private float xrotation = -30f;
    [SerializeField, Range(0, 180)] private float maxRotationAngle = 45f;


    private float waitTime;
    private Coroutine fireRoutine;

    public override void StartPhase()
    {
        angleRotation.SetXRotation(xrotation);
        angleRotation.SetYRotation(Random.Range(-maxRotationAngle, maxRotationAngle));
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
        yield return new WaitForSeconds(1f);
        while (true)
        {
            
            angleRotation.SetXRotation(xrotation);
            yield return new WaitForSeconds(waitTime);
            
            float[] angleOffsets = { -2f, 0f, 2f };
            foreach (float offset in angleOffsets)
            {
                fireEffect.Play();
                angleRotation.SetYRotation(offset);
                fire(projectilePrefab, firePoint);
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(waitTime);
            angleRotation.SetYRotation(Random.Range(-maxRotationAngle, maxRotationAngle));
        }
    }
}
