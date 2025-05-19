using UnityEngine;

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

    private bool isSprinting;
    private bool inBigRange = false;
    private bool inSmallRange = false;
    private bool isDead = false;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");

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

        if (inSmallRange)
        {
            TriggerExplosion();
        }
        else if (inBigRange && isSprinting)
        {
            Debug.Log("PATH FINDINGGGGGG");
            // Add pathfinding logic here
        }
    }

    private void TriggerExplosion()
    {
        if (isDead) return;

        explosion?.TriggerExplosion();
        isDead = true;
        animator?.SetTrigger("death");
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
