using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CheckpointManager>(out var manager))
        {
            manager.SetCheckpoint(transform);
            gameObject.GetComponent<Collider>().enabled = false;

        }
    }
}
