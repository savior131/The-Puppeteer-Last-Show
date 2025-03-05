using UnityEngine;
using System.Collections;

public class ParrySystem : MonoBehaviour
{
    [SerializeField] private float parryCooldown = 1.5f;
    [SerializeField] private float parryDuration = 0.4f;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float parryRadius = 2f;
    [SerializeField] private LayerMask detectionLayer;
    [SerializeField] private string[] detectionTags;
    [SerializeField] private ParticleSystem parryShield;
    [SerializeField] private Animator animator;
    private bool canParry = true;
    private Transform closestTarget;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canParry)
        {
            AttemptParry();
        }
    }

    private void AttemptParry()
    {
        FindClosestTarget();
        if (closestTarget != null && Vector3.Distance(transform.position, closestTarget.position) <= parryRadius)
        {
            closestTarget.GetComponent<IParriable>()?.OnParried();
        }
        StartCoroutine(ParryCooldownRoutine());
    }

    private void FindClosestTarget()
    {
        closestTarget = null;
        float minDistance = detectionRadius;
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

        foreach (Collider col in colliders)
        {
            if (IsValidTarget(col.gameObject))
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTarget = col.transform;
                }
            }
        }
    }

    private bool IsValidTarget(GameObject obj)
    {
        foreach (string tag in detectionTags)
        {
            if (obj.CompareTag(tag)) return true;
        }
        return false;
    }

    private IEnumerator ParryCooldownRoutine()
    {
        canParry = false;
        animator.SetTrigger("parry");
        parryShield.Play(); 
        yield return new WaitForSeconds(parryDuration);
        yield return new WaitForSeconds(parryCooldown - parryDuration);
        canParry = true;
    }
}
