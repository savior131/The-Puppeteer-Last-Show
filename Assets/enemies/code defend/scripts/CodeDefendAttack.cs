using System.Collections;
using UnityEngine;

public class CodeDefendAttack : MonoBehaviour
{
    [SerializeField] float rageTimer = 15f;
    [SerializeField] int rageTrigger = 3;
    [SerializeField] float attackCoolDown = 5f;
    [SerializeField] Collider wall;
    [HideInInspector] public bool isDead = false;

    float timer;
    float cooldown;
    int rage;
    bool raged = false;
    bool isInRagePhase = false;

    Animator animator;
    ImpactFlash impact;


    void Start()
    {
        rage = rageTrigger;
        animator = GetComponentInChildren<Animator>();
        impact = GetComponent<ImpactFlash>();
        if (animator == null)
        {
            Debug.LogError("Animator not found in children!");
        }
    }

    IEnumerator RagePhase()
    {
        
            isInRagePhase = true;
            timer = rageTimer;
            cooldown = attackCoolDown;

            animator.SetTrigger("rage");

            while (timer > 0f)
            {
                timer -= 0.3f;

                if (cooldown > 0f)
                {
                    cooldown -= 0.3f;
                }

                yield return new WaitForSeconds(0.3f);
            }
        
        raged = false;
        isInRagePhase = false;
        Debug.Log("Rage phase ended.");
    }
    private void OnTriggerStay(Collider other)
    {
        if (isDead)
        {
            if (wall != null)
            {
                wall.enabled = false;
            }
            return;
        }
        if (other.gameObject.CompareTag("Player") && cooldown <= 0f && isInRagePhase)
        {

            Debug.Log("Attacking player!");
            animator.SetTrigger("attack");
            cooldown = attackCoolDown;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (isDead)
        {
            if(wall != null)
            {
                wall.enabled = false;
            }    
            return;
        }
        if (collision.collider.CompareTag("Player") && cooldown <= 0f && isInRagePhase)
        {

            Debug.Log("Attacking player!");
            animator.SetTrigger("attack");
            cooldown = attackCoolDown;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isDead)
        {
            return;
        }
        if (other.gameObject.CompareTag("sword"))
        {
            if (!raged)
            {
                rage--;
                impact.TriggerFlash();
                animator.SetTrigger("hit");
                Debug.Log($"Parried! Rage left: {rage}");
            }

            if (rage <= 0 && !raged && !isInRagePhase)
            {
                raged = true;
                rage = rageTrigger;
                Debug.Log("Rage phase triggered!");
                StartCoroutine(RagePhase());
            }
        }
        Debug.Log("Entered trigger with: " + other.name);
    }
}
