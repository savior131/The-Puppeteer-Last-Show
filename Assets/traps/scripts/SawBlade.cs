using UnityEngine;

public class SawBladeMover : MonoBehaviour
{
    [SerializeField] private Vector3 direction = Vector3.right;
    [SerializeField] private float moveRange = 3f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float pauseTime = 0f;
    [SerializeField] private bool randomizeStartDirection = true;

    private Vector3 startPoint;
    private Vector3 endPoint;
    private bool movingForward;
    private float pauseTimer = 0f;

    void Start()
    {
        direction = direction.normalized;
        startPoint = transform.position;
        endPoint = startPoint + direction * moveRange;

        movingForward = !randomizeStartDirection || Random.value > 0.5f;
    }

    void Update()
    {
        if (pauseTimer > 0f)
        {
            pauseTimer -= Time.deltaTime;
            return;
        }

        Vector3 target = movingForward ? endPoint : startPoint;
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            movingForward = !movingForward;
            pauseTimer = pauseTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 d = direction.normalized * moveRange;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + d);
    }
}
