using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    [SerializeField] Transform player; // Assign your player object here in the Inspector

    void Update()
    {
        if (player != null)
        {
            // Look at the player
            Vector3 direction = player.position - transform.position;

            // Optional: Ignore vertical difference (keep only X and Z)
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
            }
        }
    }
}
