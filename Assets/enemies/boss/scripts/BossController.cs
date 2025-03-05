
using UnityEngine;
using System.Collections.Generic;

public class BossController : MonoBehaviour
{
    [SerializeField] private GameObject phasesContainer;
    private List<BossPhase> phases = new List<BossPhase>();
    private int currentPhaseIndex;
    private BossPhase currentPhase;
    private AngleRotation angleRotation;
    private Transform bossTransform;
    private void Awake()
    {
        if (phasesContainer != null)
        {
            phases.AddRange(phasesContainer.GetComponents<BossPhase>());
        }
        
    }
    void Start()
    {
        if (phases.Count > 0)
        {
            SetPhase(0);
        }
        bossTransform = transform.CopyTransform();
        bossTransform.position += Vector3.up * 1.5f;
    }

    void Update()
    {
        currentPhase?.UpdatePhase();
    }

    public void NextPhase()
    {
        if (currentPhaseIndex + 1 < phases.Count)
        {
            SetPhase(currentPhaseIndex + 1);
        }
    }

    private void SetPhase(int index)
    {
        if (currentPhase != null)
        {
            currentPhase.EndPhase();
        }
        currentPhaseIndex = index;
        currentPhase = phases[index];
        currentPhase.Initialize(this,bossTransform);
        currentPhase.StartPhase();
    }

    public AngleRotation GetAngleRotation()
    {
        return GetComponent<AngleRotation>();
    }
}

