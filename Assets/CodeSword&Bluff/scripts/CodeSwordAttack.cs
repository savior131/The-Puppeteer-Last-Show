using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CodeSwordAttack : MonoBehaviour
{
    public enum enemySwordType { BLUFF, SWORD };

    private PlayerRangeDetector detector;
    private NavMeshAgent agent;
    private Animator animator;
    private CapsuleCollider capsuleCollider;
    bool isAttacking = false;

    [SerializeField] SphereCollider parryCollider;
    [SerializeField] BoxCollider swordCollider;

    float savedRotY;
    bool rotationAdjusted = false;

   Transform player;
    [SerializeField] float chaseSpeed = 3f;
    private Coroutine attackCoroutine;

    [SerializeField] private enemySwordType enemyType;

    [SerializeField] private float attackCooldown = 2f;
    bool canAttack = true;
    bool isdead;
    
    private Collider[] allChildColliders;

    int randomFeintsCount;

    private void Start()
    {
        randomFeintsCount = 0;
        detector = GetComponent<PlayerRangeDetector>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        isdead = false;
        if (animator == null)
        {
            Debug.Log("animator is null");
        }
        if (detector == null)
        {
            Debug.Log("NULL DETECTOR");
        }
        animator.applyRootMotion = false;
        if (enemyType == enemySwordType.BLUFF)
            randomFeintsCount = Random.Range(1, 3);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void isDead()
    {
        isdead = true;
        allChildColliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in allChildColliders)
        {
            col.enabled = false;
        }
        StartCoroutine(changePositionWhenDead());
        canAttack = false;
        agent.enabled = false;
        //swordCollider.enabled = false;
        //parryCollider.enabled = false;
        StopCoroutine(attackCoroutine);
        capsuleCollider.enabled = false;

    }

    IEnumerator changePositionWhenDead()
    {
        yield return new WaitForSeconds(1.69f);
        Vector3 start = transform.position;
        Vector3 target = start + Vector3.up * -1f;
        float duration = 0.2f; // time to complete movement
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = target; // ensure exact final position
    }
    private void Update()
    {
        if (!isdead)
        {

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            Debug.DrawLine(transform.position, player.position, Color.red);

            detector.UpdateRangeStatus(distanceToPlayer);

            if (isAttacking) return;

            if (detector.isPlayerInAttackRange)
            {
                if (attackCoroutine == null && canAttack)
                    attackCoroutine = StartCoroutine(AttackCoroutine());
            }
            else if (detector.isPlayerInChaseRange)
            {
                if(attackCoroutine != null)
                    StopCoroutine(attackCoroutine);
                animator.ResetTrigger("attack");
                agent.isStopped = false;

                // Restore original rotation when returning to chase
                if (rotationAdjusted)
                {
                    transform.rotation = Quaternion.Euler(
                        transform.rotation.eulerAngles.x,
                        savedRotY,
                        transform.rotation.eulerAngles.z
                    );
                    rotationAdjusted = false;
                }

                agent.SetDestination(player.position);
                animator.SetFloat("velocity", 0.15f);
            }
            else
            {
                animator.ResetTrigger("attack");
                agent.isStopped = true;
                animator.SetFloat("velocity", 0f);
            }
        }
    }
    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        canAttack = false;

        agent.isStopped = true;
        animator.SetFloat("velocity", 0f);

        if (!rotationAdjusted)
        {
            savedRotY = transform.rotation.eulerAngles.y;
            float newY = savedRotY - 20;//+ 0;//45f;
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                newY,
                transform.rotation.eulerAngles.z
            );
            rotationAdjusted = true;
        }
        switch (enemyType)
        {
            case enemySwordType.BLUFF:
                randomFeintsCount = Random.Range(1, 3);

                break;
        }
        Debug.Log(randomFeintsCount);

        for (int i = 0; i < randomFeintsCount; i++)
        {
            Debug.Log($"Feint #{i + 1}");

            animator.SetTrigger("bluff");

            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).IsName("bluff"));

            yield return new WaitWhile(() =>
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);

            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("after for loop");


        animator.SetTrigger("attack");

        // Wait for animation to start
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("attack"));
        yield return new WaitForSeconds(0.5f);
        if (parryCollider != null)
            parryCollider.enabled = true;
        swordCollider.enabled = true;

        yield return new WaitForSeconds(0.3f);
        swordCollider.enabled = false;

        if (parryCollider != null)
            parryCollider.enabled = false;

        // Wait for animation to end
        yield return new WaitWhile(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("attack") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f
        );

        isAttacking = false;
        attackCoroutine = null;

        // Wait for cooldown before allowing another attack
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
