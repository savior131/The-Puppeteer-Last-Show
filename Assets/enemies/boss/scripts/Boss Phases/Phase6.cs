using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Phase6 : BossPhase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private ParticleSystem fireEffect;
    [SerializeField] private float xrotation = -30f;
    [SerializeField, Range(0, 180)] private int maxRotationAngle = 45;

    private Coroutine fireRoutine;
    public override void StartPhase()
    {
        angleRotation.SetXRotation(xrotation);
        angleRotation.SetYRotation(0f);
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
        int offset = 0;
        int increment = 5;
        yield return new WaitForSeconds(1f);
        while (true)
        {
         
            if (offset <= -maxRotationAngle)
            {
                increment = 5;
            }
            if (offset >= maxRotationAngle)
            {
                increment = -5;
            }
            offset += increment;
            angleRotation.SetXRotation(xrotation);
               fireEffect.Play();
               angleRotation.SetYRotation(offset);
               fire(projectilePrefab, firePoint);
               yield return new WaitForSeconds(0.1f);
            
        }
    }
    
}
