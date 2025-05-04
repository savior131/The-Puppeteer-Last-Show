using UnityEngine;
using System.Collections;

public class Phase5 : BossPhase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private ParticleSystem fireEffect;
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private Transform player;
    [SerializeField] private float xrotation = -30f;
    [SerializeField, Range(0, 180)] private float maxRotationAngle = 45f;
    [SerializeField] private float rotationSpeed = 5f;

    private Coroutine fireRoutine;

    public override void StartPhase()
    {
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
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0;
            if (direction.sqrMagnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(-direction);
                Quaternion smoothed = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.rotation = smoothed;
                angleRotation.SetYRotation(smoothed.eulerAngles.y);
                return;
            }
        }

        float randomY = Random.Range(-maxRotationAngle, maxRotationAngle);
        angleRotation.SetYRotation(randomY);
    }

    private IEnumerator FireLoop()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {

            angleRotation.SetXRotation(xrotation);

            if (player == null)
            {
                angleRotation.SetYRotation(Random.Range(-maxRotationAngle, maxRotationAngle));
            }

            yield return new WaitForSeconds(fireRate);
            fireEffect.Play();
            fire(projectilePrefab, firePoint);
        }
    }
}
