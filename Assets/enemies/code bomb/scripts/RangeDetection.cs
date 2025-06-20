using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RangeDetection : MonoBehaviour
{
    [Header("Detection Ranges")]
    [SerializeField] private float bigRange = 50.0f;
    [SerializeField] private float smallRange = 20.0f;

    [Header("Enemy Settings")]
    [SerializeField] private float sprintSpeed = 10.0f;

    private GameObject player;
    private Rigidbody playerRb;
    private Stamina playerStamina;
    private Animator animator;
    private Explosion explosion;
    private NavMeshAgent agent;
    private Coroutine runningCoroutine;

    private bool isSprinting;
    private bool inBigRange = false;
    private bool inSmallRange = false;
    private bool isDead = false;
    private bool hasScreamed = false;
    bool isScreaming = false;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody>();
            playerStamina = player.GetComponent<Stamina>();
        }

        animator = GetComponentInChildren<Animator>();
        explosion = GetComponent<Explosion>();
    }

    private void Update()
    {
        if (player == null || isDead)
            return;

        UpdatePlayerState();
        HandleRangeDetection();
    }

    private void UpdatePlayerState()
    {
        float playerSpeed = playerRb != null ? playerRb.linearVelocity.magnitude : 0f;
        isSprinting = playerStamina != null && !playerStamina.IsDrained && playerSpeed > sprintSpeed;
    }

    private void HandleRangeDetection()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        inSmallRange = distance < smallRange;
        inBigRange = distance < bigRange;
        //isScreaming = animator.GetCurrentAnimatorStateInfo(0).IsName("scream");

        if (inSmallRange)
        {
            if (runningCoroutine != null)
            {
                StopCoroutine(runningCoroutine);
                runningCoroutine = null;
            }
            TriggerExplosion();
        }
        else if (inBigRange && isSprinting)
        {

            if (runningCoroutine == null)
            {
                runningCoroutine = StartCoroutine(runCoroutine());

            }

        }
        else
        {
            if (runningCoroutine != null)
            {
                StopCoroutine(runningCoroutine);
                runningCoroutine = null;
            }
            hasScreamed = false;
            animator.SetFloat("velocity", 0.05f);
            agent.isStopped = true;
        }
    }
    IEnumerator runCoroutine()
    {
        agent.isStopped = true;
        Debug.Log("PATH FINDINGGGGGG");
        // Add pathfinding logic here
        if (!hasScreamed)
        {
            animator.SetTrigger("scream");
            hasScreamed = true;
        }
        yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).IsName("scream"));
        yield return new WaitWhile(() =>
                animator.GetCurrentAnimatorStateInfo(0).IsName("scream") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);
        animator.SetFloat("velocity", 0.15f);
        agent.isStopped = false;
        Debug.Log("start movnig");
        agent.SetDestination(player.transform.position);
    }
    private void TriggerExplosion()
    {
        if (isDead) return;

        explosion?.TriggerExplosion();
        isDead = true;
        agent.isStopped = true;
        animator?.SetTrigger("death");
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
            runningCoroutine = null;
        }

    }

    // Draw gizmos in editor to visualize ranges
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, bigRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, smallRange);
    }
}