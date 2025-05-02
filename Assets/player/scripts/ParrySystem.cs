using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParrySystem : MonoBehaviour
{
    [SerializeField] private float parryCooldown = 1.5f;
    [SerializeField] private float parryDuration = 0.4f; 
    [SerializeField] private float parryWindow = 0.3f;
    [SerializeField] private float parryRadius = 2f;
    [SerializeField] Vector3 offset;
    [SerializeField] private LayerMask detectionLayer;
    [SerializeField] private ParticleSystem parryShield;
    [SerializeField] private Animator animator;
    [SerializeField] private Health health;
   

    private bool canParry = true;
    private Coroutine parryWindowRoutine;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canParry&&!health.isDead)
        {
            if (parryWindowRoutine != null)
                StopCoroutine(parryWindowRoutine);
            parryWindowRoutine = StartCoroutine(StartParryWindow());
        }
    }

    private IEnumerator StartParryWindow()
    {
        canParry = false;
        animator.SetTrigger("parry");
        parryShield.Play();

        float timer = 0f;
        bool parrySuccessful = false;

        while (timer < parryWindow)
        {
            if (CheckForParryableTargets())
            {
                parrySuccessful = true;
                break;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        if (parrySuccessful)
        {
            ParryTargetsInRadius();
        }

        yield return new WaitForSeconds(parryCooldown);
        canParry = true;
    }

    private bool CheckForParryableTargets()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position+offset, parryRadius, detectionLayer);
        foreach (Collider col in colliders)
        {
            if (col.GetComponent<IParriable>() != null)
                return true;
        }
        return false;
    }

    private void ParryTargetsInRadius()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + offset, parryRadius, detectionLayer);
        foreach (Collider col in colliders)
        {
            IParriable parriable = col.GetComponent<IParriable>();
            if (parriable != null)
            {
                parriable.OnParried();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + offset, parryRadius);
    }
}
