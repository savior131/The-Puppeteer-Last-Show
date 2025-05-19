using System.Collections.Generic;
using UnityEngine;

public class CannonQueueManager : MonoBehaviour
{
    [SerializeField] private int initialCount = 3;
    [SerializeField] private float verticalSpacing = 1f;

    private GameObject cannonPrefab;
    private List<CannonPiece> cannonQueue = new();

    void Start()
    {
        if (transform.childCount > 0)
        {
            cannonPrefab = transform.GetChild(0).gameObject;
            CannonPiece firstPiece = cannonPrefab.GetComponent<CannonPiece>();
            firstPiece.Manager = this;
            cannonQueue.Add(firstPiece);

            // Explicitly position the first piece
            firstPiece.transform.position = transform.position;
        }
        else
        {
            Debug.LogError("CannonQueueManager requires at least one cannon as a child.");
            return;
        }

        for (int i = 1; i < initialCount; i++)
        {
            AddCannonPiece();
        }

        UpdateCannonStates();
    }

    public void AddCannonPiece()
    {
        Vector3 position = transform.position + Vector3.up * verticalSpacing * cannonQueue.Count;
        GameObject obj = Instantiate(cannonPrefab, position, cannonPrefab.transform.rotation, transform);
        CannonPiece piece = obj.GetComponent<CannonPiece>();
        piece.Manager = this;
        cannonQueue.Add(piece);
        UpdateCannonStates();
    }

    public void PopCannonPiece(CannonPiece piece)
    {
        if (cannonQueue.Remove(piece))
        {
            StartCoroutine(RepositionQueue());
        }
    }

    private IEnumerator<WaitForSeconds> RepositionQueue()
    {
        for (int i = 0; i < cannonQueue.Count; i++)
        {
            Vector3 targetPos = transform.position + Vector3.up * verticalSpacing * i;
            StartCoroutine(MoveTo(cannonQueue[i].transform, targetPos));
        }

        yield return new WaitForSeconds(0.5f);
        UpdateCannonStates();
    }

    private IEnumerator<WaitForEndOfFrame> MoveTo(Transform t, Vector3 target)
    {
        float time = 0f;
        Vector3 start = t.position;
        while (time < 0.5f)
        {
            t.position = Vector3.Lerp(start, target, time / 0.5f);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        t.position = target;
    }

    private void UpdateCannonStates()
    {
        for (int i = 0; i < cannonQueue.Count; i++)
        {
            cannonQueue[i].SetReady(i == 0);
        }

        if (cannonQueue.Count == 0)
        {
            Destroy(gameObject, 1f);
        }
    }
}