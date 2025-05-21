using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private Transform lastCheckpoint;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Health health;
    private void Update()
    {
        if (Input.GetKey(KeyCode.V))
        {
            Respawn();
        }
    }
    private void Awake()
    {
        health = GetComponent<Health>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        lastCheckpoint = null;
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        Debug.Log("Checkpoint set to: " + checkpoint.name);
        lastCheckpoint = checkpoint;
    }

    public void Respawn()
    {
        Transform spawnPoint = lastCheckpoint != null ? lastCheckpoint : null;

        if (spawnPoint == null)
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
        else
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }
        health.ResetPlayer(transform);
    }
}
