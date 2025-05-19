using UnityEngine;

public class CannonPiece : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float fireRange = 10f;
    [SerializeField] private ParticleSystem destroyEffect;

    public CannonQueueManager Manager { get; set; }

    private bool isReady;
    private bool isDead;
    private float cooldown;
    private Transform player;
    private Animator animator;

    public void SetReady(bool ready)
    {
        isReady = ready;
        cooldown = fireRate;
    }


    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (!isReady || isDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > fireRange) return;

        RotateTowardsPlayer();

        cooldown -= Time.deltaTime;
        if (cooldown <= 0f)
        {
            Fire();
            cooldown = fireRate;
        }
    }

    void Fire()
    {
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Projectile>().setReturnTarget(transform);
    }

    public void OnBulletHit()
    {
        if (isDead) return;
        isDead = true;


        destroyEffect.transform.SetParent(transform.parent);
        destroyEffect.Play();
        animator.SetTrigger("destroy");
        Destroy(gameObject, 0.2f);
        Manager?.PopCannonPiece(this);
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        if (direction.sqrMagnitude < 0.01f) return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Quaternion yOffset = Quaternion.Euler(0, -90f, 0);
        Quaternion targetRotation = lookRotation * yOffset;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.SetActive(false);
            OnBulletHit();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fireRange);
    }
}
